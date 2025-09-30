using masTer._1_Aplication.Veaws;
using masTer.Data;

namespace masTer;

public class Program
{
    static void Main(string[] args)
    {
        var context = new AppDbContext();
        var principalMenu = new PrincipalMenu(context);
        principalMenu.MostrarMenu();
    }
}