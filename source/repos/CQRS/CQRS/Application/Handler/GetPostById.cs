using CQRS.Application.DTOs;
using CQRS.Domains;
using CQRS.Infrastructure;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using CQRS.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CQRS.Application.Handler
{
    public class GetPostById : IRequestHandler<GetPostQuery, PostDto>
    {

        private readonly ICacheRepo _cacheRepo;

        public GetPostById(ICacheRepo cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }

        public async Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var result = await _cacheRepo.GetPost(request.id, cancellationToken);

            if (result == null) return null;

            return new PostDto { 
                Title = result.title,
                Description = result.description,
                Author = result.author
            };
        }
    }

}
