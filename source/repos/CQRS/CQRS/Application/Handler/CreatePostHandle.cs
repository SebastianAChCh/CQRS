using CQRS.Application.DTOs;
using CQRS.Domains;
using CQRS.Infrastructure;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.DataBase;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using MediatR;

namespace CQRS.Application.Handler
{
    public class CreatePostHandle : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepo;

        public CreatePostHandle(IPostRepository postRepo) {
            _postRepo = postRepo;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                title = request.Title,
                description = request.Description,
                author = request.Author
            };

            await _postRepo.AddPost(post, cancellationToken);

            return new PostDto { 
                Title = post.title,
                Description = post.description,
                Author = post.author
            };
        }
    }
}
