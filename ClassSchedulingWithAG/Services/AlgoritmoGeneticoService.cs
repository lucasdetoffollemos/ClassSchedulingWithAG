using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using System;
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

    //(verificar disponibilidade dos professores, caso no dia eles não possam dar aula
    //descontar mais um ponto do cromossomo) feito


    //(verificar questão de turmas = ch 40 = 1 aula, 80 = 2 aulas, 120 = 3 aulas) feito


    //seleção/cruzamento

    //mutacao

    public class AlgoritmoGeneticoService
    {
        public int[] CalculaHOrariosComAlgoritmoGnético(InputData inputData)
        {

            List<Cromossomo> populacao = new List<Cromossomo>();

            //esse 10 sera o item de qtnd cromossomos 
            for (int i = 0; i < 10; i++)
            {
                populacao.Add(IniciaPopulacao(inputData));
            }

            //aqui teremo um for com a qntd max de iterações

            Fitness(populacao, inputData);

            var cromossoSelecionado = populacao.SingleOrDefault(x => x.Nota == 1000);

            if (cromossoSelecionado != null)
                return cromossoSelecionado.DiasDaSemanaECodigosDasDisciplinas;

            List<Cromossomo> novaPopulacao = new List<Cromossomo>();
            //esse 10 sera o item de qtnd cromossomos dividido por 2
            for (int i = 0; i < 5; i++)
            {
                var pais = new List<Cromossomo>();
                for (int j = 0; j < 2; j++)
                {
                    var cromossomoPai = Selecao(populacao);
                    pais.Add(cromossomoPai);
                }

                //probabilidade de cruzamento, valor que vai vir do front
                novaPopulacao.AddRange(Cruzamento(pais[0], pais[1], 0.7));
            }


            return null;
        }

        private List<Cromossomo> Cruzamento(Cromossomo pai1, Cromossomo pai2, double probabilidadeCruzamento)
        {
            var random = new Random();
            // Verifica se o cruzamento ocorre com base na probabilidade
            if (random.NextDouble() > probabilidadeCruzamento)
            {
                // Se não ocorrer cruzamento, os filhos são cópias dos pais
                return new List<Cromossomo>
                {
                new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = (int[])pai1.DiasDaSemanaECodigosDasDisciplinas.Clone() },
                new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = (int[])pai2.DiasDaSemanaECodigosDasDisciplinas.Clone() }
                };
            }

            int tamanho = pai1.DiasDaSemanaECodigosDasDisciplinas.Length;
            int pontoDeCorte = random.Next(1, tamanho);

            Cromossomo filho1 = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[tamanho],
                Nota = 1000
            };

            Cromossomo filho2 = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[tamanho],
                Nota = 1000
            };

            // Realiza o cruzamento no ponto de corte
            for (int i = 0; i < tamanho; i++)
            {
                if (i < pontoDeCorte)
                {
                    filho1.DiasDaSemanaECodigosDasDisciplinas[i] = pai1.DiasDaSemanaECodigosDasDisciplinas[i];
                    filho2.DiasDaSemanaECodigosDasDisciplinas[i] = pai2.DiasDaSemanaECodigosDasDisciplinas[i];
                }
                else
                {
                    filho1.DiasDaSemanaECodigosDasDisciplinas[i] = pai2.DiasDaSemanaECodigosDasDisciplinas[i];
                    filho2.DiasDaSemanaECodigosDasDisciplinas[i] = pai1.DiasDaSemanaECodigosDasDisciplinas[i];
                }
            }

            return new List<Cromossomo> { filho1, filho2 };
        }

        private Cromossomo Selecao(List<Cromossomo> populacao)
        {
            //selecionar individuos para cruzar
            var random = new Random();

            // Calcula o somatório de todas as notas
            int somaNotas = populacao.Sum(c => c.Nota);

            // Gera um número aleatório entre 0 e a soma total das notas
            int roleta = random.Next(0, somaNotas);

            // Percorre a população acumulando as notas até passar o valor da roleta
            int acumulador = 0;
            foreach (var cromossomo in populacao)
            {
                acumulador += cromossomo.Nota;
                if (acumulador >= roleta)
                {
                    return cromossomo;
                }
            }

            // Retorna o último cromossomo por segurança (caso a roleta não tenha selecionado antes)
            return populacao.Last();


        }

        private void Fitness(List<Cromossomo> populacao, InputData inputData)
        {
            RetiraNotaDoCromossomoBaseadoNosConflitosDeHorarioDosProfessores(populacao, inputData);
            VerificaDisponibilidadeProfessor(populacao, inputData);
            VerificaCargaHorarioBateComQntDeAulasNaSemana(populacao, inputData);
        }

        private void VerificaCargaHorarioBateComQntDeAulasNaSemana(List<Cromossomo> populacao, InputData inputData)
        {
            foreach (var cromossomo in populacao)
            {
                var listDiasDaSemanaECodigosDasDisciplinas = cromossomo.DiasDaSemanaECodigosDasDisciplinas.ToList();


                //quebro de 10 em 10 o array de horarios
                var diasDaSemanaECodigosDasDisciplinasGrouped = listDiasDaSemanaECodigosDasDisciplinas
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / 10)
                .Select(g => g.Select(x => x.value).ToList())
                .ToList();

                foreach (var disciplinasDaSemana in diasDaSemanaECodigosDasDisciplinasGrouped)
                {
                    foreach (var codDis in disciplinasDaSemana)
                    {
                        var disciplina = GetDisciplinaByCodigo(codDis, inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());

                        if (disciplina != null)
                        {
                            var cargaHoraria = disciplina.CH;

                            //somando 2 quando a carga horario é 40, pq vai ser visitada só uma vez
                            //já as outras serão visitadas mais vezes
                            if (cargaHoraria == 40 && disciplinasDaSemana.Count(x => x == codDis) == 1)
                            {
                                cromossomo.Nota = cromossomo.Nota + 2;
                            }
                            if (cargaHoraria == 80 && disciplinasDaSemana.Count(x => x == codDis) == 2)
                            {
                                cromossomo.Nota = cromossomo.Nota + 1;
                            }

                            if (cargaHoraria == 120 && disciplinasDaSemana.Count(x => x == codDis) == 3)
                            {
                                cromossomo.Nota = cromossomo.Nota + 1;
                            }

                        }


                    }
                }
            }
        }
        // 
        private void VerificaDisponibilidadeProfessor(List<Cromossomo> populacao, InputData inputData)
        {
            foreach (var cromossomo in populacao)
            {
                for (int i = 0; i < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length; i++)
                {
                    var disciplina = GetDisciplinaByCodigo(cromossomo.DiasDaSemanaECodigosDasDisciplinas[i], inputData.Cursos.SelectMany(x => x.Disciplinas).ToList());

                    if (disciplina != null)
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
                            if (!prof.Disponibilidade.Quinta)
                            {
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
                if (NumberEndsWith(i, periodo1) || NumberEndsWith(i, periodo2))
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
            if (codigoDisciplina == 0)
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

                // Certifique-se de que há pelo menos uma fase
                if (disciplinasPorFase.Count == 0)
                    continue;

                var fasesDoCurso = disciplinasPorFase.Count;
                var qntDisciplinas = curso.Disciplinas.Count();

                for (int i = 0; i < fasesDoCurso; i++)
                {
                    var codigosAleatoriosDasDisciplinas = GeraHorariosAleatorios(disciplinasPorFase[i].Select(x => x.Codigo).ToList(), nomeCurso);

                    // Verifique se o array de códigos não está vazio
                    if (codigosAleatoriosDasDisciplinas != null && codigosAleatoriosDasDisciplinas.Length > 0)
                    {
                        for (int j = 0; j < codigosAleatoriosDasDisciplinas.Length; j++)
                        {
                            // Verifique se o índice atual não ultrapassa o limite do array
                            if (currentIndex < cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length)
                            {
                                cromossomo.DiasDaSemanaECodigosDasDisciplinas[currentIndex] = codigosAleatoriosDasDisciplinas[j];
                                currentIndex++;
                            }
                            else
                            {
                                // Caso o índice ultrapasse o tamanho do array, podemos interromper
                                break;
                            }
                        }
                    }
                }
            }

            // Defina a nota de forma mais dinâmica, caso necessário (aqui apenas um valor fixo)
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

        private List<Cromossomo> SelecionaCromossomos(List<Cromossomo> populacao, int quantidadeSelecao)
        {
            var selecionados = new List<Cromossomo>();
            var random = new Random();

            // Seleção por torneio
            for (int i = 0; i < quantidadeSelecao; i++)
            {
                var torneio = new List<Cromossomo>();

                // Escolhe aleatoriamente 2 cromossomos diferentes para o torneio
                while (torneio.Count < 2)
                {
                    var indiceAleatorio = random.Next(populacao.Count);
                    var cromossomoAleatorio = populacao[indiceAleatorio];

                    // Evita selecionar o mesmo cromossomo mais de uma vez
                    if (!torneio.Contains(cromossomoAleatorio))
                    {
                        torneio.Add(cromossomoAleatorio);
                    }
                }

                // Seleciona o cromossomo com maior nota do torneio
                var melhorCromossomo = torneio.OrderByDescending(x => x.Nota).First();
                selecionados.Add(melhorCromossomo);
            }

            return selecionados;
        }

        private void Mutacao(Cromossomo cromossomo, List<Curso> cursos)
        {
            var random = new Random();

            // Exemplo de mutação: altera aleatoriamente um gene do cromossomo
            int indiceGene = random.Next(cromossomo.DiasDaSemanaECodigosDasDisciplinas.Length);

            // Obter o código da disciplina no gene selecionado
            int codigoDisciplinaAleatorio = cromossomo.DiasDaSemanaECodigosDasDisciplinas[indiceGene];

            // Encontrar um curso que contenha a disciplina com o código correspondente
            var curso = cursos.FirstOrDefault(c => c.Disciplinas.Any(d => d.Codigo == codigoDisciplinaAleatorio));

            if (curso != null)
            {
                // Encontrar uma disciplina aleatória dentro do curso
                var disciplinaAleatoria = curso.Disciplinas[random.Next(curso.Disciplinas.Count)];

                // Aplica a mutação: substitui o código de disciplina do gene
                cromossomo.DiasDaSemanaECodigosDasDisciplinas[indiceGene] = disciplinaAleatoria.Codigo;
            }
        }

        // não sei se precisa da evolução, caso precise já temos alguma coisa.
        private void Evolucao(InputData inputData)
        {
            int tamanhoPopulacao = 100;
            int numeroDeGeracoes = 50;  
            int quantidadeSelecao = 50;  

            var populacao = new List<Cromossomo>();

            
            for (int i = 0; i < tamanhoPopulacao; i++)
            {
                var cromossomo = IniciaPopulacao(inputData);
                populacao.Add(cromossomo);
            }

            for (int geracao = 0; geracao < numeroDeGeracoes; geracao++)
            {
                var novaPopulacao = new List<Cromossomo>();

                // seleção dos cromossomos para o cruzamento
                var cromossomosSelecionados = SelecionaCromossomos(populacao, quantidadeSelecao);

                // Realiza o cruzamento para gerar novos cromossomos
                foreach (var cromossomo in cromossomosSelecionados)
                {
                    var cromossomoFilho = Cruzamento(cromossomo);
                    novaPopulacao.Add(cromossomoFilho);
                }

                // aplica a mutação nos cromossomos filhos
                foreach (var cromossomo in novaPopulacao)
                {
                    Mutacao(cromossomo, inputData.Cursos);
                }

                // atualiza a população para a próxima geração
                populacao = novaPopulacao;
            }
        }

        private Cromossomo Cruzamento(Cromossomo cromossomoPai)
        {
            var cromossomoFilho = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = (int[])cromossomoPai.DiasDaSemanaECodigosDasDisciplinas.Clone()
            };

            return cromossomoFilho;
        }

    }
}
