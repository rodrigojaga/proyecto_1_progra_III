namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// clase que existe para auxiliar a la superclase clsTarjeta
    /// agregando un monto a retirar
    /// </summary>
    public class clsTarjetaRetiro: clsTarjeta
    {
        
        public decimal dclMontoARetirar { get; set; }
        
        public clsTarjetaRetiro(string  strNumTarjeta, decimal dclMontoARetirar)
            :base(strNumTarjeta)

        {            
            this.dclMontoARetirar = dclMontoARetirar;
        }
    }
}
