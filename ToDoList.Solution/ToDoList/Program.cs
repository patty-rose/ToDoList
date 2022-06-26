using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Models
{
  public class Program
  {
    static void Main()
    {
      Console.WriteLine("Welcome to the To Do List");
      Console.WriteLine("Please add your first item!");
      AddTask();
    }

    static void PromptUser()
    {
      Console.WriteLine("Would you like to add an item to your list or view your list? (Add/View List)");
      string userResponse = Console.ReadLine();
      if (userResponse.ToLower() == "add")
      {
        AddTask();
      } else if (userResponse.ToLower() == "view list")
      {
        ViewTasks();
      } else 
      {
        Console.WriteLine("Please enter \"Add\" or \"View List\"");
        Main();
      }
    }

    static void AddTask()
    {
        Console.WriteLine("Please enter the description for the new item.");
        string newTask = Console.ReadLine();
        Item newItem = new Item(newTask);
        Console.WriteLine(newTask + " has been added to you list");
        PromptUser();        
    }

    static void ViewTasks()
    {
      List<Item> result = Item.GetAll();
      int listLength = result.Count;
      if (listLength != 0) 
      {
        Console.WriteLine("Here is your To Do List: ");
        foreach (Item thisItem in result)
        {
          Console.WriteLine(thisItem.Description);
        }
      } else 
      {
        Console.WriteLine("Your To Do List is empty! Please add your first item!");
        AddTask();
      }
    }
  }
}

