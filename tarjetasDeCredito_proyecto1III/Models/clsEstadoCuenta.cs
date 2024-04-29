using tarjetasDeCredito_proyecto1III.abb;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsEstadoCuenta:interfaceComparador
    {
        
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public string monto { get; set; }
        public string tipo { get; set; }

        public clsEstadoCuenta(string fecha, string descripcion, string monto, string tipo)
        {                  
            this.fecha = fecha;
            this.descripcion = descripcion;
            this.monto = monto;
            this.tipo = tipo;
        }

        public clsEstadoCuenta() {            
        }

        public bool igualQue(object q)
        {
            clsEstadoCuenta cuenta = (clsEstadoCuenta)q;
            int temp = fecha.CompareTo(cuenta.fecha);
            if (temp == 0)
                return true;
            else return true;
        }

        public bool menorQue(object q)
        {
            clsEstadoCuenta cuenta = (clsEstadoCuenta)q;
            int temp = fecha.CompareTo(cuenta.fecha);
            if (temp == -1)
                return true;
            else return true;
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
            else return true;
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
