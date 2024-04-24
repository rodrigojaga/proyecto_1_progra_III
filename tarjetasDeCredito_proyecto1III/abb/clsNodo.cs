namespace tarjetasDeCredito_proyecto1III.abb
{
    public class clsNodo
    {

        protected Object dato;
        protected clsNodo izdo;
        protected clsNodo dcho;

        /// <summary>
        /// Método Constructor del nodo el cual recibe un valor y asign
        /// asigna nulos a los hijos
        /// </summary>
        /// <param name="valor">hhhhhhhhhhhhh</param>
        public clsNodo(Object valor)
        {
            dato = valor;
            izdo = dcho = null;
        }

        public clsNodo(clsNodo ramaIzdo, Object valor, clsNodo ramaDcho)
        {
            this.dato = valor;
            izdo = ramaIzdo;
            dcho = ramaDcho;
        }

        // operaciones de acceso
        public Object valorNodo()
        {
            return dato;
        }

        public clsNodo subarbolIzdo() { return izdo; }
        public clsNodo subarbolDcho() { return dcho; }


        public void nuevoValor(Object d)
        {
            dato = d;
        }


        public void ramaIzdo(clsNodo n) { izdo = n; }
        public void ramaDcho(clsNodo n) { dcho = n; }
        public string visitar()
        {
            return dato.ToString()
                + "\r\n";
        }

    }
}
