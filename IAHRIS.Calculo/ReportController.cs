using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAHRIS.Calculo
{
    public class ReportController
    {
        Reports _reports;
        Calculations _calculations;
        public ReportController(DatosCalculo datos, bool usarCoeDiaria, MultiLangXML.MultiIdiomasXML traductor)
        {
            IAHRISDataSet dataset = new IAHRISDataSet();
            _reports = new Reports(datos, traductor, dataset);
            _calculations = new Calculations(datos, traductor, dataset);
        }
        public void EscribirCabecera(TestFechas.Simulacion sim, TestFechas.GeneracionInformes informes)
        {
            _reports.EscribirCabecera(sim, informes);
        }

        public void GenerarInforme1()
        {
            _calculations.CalcularINTERAnual();
            _reports.EscribirInforme1();
        }

        public void GenerarInforme1a()
        {
            _calculations.CalcularINTERAnualAlterada();
            _reports.EscribirInforme1a();
        }
        public void GenerarInforme1b()
        {
           
            _reports.EscribirInforme1b();
        }

        public void GenerarInforme2()
        {
            
            _calculations.CalcularParametrosNaturalesHabitualesReducidos();
            _reports.EscribirInforme2();
        }
        public void GenerarInforme2a()
        {
            _calculations.CalcularINTRAnualPorMeses(false);
            _reports.EscribirInforme2a();
        }

        public void generarInforme3()
        {
            
            _calculations.CalcularParametrosHabitualesReducidos();
            _reports.EscribirInforme3();
        }
        public void GenerarInforme3a()
        {
            _calculations.CalcularINTRAnualPorMeses(true);
            _reports.EscribirInforme3a();
        }
        public void GenerarInforme3b()
        {
            
            _reports.EscribirInforme3b();
        }


        public void GenerarInforme4()
        {
            _calculations.CalcularINTRAnualPorAños(false);
            _calculations.CalcularParametrosHabitualesCASO1();
            _calculations.CalculoParametrosVariabilidadDIARIAHabitual();
            _calculations.CalculoParametrosAvenidasCASO4();
            _calculations.CalculoParametrosSequiasCASO4();
            _reports.EscribirInforme4();
        }

        public void GenerarInforme4a()
        {
            _calculations.CalcularINTRAnualPorAños(false);
            _calculations.CalcularParametrosHabitualesCASO1();
            _reports.EscribirInforme4a();
        }

        //public void GenerarInforme4b()
        //{
            
        //    _reports.EscribirInforme4b();
        //}


        public void GenerarInforme5()
        {
            _calculations.CalcularINTRAnualPorAños(true);
            _calculations.CalcularParametrosHabitualesAlterados();
            _calculations.CalculoParametrosVariabilidadDIARIAHabitualAlterada();
            _calculations.CalculoParametrosAvenidasAlteradosCASO6();
            _calculations.CalculoParametrosSequiasAlteradosCASO6();
            _reports.EscribirInforme5();

            // Me._i23Simplificado = False
        }

        public void GenerarInforme5a()
        {
            _calculations.CalcularINTRAnualPorAños(true);
            _calculations.CalcularParametrosHabitualesAlterados();
            _reports.EscribirInforme5a();

            // Me._i23Simplificado = True
        }

        public void GenerarInforme5b()
        {
            _calculations.CalcularINTRAnualPorAños(true);
            _calculations.CalcularParametrosHabitualesAlterados();
            _calculations.CalculoParametrosVariabilidadDIARIAHabitualAlterada();
            _calculations.CalculoParametrosAvenidasSoloAlteradosCASO6();
            _calculations.CalculoParametrosSequiasSoloAlteradosCASO6();
            _reports.EscribirInforme5b();

            // Me._i23Simplificado = False
        }

        //public void GenerarInforme5c()
        //{
        //    _calculations.CalcularParametrosHabitualesReducidos();
        //    _calculations.CalculoParametrosVariabilidadDIARIAHabitualAlterada();
        //    _calculations.CalculoParametrosAvenidasAlteradosCASO6();
        //    _calculations.CalculoParametrosSequiasAlteradosCASO6();
        //    _reports.EscribirInforme5c();

        //    // Me._i23Simplificado = True
        //}
        public void GenerarInforme6()
        {
            _calculations.CalcularTablaCQC(false);
            //_calculations.CalcularTablaCQC(true);
            _reports.EscribirInforme6();
        }
        public void GenerarInforme6a()
        {
            _calculations.CalcularTablaCQC(true);
            _reports.EscribirInforme6a();
        }
        public void GenerarInforme6b()
        {
           // _calculations.CalcularTablaCQC(true);
            _reports.EscribirInforme6b();
        }
        public void GenerarInforme6c()
        {
            _calculations.CalcularCurvaMensualClasificadaCudales(false);
            _reports.EscribirInforme6c();
        }
        public void GenerarInforme6d()
        {
            _calculations.CalcularCurvaMensualClasificadaCudales(true);
            _reports.EscribirInforme6d();
        }
        public void GenerarInforme6e()
        {
            
            _reports.EscribirInforme6e();
        }
        public void GenerarInforme7a()
        {
            _calculations.CalcularIndicesHabitualesCASO3();
            _calculations.CalcularIndiceHabitual_I3();
            _calculations.CalcularIndiceAlteracionGlobalHabituales();
            _reports.EscribirInforme7a();
        }

        public void GenerarInforme7b()
        {
            _calculations.CalcularIndicesHabitualesCASO3();
            _calculations.CalcularIndiceAlteracionGlobalHabituales();
            _reports.EscribirInforme7b();
        }

        public void GenerarInforme7c()
        {
            _calculations.CalcularIndicesHabitualesAgregados();
            _calculations.CalcularIndiceAlteracionGlobalHabitualesAgregados();
            _reports.EscribirInforme7c();
        }

        public void GenerarInforme7d()
        {
            _calculations.CalcularIndicesAvenidasCASO6();
            _calculations.CalcularIndicesSequiasCASO6();
            _calculations.CalcularIndiceAlteracionGlobalAvenidas();
            _calculations.CalcularIndiceAlteracionGlobalSequias();
            _reports.EscribirInforme7d();
        }

        public void GenerarInforme8()
        {
            _calculations.CalcularRegimenNatural();
            _calculations.CalcularRegimenAlterado();
            _calculations.CalcularRegimenNaturalAnual();
            _calculations.CalcularRegimenAlteradoAnual();
            _reports.EscribirInforme8();
        }

        public void GenerarInforme8a()
        {
            _reports.EscribirInforme8a();
        }

        public void GenerarInforme8b()
        {
            _reports.EscribirInforme8b();
        }

        public void GenerarInforme8c()
        {
            _reports.EscribirInforme8c();
        }

        public void GenerarInforme8d()
        {
            _reports.EscribirInforme8d();
        }

      

        public void GenerarInforme9()
        {
            _calculations.CalcularReferencias();
            _reports.EscribirInforme9();
        }

        public void GenerarInforme9a()
        {
            _calculations.CalcularINTRAnualPorAños(false);
            _reports.EscribirInforme9a();
        }
        public void GenerarInforme9b()
        {
            //_calculations.CalcularINTRAnualPorAños(false);
            _reports.EscribirInforme9b();
        }
        public void GenerarInforme10a()
        {
            
            _reports.EscribirInforme10a();
        }
        public void GenerarInforme10b()
        {
            
            _reports.EscribirInforme10b();
        }
        public void GenerarInforme10c()
        {
            
            _reports.EscribirInforme10c();
        }
        public void GenerarInforme10d()
        {
            
            _reports.EscribirInforme10d();
        }

        public void EscribirFichero(bool coeD, bool coe)
        {
            _reports.CerrarExcel(coeD, coe);
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */


        public void BorrarInforme1()
        {
            _reports.DeleteReport(1);
        }
        public void BorrarInforme1a()
        {
            _reports.DeleteReport(2);
        }
        public void BorrarInforme1b()
        {
            _reports.DeleteReport(3);
        }
        public void BorrarInforme2()
        {
            _reports.DeleteReport(4);
        }
        public void BorrarInforme2a()
        {
            _reports.DeleteReport(5);
        }

        public void BorrarInforme3()
        {
            _reports.DeleteReport(6);
        }

        public void BorrarInforme3a()
        {
            _reports.DeleteReport(7);
        }
        public void BorrarInforme3b()
        {
            _reports.DeleteReport(8);
        }
        public void BorrarInforme4()
        {
            _reports.DeleteReport(9);
        }
        public void BorrarInforme4a()
        {
            _reports.DeleteReport(10);
        }

        //public void BorrarInforme4b()
        //{
        //    _reports.DeleteReport(11);
        //}

        

        public void BorrarInforme5()
        {
            _reports.DeleteReport(11);
        }

        public void BorrarInforme5a()
        {
            _reports.DeleteReport(12);
        }

        public void BorrarInforme5b()
        {
            _reports.DeleteReport(13);
        }


        public void BorrarInforme6()
        {
            _reports.DeleteReport(14);
        }

        public void BorrarInforme6a()
        {
            _reports.DeleteReport(15);
        }
        public void BorrarInforme6b()
        {
            _reports.DeleteReport(16);
        }
        public void BorrarInforme6c()
        {
            _reports.DeleteReport(17);
        }
        public void BorrarInforme6d()
        {
            _reports.DeleteReport(18);
        }
        public void BorrarInforme6e()
        {
            _reports.DeleteReport(19);
        }
        public void BorrarInforme7a()
        {
            _reports.DeleteReport(20);
        }

        public void BorrarInforme7b()
        {
            _reports.DeleteReport(21);
        }

        public void BorrarInforme7c()
        {
            _reports.DeleteReport(22);
        }

        public void BorrarInforme7d()
        {
            _reports.DeleteReport(23);
        }

        public void BorrarInforme8()
        {
            _reports.DeleteReport(24);
        }

        public void BorrarInforme8a()
        {
            _reports.DeleteReport(25);
        }

        public void BorrarInforme8b()
        {
            _reports.DeleteReport(26);
        }

        public void BorrarInforme8c()
        {
            _reports.DeleteReport(27);
        }

        public void BorrarInforme8d()
        {
            _reports.DeleteReport(28);
        }

        public void BorrarInforme9()
        {
            _reports.DeleteReport(29);
        }

        public void BorrarInforme9a()
        {
            _reports.DeleteReport(30);
        }
        public void BorrarInforme9b()
        {
            _reports.DeleteReport(31);
        }
        public void BorrarInforme10a()
        {
            _reports.DeleteReport(32);
        }
        public void BorrarInforme10b()
        {
            _reports.DeleteReport(33);
        }
        public void BorrarInforme10c()
        {
            _reports.DeleteReport(34);
        }
        public void BorrarInforme10d()
        {
            _reports.DeleteReport(35);
        }
    }
}
