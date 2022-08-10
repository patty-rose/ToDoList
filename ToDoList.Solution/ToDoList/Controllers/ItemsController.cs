using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;//to access SelectList
using Microsoft.EntityFrameworkCore;//to access EntityState
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;//to use LINQ's ToList() method

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {//Entity will search for .cshtml files in the "Items" view folder (based off htis class name)
    private readonly ToDoListContext _db;

    public ItemsController(ToDoListContext db)
    {
      _db = db;
      //this constructor allows us to set the value of our new _db property to our ToDoListContext. This is achievable due to a dependency injection we set up in our AddDbContext method in the ConfigureServices method in our Startup.cs file.
    }

    public ActionResult Index()//nstead of using a verbose GetAll() method with raw SQL, we can instead access all our Items in List form by doing the following: _db.Items.ToList()
    {
      return View(_db.Items.ToList());
    }

    [HttpPost, ActionName("Index")]
    public ActionResult SaveIsComplete(Item item)
    {
      _db.Entry(item).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

    //DEPRECATED CREATE POST
    // [HttpPost]
    // public ActionResult Create(Item item)
    // {
    //   _db.Items.Add(item);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    [HttpPost]
    public ActionResult Create(Item item, int CategoryId)
    {
      _db.Items.Add(item);//adds Item to the ItemsDbSet
      _db.SaveChanges();//save changes to database object called DB or _db
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });//
        _db.SaveChanges();
      }
      return RedirectToAction("Index");//return Index view
    }//Add() is a method we run on our DBSet property of our DBContext, while SaveChanges() is a method we run on the DBContext itself.
    //Together, they update the DBSet and then sync those changes to the database which the DBContext represents.

    public ActionResult Details(int id)//matches "id" object that we created using ActionLink in Views/Items/Index
    {
      var thisItem = _db.Items.Include(item => item.JoinEntities).ThenInclude(join => join.Category).FirstOrDefault(item => item.ItemId == id);
      //item is when the itemID = our id argument (LINQ Lambda)
      return View(thisItem);
    }//1. Our _db.Items expression gives us a list of Item objects from the database. However, if we completed the query now (using the FirstOrDefault() method), we'd simply have an Item without its related Categorys.2. We need to .Include(item => item.JoinEntities) to load the JoinEntities property of each Item. However, the JoinEntities property on an Item is just a collection of join entities, each of type ICollection<CategoryItem>. These are not the actual categories related to an Item.3.We need the actual Category objects themselves, so we use ThenInclude() method to load the Category of each CategoryItem. Remember that a CategoryItem is simply a reference to a relationship. Each CategoryItem includes the id of an Item as well as the id of a Category. We are actually returning the associated Category of a CategoryItem here.4.Finally, our FirstOrDefault() method specifies which item from the database we're working with.

    //GET action routes to form page for updating item
    public ActionResult Edit(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem); //finding specific item and passing it into view
    }

    [HttpPost]//POST actually updates item
    public ActionResult Edit(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.Entry(item).State = EntityState.Modified;//We find and update all of the properties of the item we are editing by passing the item (our route parameter) itself into the Entry() method. Then we need to update its State property to EntityState.Modified. This is so Entity knows that the entry has been modified, as it is not explicitly tracking it (we never actually retrieved the item from the database).
      _db.SaveChanges();//once entry's state is marked as modified we can save and then redirect to Index view
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult AddCategory(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
        _db.SaveChanges();
      }
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
      _db.Items.Remove(thisItem);//built in Remove method
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCategory(int joinId)
    {
      var joinEntry = _db.CategoryItem.FirstOrDefault(entry => entry.CategoryItemId == joinId);
      _db.CategoryItem.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }//This route will find the entry in the join table by using the join entry's CategoryItemId. The CategoryItemId is being passed in through the variable joinId in our route's parameter and came from the BeginForm() HTML helper method in our details view.
  }
}