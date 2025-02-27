using CQRS.Application.DTOs;
using CQRS.Infrastructure;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using MediatR;

namespace CQRS.Application.Handler
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
    {

        private readonly IPostRepository _postsRepo;

        public DeletePostHandler(IPostRepository postsRepo) { 
            _postsRepo = postsRepo;
        }

        Task<bool> IRequestHandler<DeletePostCommand, bool>.Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var response = _postsRepo.RemovePost(request.Id, cancellationToken);
            return response;
        }
    }
}
