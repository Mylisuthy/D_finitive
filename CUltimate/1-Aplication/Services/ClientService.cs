using masTer._2_Domain.Models;
using masTer.Data;
using masTer.Helpers;
using masTer.Services;

namespace masTer._1_Aplication.Services;

public class ClientService : GenericMet<Client>
{
    public ClientService(AppDbContext context) : base(context) { }

    // Registrar cliente
    public void RegisterMenu()
    {
        Console.WriteLine("------Registrar Clientes------");

        var client = new Client
        {
            Dni = ValidationHelper.ReadInput("Ingrese el DNI: "),
            Name = ValidationHelper.ReadInput("Ingrese el nombre del cliente: "),
            Age = ValidationHelper.ReadInt("Ingrese la edad del cliente: ", 18, 80),
            PhoneNumber = ValidationHelper.ReadInput("Ingrese el número del celular: "),
            Email = ValidationHelper.ReadInput("Ingrese el correo electrónico: "),
            Insurance = ValidationHelper.ReadInput("¿Tiene seguro? (Si/No): ").ToLower() == "si",
        };

        if (client.Insurance)
        {
            client.InsuranceType = ValidationHelper.ReadInput("Ingrese el tipo de seguro: ");
        }

        if (ValidationHelper.ValidateEntity(client))
        {
            RegisterAsync(client);
            Console.WriteLine($"El cliente {client.Name} se ha registrado exitosamente.");
        }
        else
        {
            Console.WriteLine("Registro cancelado. Corrija los errores e intente nuevamente.");
        }
    }

    // Listar clientes
    public async Task ListarMenu()
    {
        Console.WriteLine("-----Lista de clientes-----");
        Console.WriteLine("ID\tDNI\tNombre\tEdad\tTeléfono\tCorreo\t\t\tSeguro");
        
        var cliens = await ListAllAsync();
        
        foreach (var c in cliens)
        {
            Console.WriteLine($"{c.Id}\t{c.Dni}\t{c.Name}\t{c.Age}\t{c.PhoneNumber}\t{c.Email}\t{(c.Insurance ? "Sí" : "No")}");
        }
    }

    // Eliminar cliente
    public void EliminarMenu()
    {
        ListarMenu();
        int idEliminar = ValidationHelper.ReadInt("\nIngrese el ID del cliente que desea eliminar: ");

        DeleteAsync(idEliminar);
        Console.WriteLine($"Cliente con ID {idEliminar} eliminado correctamente.");
    }

    // Editar cliente
    public void EditarMenu()
    {
        Console.WriteLine("\n----- Editar Cliente -----");
        int idEditar = ValidationHelper.ReadInt("Ingrese el ID del cliente a editar: ");

        var cliente = GetByIdAsync(idEditar).Result; // Usamos .Result porque es Task

        if (cliente == null)
        {
            Console.WriteLine("No se encontró el cliente con ese ID.");
            return;
        }

        Console.WriteLine($"Editando cliente: {cliente.Name}");

        string nuevoNombre = ValidationHelper.ReadInput("Nuevo nombre (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoNombre)) cliente.Name = nuevoNombre;

        string nuevaEdad = ValidationHelper.ReadInput("Nueva edad (Enter para dejar igual): ", true);
        if (int.TryParse(nuevaEdad, out int edad)) cliente.Age = edad;

        string nuevoTelefono = ValidationHelper.ReadInput("Nuevo teléfono (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoTelefono)) cliente.PhoneNumber = nuevoTelefono;

        string nuevoCorreo = ValidationHelper.ReadInput("Nuevo correo (Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoCorreo)) cliente.Email = nuevoCorreo;

        string nuevoSeguro = ValidationHelper.ReadInput("¿Tiene seguro? (Si/No, Enter para dejar igual): ", true);
        if (!string.IsNullOrWhiteSpace(nuevoSeguro))
            cliente.Insurance = nuevoSeguro.ToLower() == "si";

        if (cliente.Insurance)
        {
            string nuevoTipoSeguro = ValidationHelper.ReadInput("Nuevo tipo de seguro (Enter para dejar igual): ", true);
            if (!string.IsNullOrWhiteSpace(nuevoTipoSeguro))
                cliente.InsuranceType = nuevoTipoSeguro;
        }

        if (ValidationHelper.ValidateEntity(cliente))
        {
            EditAsync(cliente);
            Console.WriteLine("Cliente actualizado correctamente.");
        }
    }
}

