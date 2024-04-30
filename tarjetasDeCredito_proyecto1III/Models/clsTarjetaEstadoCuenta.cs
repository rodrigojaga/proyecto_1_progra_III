using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// Clase que es utilizada para leer los archivos del Json y luego tomar los estados de cuenta 
    /// hereda de la superclase clsTarjeta
    /// </summary>
    public class clsTarjetaEstadoCuenta: clsTarjeta
    {

        public List<clsEstadoCuenta> estadoCuenta { get; set; }

        /// <summary>
        /// Constructor con los datos necesarios para hacer un objeto
        /// de tipo clsTarjetaEstadoCuenta
        /// </summary>
        /// <param name="nombreTarjeta"></param>
        /// <param name="numTarjeta"></param>
        /// <param name="saldo"></param>
        /// <param name="banco"></param>
        /// <param name="tipo"></param>
        /// <param name="correo"></param>
        /// <param name="pin"></param>
        /// <param name="bloqueoTemporal"></param>
        /// <param name="limiteCredito"></param>
        /// <param name="estadoCuenta"></param>
        public clsTarjetaEstadoCuenta(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, string correo, string pin, bool bloqueoTemporal, string limiteCredito
                                                                                                                        , List<clsEstadoCuenta> estadoCuenta)
            :base(nombreTarjeta, numTarjeta, saldo, banco, tipo, correo, pin, bloqueoTemporal,limiteCredito)
        {
            this.estadoCuenta = estadoCuenta;
        }

        public clsTarjetaEstadoCuenta() { }

    }
}
