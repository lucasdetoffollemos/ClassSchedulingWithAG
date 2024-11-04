namespace ClassSchedulingWithAG.Models
{
    public class Disciplina
    {
        public string Nome { get; set; }
        public string Fase { get; set; }
        public int CH { get; set; }
        public Professor Professor { get; set; }
    }
}
