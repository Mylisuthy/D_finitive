using masTer._2_Domain.Models;
using masTer.Data;
using masTer.Helpers;
using masTer.Services;

namespace masTer._1_Aplication.Services;

public class VeterinaryService : GenericMet<Veterinary>
{
    public VeterinaryService(AppDbContext context) : base(context) { }

    // Registrar veterinario
    public void RegisterMenu()
    {
        Console.WriteLine("------Registrar Veterinario------");

        var vet = new Veterinary
        {
            Dni = ValidationHelper.ReadInput("Ingrese el DNI: "),
            Name = ValidationHelper.ReadInput("Ingrese el nombre del veterinario: "),
            Age = ValidationHelper.ReadInt("Ingrese la edad del veterinario: ", 18, 80),
            PhoneNumber = ValidationHelper.ReadInput("Ingrese el número del celular: "),
            Email = ValidationHelper.ReadInput("Ingrese el correo electrónico: "),
            Speciality = ValidationHelper.ReadInput("Ingrese la especialidad: ")
        };

        if (ValidationHelper.ValidateEntity(vet))
        {
            RegisterAsync(vet);
            Console.WriteLine($"El veterinario {vet.Name} se ha registrado exitosamente.");
        }
        else
        {
            Console.WriteLine("Registro cancelado. Corrija los errores e intente nuevamente.");
        }
    }

    // Listar veterinarios
    public async Task ListarMenu()
    {
        Console.WriteLine("-----Lista de veterinarios-----");
        Console.WriteLine("ID\tDNI\tNombre\tEdad\tTeléfono\tCorreo\t\t\tEspecialidad");
        
        var Veterinaries = await ListAllAsync();
            
        foreach (var v in Veterinaries)
        {
            Console.WriteLine($"{v.Id}\t{v.Dni}\t{v.Name}\t{v.Age}\t{v.PhoneNumber}\t{v.Email}\t{v.Speciality}");
        }
    }

    // Eliminar veterinario
    public void EliminarMenu()
    {
        ListarMenu();
        int idEliminar = ValidationHelper.ReadInt("\nIngrese el ID del veterinario que desea eliminar: ");
        DeleteAsync(idEliminar);
        Console.WriteLine($"Veterinario con ID {idEliminar} eliminado correctamente.");
    }

    // Editar veterinario
    public void EditarMenu()
    {
        Console.WriteLine("\n----- Editar Veterinario -----");
        int idEditar = ValidationHelper.ReadInt("Ingrese el ID del veterinario a editar: ");

        var vet = GetByIdAsync(idEditar).Result;

        if (vet == null)
        {
            Console.WriteLine("No se encontró el veterinario con ese ID.");
            return;
        }

        Console.WriteLine($"Editando veterinario: {vet.Name}");

        string nuevoNombre = ValidationHelper.ReadInput("Nuevo nombre (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoNombre)) vet.Name = nuevoNombre;

        string nuevaEdad = ValidationHelper.ReadInput("Nueva edad (Enter para dejar igual): ", true);
        if (int.TryParse(nuevaEdad, out int edad)) vet.Age = edad;

        string nuevoTelefono = ValidationHelper.ReadInput("Nuevo teléfono (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoTelefono)) vet.PhoneNumber = nuevoTelefono;

        string nuevoCorreo = ValidationHelper.ReadInput("Nuevo correo (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoCorreo)) vet.Email = nuevoCorreo;

        string nuevaEspecialidad = ValidationHelper.ReadInput("Nueva especialidad (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevaEspecialidad)) vet.Speciality = nuevaEspecialidad;

        if (ValidationHelper.ValidateEntity(vet))
        {
            EditAsync(vet);
            Console.WriteLine("Veterinario actualizado correctamente.");
        }
    }
}
