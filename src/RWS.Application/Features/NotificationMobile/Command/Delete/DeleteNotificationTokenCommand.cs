using MediatR;
using RWS.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RWS.Application.Features.NotificationMobile.Command.Delete
{
    public class DeleteNotificationTokenCommand : IRequest<bool>
    {
        private Guid userId;

        public Guid GetUserId() => userId;
        public void SetUserId(Guid userId) => this.userId = userId;
    }

    class DeleteNotificationTokenCommandHandler : IRequestHandler<DeleteNotificationTokenCommand, bool>
    {
        private readonly IPushTokenRepository _repository;

        public DeleteNotificationTokenCommandHandler(IPushTokenRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteNotificationTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _repository.GetByUserId(request.GetUserId());

            if (token != null)
            {
                await _repository.DeleteAsync(token);
                return false;
            }

            return true;
        }
    }
}
