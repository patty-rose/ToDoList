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
      var thisCategory = _db.Categories
          .Include(category => category.JoinEntities)
          .ThenInclude(join => join.Item)
          .FirstOrDefault(category => category.CategoryId == id);
      return View(thisCategory);
    }
  }
}