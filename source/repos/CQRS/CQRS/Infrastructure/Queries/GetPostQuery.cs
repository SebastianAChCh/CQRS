using CQRS.Application.DTOs;
using CQRS.Domains;
using MediatR;

namespace CQRS.Infrastructure.Queries
{
    public record GetPostQuery(int id) : IRequest<PostDto>;
}
