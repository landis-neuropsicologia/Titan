using Titan.Website.Models;

namespace Titan.Website.Services;

public class DepoimentoService(HttpClient http)
{
    private readonly HttpClient _http = http;
    private List<Depoimento>? _depoimentos;

    public async Task<List<Depoimento>> GetDepoimentosAsync()
    {
        if (_depoimentos != null)
            return _depoimentos;

        // Em uma implementação real, busque da API
        // return await _http.GetFromJsonAsync<List<Depoimento>>("api/depoimentos") ?? new List<Depoimento>();

        // Por enquanto, retornamos dados mockados
        _depoimentos =
        [
            new() {
                Id = 1,
                Nome = "Maria Silva",
                Texto = "O atendimento da Dra. Viviane foi excepcional. Meu filho teve uma melhora significativa após a avaliação neuropsicológica e o tratamento recomendado. Toda a equipe é muito acolhedora e profissional.",
                Data = "10/03/2025",
                Avaliacao = 5,
                Cargo = "Mãe de paciente"
            },
            new() {
                Id = 2,
                Nome = "João Pereira",
                Texto = "A avaliação neuropsicológica realizada pelo Dra. Viviane foi fundamental para entender minhas dificuldades cognitivas após o AVC. O programa de reabilitação personalizado me ajudou muito a recuperar habilidades que havia perdido.",
                Data = "25/02/2025",
                Avaliacao = 5,
                Cargo = "Paciente"
            },
            new() {
                Id = 3,
                Nome = "Ana Carolina Mendes",
                Texto = "Como neurologista, tenho encaminhado diversos pacientes para a Dra. Viviane e sempre recebo retornos positivos. Os relatórios são extremamente detalhados e as recomendações de intervenção muito pertinentes.",
                Data = "15/01/2025",
                Avaliacao = 5,
                Cargo = "Neurologista"
            },
            new() {
                Id = 4,
                Nome = "Roberto Almeida",
                Texto = "O diagnóstico de TDAH do meu filho através da avaliação neuropsicológica mudou nossa vida. Finalmente entendemos suas dificuldades e conseguimos o apoio necessário na escola. Muito grata à Dra. Viviane.",
                Data = "05/12/2024",
                Avaliacao = 5,
                Cargo = "Pai de paciente"
            }
        ];

        return _depoimentos;
    }
}