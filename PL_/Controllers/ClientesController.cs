using DevExpress.DataProcessing.InMemoryDataProcessor.GraphGenerator;
using ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Cliente
        public ActionResult GetAll()
        {
            ML.Cliente clientes = new ML.Cliente();
            ML.Result result = BL.Clientes.GetAllEF();
            if(result.Success)
            {
                clientes.Clientes = result.Objects;
                ML.Result resulta = BL.Cadenas.GetAllRol();
                clientes.Cadena = new ML.Cadena();
                clientes.Cadena.Cadenas = resulta.Objects;
            }
            else
            {
                ViewBag.Message = "Ocurrio un error ";
            }
            return View(clientes);
        }

        [HttpGet]
        public JsonResult GetAllDevExpress()
        {
            ML.Result result = BL.Clientes.GetAllEF();
            if (result.Success)
            {
                return Json(result.Objects, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<ML.Cliente>(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Add(ML.Cliente cliente)
        {

            if (!string.IsNullOrEmpty(cliente.ImagenBase64))
            {
                cliente.Imagen = Convert.FromBase64String(cliente.ImagenBase64);
            }
            ML.Result result = BL.Clientes.AddLINQ(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddDevExpress(string values)
        {
            var clienteNuevo = JsonConvert.DeserializeObject<ML.Cliente>(values);
            ML.Result result = BL.Clientes.AddEFs(clienteNuevo);
            if(result.Success)
            {
                return Json(clienteNuevo);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Update(ML.Cliente cliente)
        {

            if (!string.IsNullOrEmpty(cliente.ImagenBase64))
            {
                cliente.Imagen = Convert.FromBase64String(cliente.ImagenBase64);
            }

            ML.Result result = BL.Clientes.UpdateLINQ(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPut]
        public JsonResult UpdateDevExpress(string key, string values)
        {
            ML.Cliente clienteActualizado = JsonConvert.DeserializeObject<ML.Cliente>(values);
            clienteActualizado.IdCliente = int.Parse(key);
            ML.Result resultdatos = BL.Clientes.GetById(int.Parse(key));

            if (resultdatos != null )
            {
                if (!string.IsNullOrEmpty(clienteActualizado.Sucursal))
                {
                    resultdatos.Object = clienteActualizado.Sucursal;
                }

                if (clienteActualizado.Cadena.IdCadena > 0)
                {
                    resultdatos.Object = clienteActualizado.Cadena.IdCadena;
                }

                if (!string.IsNullOrEmpty(clienteActualizado.InicioContrato))
                {
                    resultdatos.Object = clienteActualizado.InicioContrato;
                }

                resultdatos.Object = clienteActualizado.Activo;

                if (clienteActualizado.No_Cliente > 0)
                {
                    resultdatos.Object = clienteActualizado.No_Cliente;
                }

                ML.Result result = BL.Clientes.UpdateEF(clienteActualizado);
                return Json(new { success = result.Success });
            }

            return Json(new { success = false, message = "Cliente no encontrado" });
        }



        [HttpPost]
        public JsonResult DeleteEF(int IdCliente)
        {
            ML.Result result = BL.Clientes.Delete(IdCliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteDevExpress(string key)
        {
            int id = int.Parse(key);
            ML.Result result = BL.Clientes.Delete(id);
            if (result.Success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = result.Exception?.Message ?? "Error al eliminar el cliente." });
            }
        }



        [HttpGet]
        public JsonResult GetClienteById(int idCliente)
        {
            ML.Result result = BL.Clientes.GetByILINQ(idCliente);
            ML.Cliente cliente = new ML.Cliente();
            cliente = (ML.Cliente)result.Object;
            if(cliente.Imagen != null)
            {
                cliente.ImagenBase64 = Convert.ToBase64String(cliente.Imagen);
            }

            result.Object = cliente;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        

        public byte[] ConvertirBase64(string Foto)
        {
            byte[] bytes = System.Convert.FromBase64String(Foto);
            return bytes;
        }

        [HttpPost]
        public JsonResult ChangeStatus(bool Activo, int IdCliente)
        {
            ML.Result result = BL.Clientes.ChangeStatus(Activo, IdCliente);
            return Json(result);
        }
    }
}