using CQRS.Application.DTOs;
using CQRS.Domains;

namespace CQRS.Infrastructure
{
    public interface IPostRepository
    {
        public int CountElements(CancellationToken cancellationToken);
        public Task AddPost(Post post, CancellationToken cancellationToken);
        public Task<List<Post>> GetPosts(CancellationToken cancellationToken);
        public Task<Post> GetPost(int id, CancellationToken cancellationToken);
        public Task<PostDto> UpdatePost(Post postModified, CancellationToken cancellationToken);
        public Task<Boolean> RemovePost(int id, CancellationToken cancellationToken);
    }
}
