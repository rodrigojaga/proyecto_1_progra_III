namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// Clase creada con la finalidad de auxiliar a la superclase clsTarjeta
    /// Recibe un numero de tarjeta y un pin
    /// los cuales son utilizados a la hora de querer actualizar el pin de seguridad del usuario
    /// </summary>
    public class clsCambioPin
    {

        public string strNumTarjeta { get; set; }
        public string strPin { get; set;}

        public clsCambioPin() { }

        public clsCambioPin(string strNumTarjeta, string strPin)
        {
            this.strNumTarjeta = strNumTarjeta;
            this.strPin = strPin;
        }
    }
}
