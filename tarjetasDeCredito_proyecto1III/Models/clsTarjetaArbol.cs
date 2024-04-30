using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// esta clase hereda de la superclase clsTarjeta los atributos que necesita
    /// utiliza un arbol binario de busqueda para almacenar los estados de cuenta del objeto obtenido del JSON
    /// </summary>
    public class clsTarjetaArbol:clsTarjeta
    {

        public clsArbolBinarioBusqueda arbolEstado { get; set; }

        /// <summary>
        /// constructor vacio
        /// </summary>
        public clsTarjetaArbol() : base() { }

        /// <summary>
        /// Constructor de los campos
        /// </summary>
        /// <param name="nombreTarjeta"></param>
        /// <param name="numTarjeta"></param>
        /// <param name="saldo"></param>
        /// <param name="banco"></param>
        /// <param name="tipo"></param>
        public clsTarjetaArbol(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo)            
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;

        }

        /// <summary>
        /// Metodo utilizado para leer los estados de cuenta provenientes del JSON
        /// Seran almacenados en el arbol binario de busqueda
        /// </summary>
        /// <param name="estadosCuenta"></param>
        public void insertEstadoCuentas(List<clsEstadoCuenta> estadosCuenta)
        {
            if (estadosCuenta != null)
            {
                foreach (var estadoCuenta in estadosCuenta)
                {
                    arbolEstado.insertar(estadoCuenta);
                }
            }
        }
     

    }
}
