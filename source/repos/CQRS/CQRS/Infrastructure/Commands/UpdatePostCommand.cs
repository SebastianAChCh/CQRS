using CQRS.Application.DTOs;
using MediatR;

namespace CQRS.Infrastructure.Commands
{
    public record UpdatePostCommand(int Id, string Title, string Description, string Author) : IRequest<PostDto>;
}
