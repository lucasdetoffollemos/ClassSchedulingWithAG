using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace ClassSchedulingWithAG.Services
{
    public class AlgoritmoGeneticoService
    {
        public Horario CalculaHOrariosComAlgoritmoGnético(InputData inputData)
        {
            var horario = new Horario(inputData.Cursos, inputData.Professores);

            var teste = horario.MapaDeHorarios;

            var population = new List<Horario>();

            for (int i = 0; i < 10; i++) // População inicial
            {
                population.Add(new Horario(inputData.Cursos, inputData.Professores));
            }


            return null;
        }
    }
}
