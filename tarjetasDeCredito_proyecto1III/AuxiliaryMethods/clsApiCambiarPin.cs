using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    /// <summary>
    /// Esta clase es donde se ejecutan los metodos al llamar al endpoint 
    /// "cambiar_pin/{strNumTarjeta}/pin"
    /// En la el metodo apiCambiarPin dentro de TarjetaController
    /// </summary>
    public class clsApiCambiarPin
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        Queue<clsCorreoObj> colaMensajes = new Queue<clsCorreoObj>();

        public clsApiCambiarPin(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <param name="nuevoPin"></param>
        /// <returns></returns>
        public string mtdActualizarPinEnJSON(string strNumeroTarjeta, string nuevoPin)
        {
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", nombreArchivo);


            string json = System.IO.File.ReadAllText(rutaArchivo);
            List<clsTarjetaEstadoCuenta> tarjetas = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);

            // Buscar la tarjeta cuyo saldo se va a actualizar
            var tarjeta = tarjetas.FirstOrDefault(t => t.numTarjeta.ToUpper() == strNumeroTarjeta.ToUpper());

            if (tarjeta != null)
            {
                // Actualizar el pin de la tarjeta
                tarjeta.pin = nuevoPin;

                //correo usuario
                string cuerpoMensaje = $"Querido {tarjeta.nombreTarjeta}, su PIN de seguridad ha sido actualizado con exito.\r\nSu nuevo PIN es: {tarjeta.pin}";
                string correoUsuario = tarjeta.correo.ToString();
                string subjectMensaje = $"Cambio de PIN de Seguridad AMIGO {tarjeta.banco}";

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
