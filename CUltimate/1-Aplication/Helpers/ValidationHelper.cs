using System.ComponentModel.DataAnnotations;

namespace masTer.Helpers;

public static class ValidationHelper
{
    // Valida cualquier entidad usando DataAnnotations
    public static bool ValidateEntity<T>(T entity)
    {
        var context = new ValidationContext(entity);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(entity, context, results, true);

        if (!isValid)
        {
            Console.WriteLine("\n--- Errores de validación ---");
            foreach (var error in results)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            Console.WriteLine("-----------------------------\n");
        }

        return isValid;
    }

    // Entrada segura desde consola
    public static string ReadInput(string message, bool allowEmpty = false)
    {
        string input;
        do
        {
            Console.Write(message);
            input = Console.ReadLine() ?? "";

            if (!allowEmpty && string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("El valor no puede estar vacío. Inténtelo nuevamente.");
            }
        } while (!allowEmpty && string.IsNullOrWhiteSpace(input));

        return input;
    }

    // Leer un número entero con validación
    public static int ReadInt(string message, int min = int.MinValue, int max = int.MaxValue)
    {
        int value;
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out value))
            {
                if (value >= min && value <= max)
                    return value;
                else
                    Console.WriteLine($"El número debe estar entre {min} y {max}.");
            }
            else
            {
                Console.WriteLine("Ingrese un número válido.");
            }
        }
    }
}