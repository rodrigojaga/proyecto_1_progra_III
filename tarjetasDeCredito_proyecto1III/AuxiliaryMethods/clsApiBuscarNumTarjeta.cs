using tarjetasDeCredito_proyecto1III.Models;

namespace tarjetasDeCredito_proyecto1III.AuxiliaryMethods
{
    /// <summary>
    /// Esta clase es donde se ejecutan los metodos al llamar al endpoint 
    /// "listar/saldo"
    /// En la el metodo apiListarSaldo dentro de TarjetaController
    /// </summary>
    public class clsApiBuscarNumTarjeta
    {

        clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found", "Not found", "Not found", false, "Not found");
        private readonly IWebHostEnvironment _hostingEnvironment;

        public clsApiBuscarNumTarjeta(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Metodo que lee el JSON
        /// </summary>
        /// <param name="strNumeroTarjeta"></param>
        /// <returns></returns>
        private List<clsTarjeta> fncListarTarjetasNumTarjeta(string strNumeroTarjeta)
        {
            //clsTarjeta notFound = new clsTarjeta("Not found", "Not found", "Not found", "Not found", "Not found","Not found");
            List<clsTarjeta> tarjetas = new List<clsTarjeta>();
            string nombreArchivo = "tarjetas_datos.json";
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "Resources",
                nombreArchivo);
            try
            {
                string json = System.IO.File.ReadAllText(rutaArchivo);
                var tarjetasJson = System.Text.Json.JsonSerializer.Deserialize<clsTarjeta[]>(json);
                tarjetas.AddRange(tarjetasJson.Where(tarjeta =>
                tarjeta.numTarjeta.ToUpper().Equals(strNumeroTarjeta.ToUpper())));
                if (tarjetas.Count == 0)
                    return new List<clsTarjeta> { notFound };
                return tarjetas;

            }
            catch (Exception ex)
            {
                notFound.numTarjeta = ex.Message;
                return new List<clsTarjeta> { notFound };

            }

        }

        /// <summary>
        /// Metodo que interactua con el exterior
        /// </summary>
        /// <param name="strNumTarjeta"></param>
        /// <returns></returns>
        public LinkedList<clsTarjeta> fncListarSaldos(string strNumTarjeta)
        {
            var resultado = fncListarTarjetasNumTarjeta(strNumTarjeta);
            LinkedList<clsTarjeta> lkstSaldos = new LinkedList<clsTarjeta>();
            resultado.ForEach(tarjet => { lkstSaldos.AddLast(tarjet); });
            return lkstSaldos;
        }

    }
}
