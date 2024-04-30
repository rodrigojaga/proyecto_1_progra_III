using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    public class clsAPICambiarLimite
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        Queue<clsCorreoObj> colaMensajes = new Queue<clsCorreoObj>();

        public clsAPICambiarLimite(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <param name="nuevoLimite"></param>
        /// <returns></returns>
        public string fncLimiteTarjeta(string strNumeroTarjeta, string nuevoLimite)
        {
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources", nombreArchivo);


            string json = System.IO.File.ReadAllText(rutaArchivo);
            List<clsTarjetaEstadoCuenta> tarjetas = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);

            // Buscar la tarjeta cuyo saldo se va a actualizar
            var tarjeta = tarjetas.FirstOrDefault(t => t.numTarjeta.ToUpper() == strNumeroTarjeta.ToUpper());

            if (tarjeta != null)
            {
                if (tarjeta.tipo.ToUpper().Contains("CREDITO"))
                {
                    //correo usuario                                                            
                    string correoUsuario = tarjeta.correo.ToString();
                    string cuerpoMensaje = "";
                    string subjectMensaje = $"Solicitud de cambio de limite de credito a tarjeta {tarjeta.banco}, {tarjeta.tipo}";
                    if (decimal.Parse(tarjeta.limiteCredito) > decimal.Parse(nuevoLimite))
                    {

                        cuerpoMensaje = $"Querido {tarjeta.nombreTarjeta}, a su Tarjeta: {tarjeta.numTarjeta} le hemos disminuido el limite de credito maximo.\r\n" +
                            $"De {tarjeta.limiteCredito} a {nuevoLimite} \r\n" +
                        $"Para mas informacion comuniquese con el banco {tarjeta.banco}";
                    }
                    else
                    {
                        cuerpoMensaje = $"Querido {tarjeta.nombreTarjeta}, a su Tarjeta: {tarjeta.numTarjeta} le hemos aumentado el limite de credito maximo.\r\n" +
                           $"De {tarjeta.limiteCredito} a {nuevoLimite} \r\n" +
                       $"Para mas informacion comuniquese con el banco {tarjeta.banco}";
                    }

                    // Actualizar el limite de credito de la tarjeta
                    tarjeta.limiteCredito = nuevoLimite;



                    // Re-serializar la lista de objetos de vuelta a JSON
                    string nuevoJson = JsonConvert.SerializeObject(tarjetas, Formatting.Indented);

                    // Escribir el JSON actualizado de vuelta al archivo
                    System.IO.File.WriteAllText(rutaArchivo, nuevoJson);


                    clsCorreoObj correoUsuarioObj = new clsCorreoObj(correoUsuario, cuerpoMensaje, subjectMensaje);
                    colaMensajes.Enqueue(correoUsuarioObj);
                    mtdEnviarCorreo();
                    return "Se ha recibido la solicitud, espere un correo electronico para confirmar";
                }
                else
                {
                    return "Limite de credito NO aplica a tarjetas de Debito";
                }
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
