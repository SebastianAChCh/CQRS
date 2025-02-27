using CQRS.Application.DTOs;
using MediatR;

namespace CQRS.Infrastructure.Commands
{
    public record CreatePostCommand(string Title, string Description, string Author) : IRequest<PostDto>;
}
