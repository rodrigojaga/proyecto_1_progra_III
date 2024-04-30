using Newtonsoft.Json;
using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    /// <summary>
    /// Esta clase es donde se ejecutan los metodos al llamar al endpoint 
    /// "estado_cuenta" y "consulta_movimiento"
    /// En los metodos apiEstadoCuenta y apiConsultaMovimiento dentro de TarjetaController
    /// </summary>
    public class clsApiEstadoCuenta
    {
        
        private readonly IWebHostEnvironment _hostingEnvironment;

        public clsApiEstadoCuenta(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <returns></returns>
        public List<clsTarjetaEstadoCuenta> fncListarTarjetasNumTarjetaEstadoCuenta(string strNumeroTarjeta)
        {
            clsTarjetaEstadoCuenta notFound = new clsTarjetaEstadoCuenta();
            notFound.nombreTarjeta = "Not Found";
            string json = "";
            List<clsTarjetaEstadoCuenta> tarjetaEstado = new List<clsTarjetaEstadoCuenta>();
            List<clsTarjetaEstadoCuenta> tarjetas = new List<clsTarjetaEstadoCuenta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                json = System.IO.File.ReadAllText(rutaArchivo);
                tarjetaEstado = JsonConvert.DeserializeObject<List<clsTarjetaEstadoCuenta>>(json);
                foreach (var tarjeta in tarjetaEstado)
                {
                    if (tarjeta.numTarjeta.Equals(strNumeroTarjeta))
                        tarjetas.Add(tarjeta);
                    //arbol.insertar(tarjeta);
                }

                if (tarjetas.Count == 0)
                    return new List<clsTarjetaEstadoCuenta> { notFound };
                return tarjetas;

            }
            catch (Exception ex)
            {
                notFound.nombreTarjeta += ex.Message;
                return new List<clsTarjetaEstadoCuenta> { notFound };

            }

        }

    }
}
