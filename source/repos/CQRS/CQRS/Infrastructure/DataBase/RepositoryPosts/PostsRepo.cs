using CQRS.Application.DTOs;
using CQRS.Domains;
using Microsoft.EntityFrameworkCore;
using CQRS.Infrastructure;

namespace CQRS.Infrastructure.DataBase.RepositoryPosts
{
    public class PostsRepo : IPostRepository
    {
        private readonly DataBaseContext _dbContext;
        public PostsRepo(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPost(Post post, CancellationToken cancellationToken) {
            _dbContext.posts.Add(post);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Post>> GetPosts(CancellationToken cancellationToken) {
            return await _dbContext.posts.ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPost(int id_key, CancellationToken cancellationToken) {
            var item = await _dbContext.posts.FindAsync(new object[] { id_key }, cancellationToken);
            if (item == null) return null;

            return item;
        }

        public async Task<PostDto> UpdatePost(Post postModified, CancellationToken cancellationToken) {
            var values = await _dbContext.posts.FindAsync(new object[]{ postModified.id_key }, cancellationToken);

            if (values == null) return null;

            values.title = postModified.title;
            values.description = postModified.description;
            values.author = postModified.author;

            await _dbContext.SaveChangesAsync(cancellationToken);
            //await _cacheRepo.UpdatePost(postModified, cancellationToken);

            return new PostDto { 
                Id = values.id_key,
                Title = values.title,
                Description = values.description,
                Author = values.author
            };
        }

        public async Task<Boolean> RemovePost(int id_key, CancellationToken cancellationToken) {
            var values = await _dbContext.posts.FindAsync(new object[] { id_key }, cancellationToken);

            if (values == null) return false;

            _dbContext.posts.Remove(values);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public int CountElements(CancellationToken cancellationToken)
        {
            return _dbContext.posts.ToArray().Count();
        }
    }
}
