using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;//to access SelectList
using Microsoft.EntityFrameworkCore;//to access EntityState
using ToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;//to use LINQ's ToList() method

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    private readonly ToDoListContext _db;

    public CategoriesController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Category> model = _db.Categories.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
      _db.Categories.Add(category);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }

    // //GET action routes to form page for updating item
    // public ActionResult Edit(int id)
    // {
    //   Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
    //   ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    //   return View(thisItem); //finding specific item and passing it into view
    // }

    // [HttpPost] //POST actually updates item
    // public ActionResult Edit(Item item)
    // {
    //   _db.Entry(item).State = EntityState.Modified;//We find and update all of the properties of the item we are editing by passing the item (our route parameter) itself into the Entry() method. Then we need to update its State property to EntityState.Modified. This is so Entity knows that the entry has been modified, as it is not explicitly tracking it (we never actually retrieved the item from the database).
    //   _db.SaveChanges();//once entry's state is marked as modified we can save and then redirect to Index view
    //   return RedirectToAction("Index");
    // }

    // public ActionResult Delete(int id)
    // {
    //   var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
    //   return View(thisItem);
    // }
    // //POST action is named DeleteConfirmed instead of Delete since both GET + POST action methods take id as a parameter. C# will not allow us to have two methods with the same signature(method name and parameters). The POST attribute is not considered part of the method signature 

    // [HttpPost, ActionName("Delete")] //Note that our annotation includes [ActionName("Delete")]. This is so we can still utilize the proper Delete action even though we've named our method DeleteConfirmed.
    // public ActionResult DeleteConfirmed(int id)
    // {
    //   var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
    //   _db.Items.Remove(thisItem);//build in Remove method
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }
    //**************** code replaced by entity VVVVV
    // [HttpGet("/categories")]
    // public ActionResult Index()
    // {
    //   List<Category> allCategories = Category.GetAll();
    //   return View(allCategories);
    // }

    // [HttpGet("/categories/new")]
    // public ActionResult New()
    // {
    //   return View();
    // }

    // [HttpPost("/categories")]
    // public ActionResult Create(string categoryName)//Within the route, we create a new Category with this name and then call the RedirectToAction method with "Index" passed in as the argument to send the user back to the Index route.
    // {
    //   Category newCategory = new Category(categoryName);
    //   return RedirectToAction("Index");
    // }

    // [HttpGet("/categories/{id}")]
    // public ActionResult Show(int id)//we must pass two types of objects to the view. However, View() can only accept one model argument. To work around this, we do the following:
    // {
    //   Dictionary<string, object> model = new Dictionary<string, object>();//Create a new Dictionary called model because a Dictionary can hold multiple key-value pairs.
    //   Category selectedCategory = Category.Find(id);
    //   List<Item> categoryItems = selectedCategory.Items;
    //   model.Add("category", selectedCategory);
    //   model.Add("items", categoryItems);//Add both the Category and its associated Items to this Dictionary. These will be stored with the keys "category" and "items".
    //   return View(model);
    // }

    // [HttpPost("/categories/{categoryId}/items")]
    // public ActionResult Create(int categoryId, string itemDescription)
    // {
    //   Dictionary<string, object> model = new Dictionary<string, object>();

    //   Category foundCategory = Category.Find(categoryId);

    //   Item newItem = new Item(itemDescription);
    //   newItem.Save();

    //   foundCategory.AddItem(newItem);

    //   List<Item> categoryItems = foundCategory.Items;

    //   model.Add("items", categoryItems);

    //   model.Add("category", foundCategory);

    //   return View("Show", model);//Finally, we pass in our model data to View(), instructing it to render the Category detail page, which is the Show.cshtml view. Category detail page requires that this function passes out the category and all items included in it.
    // }
  }
}