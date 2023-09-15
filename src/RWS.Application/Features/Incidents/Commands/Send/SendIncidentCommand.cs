using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;
using AutoMapper;
using FirebaseAdmin.Messaging;
using RWS.Domain.Entities;

namespace RWS.Application.Features.Incidents.Commands.Send
{
    public class SendIncidentCommand : IRequest<Response<IncidentViewModel>>
    {
        public Guid Id { get; set; }
    }

    public class SendIncidentCommandHandler : IRequestHandler<SendIncidentCommand, Response<IncidentViewModel>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IPushTokenRepository _pushTokenRepository;
        private readonly IMapper _mapper;

        public SendIncidentCommandHandler(
            IIncidentRepository repository, 
            IMapper mapper, 
            IPushTokenRepository pushTokenRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _pushTokenRepository = pushTokenRepository;
        }

        public async Task<Response<IncidentViewModel>> Handle(SendIncidentCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(request.Id);

            if (model == null)
            {
                throw new ApiException($"Инцидент не найден.");
            }
            else
            {
                model.IsSend = true;
                model.SendingDateTime = DateTime.Now;

                await _repository.UpdateAsync(model);
                var viewModel = _mapper.Map<IncidentViewModel>(model);

                var pushTokens = await _pushTokenRepository.GetAllAsync();
                await CollectAndSendData(pushTokens);

                return new Response<IncidentViewModel>(viewModel, "Инцидент успешно отправлен.");
            }
        }

        private async Task CollectAndSendData(IReadOnlyCollection<PushToken> notifications)
        {
            var messages = new List<Message>();

            foreach (var notification in notifications)
            {
                Message message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = "Title",
                        Body = "Message"
                    }
                };

                message.Apns = new ApnsConfig();
                message.Apns.Aps = new Aps();

                message.Android = new AndroidConfig();
                message.Android.Notification = new AndroidNotification();
                //message.Android.Priority = Priority.High;

                message.Token = notification.Token;

                message.Data = new Dictionary<string, string>()
                {
                    {"action", "Notification"},
                    {"id", notification.Id.ToString()}
                };

                message.Apns.Aps.Sound = "default";
                message.Android.Notification.Sound = "default";
                messages.Add(message);
            }

            if (messages.Count == 0)
                return;

            await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
        }
    }
}
