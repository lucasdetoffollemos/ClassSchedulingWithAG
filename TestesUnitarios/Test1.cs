using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.ViewModels;
using ClassSchedulingWithAG.Services;
using System.Reflection;

namespace Testes
{
    [TestClass]
    public sealed class TestUnitarios
    {

        private InputData CriarInputDataTeste()
        {
            return new InputData
            {
                Cursos = new List<Curso>
        {
            new Curso
            {
                Nome = "Curso de Teste",
                Disciplinas = new List<Disciplina>
                {
                    new Disciplina { Codigo = 1, Nome = "Disciplina1", Fase = 1, Professor = "Prof1", CH = 40 },
                    new Disciplina { Codigo = 2, Nome = "Disciplina2", Fase = 1, Professor = "Prof2", CH = 30 },
                    new Disciplina { Codigo = 3, Nome = "Disciplina3", Fase = 1, Professor = "Prof3", CH = 20 },
                    new Disciplina { Codigo = 4, Nome = "Disciplina4", Fase = 1, Professor = "Prof4", CH = 10 }
                }
            }
        }
            };
        }

        [TestMethod]
        public void Mutacao_SemMutacao_NaoAlteraCromossomo()
        {
            // Arrange
            var cromossomo = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[] { 1, 2, 3, 4, 5 }
            };

            var inputData = new InputData
            {
                Cursos = new List<Curso>()
            };

            var algoritmoGenetico = new AlgoritmoGeneticoService();
            var original = (int[])cromossomo.DiasDaSemanaECodigosDasDisciplinas.Clone();

            // Act
            algoritmoGenetico.Mutacao(cromossomo, inputData, probabilidadeMutacao: 0.0);

            // Assert
            CollectionAssert.AreEqual(original, cromossomo.DiasDaSemanaECodigosDasDisciplinas);
        }

        [TestMethod]
        public void Mutacao_AltaProbabilidade_AlteraCromossomo()
        {
            // Arrange
            var cromossomo = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[] { 1, 2, 3, 4, 5 }
            };

            var inputData = new InputData
            {
                Cursos = new List<Curso>
        {
            new Curso
            {
                Disciplinas = new List<Disciplina>
                {
                    new Disciplina { Codigo = 1, Nome = "Disciplina1", Fase = 1, Professor = "Prof1", CH = 40 },
                    new Disciplina { Codigo = 2, Nome = "Disciplina2", Fase = 1, Professor = "Prof2", CH = 30 },
                    new Disciplina { Codigo = 3, Nome = "Disciplina3", Fase = 1, Professor = "Prof3", CH = 20 },
                    new Disciplina { Codigo = 4, Nome = "Disciplina4", Fase = 1, Professor = "Prof4", CH = 10 },
                    new Disciplina { Codigo = 5, Nome = "Disciplina5", Fase = 1, Professor = "Prof5", CH = 50 }
                }
            }
        }
            };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.Mutacao(cromossomo, inputData, probabilidadeMutacao: 1.0);

            // Assert
            CollectionAssert.Contains(new int[] { 1, 2, 3, 4, 5 }, cromossomo.DiasDaSemanaECodigosDasDisciplinas[0]);
        }

        [TestMethod]
        public void Mutacao_ComProbabilidadeZero_NaoAlteraCromossomo()
        {
            // Arrange
            var cromossomo = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[] { 1, 2, 3, 4, 5 }
            };

            var inputData = new InputData
            {
                Cursos = new List<Curso>
            {
                new Curso
                {
                    Disciplinas = new List<Disciplina>
                    {
                        new Disciplina { Codigo = 1, Nome = "Disciplina1", Fase = 1, Professor = "Prof1", CH = 40 },
                        new Disciplina { Codigo = 2, Nome = "Disciplina2", Fase = 1, Professor = "Prof2", CH = 30 },
                        new Disciplina { Codigo = 3, Nome = "Disciplina3", Fase = 1, Professor = "Prof3", CH = 20 },
                        new Disciplina { Codigo = 4, Nome = "Disciplina4", Fase = 1, Professor = "Prof4", CH = 10 },
                        new Disciplina { Codigo = 5, Nome = "Disciplina5", Fase = 1, Professor = "Prof5", CH = 50 }
                    }
                }
            }
            };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.Mutacao(cromossomo, inputData, probabilidadeMutacao: 0.0);

            // Assert
            // Verifica que o cromossomo permanece inalterado
            CollectionAssert.AreEqual(
                cromossomo.DiasDaSemanaECodigosDasDisciplinas,
                new int[] { 1, 2, 3, 4, 5 }
            );
        }

        [TestMethod]
        public void CalculaHorarios_ComParametrosInvalidos_DeveRetornarNull()
        {
            // Arrange
            var inputData = CriarInputDataTeste();
            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            var resultado = algoritmoGenetico.CalculaHOrariosComAlgoritmoGnético(
                inputData,
                cromossomos: 0,
                cromossomosPorElitismo: 0,
                probabilidadeCruzamento: 0,
                probabilidadeMutacao: 0,
                quantidadeMaxInteracoes: 0,
                interacoesSemMelhorias: 0
            );

            // Assert
            Assert.IsNull(resultado, "O método deve retornar null quando os parâmetros são inválidos.");
        }

        [TestMethod]
        public void GeraHorariosAleatorios_ComNomeCursoTecnico_DeveRetornarArrayComZerosNosPrimeirosIndices()
        {
            // Arrange
            var algoritmoGenetico = new AlgoritmoGeneticoService();
            var disciplinasPorFase = new List<int> { 1, 2, 3, 4, 5 }; // Apenas alguns exemplos de disciplinas
            string nomeCurso = "Técnico em Informática"; // Nome do curso que contém "técnico"

            // Act
            var resultado = algoritmoGenetico.GeraHorariosAleatorios(disciplinasPorFase, nomeCurso);

            // Assert
            Assert.AreEqual(0, resultado[0], "O primeiro índice deve ser 0 quando o nome do curso contém 'técnico'.");
            Assert.AreEqual(0, resultado[1], "O segundo índice deve ser 0 quando o nome do curso contém 'técnico'.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GeraHorariosAleatorios_ComListaDeDisciplinasVazia_DeveLancarExcecao()
        {
            // Arrange
            var algoritmoGenetico = new AlgoritmoGeneticoService();
            var disciplinasPorFase = new List<int>(); // lista vem vazia
            string nomeCurso = "Curso Superior";

            // Act
            var resultado = algoritmoGenetico.GeraHorariosAleatorios(disciplinasPorFase, nomeCurso);

            // Assert
            // nao precisa de assert, pode por deixar vazio pq a lista está vazia
        }

        [TestMethod]
        public void GeraHorariosAleatorios_ComNomeCursoNaoTecnico_DeveRetornarArrayComValoresAleatorios()
        {
            // Arrange
            var algoritmoGenetico = new AlgoritmoGeneticoService();
            var disciplinasPorFase = new List<int> { 1, 2, 3, 4, 5 }; // disc
            string nomeCurso = "Curso Superior"; // curso superior q n tem "técnico"

            // Act
            var resultado = algoritmoGenetico.GeraHorariosAleatorios(disciplinasPorFase, nomeCurso);

            // Assert
            Assert.IsTrue(resultado[0] >= 1 && resultado[0] <= 5, "O valor do primeiro índice deve estar dentro do intervalo das disciplinas.");
            Assert.IsTrue(resultado[1] >= 1 && resultado[1] <= 5, "O valor do segundo índice deve estar dentro do intervalo das disciplinas.");
            // Validação para outros índices também pode ser realizada
        }

        [TestMethod]
        public void GetProfessorByName_ComNomeValido_DeveRetornarProfessor()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João" },
        new Professor { Nome = "Maria" },
        new Professor { Nome = "José" }
    };

            string nomeProfessor = "Maria";
            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act - Usando reflexão para invocar o método privado
            var metodo = typeof(AlgoritmoGeneticoService).GetMethod("GetProfessorByName", BindingFlags.NonPublic | BindingFlags.Instance);
            var resultado = metodo.Invoke(algoritmoGenetico, new object[] { nomeProfessor, professores });

            // Assert
            var professorResultado = resultado as Professor;
            Assert.IsNotNull(professorResultado, "O professor não deve ser nulo.");
            Assert.AreEqual(nomeProfessor, professorResultado?.Nome, "O nome do professor retornado deve ser o mesmo.");
        }

        [TestMethod]
        public void GetProfessorByName_ComNomeInexistente_DeveRetornarNull()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João" },
        new Professor { Nome = "Maria" },
        new Professor { Nome = "José" }
    };

            string nomeProfessor = "Carlos";
            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act - Usando reflexão para invocar o método privado
            var metodo = typeof(AlgoritmoGeneticoService).GetMethod("GetProfessorByName", BindingFlags.NonPublic | BindingFlags.Instance);
            var resultado = metodo.Invoke(algoritmoGenetico, new object[] { nomeProfessor, professores });

            // Assert
            Assert.IsNull(resultado, "O método deve retornar null quando o nome do professor não for encontrado.");
        }

        [TestMethod]
        public void VerificaSeProfessorEstaEmAlgumaDisciplina_ComCromossomoSemDisciplinas_NaoDeveAlterarNota()
        {
            // Arrange
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = "João" },
        new Disciplina { Codigo = 102, Professor = "Maria" }
    };

            var cromossomo = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[] { 0, 0, 0, 0, 0 }
            };

            int periodo1 = 1, periodo2 = 2;
            string nomeProfessor = "João";
            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaSeProfessorEstaEmAlgumaDisciplina(periodo1, periodo2, nomeProfessor, disciplinas, cromossomo);

            // Assert
            Assert.AreEqual(0, cromossomo.Nota, "A nota do cromossomo não deve ser alterada.");
        }

        [TestMethod]
        public void VerificaSeProfessorEstaEmAlgumaDisciplina_ComProfessorNaoNoPeriodo_NaoDeveDescontarPontos()
        {
            // Arrange
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = "João" },
        new Disciplina { Codigo = 102, Professor = "Maria" }
    };

            var cromossomo = new Cromossomo
            {
                DiasDaSemanaECodigosDasDisciplinas = new int[] { 101, 102, 103, 104, 105 }
            };

            int periodo1 = 1, periodo2 = 2;
            string nomeProfessor = "Carlos"; // Professor não presente
            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaSeProfessorEstaEmAlgumaDisciplina(periodo1, periodo2, nomeProfessor, disciplinas, cromossomo);

            // Assert
            Assert.AreEqual(0, cromossomo.Nota, "A nota do cromossomo não deve ser alterada.");
        }

        [TestMethod]
        public void VerificaDisponibilidadeProfessor_ComProfessorDisponivel_DeveNaoDescontarPontos()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João", Disponibilidade = new Disponibilidade { Segunda = true, Terca = true, Quarta = true, Quinta = true, Sexta = true } }
    };
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = "João" }
    };
            var cursos = new List<Curso>
    {
        new Curso { Disciplinas = disciplinas }
    };
            var inputData = new InputData { Cursos = cursos, Professores = professores };
            var cromossomo = new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = new int[] { 101 } };
            var populacao = new List<Cromossomo> { cromossomo };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaDisponibilidadeProfessor(populacao, inputData);

            // Assert
            Assert.AreEqual(0, cromossomo.Nota, "A nota não deve ser descontada quando o professor está disponível.");
        }

        [TestMethod]
        public void VerificaDisponibilidadeProfessor_ComProfessorIndisponivel_DeveDescontarPontos()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João", Disponibilidade = new Disponibilidade { Segunda = false, Terca = true, Quarta = true, Quinta = true, Sexta = true } }
    };
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = "João" }
    };
            var cursos = new List<Curso>
    {
        new Curso { Disciplinas = disciplinas }
    };
            var inputData = new InputData { Cursos = cursos, Professores = professores };
            var cromossomo = new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = new int[] { 101 } };
            var populacao = new List<Cromossomo> { cromossomo };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaDisponibilidadeProfessor(populacao, inputData);

            // Assert
            Assert.AreEqual(-1, cromossomo.Nota, "A nota deveria ser descontada em 1 ponto quando o professor não está disponível.");
        }

        [TestMethod]
        public void VerificaDisponibilidadeProfessor_ComDisciplinaSemProfessor_DeveNaoDescontarPontos()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João", Disponibilidade = new Disponibilidade { Segunda = true, Terca = true, Quarta = true, Quinta = true, Sexta = true } }
    };
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = null } // Professor não atribuído
    };
            var cursos = new List<Curso>
    {
        new Curso { Disciplinas = disciplinas }
    };
            var inputData = new InputData { Cursos = cursos, Professores = professores };
            var cromossomo = new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = new int[] { 101 } };
            var populacao = new List<Cromossomo> { cromossomo };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaDisponibilidadeProfessor(populacao, inputData);

            // Assert
            Assert.AreEqual(0, cromossomo.Nota, "A nota não deve ser descontada quando não há professor para a disciplina.");
        }


        [TestMethod]
        public void VerificaDisponibilidadeProfessor_ComDisciplinaInexistente_DeveNaoDescontarPontos()
        {
            // Arrange
            var professores = new List<Professor>
    {
        new Professor { Nome = "João", Disponibilidade = new Disponibilidade { Segunda = true, Terca = true, Quarta = true, Quinta = true, Sexta = true } }
    };
            var disciplinas = new List<Disciplina>
    {
        new Disciplina { Codigo = 101, Professor = "João" }
    };
            var cursos = new List<Curso>
    {
        new Curso { Disciplinas = disciplinas }
    };
            var inputData = new InputData { Cursos = cursos, Professores = professores };
            var cromossomo = new Cromossomo { DiasDaSemanaECodigosDasDisciplinas = new int[] { 999 } }; // Código de disciplina inexistente
            var populacao = new List<Cromossomo> { cromossomo };

            var algoritmoGenetico = new AlgoritmoGeneticoService();

            // Act
            algoritmoGenetico.VerificaDisponibilidadeProfessor(populacao, inputData);

            // Assert
            Assert.AreEqual(0, cromossomo.Nota, "A nota não deve ser descontada quando a disciplina não existe.");
        }


    }
}
