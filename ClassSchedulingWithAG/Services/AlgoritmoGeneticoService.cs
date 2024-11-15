﻿using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.Intrinsics.X86;

namespace ClassSchedulingWithAG.Services
{
    //criar um array para as disciplinas de todas as matérias da semana, com o tamanho
    //gerar aleatorio os códigos, mesmo que se repitam


    //fitness
    //(verificar choque de professors, onde para cada dia da semana
    //criar um array com os professores, e a cada aula que ele da, acrecento 1
    //caso esse array algum professor tenha nota 2 ou mais, descontar da nota do cromosso
    //nota do cromosso = 100) feito

    //verificar disponibilidade dos professores, caso no dia eles não possam dar aula
    //descontar mais um ponto do cromossomo


    //verificar questão de turmas = ch 40 = 1 aula, 80 = 2 aulas 


    //seleção/cruzamento

    //mutacao

    public class AlgoritmoGeneticoService
    {
        public Horario CalculaHOrariosComAlgoritmoGnético(InputData inputData)
        {

            List<Cromossomo> populacao = new List<Cromossomo>();

            for (int i = 0; i < 10; i++)
            {
                populacao.Add(IniciaPopulacao(inputData));
            }

            Fitness(populacao, inputData);

            return null;
        }

        private void Fitness(List<Cromossomo> populacao, InputData inputData)
        {
            RetiraNotaDoCromossomoBaseadoNosConflitosDeHorarioDosProfessores(populacao, inputData);
            VerificaDisponibilidadeProfessor(populacao, inputData);

        }

        private void VerificaDisponibilidadeProfessor(List<Cromossomo> populacao, InputData inputData)
        {
            foreach (var cromossomo in populacao)
            {
                for (int i = 0; i < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length; i++)
                {
                    var disciplina = GetDisciplinaByCodigo(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i], inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());

                    if(disciplina != null)
                    {
                        var nomeProfessor = disciplina.Professor;

                        Professor prof = GetProfessorByName(nomeProfessor, inputData.Professores);

                        //se ele não pode dar aula algum dia
                        if (prof != null) 
                        {
                            if (!prof.Disponibilidade.Segunda)
                            {
                                VerificaSeProfessorEstaEmAlgumaDisciplina(0, 1, nomeProfessor, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList(), cromossomo);
                            }
                            if (!prof.Disponibilidade.Terca)
                            {
                                VerificaSeProfessorEstaEmAlgumaDisciplina(2, 3, nomeProfessor, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList(), cromossomo);
                            }
                            if (!prof.Disponibilidade.Quarta)
                            {
                                VerificaSeProfessorEstaEmAlgumaDisciplina(4, 5, nomeProfessor, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList(), cromossomo);
                            }
                            if (!prof.Disponibilidade.Quinta) {
                                VerificaSeProfessorEstaEmAlgumaDisciplina(6, 7, nomeProfessor, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList(), cromossomo);
                            }
                            if (!prof.Disponibilidade.Sexta)
                            {
                                VerificaSeProfessorEstaEmAlgumaDisciplina(8, 9, nomeProfessor, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList(), cromossomo);
                            }




                        }

                         
                        //verificar se alguma ele da aula
                        //caso de descontar um ponto da nota
                    }

                }
            }
        }

        private void VerificaSeProfessorEstaEmAlgumaDisciplina(int periodo1, int periodo2, string nomeProfessor, List<Disciplina> disciplinas, Cromossomo cromossomo)
        {
            List<int> codPrimeiroESegundoPerido = new List<int>();

            for (int i = 0; i < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length; i++) 
            {
                if(NumberEndsWith(i, periodo1) || NumberEndsWith(i, periodo2))
                {
                    //add todas as disciplinas do dia
                    codPrimeiroESegundoPerido.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                }
            }

            //verifica se alguma dessas disciplinas é ministrada pelo professor
            //caso for, desconta 1 ponto
            foreach (var cod in codPrimeiroESegundoPerido)
            {
                var disciplina = GetDisciplinaByCodigo(cod, disciplinas);

                if (disciplina != null)
                {
                    if (disciplina.Professor.Equals(nomeProfessor))
                        cromossomo.Nota = cromossomo.Nota - 1;
                }
            }
        }

        private Professor GetProfessorByName(string nomeProfessor, List<Professor> professores)
        {
            return professores.FirstOrDefault(x => x.Nome.Equals(nomeProfessor));
        }

        private void RetiraNotaDoCromossomoBaseadoNosConflitosDeHorarioDosProfessores(List<Cromossomo> populacao, InputData inputData)
        {
            //pegar os cursos
            //pegar as disciplinas
            //pegar o professor da e o código da disciplina
            foreach (var cromossomo in populacao)
            {
                //manha
                var listSegundaPrimeiroPeriodoManha = new List<int>();
                var listSegundaSegundoPeriodoManha = new List<int>();
                var listTercaPrimeiroPeriodoManha = new List<int>();
                var listTercaSegundoPeriodoManha = new List<int>();
                var listQuartaPrimeiroPeriodoManha = new List<int>();
                var listQuartaSegundoPeriodoManha = new List<int>();
                var listQuintaPrimeiroPeriodoManha = new List<int>();
                var listQuintaSegundoPeriodoManha = new List<int>();
                var listSextaPrimeiroPeriodoManha = new List<int>();
                var listSextaSegundoPeriodoManha = new List<int>();


                for (int i = 0; i < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length / 2; i++)
                {
                    if (NumberEndsWith(i, 0))
                    {
                        listSegundaPrimeiroPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 1))
                    {
                        listSegundaSegundoPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 2))
                    {
                        listTercaPrimeiroPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 3))
                    {
                        listTercaSegundoPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 4))
                    {
                        listQuartaPrimeiroPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 5))
                    {
                        listQuartaSegundoPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 6))
                    {
                        listQuintaPrimeiroPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 7))
                    {
                        listQuintaSegundoPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }


                    if (NumberEndsWith(i, 8))
                    {
                        listSextaPrimeiroPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 9))
                    {
                        listSextaSegundoPeriodoManha.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }


                }

                //pegar as diciplina baseadas nas lista de periodo
                //verificar se o professor aparece mais de uma vez
                //cada vez que o professor aparecer, acrescentar uma nota
                //somar todas as notas e descontar do cromossomo
                //segunda primeiro periodo
                Dictionary<string, int> professoresESuasNotasManha = PreencherDicitionaryProfessores(inputData.Professores);
                foreach (var cod in listSegundaPrimeiroPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSegundaSegundoPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listTercaPrimeiroPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listTercaSegundoPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuartaPrimeiroPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuartaSegundoPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuintaPrimeiroPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuintaSegundoPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSextaPrimeiroPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSextaSegundoPeriodoManha)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasManha.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasManha[nomeProfessor]++;
                        }

                    }
                }

                //falta agora arrumar os horarios para professores que dão aula em matutino e ves´p
                //não deixar conflitar

                //somar todos as notas de professores que tem conflito e diminuir do cromossomo
                var notaGeralManha = professoresESuasNotasManha.Values.Where(x => x > 1).Sum();


                //tarde
                var listSegundaPrimeiroPeriodoTarde = new List<int>();
                var listSegundaSegundoPeriodoTarde = new List<int>();
                var listTercaPrimeiroPeriodoTarde = new List<int>();
                var listTercaSegundoPeriodoTarde = new List<int>();
                var listQuartaPrimeiroPeriodoTarde = new List<int>();
                var listQuartaSegundoPeriodoTarde = new List<int>();
                var listQuintaPrimeiroPeriodoTarde = new List<int>();
                var listQuintaSegundoPeriodoTarde = new List<int>();
                var listSextaPrimeiroPeriodoTarde = new List<int>();
                var listSextaSegundoPeriodoTarde = new List<int>();


                for (int i = cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length / 2; i < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length; i++)
                {
                    if (NumberEndsWith(i, 0))
                    {
                        listSegundaPrimeiroPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 1))
                    {
                        listSegundaSegundoPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 2))
                    {
                        listTercaPrimeiroPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 3))
                    {
                        listTercaSegundoPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 4))
                    {
                        listQuartaPrimeiroPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 5))
                    {
                        listQuartaSegundoPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 6))
                    {
                        listQuintaPrimeiroPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 7))
                    {
                        listQuintaSegundoPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }


                    if (NumberEndsWith(i, 8))
                    {
                        listSextaPrimeiroPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }

                    if (NumberEndsWith(i, 9))
                    {
                        listSextaSegundoPeriodoTarde.Add(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i]);
                    }
                }

                Dictionary<string, int> professoresESuasNotasTarde = PreencherDicitionaryProfessores(inputData.Professores);
                foreach (var cod in listSegundaPrimeiroPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSegundaSegundoPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listTercaPrimeiroPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listTercaSegundoPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuartaPrimeiroPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuartaSegundoPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuintaPrimeiroPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listQuintaSegundoPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSextaPrimeiroPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }

                foreach (var cod in listSextaSegundoPeriodoTarde)
                {
                    //pegar o professor q da aula
                    if (cod != 0)
                    {
                        var disciplina = GetDisciplinaByCodigo(cod, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());


                        var nomeProfessor = disciplina.Professor;

                        if (professoresESuasNotasTarde.ContainsKey(nomeProfessor))
                        {
                            //incrementa nota proferror
                            professoresESuasNotasTarde[nomeProfessor]++;
                        }

                    }
                }


                //somar todos as notas de professores que tem conflito e diminuir do cromossomo
                var notaGeralTarde = professoresESuasNotasTarde.Values.Where(x => x > 1).Sum();

                cromossomo.Nota = cromossomo.Nota - (notaGeralManha + notaGeralTarde);
            }
        }

        private Dictionary<string, int> PreencherDicitionaryProfessores(List<Professor> professores)
        {
            var professoresESuasNotas = new Dictionary<string, int>();
            foreach (var prof in professores)
            {
                professoresESuasNotas.Add(prof.Nome, 0);
            }

            return professoresESuasNotas;
        }

        bool NumberEndsWith(int number, int end)
        {
            return number % 10 == end;
        }

        private Disciplina GetDisciplinaByCodigo(int codigoDisciplina, List<Disciplina> disciplinas)
        {
            if(codigoDisciplina == 0)
            {
                return null;
            }
            return disciplinas.FirstOrDefault(x => x.Codigo == codigoDisciplina);
        }

        private Cromossomo IniciaPopulacao(InputData inputData)
        {
            var cromossomo = new Cromossomo();

            int currentIndex = 0;

            foreach (var curso in inputData.Cursos)
            {
                var nomeCurso = curso.Nome;
                var disciplinasPorFase = curso.Disciplinas.GroupBy(x => x.Fase).ToList();

                var fasesDoCurso = disciplinasPorFase.Count();

                var qntDisciplinas = curso.Disciplinas.Count();

                for (int i = 0; i < fasesDoCurso; i++)
                {
                    var codigosAleatoriosDasDisciplinas = GeraHorariosAleatorios(disciplinasPorFase[i].Select(x => x.Codigo).ToList(), nomeCurso);

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

            cromossomo.Nota = 1000;

            return cromossomo;

        }

        private int[] GeraHorariosAleatorios(List<int> disciplinasPorFase, string nomeCurso)
        {
            var random = new Random();
            var disciplinaAleatorias = new int[10];
            int i = 0;
            while (i < 10)
            {
                if ((i == 0 || i == 1) && nomeCurso.ToLower().Contains("técnico"))
                {
                    disciplinaAleatorias[i] = 0;
                }
                else
                {
                    int numberandon = disciplinasPorFase[random.Next(0, disciplinasPorFase.Count)];

                    //var numberRandon = disciplinasPorFase[numberandonIndex];

                    disciplinaAleatorias[i] = numberandon;
                }

                i++;


            }

            return disciplinaAleatorias;

        }
    }
}
