using CQRS.Domains;

namespace CQRS.Infrastructure
{
    public interface ICacheRepo
    {
        Task<Post> GetPost(int id, CancellationToken cancellationToken);
        Task UpdatePost(Post post, CancellationToken cancellationToken);
        Task<Dictionary<string, string>> GetPosts(CancellationToken cancellationToken);

    }
}
