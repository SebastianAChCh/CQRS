using CQRS.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading;

namespace CQRS.Infrastructure.DataBase.RepositoryPosts
{
    public class CacheRepo : ICacheRepo
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IPostRepository _postsRepo;

        public CacheRepo(IDistributedCache distributedCache, IConnectionMultiplexer redisConnection, IPostRepository postsRepo) {
            _distributedCache = distributedCache;
            _redisConnection = redisConnection;
            _postsRepo = postsRepo;
        }

        public async Task<Post> GetPost(int id, CancellationToken cancellationToken) {
            string key = $"member-{id}";

            Post posts;

            string post = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(post))
            {
                posts = await _postsRepo.GetPost(id, cancellationToken);
                if (posts == null) return posts;

                await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(posts));
                return posts;
            }

            return JsonSerializer.Deserialize<Post>(post);
        }

        public async Task UpdatePost(Post post, CancellationToken cancellationToken) {
            var db = _redisConnection.GetDatabase();

            await db.StringSetAsync(post.id_key.ToString(),
                JsonSerializer.Serialize<Post>(
                   new Post { title = post.title, description = post.description, author = post.author }
                    )
                );
        }

        public async Task<Dictionary<string, string>> GetPosts(CancellationToken cancellationToken) {
            var db = _redisConnection.GetDatabase();
            var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
            var keys = server.Keys();
            var quantityElementsDb = _postsRepo.CountElements(cancellationToken);

            if (keys.Count() == 0 || keys.Count() != quantityElementsDb) {
                var values = await _postsRepo.GetPosts(cancellationToken);
                if (values == null) return null;
                values.ForEach(value => {
                    db.StringSetAsync(value.id_key.ToString(), JsonSerializer.Serialize<Post>(
                        new Post { title = value.title, description = value.description, author = value.author }
                    ));
                });
            }

            var posts = new Dictionary<string, string>();

            foreach (var key in keys)
            {
                var keyType = db.KeyType(key.ToString());
                if (keyType == RedisType.String)
                {
                    var value = db.StringGet(key.ToString());
                    posts.Add(key.ToString(), value.ToString());
                }
            }

            return posts;
        }



    }
}
