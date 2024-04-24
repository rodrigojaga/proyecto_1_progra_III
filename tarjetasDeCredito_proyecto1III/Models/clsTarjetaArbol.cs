using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjetaArbol
    {

        public string nombreTarjeta { get; set; }
        public string numTarjeta { get; set; }
        public string saldo { get; set; }
        public string banco { get; set; }
        public string tipo { get; set; }
        public clsArbolBinarioBusqueda arbolEstado { get; set; }



        public clsTarjetaArbol(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, clsArbolBinarioBusqueda arbolEstado)
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;
            this.arbolEstado = arbolEstado;
        }

        public clsTarjetaArbol() { }

    }
}
