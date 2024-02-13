using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        private readonly IUnidadTrabajo _UnidadTrabajo;
        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _UnidadTrabajo = unidadTrabajo;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        { 
            Bodega bodega= new Bodega();
            if (id == null)
            {
                bodega.Estado = true;
                return View(bodega);
            }
            
            bodega = await _UnidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());

            if (bodega == null)
            {
                return NotFound();
            }

            return View(bodega);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    await _UnidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa]="Bodega creada exitosamente";
                }
                else 
                {
                     _UnidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega actualizada exitosamente";
                }

                await _UnidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error el registrar la bodega";
            return View(bodega);
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _UnidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDB = await _UnidadTrabajo.Bodega.Obtener(id);
            if (bodegaDB == null)
            {
                return Json(new { success = false, message = "Error al Eliminar la Bodega" });
            }

            _UnidadTrabajo.Bodega.Remover(bodegaDB);
            await _UnidadTrabajo.Guardar();
            return Json(new { success = true, message = "bodega borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _UnidadTrabajo.Bodega.ObtenerTodos();
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

//Todo Video 42  Validar nombre