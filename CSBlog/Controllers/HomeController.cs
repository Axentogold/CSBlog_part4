using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using CSBlog.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CSBlog.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    _logger.LogDebug(1, "NLog injected into HomeController");
    return RedirectToAction("Index", "Article");
  }

  public IActionResult Privacy()
  {

    _logger.LogInformation("Hello, this is Privacy page!");
    throw new ArgumentException("Privacy went belly up");
    // return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
    var statusCode = exception?.Error.GetType().Name switch
    {
      "ArgumentException"  => HttpStatusCode.BadRequest,
      _ => HttpStatusCode.ServiceUnavailable
    };

    // return Problem(detail: exception?.Error.Message, statusCode: (int) statusCode);
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  public IActionResult Oops()
  {
    return View();
  }
  
}