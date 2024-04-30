using System.Runtime.CompilerServices;
using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjeta:interfaceComparador
    {

        public string nombreTarjeta { get; set; }
        public string numTarjeta { get; set; }
        public string saldo { get; set; }
        public string banco { get; set; }
        public string tipo { get; set; }
        public string correo { get; set; }
        public string pin { get; set; }
        public bool bloqueoTemporal { get; set; }
        public string limiteCredito { get; set; }




        public clsTarjeta(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, string correo, string pin, bool bloqueoTemporal, string limiteCredito)
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;
            this.correo = correo;
            this.pin = pin;
            this.bloqueoTemporal = bloqueoTemporal;
            this.limiteCredito = limiteCredito;
        }

        public clsTarjeta(string numTarjeta, bool bloqueoTemporal)
        {
            this.numTarjeta = numTarjeta;
            this.bloqueoTemporal = bloqueoTemporal;
        }

        public clsTarjeta(string numTarjeta, string limiteCredito)
        {
            this.numTarjeta=numTarjeta;
            this.limiteCredito=limiteCredito;
        }

        public clsTarjeta(string numTarjeta)
        {
            this.numTarjeta = numTarjeta;
        }

        public clsTarjeta() { }

        public bool igualQue(object q)
        {
            clsTarjeta tar = (clsTarjeta)q;
            if (this.numTarjeta.CompareTo(tar.numTarjeta) == 0)
                return true;
            return false;
        }

        public bool menorQue(object q)
        {
            clsTarjeta tar = (clsTarjeta)q;
            if (this.numTarjeta.CompareTo(tar.numTarjeta) == -1)
                return true;
            return false;
        }

        public bool menorIgualQue(object q)
        {
            throw new NotImplementedException();
        }

        public bool mayorQue(object q)
        {
            clsTarjeta tar = (clsTarjeta)q;
            if (this.numTarjeta.CompareTo(tar.numTarjeta) == 1)
                return true;
            return false;
        }

        public bool mayorIgualQue(object q)
        {
            throw new NotImplementedException();
        }
    }
}
