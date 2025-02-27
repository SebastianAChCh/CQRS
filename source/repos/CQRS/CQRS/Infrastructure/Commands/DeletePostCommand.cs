using MediatR;

namespace CQRS.Infrastructure.Commands
{
    public record DeletePostCommand(int Id) : IRequest<bool>;
}
