using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class UsuarioAplicacion: IdentityUser
    {
        [Required(ErrorMessage ="Requerido")]
        [MaxLength(80)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(2000)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(60)]
        public string Pais { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
