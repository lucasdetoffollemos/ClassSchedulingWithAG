using ClassSchedulingWithAG.Models;
using ClassSchedulingWithAG.Services;
using ClassSchedulingWithAG.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClassSchedulingWithAG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassSchedullingController : ControllerBase
    {
      
        private readonly ILogger<ClassSchedullingController> _logger;

        public ClassSchedullingController(ILogger<ClassSchedullingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post(DataFromFront data)
        {
            if (data.file == null || data.file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = new StreamReader(data.file.OpenReadStream()))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var jsonContent = await stream.ReadToEndAsync();

                    var inputData = JsonSerializer.Deserialize<InputData>(jsonContent, options);

                    if (inputData == null)
                    {
                        return BadRequest("Invalid JSON structure.");
                    }

                    var algoritmoGenetico = new AlgoritmoGeneticoService();

                    var cromossomSelecionado = algoritmoGenetico.CalculaHOrariosComAlgoritmoGnético(inputData, data.Cromossomos, data.CromossomosPorElitismo, data.ProbabilidadeCruzamento, data.ProbabilidadeMutacao, data.QuantidadeMaxInteracoes, data.InteracoesSemMelhorias);

                    if(cromossomSelecionado == null)
                    {
                        return BadRequest($"Response null");

                    }


                    var dataToSend = new DataToSend();
                    dataToSend.NotaDoMaiorCromosso = cromossomSelecionado.Nota;
                    dataToSend.QuantidadeDeIterações = cromossomSelecionado.QuantidadeDeIterações;
                    dataToSend.TempoDeExecuçãoEmMinutos = cromossomSelecionado.TempoDeExecuçãoEmMinutos;
                    dataToSend.Cursos = algoritmoGenetico.GeraHorarios(cromossomSelecionado.DiasDaSemanaECodigosDasDisciplinas, inputData.Cursos);

                    return Ok(dataToSend);
                }
            }
            catch (JsonException ex)
            {
                return BadRequest($"Invalid JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
