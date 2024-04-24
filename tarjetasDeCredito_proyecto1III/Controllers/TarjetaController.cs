using Microsoft.AspNetCore.Mvc;
using tarjetasDeCredito_proyecto1III.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Controllers
{

    [ApiController]
    [Route("tarjeta")]
    public class TarjetaController : ControllerBase
    {
        clsArbolBinarioBusqueda arbol = new clsArbolBinarioBusqueda();
        Queue<clsTarjetaRetiro> queColaMonto = new Queue<clsTarjetaRetiro>();

        private readonly IWebHostEnvironment _hostingEnvironment;

        public TarjetaController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route ("listar")]
        public dynamic apiListarTarjeta(string strNombreTarjeta)
        {
            var resultado = fncListarTarjetas(strNombreTarjeta);
            
            if (!resultado[0].nombreTarjeta.Equals("Not found") && fncCheckToken())                 
            {

                return resultado;

            }
            else
            {
                return "Access Denied or not found + "+ resultado[0].nombreTarjeta;
            }    

        }

        [HttpGet]
        [Route("listar/saldo")]
        public dynamic apiListarSaldo(string strNombreTarjeta)
        {
            var resultado = fncListarSaldos(strNombreTarjeta);

            if (!resultado.First.Value.nombreTarjeta.Equals("Not found") && fncCheckToken())
            {

                List<string> saldo = new List<string>();
                foreach (var item in resultado)
                {
                    saldo.Add($"{item.banco} - {item.saldo} - {item.tipo}");
                }

                return saldo;

            }
            else
            {
                return "Access Denied or not found";
            }

        }

       
        [HttpPost]
        [Route("actualizar_saldo")]
        public dynamic apiActualizarSaldo(string strNumeroTarjeta, decimal montoRetiro)
        {

            queColaMonto.Enqueue(new clsTarjetaRetiro(fncListarTarjetasNumTarjeta(strNumeroTarjeta).First(), montoRetiro));
            return retirar(queColaMonto);
            
        }

        [HttpGet]
        [Route("estado_cuenta")]
        public dynamic apiEstadoCuenta(string strNumTarjeta)
        {
            return fncListarTarjetasNumTarjetaEstadoCuenta(strNumTarjeta);
        }


        private string retirar(Queue<clsTarjetaRetiro> tarjeta)
        {

            clsTarjetaRetiro a = tarjeta.Dequeue();

            try
            {

                ActualizarSaldoEnJson(a.tarjetaAretirar.numTarjeta, a.dclMontoARetirar);
                return "Saldo actualizado exitosamente.";

            }
            catch (Exception ex)
            {
                return $"Error al actualizar el saldo: {ex.Message}";
            }

        }

        private void ActualizarSaldoEnJson(string strNumeroTarjeta, decimal montoRetiro)
        {
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", nombreArchivo);

            
            string json = System.IO.File.ReadAllText(rutaArchivo);
            List<clsTarjeta> tarjetas = JsonConvert.DeserializeObject<List<clsTarjeta>>(json);

            // Buscar la tarjeta cuyo saldo se va a actualizar
            var tarjeta = tarjetas.FirstOrDefault(t => t.numTarjeta.ToUpper() == strNumeroTarjeta.ToUpper());
            if (tarjeta != null)
            {
                // Actualizar el saldo de la tarjeta
                tarjeta.saldo = (decimal.Parse(tarjeta.saldo) - montoRetiro).ToString();

                // Serializar la lista de objetos de vuelta a JSON
                string nuevoJson = JsonConvert.SerializeObject(tarjetas, Formatting.Indented);

                // Escribir el JSON actualizado de vuelta al archivo
                System.IO.File.WriteAllText(rutaArchivo, nuevoJson);
            }
            else
            {
                throw new Exception("No se encontró la tarjeta especificada.");
            }
        }



        private List<clsTarjeta> fncListarTarjetas(string strNombreTarjeta)
        {
            clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found");
            List<clsTarjeta> tarjetas = new List<clsTarjeta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", 
                nombreArchivo);
            try
            {
                string json = System.IO.File.ReadAllText(rutaArchivo);
                var tarjetasJson = System.Text.Json.JsonSerializer.Deserialize<clsTarjeta[]>(json);
                tarjetas.AddRange(tarjetasJson.Where(tarjeta =>
                tarjeta.nombreTarjeta.ToUpper().Equals(strNombreTarjeta.ToUpper())));
                if (tarjetas.Count == 0)               
                    return new List<clsTarjeta> { notFound };
                return tarjetas;
                
            }
            catch (Exception ex)
            {
                notFound.nombreTarjeta = ex.Message;
                
                return new List<clsTarjeta> { notFound };

            }            
            
        }

        private List<clsTarjeta> fncListarTarjetasNumTarjeta(string strNumeroTarjeta)
        {
            clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found");
            List<clsTarjeta> tarjetas = new List<clsTarjeta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                string json = System.IO.File.ReadAllText(rutaArchivo);
                var tarjetasJson = System.Text.Json.JsonSerializer.Deserialize<clsTarjeta[]>(json);
                tarjetas.AddRange(tarjetasJson.Where(tarjeta =>
                tarjeta.numTarjeta.ToUpper().Equals(strNumeroTarjeta.ToUpper())));
                if (tarjetas.Count == 0)
                    return new List<clsTarjeta> { notFound };
                return tarjetas;

            }
            catch (Exception ex)
            {
                notFound.numTarjeta = ex.Message;
                return new List<clsTarjeta> { notFound };

            }

        }

        private List<clsTarjetaEstadoCuenta> fncListarTarjetasNumTarjetaEstadoCuenta(string strNumeroTarjeta)
        {
            clsTarjetaEstadoCuenta notFound = new clsTarjetaEstadoCuenta();
            notFound.nombreTarjeta = "Not Found";
            List<clsTarjetaEstadoCuenta> tarjetas = new List<clsTarjetaEstadoCuenta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                string json = System.IO.File.ReadAllText(rutaArchivo);
                List<clsTarjetaEstadoCuenta> tarjetaEstado = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);
                foreach(var tarjeta in tarjetaEstado)
                {
                    if (tarjeta.numTarjeta.Equals(strNumeroTarjeta)) ;
                    tarjetas.Add(tarjeta);
                    //arbol.insertar(tarjeta);
                }

                if (tarjetas.Count == 0)
                    return new List<clsTarjetaEstadoCuenta> { notFound };
                return tarjetas;

            }
            catch (Exception ex)
            {
                notFound.nombreTarjeta += ex.Message;
                return new List<clsTarjetaEstadoCuenta> { notFound };

            }

        }


        private LinkedList<clsTarjeta> fncListarSaldos(string strNombreTarjeta)
        {
            var resultado = fncListarTarjetas(strNombreTarjeta);
            LinkedList<clsTarjeta> lkstSaldos = new LinkedList<clsTarjeta>();
            resultado.ForEach(tarjet => { lkstSaldos.AddLast(tarjet); });
            return lkstSaldos;
        }

        private bool fncCheckToken()
        {
            string tokenAuthentication = Request.Headers.Where(x => 
            x.Key == "Authorization").FirstOrDefault().Value;
            
            return tokenAuthentication.Equals("token");
        }


        

        

    }
}
