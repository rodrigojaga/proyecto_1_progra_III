using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    /// <summary>
    /// Esta clase es donde se ejecutan los metodos al llamar al endpoint 
    /// "listar"
    /// En la el metodo apiListarTarjetas dentro de TarjetaController
    /// </summary>
    public class clsApiListarTarjetas
    {
        clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found", "Not found", "Not found", false, "Not found");
        private readonly IWebHostEnvironment _hostingEnvironment;

        public clsApiListarTarjetas(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNombreTarjeta"></param>
        /// <returns></returns>
        public List<clsTarjeta> fncListarTarjetas(string strNombreTarjeta)
        {

            List<clsTarjeta> tarjetas = new List<clsTarjeta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                string json = System.IO.File.ReadAllText(rutaArchivo);
                var tarjetasJson = System.Text.Json.JsonSerializer.Deserialize<clsTarjeta[]>(json);
                tarjetas.AddRange(tarjetasJson.Where(tarjeta =>
                tarjeta.nombreTarjeta.ToUpper().Equals(strNombreTarjeta.ToUpper())));
                if (tarjetas.Count == 0)
                    return new List<clsTarjeta> { notFound };
                return tarjetas;

            }
            catch (Exception ex)
            {
                notFound.nombreTarjeta = ex.Message;

                return new List<clsTarjeta> { notFound };

            }

        }

    }
}
