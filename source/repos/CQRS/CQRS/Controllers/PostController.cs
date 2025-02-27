using CQRS.Application.DTOs;
using CQRS.Application.Handler;
using CQRS.Domains;
using CQRS.Infrastructure.Commands;
using CQRS.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<PostDto>> GetAllPosts(){
            return await _mediator.Send(new GetPostsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetById(int id)
        {
            var response = new GetPostQuery(id);
            var post = await _mediator.Send(response);

            if (post == null) { 
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(CreatePostCommand post) {
            var postCreated = await _mediator.Send(post);
            return CreatedAtAction(nameof(GetById), new { id = postCreated.Id }, postCreated);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, UpdatePostCommand post)
        {
            if (string.IsNullOrEmpty(id.ToString()) || id != post.Id) {
                return BadRequest();
            }

            var postCreated = await _mediator.Send(post );

            if (postCreated == null) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeletePost(int id)
        {
            var postDeleted = await _mediator.Send(new DeletePostCommand(id));

            return postDeleted;
        }


    }
}
