using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjetaEstadoCuenta:interfaceComparador
    {

        public string nombreTarjeta { get; set; }
        public string numTarjeta { get; set; }
        public string saldo { get; set; }
        public string banco { get; set; }
        public string tipo { get; set; }
        public List<clsEstadoCuenta> estadoCuenta { get; set; }



        public clsTarjetaEstadoCuenta(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, List<clsEstadoCuenta> estadoCuenta)
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;
            this.estadoCuenta = estadoCuenta;
        }

        public clsTarjetaEstadoCuenta() { }

        public bool igualQue(object q)
        {
            clsTarjetaEstadoCuenta cuenta = (clsTarjetaEstadoCuenta)q;
            int temp = numTarjeta.CompareTo(cuenta.numTarjeta);
            if (temp == 0)
                return true;
            else return false;
        }

        public bool menorQue(object q)
        {
            clsTarjetaEstadoCuenta cuenta = (clsTarjetaEstadoCuenta)q;
            int temp = numTarjeta.CompareTo(cuenta.numTarjeta);
            if (temp == -1)
                return true;
            else return false;
        }

        public bool menorIgualQue(object q)
        {
            throw new NotImplementedException();
        }

        public bool mayorQue(object q)
        {
            clsTarjetaEstadoCuenta cuenta = (clsTarjetaEstadoCuenta)q;
            int temp = numTarjeta.CompareTo(cuenta.numTarjeta);
            if (temp == 1)
                return true;
            else return false;
        }

        public bool mayorIgualQue(object q)
        {
            throw new NotImplementedException();
        }
    }
}
