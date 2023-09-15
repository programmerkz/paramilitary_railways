using AutoMapper;
using MediatR;
using RWS.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RWS.Domain.Entities;

namespace RWS.Application.Features.NotificationMobile.Command.Create
{
    public class CreateNotificationTokenCommand : IRequest<bool>
    {
        private Guid userId;
        public string Token { get; set; }

        public Guid GetUserId() => userId;
        public void SetUserId(Guid userId) => this.userId = userId;
    }

    class CreateNotificationTokenCommandHandler : IRequestHandler<CreateNotificationTokenCommand, bool>
    {
        private readonly IPushTokenRepository _repository;
        private readonly IMapper _mapper;

        public CreateNotificationTokenCommandHandler(IPushTokenRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateNotificationTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _repository.GetByUserId(request.GetUserId());

            if (token != null)
            {
                await _repository.DeleteAsync(token);
            }

            token = _mapper.Map<PushToken>(request);
            token.UserId = request.GetUserId();
            await _repository.AddAsync(token);

            return true;
        }
    }
}
