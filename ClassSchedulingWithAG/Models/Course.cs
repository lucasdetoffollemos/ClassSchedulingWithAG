namespace ClassSchedulingWithAG.Models
{
    public class Curso
    {
        public string Nome { get; set; }

        public string Periodo { get; set; }

        public List<bool> DiasDaSemana { get; set; }

        public List<Disciplina> Disciplinas { get; set; }


    }
}
