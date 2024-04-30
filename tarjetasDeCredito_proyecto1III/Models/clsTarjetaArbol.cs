using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjetaArbol:clsTarjeta
    {

        //public string nombreTarjeta { get; set; }
        //public string numTarjeta { get; set; }
        //public string saldo { get; set; }
        //public string banco { get; set; }
        //public string tipo { get; set; }
        public clsArbolBinarioBusqueda arbolEstado { get; set; }



        public clsTarjetaArbol(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, string correo, string pin, bool bloqueoTemporal, string limiteCredito, clsArbolBinarioBusqueda arbolEstado)
            : base(nombreTarjeta, numTarjeta, saldo, banco, tipo,correo,pin, bloqueoTemporal,limiteCredito)
        {
            this.arbolEstado = arbolEstado;
        }

        public clsTarjetaArbol() : base() { }

        public clsTarjetaArbol(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo)
            //:base(nombreTarjeta, numTarjeta, saldo, banco, tipo)
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;

        }

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

        

        public override string ToString()
        {
            return $"{nombreTarjeta} - {saldo} ";
        }

    }
}
