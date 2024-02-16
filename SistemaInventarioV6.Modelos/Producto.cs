using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} Rquerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        [MaxLength(60)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        public double Costo { get; set; }

        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "{0} Rquerido")]
        public int MarcaId { get; set; }

        public int? PadreId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        public virtual Producto Padre { get; set; }


    }
}
