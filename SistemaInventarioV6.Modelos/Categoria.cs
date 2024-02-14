using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(60, ErrorMessage = "60 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(100, ErrorMessage = "{0} Caracteres")]
        public string Descripcion { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}
