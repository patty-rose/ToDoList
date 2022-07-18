using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)//Within the route, we create a new Category with this name and then call the RedirectToAction method with "Index" passed in as the argument to send the user back to the Index route.
    {
      Category newCategory = new Category(categoryName);
      return RedirectToAction("Index");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)//we must pass two types of objects to the view. However, View() can only accept one model argument. To work around this, we do the following:
    {
      Dictionary<string, object> model = new Dictionary<string, object>();//Create a new Dictionary called model because a Dictionary can hold multiple key-value pairs.
      Category selectedCategory = Category.Find(id);
      List<Item> categoryItems = selectedCategory.Items;
      model.Add("category", selectedCategory);
      model.Add("items", categoryItems);//Add both the Category and its associated Items to this Dictionary. These will be stored with the keys "category" and "items".
      return View(model);
    }

    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemDescription)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();

      Category foundCategory = Category.Find(categoryId);

      Item newItem = new Item(itemDescription);
      newItem.Save();

      foundCategory.AddItem(newItem);

      List<Item> categoryItems = foundCategory.Items;

      model.Add("items", categoryItems);

      model.Add("category", foundCategory);

      return View("Show", model);//Finally, we pass in our model data to View(), instructing it to render the Category detail page, which is the Show.cshtml view. Category detail page requires that this function passes out the category and all items included in it.
    }
  }
}