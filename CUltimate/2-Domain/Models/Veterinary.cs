using System.ComponentModel.DataAnnotations;

namespace masTer._2_Domain.Models;

public class Veterinary : Person
{
    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [MaxLength(100, ErrorMessage = "La especialidad no puede superar los 100 caracteres.")]
    public string Speciality { get; set; } = string.Empty;
}