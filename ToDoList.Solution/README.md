Since our appsettings.json has been put in our .gitignore, users will not have access to it when cloning our project. So, you will need to add specific instructions in your README, telling the user where to create the file, and what code to include in it -- obviously, it would be against the point of ignoring it, if you included the username and password in these instructions, so make sure that's formatted in the same way as we've shown it above, such as ...database=to_do_list;uid=[YOUR-USERNAME-HERE];pwd=[YOUR-PASSWORD-HERE]...

ADD these packages for entity framework
$ dotnet add package Microsoft.EntityFrameworkCore -v 5.0.0
$ dotnet add package Pomelo.EntityFrameworkCore.MySql -v 5.0.0-alpha.2