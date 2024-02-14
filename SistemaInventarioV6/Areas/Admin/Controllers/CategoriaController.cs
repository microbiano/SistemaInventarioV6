using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly IUnidadTrabajo _UnidadTrabajo;
        public CategoriaController(IUnidadTrabajo unidadTrabajo)
        {
            _UnidadTrabajo = unidadTrabajo;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        { 
            Categoria categoria= new Categoria();
            if (id == null)
            {
                categoria.Estado = true;
                return View(categoria);
            }
            
            categoria = await _UnidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.Id == 0)
                {
                    await _UnidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa]="categoria creada exitosamente";
                }
                else 
                {
                     _UnidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "categoria actualizada exitosamente";
                }

                await _UnidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error el registrar la categoria";
            return View(categoria);
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _UnidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categotiaDB = await _UnidadTrabajo.Categoria.Obtener(id);
            if (categotiaDB == null)
            {
                return Json(new { success = false, message = "Error al Eliminar la categoria" });
            }

            _UnidadTrabajo.Categoria.Remover(categotiaDB);
            await _UnidadTrabajo.Guardar();
            return Json(new { success = true, message = "categoria borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _UnidadTrabajo.Categoria.ObtenerTodos();
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