namespace tarjetasDeCredito_proyecto1III.abb
{
    public class clsArbolBinario
    {

        protected clsNodo raiz;

        public clsArbolBinario()
        {
            raiz = null;
        }

        public clsArbolBinario(clsNodo raiz)
        {
            this.raiz = raiz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public clsNodo raizArbol()
        {
            return raiz;
        }

        /// <summary>
        /// Comprueba el estatus del árbol
        /// </summary>
        /// <returns></returns>
        bool esVacio()
        {
            return raiz == null;
        }

        public static clsNodo nuevoArbol(clsNodo ramaIzqda, Object dato, clsNodo ramaDrcha)
        {
            return new clsNodo(ramaIzqda, dato, ramaDrcha);
        }


        //binario en preorden
        public static string preorden(clsNodo r)
        {
            if (r != null)
            {
                return r.visitar() + preorden(r.subarbolIzdo()) +
                    preorden(r.subarbolDcho());
            }
            return "";
        }

        // Recorrido de un árbol binario en inorden
        public static string inorden(clsNodo r)
        {
            if (r != null)
            {
                return inorden(r.subarbolIzdo())
                    + r.visitar() + inorden(r.subarbolDcho());
            }
            return "";
        }

        // Recorrido de un árbol binario en postorden
        public static string postorden(clsNodo r)
        {
            if (r != null)
            {
                return postorden(r.subarbolIzdo()) + postorden(r.subarbolDcho()) + r.visitar();
            }
            return "";
        }

        //Devuelve el número de nodos que tiene el árbol
        public static int numNodos(clsNodo raiz)
        {
            if (raiz == null)
                return 0;
            else
                return 1 + numNodos(raiz.subarbolIzdo()) +
                numNodos(raiz.subarbolDcho());
        }

    }
}
