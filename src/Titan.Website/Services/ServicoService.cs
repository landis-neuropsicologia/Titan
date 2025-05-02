using Titan.Website.Models;

namespace Titan.Website.Services;

public class ServicoService(HttpClient http)
{
    private readonly HttpClient _http = http;
    private List<Servico>? _servicos;

    public async Task<List<Servico>> GetServicosAsync()
    {
        if (_servicos != null)
            return _servicos;

        // Em uma implementação real, busque da API
        // return await _http.GetFromJsonAsync<List<Servico>>("api/servicos") ?? new List<Servico>();

        // Por enquanto, retornamos dados mockados
        _servicos =
        [
            new Servico()
            {
                Id = 1,
                Nome = "Avaliação Neuropsicológica",
                Descricao = "Avaliação das funções cognitivas, como memória, atenção, linguagem e funções executivas.",
                DescricaoDetalhada = "A avaliação neuropsicológica é um processo detalhado de investigação das funções cognitivas, como memória, atenção, linguagem, funções executivas, habilidades visuoespaciais e aspectos comportamentais e emocionais. É realizada por meio de entrevistas, observação clínica e testes específicos.",
                Imagem = "images/servico1.jpg",
                Indicacoes =
                [
                    "Dificuldades de aprendizagem e transtornos do neurodesenvolvimento (TDAH, TEA, dislexia)",
                    "Transtornos neurológicos (demências, AVC, epilepsia, traumatismo cranioencefálico)",
                    "Transtornos psiquiátricos (depressão, transtornos de ansiedade, esquizofrenia)",
                    "Acompanhamento de intervenções medicamentosas e cirúrgicas",
                    "Orientação vocacional e avaliação pericial"
                ],
                Etapas =
                [
                    "Entrevista inicial para coleta de histórico",
                    "Sessões de aplicação de testes neuropsicológicos",
                    "Análise e interpretação dos resultados",
                    "Entrevista devolutiva com esclarecimentos e orientações",
                    "Elaboração de relatório detalhado"
                ],
                Preco = 1500.00m,
                DestaquePrincipal = true
            },
            new Servico()
            {
                Id = 2,
                Nome = "Reabilitação Neuropsicológica",
                Descricao = "Programas personalizados de reabilitação para pessoas com déficits cognitivos.",
                DescricaoDetalhada = "A reabilitação neuropsicológica é um processo terapêutico que visa melhorar o funcionamento cognitivo, emocional e comportamental de pessoas que apresentam déficits devido a lesões cerebrais ou transtornos neuropsicológicos.",
                Imagem = "images/servico2.jpg",
                Indicacoes =
                [
                    "Déficits cognitivos decorrentes de lesões cerebrais (AVC, TCE)",
                    "Transtornos neurodegenerativos (Alzheimer, Parkinson)",
                    "Sequelas neuropsicológicas de condições médicas",
                    "Transtornos do neurodesenvolvimento"
                ],
                Etapas =
                [
                    "Avaliação neuropsicológica inicial",
                    "Elaboração de programa personalizado",
                    "Sessões de reabilitação",
                    "Reavaliações periódicas",
                    "Orientações para a família"
                ],
                Preco = 250.00m,
                DestaquePrincipal = true
            },
            new Servico()
            {
                Id = 3,
                Nome = "Acompanhamento de Transtornos do Neurodesenvolvimento",
                Descricao = "Atendimento especializado para TDAH, TEA, dislexia e outros transtornos.",
                DescricaoDetalhada = "É um serviço especializado voltado para crianças e adolescentes com transtornos do neurodesenvolvimento, como TDAH, TEA, dislexia e outros transtornos de aprendizagem.",
                Imagem = "images/servico3.jpg",
                Indicacoes =
                [
                    "Transtorno do Déficit de Atenção e Hiperatividade (TDAH)",
                    "Transtorno do Espectro Autista (TEA)",
                    "Transtornos Específicos de Aprendizagem (dislexia, discalculia, disgrafia)",
                    "Atraso no desenvolvimento cognitivo",
                    "Dificuldades escolares"
                ],
                Etapas =
                [
                    "Avaliação neuropsicológica inicial",
                    "Intervenção neuropsicológica individualizada",
                    "Orientação aos pais e responsáveis",
                    "Contato com a escola e outros profissionais",
                    "Reavaliações periódicas para acompanhar o progresso"
                ],
                Preco = 200.00m,
                DestaquePrincipal = false
            }
        ];

        return _servicos;
    }

    public async Task<Servico?> GetServicoByIdAsync(int id)
    {
        var servicos = await GetServicosAsync();
        return servicos.FirstOrDefault(s => s.Id == id);
    }
}
