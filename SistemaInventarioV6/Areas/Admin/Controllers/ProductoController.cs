using Humanizer;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Modelos.ViewModels;
using SistemaInventarioV6.Utilidades;
using System.Drawing;
using System.IO;
using System.IO.Pipes;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _UnidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(IUnidadTrabajo unidadTrabajo,IWebHostEnvironment webHostEnvironment)
        {
            _UnidadTrabajo = unidadTrabajo;
            _webHostEnvironment= webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM() {
                producto = new Producto(),
                CategoriaLista = _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Categoria"),
                MarcaLista = _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Marca"),
                PadreLista = _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Producto")
            };

            if (id == null)
            {
                productoVM.producto.Estado = true;
                return View(productoVM);
            }
            else 
            {
               productoVM.producto = await _UnidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.producto == null)
                {
                    return NotFound();
                }

                return View(productoVM);
            }            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.producto.Id == 0)
                {
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoVM.producto.ImagenUrl = fileName + extension;
                    await _UnidadTrabajo.Producto.Agregar(productoVM.producto);
                }
                else
                {
                    var objProducto = await _UnidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.producto.Id, isTracking: false);
                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //borrar imagen
                        var anteriorImagen = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorImagen))
                        {
                            System.IO.File.Delete(anteriorImagen);
                        }

                        //Creo Nueva Imagen
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.producto.ImagenUrl = fileName + extension;
                    }
                    else
                    {
                        productoVM.producto.ImagenUrl = productoVM.producto.ImagenUrl;
                    }

                    _UnidadTrabajo.Producto.Actualizar(productoVM.producto);
                }

                TempData[DS.Exitosa] = "Producto Guardado con exito";
                await _UnidadTrabajo.Guardar();
                return View("Index");
            }
            else
            {
                productoVM.CategoriaLista = _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Categoria");
                productoVM.MarcaLista= _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Marca");
                productoVM.PadreLista = _UnidadTrabajo.Producto.ObtenerTodosDropDownList("Producto");
                return View(productoVM);
            }            
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _UnidadTrabajo.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDB = await _UnidadTrabajo.Producto.Obtener(id);
            if (productoDB == null)
            {
                return Json(new { success = false, message = "Error al Eliminar el producto" });
            }

            //eliminar imagen Fisica
            string Upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var imagenEliminar = Path.Combine(Upload + productoDB.ImagenUrl);
            if (System.IO.File.Exists(imagenEliminar))
            { 
                System.IO.File.Delete(imagenEliminar);
            }

            _UnidadTrabajo.Producto.Remover(productoDB);
            await _UnidadTrabajo.Guardar();
            return Json(new { success = true, message = "producto borrada exitosamente" });
        }

        [ActionName("ValidarSerie")]
        public async Task<IActionResult> Validarserie(string serie, int id = 0)
        {
            bool valor = false;
            var lista = await _UnidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.ToUpper().Trim() == serie.ToUpper().Trim());
            }
            else
            {
                valor = lista.Any(b => b.NumeroSerie.ToUpper().Trim() == serie.ToUpper().Trim() && b.Id!=id);
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

