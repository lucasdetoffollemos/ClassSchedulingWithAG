namespace ClassSchedulingWithAG.ViewModels
{
    public class DataFromFront
    {

        public IFormFile file { get; set; }

        public int ProbabilidadeCruzamento { get; set; }

        public int ProbabilidadeMutacao { get; set; }

        public int Cromossomos { get; set; }

        public int CromossomosPorElitismo { get; set; }

        public int QuantidadeMaxInteracoes { get; set; }

        public int InteracoesSemMelhorias { get; set; }

    }
}
