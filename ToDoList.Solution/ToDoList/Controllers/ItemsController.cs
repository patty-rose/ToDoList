using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    // [HttpGet("/items")] //we have two /items routes but because one is HttpGet and the other HttpPost the server can distinguish
    // public ActionResult Index() //Index pairs with Index.cshtml
    // {
    //   List<Item> allItems = Item.GetAll();
    //   return View(allItems);
    // } ojbects nested in parent objects do not need an index route

    [HttpGet("/categories/{categoryId}/items/new")] //names the url path
    public ActionResult New(int categoryId) //matches name of the cshtml doc
    {
      Category category = Category.Find(categoryId);
      return View(category); //CreateForm.cshtml does all the work of creating the form here.. this method simply tells our browser to grab it and display it.
    }

    // [HttpPost("/items")] //names the url path AND matches our forms action. When the form with this action is submitted this route will be invoked.
    // public ActionResult Create(string description) //description MATCHES the name attribute of our forms field that coorelates to this.
    // {
    //   Item myItem = new Item(description);
    //   return RedirectToAction("Index");// redirectToAction(route) calls on Index route after create(callback routes)

    //   //return View("Index", myItem); //"Index" specifies the Index.cshtml view since we are no longer routing to a view with the same name as our method (Create- in this case)
    //   //myItem specifies Model property as called on in the Index view w/ @Model.Description 
    // }

    [HttpPost("/items/delete")]
    public ActionResult DeleteAll()
    {
      Item.ClearAll();
      return View();
    }

    [HttpGet("/categories/{categoryId}/items/{itemId}")]//curly brackets in route decorator is called Dynamic Routing-- it changes depending on user use. This particular dynamic route matches the method parameters
    public ActionResult Show(int categoryId, int itemId)
    {
      Item item = Item.Find(itemId);
      Category category = Category.Find(categoryId);
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("item", item);
      model.Add("category", category);//we're using a dictionary to pass both items into the return View(), it cannot take two arguments.
      return View(model);
    }
  }
}