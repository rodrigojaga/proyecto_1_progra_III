using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    /// <summary>
    /// Clase que trabaja en conjunto con el arbol binario de busqueda que se encuentra en la clase
    /// clsTarjetaArbol.
    /// Aqui estan declarados los diferentes atributos definidos en el JSON 
    /// Implementa la interfaz interfaceComparador para determinar el orden de ingreso de los estados 
    /// de cuenta dentro del Arbol Binario  de Busqueda
    /// </summary>
    public class clsEstadoCuenta:interfaceComparador
    {
        
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public string monto { get; set; }
        public string tipo { get; set; }

        /// <summary>
        /// constructor con los datos de los estados de cuenta
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="descripcion"></param>
        /// <param name="monto"></param>
        /// <param name="tipo"></param>
        public clsEstadoCuenta(string fecha, string descripcion, string monto, string tipo)
        {                  
            this.fecha = fecha;
            this.descripcion = descripcion;
            this.monto = monto;
            this.tipo = tipo;
        }
        /// <summary>
        /// constructor vacio
        /// </summary>
        public clsEstadoCuenta() {            
        }

        public bool igualQue(object q)
        {
            clsEstadoCuenta cuenta = (clsEstadoCuenta)q;
            int temp = fecha.CompareTo(cuenta.fecha);
            if (temp == 0)
                return true;
            else return false;
        }

        public bool menorQue(object q)
        {
            clsEstadoCuenta cuenta = (clsEstadoCuenta)q;
            int temp = fecha.CompareTo(cuenta.fecha);
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
            clsEstadoCuenta cuenta = (clsEstadoCuenta)q;
            int temp = fecha.CompareTo(cuenta.fecha);
            if (temp == 1)
                return true;
            else return false;
        }

        public bool mayorIgualQue(object q)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{fecha} | {monto} | {tipo} | {descripcion}";
        }
    }
}
