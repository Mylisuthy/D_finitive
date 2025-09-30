using System.ComponentModel.DataAnnotations;

namespace masTer._2_Domain.Models;

public class Client : Person
{
    [Required(ErrorMessage = "Debe indicar si tiene seguro.")]
    public bool Insurance { get; set; }

    [MaxLength(50, ErrorMessage = "El tipo de seguro no puede superar los 50 caracteres.")]
    public string? InsuranceType { get; set; }
}