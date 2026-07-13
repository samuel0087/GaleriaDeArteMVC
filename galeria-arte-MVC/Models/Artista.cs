using System.ComponentModel.DataAnnotations;

namespace galeria_arte_MVC.Models
{
    public class Artista
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        public string Pais { get; set; }
    }
}
