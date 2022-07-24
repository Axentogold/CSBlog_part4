using CSBlog.Core.Repository;
using CSBlog.Models.Blog;


namespace CSBlog.Data.Repository;

public class TagRepository : IRepository<Tag>
{
  private readonly ApplicationDbContext _context;

  public TagRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  private static Tag DefaultTag =>
    new()
    {
      Id = "0",
      TagName = "Not Found"
    };


  public ICollection<Tag> GetAll()
  {
    var tags = _context.Tags.ToList();
    return tags;
  }

  public Tag GetById(string? id)
  {
    return _context.Tags.FirstOrDefault(t => t.Id != null && t.Id.Equals(id)) ?? DefaultTag;
  }


  public Tag GetByName(string? name)
  {
    var tag = GetAll().FirstOrDefault(t => t.TagName == name);
    return tag ?? DefaultTag;
  }

  public async Task Create(Tag item)
  {
    await _context.Tags.AddAsync(item);
    await _context.SaveChangesAsync();
    Console.WriteLine($"Task Create: {item}");
  }

  public async Task Update(Tag item)
  {
    var tag = GetById(item.Id);
    tag.TagName = item.TagName;
    _context.Tags.Update(tag);
    await _context.SaveChangesAsync();
  }

  public async Task Delete(Tag item)
  {
    var tag = GetById(item.Id);
    _context.Tags.Remove(tag);
    await _context.SaveChangesAsync();
  }
}