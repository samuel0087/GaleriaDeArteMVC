using System.ComponentModel.DataAnnotations;

namespace galeria_arte_MVC.Models
{
    public class Exposicion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha de Finalizacion")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal {  get; set; }
        public List<Obra>? ObrasExpuestas { get; set; }
    }
}
