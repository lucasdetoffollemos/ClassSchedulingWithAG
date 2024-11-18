namespace ClassSchedulingWithAG.Models
{
    public class Cromossomo
    {
        public int QuantidadeDeIterações { get; set; }

        public double TempoDeExecuçãoEmMinutos { get; set; }

        public int Nota {  get; set; }

       public int[] DiasDaSemanaECodigosDasDisciplinas { get; set; }

        public Cromossomo()
        {
               DiasDaSemanaECodigosDasDisciplinas = new int[240];
        }
    }
}
