using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{

    /// <summary>
    /// Esta clase es donde se ejecutan los metodos al llamar al endpoint 
    /// "bloqueo_temporal/{strNumTarjeta}/YN"
    /// En la el metodo apiBloqueoTemporal dentro de TarjetaController
    /// </summary>
    public class clsApiBloqueoTemporal
    {
        //para leer el JSON
        private readonly IWebHostEnvironment _hostingEnvironment;
        //Cola de mensajes a enviar al usuario
        Queue<clsCorreoObj> colaMensajes = new Queue<clsCorreoObj>();

        /// <summary>
        /// Brinda el acceso a la lectura del JSON dentro del proyecto
        /// </summary>
        public clsApiBloqueoTemporal(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        public string fncBloqeuarTarjeta(string strNumeroTarjeta, bool isBlocked)
        {
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", nombreArchivo);


            string json = System.IO.File.ReadAllText(rutaArchivo);
            List<clsTarjetaEstadoCuenta> tarjetas = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);

            // Buscar la tarjeta cuyo saldo se va a actualizar
            var tarjeta = tarjetas.FirstOrDefault(t => t.numTarjeta.ToUpper() == strNumeroTarjeta.ToUpper());

            if (tarjeta != null)
            {
                // Actualizar el bloqueo de la tarjeta
                tarjeta.bloqueoTemporal = isBlocked;

                //correo usuario
                string cuerpoMensaje = $"Querido {tarjeta.nombreTarjeta}, su Tarjeta: {tarjeta.numTarjeta} ha sido desbloqueada. \r\n" +
                    $"Para mas informacion comuniquese con el banco {tarjeta.banco}";
                string subjectMensaje = $"Solicitud de desbloqueo de tarjeta {tarjeta.banco}, {tarjeta.tipo}";

                if (isBlocked)
                {
                    cuerpoMensaje = $"Querido {tarjeta.nombreTarjeta}, su Tarjeta: {tarjeta.numTarjeta} ha sido bloqueada temporalmente. \r\n" +
                    $"Para mas informacion comuniquese con el banco {tarjeta.banco}";
                    subjectMensaje = $"Solicitud de bloquedo temporal de tarjeta {tarjeta.banco}, {tarjeta.tipo}";
                }

                string correoUsuario = tarjeta.correo.ToString();


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
