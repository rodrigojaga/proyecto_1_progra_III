namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// Clase creada para crear objetos con la informacion necesaria para 
    /// Notificar a los usuarios sobre sus solicitudes
    /// </summary>
    public class clsCorreoObj
    {

        public string correoUsuario {  get; set; }
        public string cuerpoMensaje {  get; set; }
        public string asunto { get; set; }

        public clsCorreoObj(string correoUsuario, string cuerpoMensaje, string asunto)
        {
            this.asunto = asunto;
            this.correoUsuario = correoUsuario;
            this.cuerpoMensaje = cuerpoMensaje;
        }

    }
}
