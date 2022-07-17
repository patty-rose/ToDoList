using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Item
  {
    // auto implemented properties
    public string Description { get; set; }
    public int Id { get; } //called a readonly property if using get; but not set;

    // constructor
    public Item(string description)
    {
      Description = description;
      //removing _instances in exchange for database
      // _instances.Add(this); 
      // Id = _instances.Count; // assigns ID as length of _instances
    }

    //overloaded constructor has multiple constructors so our app can instantiate Item in multiple ways (here there are two ways with two different parameter specifications vvv & ^^^)
    public Item(string description, int id)
    {
        Description = description;
        Id = id;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> { };

        //1. open MySqlConnection
        MySqlConnection conn = DB.Connection();
        conn.Open();

        //2. define a MySqlCommand - needs a ExecuteReader() (or similar method) for retrieving data (locating and returning entries)
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = "SELECT * FROM items;"; //constructs SQL query
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

        //3. create while statement to specify how to pull info from many rows/columns in our DB
        while (rdr.Read()) //rdr is reading results from the DB and then advancing until end(or until rdr.Read() returns false).
        {
            int itemId = rdr.GetInt32(0);
            string itemDescription = rdr.GetString(1);
            Item newItem = new Item(itemDescription, itemId);
            allItems.Add(newItem);
        }//while loop takes each item (a row in the table represented as an array(columns are an index point(0 is Id))) the rdr reads and translates it into in Item object our app understands. Each parameter the constructor calls for needs to be accounted for in this loop.

        //4. close connection
        conn.Close(); //Communicating with a DB is a resource-intensive process so we must close our DB connection when we're done.
        if (conn != null)
        {
            conn.Dispose();
        }
        return allItems;
    }

    public static void ClearAll()
    {
      //1. open a MySqlConnection
      MySqlConnection conn = DB.Connection();
      conn.Open(); 
      //create a MySqlConnection object called conn by calling on the Connection method from the DB class created in this project(Models/Database.cs) that connection method calls on the connection string stated in the Startup.cs file. 

      //2. define a MySqlCommand (don't need a reader object (rdr) when we don't need anything returned, we just want to remotely modify (adding, deleting, or updating entries))
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "DELETE FROM items;";
      //define the CommandText property of cmd as the SQL statement "DELETE FROM items;", which will clear the entire items table in our database

      cmd.ExecuteNonQuery();
      //SQL statements that modify data instead of querying and returning data are executed with the ExecuteNonQuery() method

      //3. Close connection
      conn.Close();
      if (conn != null) //error-handling-- force dispose of conn.Close() was unsuccessful
      {
        conn.Dispose();
      }
    }

    public static Item Find(int searchId) //static because it needs to sift through ALL Items
    {
      Item placeholderItem = new Item("placeholder item");
    return placeholderItem;
    }
  }
}