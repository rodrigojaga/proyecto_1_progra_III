namespace tarjetasDeCredito_proyecto1III.abb
{
    public class clsArbolBinarioBusqueda:clsArbolBinario
    {

        public clsArbolBinarioBusqueda() : base()
        {
        }

        public clsArbolBinarioBusqueda(clsNodo nodo) : base(nodo)
        {
        }

        public clsNodo buscar(Object buscado)
        {
            interfaceComparador dato;
            dato = (interfaceComparador)buscado;
            if (raiz == null)
                return null;
            else
                return buscar(raizArbol(), dato);
        }

        protected clsNodo buscar(clsNodo raizSub, interfaceComparador buscado)
        {
            if (raizSub == null)
                return null;
            else if (buscado.igualQue(raizSub.valorNodo()))
                return raiz;
            else if (buscado.menorQue(raizSub.valorNodo()))
                return buscar(raizSub.subarbolIzdo(), buscado);
            else
                return buscar(raizSub.subarbolDcho(), buscado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buscado"></param>
        /// <returns></returns>
        public clsNodo buscarIterativo(Object buscado)
        {


            interfaceComparador dato;
            bool encontrado = false;
            clsNodo raizSub = raiz;
            dato = (interfaceComparador)buscado;

            while (!encontrado && raizSub != null)
            {
                if (dato.igualQue(raizSub.valorNodo()))
                    encontrado = true;
                else if (dato.menorQue(raizSub.valorNodo()))
                    raizSub = raizSub.subarbolIzdo();
                else
                    raizSub = raizSub.subarbolDcho();
            }
            return raizSub;
        }

        public void insertar(Object valor)
        {
            interfaceComparador dato;
            dato = (interfaceComparador)valor;
            raiz = insertar(raiz, dato);
        }

        //método interno para realizar la operación
        protected clsNodo insertar(clsNodo raizSub, interfaceComparador dato)
        {
            if (raizSub == null)
                raizSub = new clsNodo(dato);
            else if (dato.menorQue(raizSub.valorNodo()))
            {
                clsNodo iz;
                iz = insertar(raizSub.subarbolIzdo(), dato);
                raizSub.ramaIzdo(iz);
            }
            else if (dato.mayorQue(raizSub.valorNodo()))
            {
                clsNodo dr;
                dr = insertar(raizSub.subarbolDcho(), dato);
                raizSub.ramaDcho(dr);
            }
            else throw new Exception("Nodo duplicado");
            return raizSub;
        }

        public void eliminar(Object valor)
        {
            interfaceComparador dato;
            dato = (interfaceComparador)valor;
            raiz = eliminar(raiz, dato);
        }

        //método interno para realizar la operación
        protected clsNodo eliminar(clsNodo raizSub, interfaceComparador dato)
        {
            if (raizSub == null)
                throw new Exception("No encontrado el nodo con la clave");
            else if (dato.menorQue(raizSub.valorNodo()))
            {
                clsNodo iz;
                iz = eliminar(raizSub.subarbolIzdo(), dato);
                raizSub.ramaIzdo(iz);
            }
            else if (dato.mayorQue(raizSub.valorNodo()))
            {
                clsNodo dr;
                dr = eliminar(raizSub.subarbolDcho(), dato);
                raizSub.ramaDcho(dr);
            }
            else // Nodo encontrado
            {
                clsNodo q;
                q = raizSub; // nodo a quitar del árbol
                if (q.subarbolIzdo() == null)
                    raizSub = q.subarbolDcho();
                else if (q.subarbolDcho() == null)
                    raizSub = q.subarbolIzdo();
                else
                { // tiene rama izquierda y derecha
                    q = reemplazar(q);
                }
                q = null;
            }
            return raizSub;
        }

        // método interno para susutituir por el mayor de los menores
        private clsNodo reemplazar(clsNodo act)
        {
            clsNodo a, p;
            p = act;
            a = act.subarbolIzdo(); // rama de nodos menores
            while (a.subarbolDcho() != null)
            {
                p = a;
                a = a.subarbolDcho();
            }
            act.nuevoValor(a.valorNodo());
            if (p == act)
                p.ramaIzdo(a.subarbolIzdo());
            else
                p.ramaDcho(a.subarbolIzdo());
            return a;
        }

    }
}
