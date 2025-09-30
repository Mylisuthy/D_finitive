# Hi, welcome's to my veterinary Project

First, I need to clarify this; to understand the code in the project,
you need to know concepts about C# such as basic logic (data types, operators,
functions, conditionals), and other concepts like SQL, LINQ, OOP, interfaces,
and many more concepts. I recommend you explore the beautiful world of C#.

## No more distractions let's start with the sauce.

Ok'a ma brod@h, start whit the proces to Installed the dependences,
you can use that comands to install EF Core, Design, and pomelo.
```shell
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Pomelo.EntityFrameworkCore.MySql
```

When the installation is complete, you need to navigate the project structure
and find the file named "AppDbContext" in the Infrastructure folder. Then,
modify the code with your MySQL configuration.

*you will find something similar to this*
```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseMySql(
            "server=localhost;" +
            "database=Define;" +
            "user=YourUser;" +
            "password=YourPassword",
            new MySqlServerVersion(new Version(8, 0, 36))
        );
    }
}

// Additional information: you must enter your MySQL credentials and have it running.
```

## Checks

Now you can use these commands to check and initialize the program and create the database tables.

```shell
# Install dependencies if you have already initialized the program.
dotnet tool install --global dotnet-ef
# start creating the database
dotnet ef migrations add InitialCreate
# update the changes
dotnet ef database update
```

## 