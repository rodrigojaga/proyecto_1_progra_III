namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsTarjeta
    {

        public string nombreTarjeta { get; set; }
        public string numTarjeta { get; set; }
        public string saldo { get; set; }
        public string banco { get; set; }
        public string tipo { get; set; }
        public string correo { get; set; }
        


        public clsTarjeta(string nombreTarjeta, string numTarjeta, string saldo, string banco, string tipo, string correo)
        {
            this.nombreTarjeta = nombreTarjeta;
            this.numTarjeta = numTarjeta;
            this.saldo = saldo;
            this.banco = banco;
            this.tipo = tipo;
            this.correo = correo;
        }

        

    }
}
