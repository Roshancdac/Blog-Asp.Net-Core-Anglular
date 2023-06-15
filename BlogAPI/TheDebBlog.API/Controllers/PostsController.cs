using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheDebBlog.API.Data;
using TheDebBlog.API.Models.DTO;
using TheDebBlog.API.Models.Entities;

namespace TheDebBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly TheDevBlogDbContext _theDevBlogDbContext;

        public PostsController(TheDevBlogDbContext theDevBlogDbContext) {
            _theDevBlogDbContext = theDevBlogDbContext; }

       [HttpGet]
       public async Task<IActionResult> GetAllPosts() 
            => Ok(await _theDevBlogDbContext.Posts.ToListAsync());

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _theDevBlogDbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            var post = new Post()
            {
                Id = Guid.NewGuid(),
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Sunmary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visisble = addPostRequest.Visible,
            };
           await  _theDevBlogDbContext.Posts.AddAsync(post);
           var status = await _theDevBlogDbContext.SaveChangesAsync() < 0;
           return CreatedAtAction(nameof(GetPostById), new { id = post.Id },post);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatePostRequest)
        {
            var existingPost = await _theDevBlogDbContext.Posts.FindAsync(id);
            if (existingPost != null)
            {
                existingPost.Title = updatePostRequest.Title;
                existingPost.Content = updatePostRequest.Content;
                existingPost.Author = updatePostRequest.Author;
                existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
                existingPost.PublishDate = updatePostRequest.PublishDate;
                existingPost.UpdatedDate = updatePostRequest.UpdatedDate;
                existingPost.Sunmary = updatePostRequest.Summary;
                existingPost.UrlHandle = updatePostRequest.UrlHandle;
                existingPost.Visisble = updatePostRequest.Visible;
                await _theDevBlogDbContext.SaveChangesAsync();
                return Ok(existingPost);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingPost = await _theDevBlogDbContext.Posts.FindAsync(id);
            if (existingPost != null)
            {
                _theDevBlogDbContext.Remove(existingPost);
                await _theDevBlogDbContext.SaveChangesAsync();
                return Ok(existingPost);
            }
            return NotFound();
        }
    }
}
