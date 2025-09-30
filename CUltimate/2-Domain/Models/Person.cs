using System.ComponentModel.DataAnnotations;

namespace masTer._2_Domain.Models;

public abstract class Person
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(10, MinimumLength = 6, ErrorMessage = "El DNI debe tener entre 6 y 10 caracteres.")]
    public string Dni { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(18, 80, ErrorMessage = "La edad debe estar entre 18 y 80 años.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Formato de teléfono no válido.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    public string Email { get; set; } = string.Empty;
}