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
using System.Text;

namespace tarjetasDeCredito_proyecto1III.Controllers
{

    [ApiController]
    [Route("tarjeta")]
    public class TarjetaController : ControllerBase
    {
        clsArbolBinarioBusqueda arbol = new clsArbolBinarioBusqueda();
        Queue<clsTarjetaRetiro> queColaMonto = new Queue<clsTarjetaRetiro>();
        clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found", "Not found");
        Queue<clsCorreoObj>  colaMensajes = new Queue<clsCorreoObj>();

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
        public dynamic apiListarSaldo(string strNumTarjeta)
        {
            var resultado = fncListarSaldos(strNumTarjeta);

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
            return fncRetirar(queColaMonto);
            
        }

        [HttpGet]
        [Route("estado_cuenta")]
        public dynamic apiEstadoCuenta(string strNumTarjeta)
        {
            
            List<clsTarjetaEstadoCuenta> a = fncListarTarjetasNumTarjetaEstadoCuenta(strNumTarjeta);         
            clsTarjetaArbol ta = new clsTarjetaArbol();
            
            foreach (var item in a)
            {
                ta = new clsTarjetaArbol(item.nombreTarjeta,item.numTarjeta,item.saldo,item.banco,item.tipo);
                ta.arbolEstado = new clsArbolBinarioBusqueda();

                if (item.estadoCuenta != null)
                {
                    ta.insertEstadoCuentas(item.estadoCuenta);
                }

                
                
            }
            string data = clsArbolBinarioBusqueda.inorden(ta.arbolEstado.raizArbol());
            return $"{ta.nombreTarjeta} - {ta.numTarjeta} - {data}";


        }

        [HttpGet]
        [Route("consulta_movimiento")]
        public dynamic apiConsultaMovimiento(string strNumTarjeta)
        {
            //pilas para controlar movimientos

            List<clsTarjetaEstadoCuenta> a = fncListarTarjetasNumTarjetaEstadoCuenta(strNumTarjeta);
            clsTarjetaArbol ta = new clsTarjetaArbol();
            Stack<clsEstadoCuenta> stkMovimiento = new Stack<clsEstadoCuenta>();

            foreach (var item in a)
            {
                ta = new clsTarjetaArbol(item.nombreTarjeta, item.numTarjeta, item.saldo, item.banco, item.tipo);
                ta.arbolEstado = new clsArbolBinarioBusqueda();

                if (item.estadoCuenta != null)
                {
                    foreach (var estadoCuenta in item.estadoCuenta)
                    {
                        stkMovimiento.Push(estadoCuenta);
                    }
                }



            }

            StringBuilder sb = new StringBuilder();
            while (stkMovimiento.Count>0)
            {
                sb.Append(stkMovimiento.Pop().ToString() + " | \r\n");
                
            }


            string data = sb.ToString();

            if (fncCheckToken())
                return $"{ta.nombreTarjeta} - {ta.numTarjeta} - {data}";
            else return "Access Denied or not found";


        }


        private string fncRetirar(Queue<clsTarjetaRetiro> tarjeta)
        {

            clsTarjetaRetiro a = tarjeta.Dequeue();

            try
            {

                mtdActualizarSaldoEnJson(a.tarjetaAretirar.numTarjeta, a.dclMontoARetirar);
                return "Saldo actualizado exitosamente. Se le ha notificado al usuario";

            }
            catch (Exception ex)
            {
                return $"Error al actualizar el saldo: {ex.Message}";
            }

        }

        


        private void mtdActualizarSaldoEnJson(string strNumeroTarjeta, decimal montoRetiro)
        {
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", nombreArchivo);
            

            string json = System.IO.File.ReadAllText(rutaArchivo);
            List<clsTarjetaEstadoCuenta> tarjetas = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);

            // Buscar la tarjeta cuyo saldo se va a actualizar
            var tarjeta = tarjetas.FirstOrDefault(t => t.numTarjeta.ToUpper() == strNumeroTarjeta.ToUpper());

            clsEstadoCuenta newEstadoCuenta = new clsEstadoCuenta();
            string cuerpoMensaje = "";
            string subjectMensaje = "";
            if (montoRetiro < 0)
            {
                newEstadoCuenta = new clsEstadoCuenta(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").ToString(),"Pago X Servicio",montoRetiro.ToString(),"Débito");
                cuerpoMensaje += $"Pago X Servicio o producto | {montoRetiro.ToString()} el dia {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").ToString()} ";
                subjectMensaje = "Pago X Servicio o producto";
            }
            else
            {
                newEstadoCuenta = new clsEstadoCuenta(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").ToString(), "Deposito de dinero", montoRetiro.ToString(), "Deposito");
                cuerpoMensaje += $"Deposito de dinero de {montoRetiro.ToString()} el dia {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").ToString()} ";
                subjectMensaje = "Deposito de dinero ";
            }

            if (tarjeta != null)
            {
                // Actualizar el saldo de la tarjeta
                tarjeta.saldo = (decimal.Parse(tarjeta.saldo) + montoRetiro).ToString();

                //El correo del usuario
                string correoUsuario = tarjeta.correo.ToString();
                cuerpoMensaje += $" \r\nLa tarjeta {tarjeta.nombreTarjeta} del banco {tarjeta.banco} \r\nEstado actual en la cuenta {tarjeta.saldo}";

                //Crear nuevo estado de Cuenta
                tarjeta.estadoCuenta.Add(newEstadoCuenta);
                
                
                

                // Re-serializar la lista de objetos de vuelta a JSON
                string nuevoJson = JsonConvert.SerializeObject(tarjetas, Formatting.Indented);

                // Escribir el JSON actualizado de vuelta al archivo
                System.IO.File.WriteAllText(rutaArchivo, nuevoJson);


                clsCorreoObj correoUsuarioObj = new clsCorreoObj(correoUsuario,cuerpoMensaje,subjectMensaje);
                colaMensajes.Enqueue(correoUsuarioObj);
                mtdEnviarCorreo();


            }
            else
            {
                throw new Exception("No se encontró la tarjeta especificada.");
            }
        }


        private void mtdEnviarCorreo()
        {
            clsCorreo eCorreo = new clsCorreo();
            clsCorreoObj usuarioDestino = colaMensajes.Dequeue();
            try
            {
                eCorreo.fncCrearMensajeP(usuarioDestino.correoUsuario, usuarioDestino.asunto, usuarioDestino.cuerpoMensaje);

            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        private List<clsTarjeta> fncListarTarjetas(string strNombreTarjeta)
        {
            
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
            //clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found","Not found");
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
            string json = "";
            List<clsTarjetaEstadoCuenta> tarjetaEstado = new List<clsTarjetaEstadoCuenta>();
            List<clsTarjetaEstadoCuenta> tarjetas = new List<clsTarjetaEstadoCuenta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                json = System.IO.File.ReadAllText(rutaArchivo);
                tarjetaEstado = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);
                foreach(var tarjeta in tarjetaEstado)
                {
                    if (tarjeta.numTarjeta.Equals(strNumeroTarjeta))
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


        private LinkedList<clsTarjeta> fncListarSaldos(string strNumTarjeta)
        {
            var resultado = fncListarTarjetasNumTarjeta(strNumTarjeta);
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
