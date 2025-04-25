namespace IAHRIS.Calculo.CaudalesEcologicos.Escenarios
{
    interface IEscenario
    {
        void CargarDatos(double[] datos);

        EscenarioDTO GetEscenario(int id);
  
    }
}
