// using CSBlog.Data;
// using CSBlog.Models.Blog;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace CSBlog.Controllers.api;
//
// [Route("api/[controller]")]
// [ApiController]
// public class TagsController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;
//
//     public TagsController(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     // GET: api/Tags
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
//     {
//         return await _context.Tags.ToListAsync();
//     }
//
//     // GET: api/Tags/5
//     [HttpGet("{id}")]
//     public async Task<ActionResult<Tag>> GetTag(string id)
//     {
//         var tag = await _context.Tags.FindAsync(id);
//
//         if (tag == null)
//         {
//             return NotFound();
//         }
//
//         return tag;
//     }
//
//     // PUT: api/Tags/5
//     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//     [HttpPut("{id}")]
//     public async Task<IActionResult> PutTag(string id, Tag tag)
//     {
//         if (id != tag.Id)
//         {
//             return BadRequest();
//         }
//
//         _context.Entry(tag).State = EntityState.Modified;
//
//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!TagExists(id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw;
//             }
//         }
//
//         return NoContent();
//     }
//
//     // POST: api/Tags
//     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//     [HttpPost]
//     public async Task<ActionResult<Tag>> PostTag(Tag tag)
//     {
//         _context.Tags.Add(tag);
//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateException)
//         {
//             if (tag.Id != null && TagExists(tag.Id))
//             {
//                 return Conflict();
//             }
//
//             throw;
//         }
//
//         return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
//     }
//
//     // DELETE: api/Tags/5
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteTag(string id)
//     {
//         var tag = await _context.Tags.FindAsync(id);
//         if (tag == null)
//         {
//             return NotFound();
//         }
//
//         _context.Tags.Remove(tag);
//         await _context.SaveChangesAsync();
//
//         return NoContent();
//     }
//
//     private bool TagExists(string id)
//     {
//         return (_context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
//     }
// }

