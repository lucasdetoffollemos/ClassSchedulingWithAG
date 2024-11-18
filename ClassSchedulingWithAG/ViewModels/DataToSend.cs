namespace ClassSchedulingWithAG.ViewModels
{
    public class DataToSend
    {
        public int NotaDoMaiorCromosso { get; set; }

        public int QuantidadeDeIterações { get; set; }

        public double TempoDeExecuçãoEmMinutos { get; set; }

        public List<CursoDTO> Cursos { get; set; }
    }
    public class CursoDTO
    {
        public CursoDTO()
        {
             Disciplinas = new List<DisciplinaDTO>();
        }
        public string NomeCurso { get; set; }

        public List<DisciplinaDTO> Disciplinas { get; set; }
    }

    public class DisciplinaDTO
    {
        public string Dia { get; set; }
        public string Periodo { get; set; }
        public string NomeProfessor { get; set; }
        public string NomeDisciplina { get; set; }
        public int Fase { get; set; }
        public int CH { get; set; }
    }
}
