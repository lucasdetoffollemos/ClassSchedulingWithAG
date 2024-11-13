using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace ClassSchedulingWithAG.Services
{
    //criar um array para as disciplinas de todas as matérias da semana, com o tamanho
    //gerar aleatorio os códigos, mesmo que se repitam


    //fitness
    //verificar choque de professors, onde para cada dia da semana
    //criar um array com os professores, e a cada aula que ele da, acrecento 1
    //caso esse array algum professor tenha nota 2 ou mais, descontar da nota do cromosso
    //nota do cromosso = 100
    //verificar disponibilidade dos professores, caso no dia eles não possam dar aula
    //descontar mais um ponto do cromossomo
    //verificar questão de turmas = ch 40 = 1 aula, 80 = 2 aulas 


    //seleção/cruzamento

    public class AlgoritmoGeneticoService
    {
        public Horario CalculaHOrariosComAlgoritmoGnético(InputData inputData)
        {

            IniciaPopulacao(inputData);
            
            return null;
        }

        private void IniciaPopulacao(InputData inputData)
        {
            var cromossomo = new Cromossomo();

            int currentIndex = 0;

            foreach (var curso in inputData.Cursos)
            {

                var disciplinasPorFase = curso.Disciplinas.GroupBy(x => x.Fase).ToList();

                var fasesDoCurso = disciplinasPorFase.Count();

                var qntDisciplinas = curso.Disciplinas.Count();

                for (int i = 0; i < fasesDoCurso; i++)
                {
                    var codigosAleatoriosDasDisciplinas = GeraHorariosAleatorios(disciplinasPorFase[i].Select(x => x.codigo).ToList());

                    for (int j = 0; j < codigosAleatoriosDasDisciplinas.Length; j++)
                    {
                        if (currentIndex < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length)
                        {
                            cromossomo.DiasDaSemanaECodigosDasDisciplinas[currentIndex] = codigosAleatoriosDasDisciplinas[j];
                            currentIndex++;
                        }
                    }
                }
            }

            cromossomo.Nota = 100;

        }

        private int[] GeraHorariosAleatorios(List<int> disciplinasPorFase)
        {
            var random = new Random();
            var disciplinaAleatorias = new int[10];
            int i = 0;
            while (i < 10)
            {
                int numberandon = disciplinasPorFase[random.Next(0, disciplinasPorFase.Count)];

                //var numberRandon = disciplinasPorFase[numberandonIndex];

                disciplinaAleatorias[i] = numberandon;

                i++;
            }

            return disciplinaAleatorias;

        }
    }
}
