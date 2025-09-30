using System.ComponentModel.DataAnnotations.Schema;

namespace masTer._2_Domain.Models;

public class Pet : Animal
{
    public int IdPerson { get; set; }
    [ForeignKey("IdPerson")]
    public Client client { get; set; }
}