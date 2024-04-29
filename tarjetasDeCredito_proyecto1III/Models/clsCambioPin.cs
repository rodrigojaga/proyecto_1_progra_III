namespace tarjetasDeCredito_proyecto1III.Models
{
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
