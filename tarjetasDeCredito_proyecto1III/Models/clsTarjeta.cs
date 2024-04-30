using System.Runtime.CompilerServices;
using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// Objeto principal 
    /// Superclase
    /// Hereda a multiples clases para que estas puedan acceder a los atributos
    /// implementa la interfaz interfaceComparador
    /// </summary>
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



        /// <summary>
        /// Constructor utilizado para devolver un objeto clsTarjeta que tiene como atributos unicamente "NotFound"
        /// Objeto el cual es utilizado en caso de que el sistema no encuentre la tarjeta buscada dentro del archivo JSON
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


        /// <summary>
        /// Constructor utilizado en el controller TarjetaController 
        /// para asignar los valores de los cambios que quieren hacerse en bloqueo temporal
        /// true en caso de querer bloquear la tarjeta
        /// false en caso de querer desbloquearla
        /// </summary>
        /// <param name="numTarjeta"></param>
        /// <param name="bloqueoTemporal"></param>
        public clsTarjeta(string numTarjeta, bool bloqueoTemporal)
        {
            this.numTarjeta = numTarjeta;
            this.bloqueoTemporal = bloqueoTemporal;
        }


        /// <summary>
        /// Constructor utilizado en el controller TarjetaController 
        /// para asignar los valores de los cambios que quieren hacerse en el cambio 
        /// de limite de credito permitido por la tarjeta o el banco
        /// </summary>
        /// <param name="numTarjeta"></param>
        /// <param name="limiteCredito"></param>
        public clsTarjeta(string numTarjeta, string limiteCredito)
        {
            this.numTarjeta=numTarjeta;
            this.limiteCredito=limiteCredito;
        }

        /// <summary>
        /// constructor utilizado a la hora de crear el objeto con el que se buscara dentro del arbol Binario de busqueda 
        /// utilizado en el bloqueo de tarjetas
        /// </summary>
        /// <param name="numTarjeta"></param>
        public clsTarjeta(string numTarjeta)
        {
            this.numTarjeta = numTarjeta;
        }


        /// <summary>
        /// constructor vacio
        /// </summary>
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
