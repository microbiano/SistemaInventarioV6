using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _UnidadTrabajo;
        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _UnidadTrabajo = unidadTrabajo;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        { 
            Marca marca= new Marca();
            if (id == null)
            {
                marca.Estado = true;
                return View(marca);
            }

            marca = await _UnidadTrabajo.Marca.Obtener(id.GetValueOrDefault());

            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _UnidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa]="Marca creada exitosamente";
                }
                else 
                {
                     _UnidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Marca actualizada exitosamente";
                }

                await _UnidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error el registrar la categoria";
            return View(marca);
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _UnidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDB = await _UnidadTrabajo.Marca.Obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, message = "Error al Eliminar la categoria" });
            }

            _UnidadTrabajo.Marca.Remover(marcaDB);
            await _UnidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _UnidadTrabajo.Marca.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToUpper().Trim() == nombre.ToUpper().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToUpper().Trim() == nombre.ToUpper().Trim() && b.Id!=id);
            }

            if (valor)
            {
                return Json(new { data = true });
            }

            return Json(new { success = false });        
        }

        #endregion
    }
}

//Todo Video 44  Validar Categorias