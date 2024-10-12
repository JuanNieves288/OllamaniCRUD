using ML;
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
            ML.Result result = BL.Clientes.GetAllLINQ();
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


        [HttpPost]
        public JsonResult Add(ML.Cliente cliente)
        {

            if (!string.IsNullOrEmpty(cliente.ImagenBase64))
            {
                cliente.Imagen = Convert.FromBase64String(cliente.ImagenBase64);
            }
            ML.Result result = BL.Clientes.Add(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(ML.Cliente cliente)
        {

            if (!string.IsNullOrEmpty(cliente.ImagenBase64))
            {
                cliente.Imagen = Convert.FromBase64String(cliente.ImagenBase64);
            }

            ML.Result result = BL.Clientes.Update(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetClienteById(int idCliente)
        {
            ML.Result result = BL.Clientes.GetById(idCliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteEF(int IdCliente)
        {
            ML.Result result = BL.Clientes.DeleteEF(IdCliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public byte[] ConvertirBase64(string Foto)
        {
            byte[] bytes = System.Convert.FromBase64String(Foto);
            return bytes;
        }

        //[HttpPost]
        //public JsonResult ChangeStatus(bool Activo, int IdCliente)
        //{
        //    ML.Result result = BL.Cadenas.ClienteChangeStatus(Activo, IdCliente);
        //    return Json(result);
        //}
    }
}