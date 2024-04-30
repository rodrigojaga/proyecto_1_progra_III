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
using tarjetasDeCredito_proyecto1III.AuxiliaryMethods;

namespace tarjetasDeCredito_proyecto1III.Controllers
{

    [ApiController]
    [Route("tarjeta")]
    public class TarjetaController : ControllerBase
    {        
        Queue<clsTarjetaRetiro> queColaMonto = new Queue<clsTarjetaRetiro>();                
        LinkedList<clsCambioPin> lklistCambioPin = new LinkedList<clsCambioPin>();
        clsArbolBinarioBusqueda bloqueoTarjetas = new clsArbolBinarioBusqueda();
        Stack<clsTarjeta> aumentoLimite = new Stack<clsTarjeta>();

        private readonly IWebHostEnvironment _hostingEnvironment;
        public TarjetaController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// Este endpoint busca todas las tarjetas que tengan el nombre ingresado
        /// </summary>
        /// <param name="strNombreTarjeta"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listar")]
        public dynamic apiListarTarjeta(string strNombreTarjeta)
        {
            clsApiListarTarjetas conn = new clsApiListarTarjetas(_hostingEnvironment);
            var resultado = conn.fncListarTarjetas(strNombreTarjeta);

            if (!resultado[0].nombreTarjeta.Equals("Not found") //&& fncCheckToken()
                )
            {

                return resultado;

            }
            else
            {
                return "Access Denied or not found + " + resultado[0].nombreTarjeta;
            }

        }

        /// <summary>
        /// Esta API brinda informacion general de la tarjeta que se esta buscando
        /// con el numero de tarjeta ingresado
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listar/saldo")]
        public dynamic apiListarSaldo(string strNumTarjeta)
        {
            clsApiBuscarNumTarjeta conn = new clsApiBuscarNumTarjeta(_hostingEnvironment);
            var resultado = conn.fncListarSaldos(strNumTarjeta);

            if (!resultado.First.Value.nombreTarjeta.Equals("Not found") //&& fncCheckToken()
                )
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

        /// <summary>
        /// Esta API actualiza el saldo de la tarjeta
        /// el valor ingresado puede ser negativo o positivo 
        /// EL usuario recibira un correo notificandole del deposito o del retiro de dinero 
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <param name="monto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("actualizar_saldo/{strNumTarjeta}/monto")]
        public dynamic apiActualizarSaldo(string strNumTarjeta, [FromBody] decimal monto)
        {
            clsApiCambiarMonto conn = new clsApiCambiarMonto(_hostingEnvironment);
            queColaMonto.Enqueue(new clsTarjetaRetiro(strNumTarjeta, monto));
            string aux = conn.fncRetirar(queColaMonto);
            if (aux.ToUpper().Contains("OK"))
                return "Saldo actualizado";
            else return aux;

        }

        /// <summary>
        /// Lista el estado de cuenta de la tarjeta ingresada
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("estado_cuenta")]
        public dynamic apiEstadoCuenta(string strNumTarjeta)
        {
            clsApiEstadoCuenta conn = new clsApiEstadoCuenta(_hostingEnvironment);
            List<clsTarjetaEstadoCuenta> a = conn.fncListarTarjetasNumTarjetaEstadoCuenta(strNumTarjeta);
            clsTarjetaArbol ta = new clsTarjetaArbol();

            foreach (var item in a)
            {
                ta = new clsTarjetaArbol(item.nombreTarjeta, item.numTarjeta, item.saldo, item.banco, item.tipo);
                ta.arbolEstado = new clsArbolBinarioBusqueda();

                if (item.estadoCuenta != null)
                {
                    ta.insertEstadoCuentas(item.estadoCuenta);
                }



            }
            string data = clsArbolBinarioBusqueda.inorden(ta.arbolEstado.raizArbol());
            return $"{ta.nombreTarjeta} - {ta.numTarjeta} - {ta.tipo} \r\n{data}";


        }

        /// <summary>
        /// Muestra los movimientos hechos por la tarjeta
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("consulta_movimiento")]
        public dynamic apiConsultaMovimiento(string strNumTarjeta)
        {
            //pilas para controlar movimientos
            clsApiEstadoCuenta conn = new clsApiEstadoCuenta(_hostingEnvironment);
            List<clsTarjetaEstadoCuenta> a = conn.fncListarTarjetasNumTarjetaEstadoCuenta(strNumTarjeta);
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
            while (stkMovimiento.Count > 0)
            {
                sb.Append(stkMovimiento.Pop().ToString() + " | \r\n");

            }


            string data = sb.ToString();

            //if (fncCheckToken())
                return $"{ta.nombreTarjeta} - {ta.numTarjeta} - {data}";
            //else return "Access Denied or not found";


        }

        /// <summary>
        /// Permite cambiar el PIN de seguridad de la Tarjeta, notificandole al usuario el cambio
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <param name="nuevoPin"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cambiar_pin/{strNumTarjeta}/pin")]
        public dynamic apiCambiarPin(string strNumTarjeta, [FromBody] string nuevoPin)
        {
            lklistCambioPin.AddLast(new clsCambioPin(strNumTarjeta, nuevoPin));


            string temp = leerLista();
            if (temp.ToUpper().Equals("OK"))
                return "Su solicitud ha sido recibida, espere el correo para saber como va su proceso ";
            return "Access Denied or not found";
        }

        /// <summary>
        /// Bloquea la tarjeta hasta que se haga la accion contraria
        /// Se le notifica al usuario del cambio
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <param name="YN"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("bloqueo_temporal/{strNumTarjeta}/YN")]
        public dynamic apiBloqueoTemporal(string strNumTarjeta, [FromBody] string YN)
        {

            if(YN.ToUpper().Equals("YES") || YN.ToUpper().Equals("SI") || YN.ToUpper().Equals("TRUE"))
            {
                clsTarjeta tjBl = new clsTarjeta(strNumTarjeta,true);
                bloqueoTarjetas.insertar(tjBl);
                if(bloqueoArbol(strNumTarjeta).ToUpper().Equals("OK"))
                    return "Ha solicitado el bloqueo temporal de su tarjeta revise su correo para confirmar el inicio del proceso";
                return "Algo ha salido mal, comuniquese con el banco para seguir con su proceso";
            }
            else if(YN.ToUpper().Equals("NO") || YN.ToUpper().Equals("NO")|| YN.ToUpper().Equals("FALSE"))
            {
                clsTarjeta tjBl = new clsTarjeta(strNumTarjeta, false);
                bloqueoTarjetas.insertar(tjBl);
                if(bloqueoArbol(strNumTarjeta).ToUpper().Equals("OK"))
                    return "Ha solicitado quitar el bloqueo temporal de la tarjeta, revise su correo para confirmar el inicio del proceso";
                return "Algo ha salido mal, comuniquese con el banco para seguir con su proceso";
            }
            else
            {
                return "Access Denied or not found";
            }

        }
        /// <summary>
        /// Permite incrementar o decrementar el limite de credito en las tarjetas en caso
        /// de que este aplique
        /// Se le notifica al usuario de los cambios 
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <param name="nuevoLimite"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("limite_de_credito/{strNumTarjeta}/nuevoLimite")]
        public dynamic apiIncrementoLimite(string strNumTarjeta, [FromBody] string nuevoLimite)
        {
            aumentoLimite.Push(new clsTarjeta(strNumTarjeta,nuevoLimite));

            return incrementoLimite();
        }



        /// <summary>
        /// Metodo utilizado por el apiCambiarPin 
        /// </summary>
        /// <returns></returns>
        private string leerLista()
        {
            clsApiCambiarPin conn = new clsApiCambiarPin(_hostingEnvironment);
            foreach (var item in lklistCambioPin)
            {
                return conn.mtdActualizarPinEnJSON(item.strNumTarjeta, item.strPin);
            }
            return "";
        }

        /// <summary>
        /// Metodo utilizado por el apiBloqueoTemporal
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <returns></returns>
        private string bloqueoArbol(string strNumTarjeta)
        {
            clsApiBloqueoTemporal conn = new clsApiBloqueoTemporal(_hostingEnvironment);
            clsTarjeta tj = (clsTarjeta)bloqueoTarjetas.buscarIterativo(new clsTarjeta(strNumTarjeta)).valorNodo();
            return conn.fncBloqeuarTarjeta(tj.numTarjeta, tj.bloqueoTemporal);
        }

        /// <summary>
        /// Metodo utilizado por el apiIncrementoLimite
        /// </summary>
        /// <returns></returns>
        private string incrementoLimite()
        {
            clsAPICambiarLimite conn = new clsAPICambiarLimite(_hostingEnvironment);
            clsTarjeta tj = aumentoLimite.Pop();
            return conn.fncLimiteTarjeta(tj.numTarjeta,tj.limiteCredito);
        }

        /// <summary>
        /// Metodo de Seguridad en donde se verifica que se este recibiendo un token
        /// </summary>
        /// <returns></returns>
        private bool fncCheckToken()
        {
            string tokenAuthentication = Request.Headers.Where(x => 
            x.Key == "Authorization").FirstOrDefault().Value;
            
            return tokenAuthentication.Equals("token");
        }

    }
}
