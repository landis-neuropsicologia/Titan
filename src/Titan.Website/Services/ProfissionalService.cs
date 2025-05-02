using Titan.Website.Models;

namespace Titan.Website.Services;

public class ProfissionalService(HttpClient http)
{
    private readonly HttpClient _http = http;
    private List<Profissional>? _profissionais;

    public async Task<List<Profissional>> GetProfissionaisAsync()
    {
        if (_profissionais != null)
            return _profissionais;

        // Em uma implementação real, busque da API
        // return await _http.GetFromJsonAsync<List<Profissional>>("api/profissionais") ?? new List<Profissional>();

        // Por enquanto, retornamos dados mockados
        _profissionais =
        [
            new() {
                Id = 1,
                Nome = "Dra. Viviane Balero Landis",
                Especialidade = "Neuropsicologia Clínica",
                Descricao = "Especialista em avaliação neuropsicológica e reabilitação cognitiva com mais de 15 anos de experiência.",
                Imagem = "images/profissional1.jpg",
                Curriculo = "Doutora em Neurociências pela Universidade de São Paulo (USP), com especialização em Neuropsicologia pelo Conselho Federal de Psicologia. Atuação em hospitais de referência e clínicas especializadas.",
                Email = "viviane@landis-neuropsicologia.com.br",
                Formacoes =
                [
                    "Doutorado em Neurociências - USP",
                    "Mestrado em Psicologia Clínica - UFRJ",
                    "Especialização em Neuropsicologia - CFP"
                ],
                Certificacoes =
                [
                    "Certificação em Reabilitação Neuropsicológica",
                    "Certificação em Avaliação Neuropsicológica Infantil",
                    "Membro da Sociedade Brasileira de Neuropsicologia"
                ]
            },
            new() {
                Id = 2,
                Nome = "Dr. Rafael Martins",
                Especialidade = "Neuropsicologia Infantil",
                Descricao = "Especialista em diagnóstico e intervenção em transtornos do neurodesenvolvimento em crianças e adolescentes.",
                Imagem = "images/profissional2.jpg",
                Curriculo = "Mestre em Psicologia do Desenvolvimento pela PUC-SP, com especialização em Neuropsicologia Infantil. Experiência no diagnóstico e intervenção de transtornos do neurodesenvolvimento como TDAH, TEA e dificuldades de aprendizagem.",
                Email = "rafael.martins@landis-neuropsicologia.com.br",
                Formacoes =
                [
                    "Mestrado em Psicologia do Desenvolvimento - PUC-SP",
                    "Especialização em Neuropsicologia Infantil - UNIFESP",
                    "Graduação em Psicologia - USP"
                ],
                Certificacoes = new List<string>
                {
                    "Certificação em Avaliação do TDAH",
                    "Certificação em Intervenção em TEA",
                    "Membro da Associação Brasileira de Neuropsicologia"
                }
            }
        ];

        return _profissionais;
    }

    public async Task<Profissional?> GetProfissionalByIdAsync(int id)
    {
        var profissionais = await GetProfissionaisAsync();
        return profissionais.FirstOrDefault(p => p.Id == id);
    }
}
