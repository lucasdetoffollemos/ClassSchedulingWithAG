# Algoritmo Genético

## Descrição do Problema
O arquivo Disciplinas e Professores contém as disciplinas ministradas nos cursos de Ciência da Computação, Engenharia Mecânica, Engenharia Química, Técnico em Informática para Internet, Técnico em Mecatrônica e Técnico em Administração, assim como suas respectivas cargas horárias e professores responsáveis, e a tabela com as disponibilidades de cada professor.

Implementamos um Algoritmo Genético para gerar o horário de aulas do semestre para estes cursos considerando os dados disponibilizados.

Nosso backend disponibiliza uma API com um POST para ser feito as comunicações do nosso frontend com o backend, seguindo os parâmetros abaixo:
- Probabilidade de Cruzamento;
- Probabilidade de Mutação;
- Quantidade de cromossomos selecionados por elitismo;
- Quantidade máxima de iterações;
- Quantidade de iterações sem melhorias que deve ser atingida para a busca ser interrompida.

O sistema deve apresentar:
- O horário gerado;
- A nota do melhor cromossomo;
- A quantidade de iterações;
- O tempo de execução.

## Instalação
### Backend
```bash
git clone https://github.com/lucasdetoffollemos/ClassSchedulingWithAG.git
cd ClassSchedulingWithAG

Verifique se possuí instalado o DOTNET 8 no seu ambiente.

Execute o comando "dotnet run" para executar a API.
