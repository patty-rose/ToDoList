using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Item
  {
    // auto implemented properties
    public string Description { get; set; }
    public int Id { get; } //called a readonly property if using get; but not set;
    private static List<Item> _instances = new List<Item>{};

    // constructor
    public Item(string description)
    {
      Description = description;
      _instances.Add(this); 
      Id = _instances.Count; // assigns ID as length of _instances
    }

    public static List<Item> GetAll()
    {
      return _instances;
    }

    public static void ClearAll()
    {
      _instances.Clear();
    }
  }
}