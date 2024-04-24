namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjetaRetiro
    {

        public clsTarjeta tarjetaAretirar { get; set; }
        public decimal dclMontoARetirar { get; set; }
        
        public clsTarjetaRetiro(clsTarjeta tarjetaAretirar, decimal dclMontoARetirar)
        {
            this.tarjetaAretirar = tarjetaAretirar;
            this.dclMontoARetirar = dclMontoARetirar;
        }
    }
}
