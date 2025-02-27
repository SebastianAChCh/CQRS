using CQRS.Application.DTOs;
using CQRS.Domains;
using CQRS.Infrastructure;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using MediatR;

namespace CQRS.Application.Handler
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {

        public readonly IPostRepository _postRepo;

        public UpdatePostHandler(IPostRepository postRepo) {
            _postRepo = postRepo;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var value = await _postRepo.UpdatePost(new Post { id_key = request.Id, title = request.Title, description = request.Description, author = request.Author}, cancellationToken);

            if (value == null) {
                return null;
            }

            return new PostDto
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Author = value.Author
            };
        }
    }
}