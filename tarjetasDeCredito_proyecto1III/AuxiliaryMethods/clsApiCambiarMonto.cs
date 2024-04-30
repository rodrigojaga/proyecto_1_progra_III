using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.Models;


namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    /// <summary>
    /// Clase utilizada en el metodo apiActualizarSaldo en la clase TarjetaController
    /// y el endpoint "actualizar_saldo/{strNumTarjeta}/monto"
    /// </summary>
    public class clsApiCambiarMonto
    {

        
        Queue<clsCorreoObj> colaMensajes = new Queue<clsCorreoObj>();
        private readonly IWebHostEnvironment _hostingEnvironment;

        public clsApiCambiarMonto(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Funcion que interactua con el exterior
        /// </summary>
        /// <param name="tarjeta"></param>
        /// <returns></returns>
        public string fncRetirar(Queue<clsTarjetaRetiro> tarjeta)
        {

            clsTarjetaRetiro a = tarjeta.Dequeue();

            try
            {
                return mtdActualizarSaldoEnJson(a.numTarjeta, a.dclMontoARetirar);
            }
            catch (Exception ex)
            {
                return $"Error al actualizar el saldo: {ex.Message}";
            }

        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <param name="montoRetiro"></param>
        /// <returns></returns>
        private string mtdActualizarSaldoEnJson(string strNumeroTarjeta, decimal montoRetiro)
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
                newEstadoCuenta = new clsEstadoCuenta(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").ToString(), "Pago X Servicio", montoRetiro.ToString(), "Débito");
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


                clsCorreoObj correoUsuarioObj = new clsCorreoObj(correoUsuario, cuerpoMensaje, subjectMensaje);
                colaMensajes.Enqueue(correoUsuarioObj);
                mtdEnviarCorreo();
                return "OK";


            }
            else
            {
                return ("No se encontró la tarjeta especificada.");
            }
        }

        /// <summary>
        /// Envia los mensajes por correo al usuario
        /// </summary>
        /// <exception cref="Exception"></exception>
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

    }
}
