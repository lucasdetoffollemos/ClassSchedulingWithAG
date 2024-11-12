using System;
using System.Security.Cryptography;

namespace ClassSchedulingWithAG.Models
{
    public class Horario
    {
        public Random random = new Random();
        public List<Curso> Cursos { get; set; }
        public List<Professor> Professores { get; set; }
        public Dictionary<string, List<string>> MapaDeHorarios { get; set; }

        public Horario(List<Curso> cursos, List<Professor> professores)
        {
            Cursos = cursos;
            Professores = professores;
            MapaDeHorarios = GerandoHorariosAleatorios();
        }

        private Dictionary<string, List<string>> GerandoHorariosAleatorios()
        {
            // Gera um horário aleatório respeitando as restrições
            var horario = new Dictionary<string, List<string>>()
                {
                    { "segunda", new List<string>() },
                    { "terça", new List<string>() },
                    { "quarta", new List<string>() },
                    { "quinta", new List<string>() },
                    { "sexta", new List<string>() }
                };

            // Preencha o horário com disciplinas de forma aleatória (respeitando a disponibilidade)
            foreach (var curso in Cursos)
            {
                foreach (var disciplina in curso.Disciplinas)
                {
                    var professor = Professores.FirstOrDefault(p => p.Nome == disciplina.Professor);
                    if (professor != null)
                    {
                        var diasDisponiveis = new List<string>();
                        if (professor.Disponibilidade.Segunda)
                        {
                            diasDisponiveis.Add("segunda");
                        }

                        if (professor.Disponibilidade.Terca)
                        {
                            diasDisponiveis.Add("terça");
                        }

                        if (professor.Disponibilidade.Quarta)
                        {
                            diasDisponiveis.Add("quarta");
                        }

                        if (professor.Disponibilidade.Quinta)
                        {
                            diasDisponiveis.Add("quinta");
                        }

                        if (professor.Disponibilidade.Sexta)
                        {
                            diasDisponiveis.Add("sexta");
                        }

                        if (diasDisponiveis.Count > 0)
                        {
                            var diaAleatorio = diasDisponiveis[random.Next(diasDisponiveis.Count)];
                            horario[diaAleatorio].Add(disciplina.Nome);
                        }
                    }
                }
            }

            return horario;
        }

        public int Fitness()
        {
            // Calcula a adequação da solução
            int score = 0;

            // Verifique a disponibilidade do professor e evite conflitos (simplificado)
            foreach (var curso in Cursos)
            {
                foreach (var disciplina in curso.Disciplinas)
                {
                    var professor = Professores.FirstOrDefault(p => p.Nome == disciplina.Professor);
                    if (professor != null)
                    {
                        foreach (var dia in MapaDeHorarios.Keys)
                        {
                            if (professor.Disponibilidade.Segunda && MapaDeHorarios[dia].Contains(disciplina.Nome))
                            {
                                score++;
                            }

                            if (professor.Disponibilidade.Segunda && MapaDeHorarios["segunda"].Contains(disciplina.Nome))
                            {
                                score++;
                            }
                        }
                    }
                }
            }

            return score;
        }
    }
}
