using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace ClassSchedulingWithAG.Services
{
    public class AlgoritmoGeneticoService
    {
        public Horario CalculaHOrariosComAlgoritmoGnético(InputData inputData)
        {
            //Dictionary<string, int[]> diasDaSemanaECodigosDasDisciplinas = new Dictionary<string, int[]>();
            //ex: 0: segunda : 

            foreach (var curso in inputData.Cursos) 
            {

                var disciplinasPorFase = curso.Disciplinas.GroupBy(x => x.Fase).ToList();
                var fasesDoCurso = disciplinasPorFase.Count();

                var qntDisciplinas = curso.Disciplinas.Count();

                for (int i = 0; i < fasesDoCurso; i++) 
                {
                    GeraHorariosAleatorios(disciplinasPorFase[i].Select(x => x.codigo).ToList());
                    

                }
                
            }
            //computação de 1 a 10 a segunda fase, inserir os códigos das disiplinas
            //computação de 11 a 20 a quarta fase
            //computação de 21 a 30 a sexta fase
            //computação de 31 a 40 a oitava fase
            

            //para as disciplinas que não tem aulas todos os dias, preencher com código 0

            /*var horario = new Horario(inputData.Cursos, inputData.Professores);

            var teste = horario.MapaDeHorarios;

            var population = new List<Horario>();

            for (int i = 0; i < 10; i++) // População inicial
            {
                population.Add(new Horario(inputData.Cursos, inputData.Professores));
            }*/

            return null;
        }

        private void GeraHorariosAleatorios(List<int> disciplinasPorFase)
        {
            var random = new Random();
            var disciplinaAleatorias = new int[10];
            int i = 0;
            List<int> numbersUsed = new List<int>();
            while (i < 10)
            {

                int numberandon = disciplinasPorFase[random.Next(disciplinasPorFase.First()-1, disciplinasPorFase.Last())];

                if (NumberCanBeUsed(numberandon, numbersUsed))
                {
                    disciplinaAleatorias[i] = numberandon;
                    numbersUsed.Add(numberandon);
                    i++;
                }
                    
            }


        }

        private bool NumberCanBeUsed(int numberandon, List<int> numbersUsed)
        {
            var countNumbers = numbersUsed.Where(x => x == numberandon);

            if (countNumbers.Count() >= 2)
            {
                return false;
            }

            return true;
        }
    }
}
