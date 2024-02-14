using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public MarcaRepositorio(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Actualizar(Marca marca)
        {
            var MarcaDB= _db.Marcas.FirstOrDefault(b=>b.Id== marca.Id);
            if (MarcaDB != null)
            {
                MarcaDB.Nombre=marca.Nombre;
                MarcaDB.Descripcion= marca.Descripcion;
                MarcaDB.Estado= marca.Estado;
                _db.SaveChanges();
            }
        }
    }
}
