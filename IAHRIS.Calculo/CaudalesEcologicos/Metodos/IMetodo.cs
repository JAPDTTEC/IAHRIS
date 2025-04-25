using IAHRIS.Calculo.CaudalesEcologicos.Escenarios;


namespace IAHRIS.Calculo.CaudalesEcologicos.Metodos
{
    
    public interface IMetodo
    {
        string Nombre { get; set; }
        double[] CalcularCaudalesEcologicos(double[][] caudales);
    }
}
