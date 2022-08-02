using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;//to call UseMySql
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Models; //to call <ToDoListContext>

namespace ToDoList
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json");//this line replaces .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddEntityFrameworkMySql()//adding a form of entity that understands MySQL - addDbContext is a representation of our DB that we are calling ToDoListContext in the carrots below- options set it to use our default connection 
        .AddDbContext<ToDoListContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();
      app.UseRouting();
      app.UseStaticFiles(); //use static files like images and CSS

      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });

      //The template option tells .NET how to treat routes. This configuration, known as conventional routing, matches a path like /Items/Details/6 to its specific controller action by looking for the Details action route in the Items controller. Then it binds the value of id to 6. We won't change routes in this class, but be aware that .NET routing conventions can be configured.

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("Something has gone wrong");
      });
    }
  }

  //best-practice is to store connection string in appsettings.json due to Entity configuration
  // public static class DBConfiguration
  // {
  //   public static string ConnectionString = "server=localhost;user id=root;password=epicodus;port=3306;database=to_do_list;";
  //   //server-ids db server-- localhost cus it's on our machine, not online
  //   //user id -ids db user
  //   //password because our db doesn't hold sensitive info
  //   //port iids the port MySql is running on-- default MySql server is 3306
  //   //database= database name
  // }
}