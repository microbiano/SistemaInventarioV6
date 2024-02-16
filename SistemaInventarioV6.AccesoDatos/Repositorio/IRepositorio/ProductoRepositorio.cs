using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepositorio(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            var productoDB= _db.Productos.FirstOrDefault(b=>b.Id== producto.Id);
            if (productoDB != null)
            {
                if (producto != null)
                {
                    productoDB.ImagenUrl = producto.ImagenUrl;
                }

                productoDB.NumeroSerie = producto.NumeroSerie;
                productoDB.Descripcion = producto.Descripcion;
                productoDB.Precio = producto.Precio;
                productoDB.Costo = producto.Costo;
                productoDB.Estado = producto.Estado;
                productoDB.CategoriaId = producto.CategoriaId;
                productoDB.MarcaId = producto.MarcaId;
                productoDB.PadreId = producto.PadreId;

                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == "Categoria")
            {
                return _db.Categorias.Where(x => x.Estado == true).Select(x => new SelectListItem
                {
                    Text = x.Descripcion,
                    Value = x.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(x => x.Estado == true).Select(x => new SelectListItem
                {
                    Text = x.Descripcion,
                    Value = x.Id.ToString()
                });
            }

            if (obj == "Producto")
            {
                return _db.Productos.Where(x => x.Estado == true).Select(x => new SelectListItem
                {
                    Text = x.Descripcion,
                    Value = x.Id.ToString()
                });
            }

            return null;
        }
    }
}
