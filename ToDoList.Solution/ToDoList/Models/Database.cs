using System;
using MySql.Data.MySqlClient;//connects tp MySql Connector- imports from MySqlconnector package
using ToDoList;//connects us to the Startup.cs (same namespace) so we can access DBconfig class

namespace ToDoList.Models
{
  public class DB
  {
    public static MySqlConnection Connection()
    {
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);//this is the connection string in Startup.cs
      return conn;
    }
  }
}
//We can now call DB.Connection() from anywhere in our application to communicate with our database. Calling this method instantiates and returns a connection to our database named conn.