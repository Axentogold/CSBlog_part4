// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using CSBlog.Data;
// using CSBlog.Models.Blog;
//
// namespace CSBlog.Controllers.api
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class CommentsController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;
//
//         public CommentsController(ApplicationDbContext context)
//         {
//             _context = context;
//         }
//
//         // GET: api/Comments
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
//         {
//             return await _context.Comments.ToListAsync();
//         }
//
//         // GET: api/Comments/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Comment>> GetComment(string id)
//         {
//             var comment = await _context.Comments.FindAsync(id);
//
//             if (comment == null)
//             {
//                 return NotFound();
//             }
//
//             return comment;
//         }
//
//         // PUT: api/Comments/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutComment(string id, Comment comment)
//         {
//             if (id != comment.Id)
//             {
//                 return BadRequest();
//             }
//
//             _context.Entry(comment).State = EntityState.Modified;
//
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!CommentExists(id))
//                 {
//                     return NotFound();
//                 }
//
//                 throw;
//             }
//
//             return NoContent();
//         }
//
//         // POST: api/Comments
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<Comment>> PostComment(Comment comment)
//         {
//             _context.Comments.Add(comment);
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateException)
//             {
//                 if (comment.Id != null && CommentExists(comment.Id))
//                 {
//                     return Conflict();
//                 }
//
//                 throw;
//             }
//
//             return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
//         }
//
//         // DELETE: api/Comments/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteComment(string id)
//         {
//             var comment = await _context.Comments.FindAsync(id);
//             if (comment == null)
//             {
//                 return NotFound();
//             }
//
//             _context.Comments.Remove(comment);
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//
//         private bool CommentExists(string id)
//         {
//             return _context.Comments.Any(e => e.Id == id);
//         }
//     }
// }

