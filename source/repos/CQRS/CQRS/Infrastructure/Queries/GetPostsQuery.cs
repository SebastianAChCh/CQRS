using CQRS.Application.DTOs;
using CQRS.Domains;
using MediatR;
using System.Collections;

namespace CQRS.Infrastructure.Queries
{
    public record GetPostsQuery() : IRequest<IEnumerable<PostDto>>;
}
