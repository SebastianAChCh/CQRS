using CQRS.Application.DTOs;
using CQRS.Domains;
using CQRS.Infrastructure;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using CQRS.Infrastructure.Queries;
using MediatR;
using System.Collections;
using System.Linq;

namespace CQRS.Application.Handler
{
    public class GetAllPosts : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
    {

        private readonly ICacheRepo _cacheRepo;

        public GetAllPosts(ICacheRepo cacheRepo) {
            _cacheRepo = cacheRepo;
        }

        async Task<IEnumerable<PostDto>> IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>.Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _cacheRepo.GetPosts(cancellationToken);

            result.ToList().ForEach(res => Console.WriteLine(res.Value + " " + res.Key));

            return result.Select(res => new PostDto
            {
                Title = res.Value,
                Description = res.Value,
                Author = res.Value
            });
        }
    }
}
