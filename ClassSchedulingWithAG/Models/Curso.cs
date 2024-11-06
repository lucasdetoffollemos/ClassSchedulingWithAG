namespace ClassSchedulingWithAG.Models
{
    public class Curso
    {
        public string Nome { get; set; }

        //public string Periodo { get; set; }

        public DiasDaSemana DiasDaSemana { get; set; }

        public List<Disciplina> Disciplinas { get; set; }
    }
}
