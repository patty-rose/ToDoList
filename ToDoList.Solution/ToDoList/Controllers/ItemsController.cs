using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;//to access SelectList
using Microsoft.EntityFrameworkCore;//to access EntityState
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;//to use LINQ's ToList() method

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;

    public ItemsController(ToDoListContext db)
    {
      _db = db;
      //this constructor allows us to set the value of our new _db property to our ToDoListContext. This is achievable due to a dependency injection we set up in our AddDbContext method in the ConfigureServices method in our Startup.cs file.
    }

    public ActionResult Index()//nstead of using a verbose GetAll() method with raw SQL, we can instead access all our Items in List form by doing the following: _db.Items.ToList()
    {
      List<Item> model = _db.Items.Include(item => item.Category).ToList();//.Include() ~ for each database item inlude the CAtegory it belongs to and then put all the Items into list
      return View(model);
      //1. db is an instance of our DbContext class. It's holding a reference to our database.//2. Once there, it looks for an object named Items. This is the DbSet we declared in ToDoListContext.cs.//3. LINQ turns this DbSet into a list using the ToList() method, which comes from the System.Linq namespace.//4. This expression is what creates the model we'll use for the Index view.
    }

    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");//Creating a ViewBag.Category ID property as a Select List object ensures we are creating new items within CAtegories that already exist.A SelectList will provide a list of the data needed to create an html <select> list of all the categories from our database. The displayed text of each <option> will be the Category's Name property, and the value of the <option> will be the Category's CategoryId.
      //selectList arguments-- 1. the data that will populate <option> elements (list of categories from our DB) 2. the value of every <option> element (Category's CAtegoryId) 3. displayed text (name of CAtegory)
      return View();
    }

    [HttpPost]
    public ActionResult Create(Item item)//takes Item as argument
    {
      _db.Items.Add(item);//adds Item to the ItemsDbSet
      _db.SaveChanges();//save changes to database object called DB or _db
      return RedirectToAction("Index");//return Index view
    }//Add() is a method we run on our DBSet property of our DBContext, while SaveChanges() is a method we run on the DBContext itself.
    //Together, they update the DBSet and then sync those changes to the database which the DBContext represents.

    public ActionResult Details(int id)//matches "id" object that we created using ActionLink in Views/Items/Index
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);//LINQ method with a lambda-- start looking at _db.Items(our items table), then find any items where the ItemId matches our id argument
      return View(thisItem);
    }

    //GET action routes to form page for updating item
    public ActionResult Edit(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem); //finding specific item and passing it into view
    }

    [HttpPost] //POST actually updates item
    public ActionResult Edit(Item item)
    {
      _db.Entry(item).State = EntityState.Modified;//We find and update all of the properties of the item we are editing by passing the item (our route parameter) itself into the Entry() method. Then we need to update its State property to EntityState.Modified. This is so Entity knows that the entry has been modified, as it is not explicitly tracking it (we never actually retrieved the item from the database).
      _db.SaveChanges();//once entry's state is marked as modified we can save and then redirect to Index view
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }
    //POST action is named DeleteConfirmed instead of Delete since both GET + POST action methods take id as a parameter. C# will not allow us to have two methods with the same signature(method name and parameters). The POST attribute is not considered part of the method signature 

    [HttpPost, ActionName("Delete")] //Note that our annotation includes [ActionName("Delete")]. This is so we can still utilize the proper Delete action even though we've named our method DeleteConfirmed.
    public ActionResult DeleteConfirmed(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      _db.Items.Remove(thisItem);//build in Remove method
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    //*********************************
    // ALL THE BELOW CODE IS COVERED BY ENTITY!!
    // [HttpGet("/items")] //we have two /items routes but because one is HttpGet and the other HttpPost the server can distinguish
    // // public ActionResult Index() //Index pairs with Index.cshtml
    // // {
    // //   List<Item> allItems = Item.GetAll();
    // //   return View(allItems);
    // // } ojbects nested in parent objects do not need an index route

    // [HttpGet("/categories/{categoryId}/items/new")] //names the url path
    // public ActionResult New(int categoryId) //matches name of the cshtml doc
    // {
    //   Category category = Category.Find(categoryId);
    //   return View(category); //CreateForm.cshtml does all the work of creating the form here.. this method simply tells our browser to grab it and display it.
    // }

    // // [HttpPost("/items")] //names the url path AND matches our forms action. When the form with this action is submitted this route will be invoked.
    // // public ActionResult Create(string description) //description MATCHES the name attribute of our forms field that coorelates to this.
    // // {
    // //   Item myItem = new Item(description);
    // //   return RedirectToAction("Index");// redirectToAction(route) calls on Index route after create(callback routes)

    // //   //return View("Index", myItem); //"Index" specifies the Index.cshtml view since we are no longer routing to a view with the same name as our method (Create- in this case)
    // //   //myItem specifies Model property as called on in the Index view w/ @Model.Description 
    // // }

    // [HttpPost("/items/delete")]
    // public ActionResult DeleteAll()
    // {
    //   Item.ClearAll();
    //   return View();
    // }

    // [HttpGet("/categories/{categoryId}/items/{itemId}")]//curly brackets in route decorator is called Dynamic Routing-- it changes depending on user use. This particular dynamic route matches the method parameters
    // public ActionResult Show(int categoryId, int itemId)
    // {
    //   Item item = Item.Find(itemId);
    //   Category category = Category.Find(categoryId);
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   model.Add("item", item);
    //   model.Add("category", category);//we're using a dictionary to pass both items into the return View(), it cannot take two arguments.
    //   return View(model);
    // }
  }
}