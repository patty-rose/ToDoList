using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")] // /=homepage
    public ActionResult Index() //Index pairs with Index.cshtml
    {
      return View();
    }
  }
}