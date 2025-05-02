using Titan.Website.Models;

namespace Titan.Website.Services;

public class BlogService(HttpClient http)
{
    private readonly HttpClient _http = http;
    private List<ArtigoBlog>? _artigos;

    public async Task<List<ArtigoBlog>> GetArtigosAsync()
    {
        if (_artigos != null)
            return _artigos;

        // Em uma implementação real, busque da API
        // return await _http.GetFromJsonAsync<List<ArtigoBlog>>("api/artigos") ?? new List<ArtigoBlog>();

        // Por enquanto, retornamos dados mockados
        _artigos =
        [
            new() 
            {
                Id = 1,
                Titulo = "A importância da avaliação neuropsicológica no diagnóstico do TDAH",
                Resumo = "Entenda como a avaliação neuropsicológica pode auxiliar no diagnóstico e tratamento do TDAH.",
                Conteudo = @"<p>A avaliação neuropsicológica é uma ferramenta fundamental no diagnóstico e tratamento do Transtorno de Déficit de Atenção e Hiperatividade (TDAH). Este processo avaliativo permite uma análise detalhada do funcionamento cognitivo, identificando padrões específicos de déficits que são característicos deste transtorno.</p>
                        
                <p>O TDAH é caracterizado por um padrão persistente de desatenção e/ou hiperatividade-impulsividade que interfere no funcionamento e desenvolvimento do indivíduo. A avaliação neuropsicológica permite identificar alterações específicas nas funções executivas, como controle inibitório, memória operacional, atenção sustentada e dividida, flexibilidade cognitiva, entre outras.</p>
                        
                <p>Através de uma bateria de testes cognitivos, entrevistas clínicas e questionários, o neuropsicólogo pode obter dados objetivos sobre o funcionamento cognitivo do paciente, diferenciando o TDAH de outras condições com sintomas semelhantes, como transtornos de aprendizagem, transtornos de ansiedade ou problemas de sono.</p>
                        
                <p>Além de auxiliar no diagnóstico, a avaliação neuropsicológica fornece informações valiosas para o planejamento do tratamento, identificando as forças e fraquezas cognitivas do indivíduo, o que permite uma intervenção mais direcionada e eficaz.</p>
                        
                <p>No consultório Landis Neuropsicologia, oferecemos avaliações neuropsicológicas completas, realizadas por profissionais especializados, que podem ajudar no diagnóstico preciso do TDAH e no desenvolvimento de estratégias de intervenção personalizadas.</p>",
                Imagem = "images/blog1.jpg",
                Data = "15/04/2025",
                Autor = "Dra. Ana Landis",
                Tags = ["TDAH", "Avaliação Neuropsicológica", "Diagnóstico"]
            },
            new() 
            {
                Id = 2,
                Titulo = "Reabilitação neuropsicológica após AVC",
                Resumo = "Conheça as técnicas e benefícios da reabilitação neuropsicológica em pacientes que sofreram AVC.",
                Conteudo = @"<p>O Acidente Vascular Cerebral (AVC) é uma das principais causas de incapacidade em adultos e pode resultar em diversos déficits cognitivos, como problemas de memória, atenção, linguagem, percepção visoespacial e funções executivas. A reabilitação neuropsicológica desempenha um papel crucial na recuperação desses pacientes.</p>
                        
                <p>A reabilitação neuropsicológica após AVC consiste em um conjunto de técnicas e estratégias destinadas a melhorar o funcionamento cognitivo, emocional e comportamental do paciente. O objetivo é ajudar o indivíduo a retomar suas atividades diárias com a maior independência possível, apesar das limitações impostas pelo AVC.</p>
                        
                <p>Este processo inicia-se com uma avaliação neuropsicológica detalhada, que identifica os déficits específicos e as capacidades preservadas. Com base nestes resultados, é desenvolvido um programa de reabilitação personalizado, que pode incluir diferentes abordagens:</p>
                        
                <ul>
                    <li>Exercícios de estimulação cognitiva específicos para cada função afetada</li>
                    <li>Desenvolvimento de estratégias compensatórias</li>
                    <li>Adaptação do ambiente</li>
                    <li>Apoio psicológico para lidar com os aspectos emocionais da recuperação</li>
                    <li>Orientação familiar</li>
                </ul>
                        
                <p>Estudos científicos têm demonstrado que a reabilitação neuropsicológica iniciada precocemente e mantida de forma consistente pode resultar em melhorias significativas no funcionamento cognitivo e na qualidade de vida após o AVC.</p>
                        
                <p>No consultório Landis Neuropsicologia, contamos com profissionais especializados em reabilitação neuropsicológica após AVC, oferecendo programas de intervenção individualizados e baseados em evidências científicas.</p>",
                Imagem = "images/blog2.jpg",
                Data = "02/04/2025",
                Autor = "Dr. Rafael Martins",
                Tags = ["AVC", "Reabilitação Neuropsicológica", "Cognição"]
            },
            new()
            {
                Id = 3,
                Titulo = "Sinais de alerta para transtornos de aprendizagem em crianças",
                Resumo = "Aprenda a identificar os principais sinais que podem indicar transtornos de aprendizagem em crianças em idade escolar.",
                Conteudo = @"<p>Os transtornos de aprendizagem afetam aproximadamente 5-15% das crianças em idade escolar e podem impactar significativamente o desempenho acadêmico, a autoestima e o desenvolvimento social. Identificar os sinais precocemente é fundamental para um diagnóstico adequado e intervenção eficaz.</p>
                        
                <p>Neste artigo, apresentamos os principais sinais de alerta que pais e educadores devem observar, organizados por faixas etárias e áreas de desenvolvimento. É importante ressaltar que a presença de um ou mais sinais não significa necessariamente que a criança tenha um transtorno de aprendizagem, mas indica a necessidade de uma avaliação mais detalhada.</p>
                        
                <h3>Sinais na pré-escola (3-5 anos)</h3>
                <ul>
                    <li>Dificuldade para aprender cores, formas, letras ou números</li>
                    <li>Problemas com rimas ou cantigas infantis</li>
                    <li>Vocabulário limitado</li>
                    <li>Dificuldade em seguir instruções simples</li>
                    <li>Problemas de coordenação motora</li>
                </ul>
                        
                <h3>Sinais nos anos iniciais do ensino fundamental (6-9 anos)</h3>
                <ul>
                    <li>Dificuldade persistente na leitura e escrita</li>
                    <li>Erros frequentes na leitura (inversão, omissão ou substituição de letras)</li>
                    <li>Dificuldade em compreender o que lê</li>
                    <li>Problemas com operações matemáticas básicas</li>
                    <li>Dificuldade em organizar o material escolar e as tarefas</li>
                </ul>
                        
                <h3>Sinais nos anos finais do ensino fundamental (10-14 anos)</h3>
                <ul>
                    <li>Persistência das dificuldades anteriores</li>
                    <li>Problemas com compreensão de textos mais complexos</li>
                    <li>Dificuldade em expressar ideias por escrito</li>
                    <li>Problemas com matemática mais avançada</li>
                    <li>Dificuldade em planejar e organizar trabalhos escolares</li>
                </ul>
                        
                <p>Se você identificou vários desses sinais em seu filho ou aluno, é recomendável buscar uma avaliação neuropsicológica. Este tipo de avaliação pode identificar não apenas a presença de um transtorno de aprendizagem, mas também as forças e fraquezas cognitivas da criança, o que permite uma intervenção personalizada e eficaz.</p>
                        
                <p>Na Landis Neuropsicologia, realizamos avaliações neuropsicológicas completas em crianças e adolescentes, proporcionando diagnósticos precisos e orientações para intervenção junto à família e à escola.</p>",
                Imagem = "images/blog3.jpg",
                Data = "20/03/2025",
                Autor = "Dra. Ana Landis",
                Tags = ["Transtornos de Aprendizagem", "Crianças", "Educação"]
            }
        ];

        return _artigos;
    }

    public async Task<ArtigoBlog?> GetArtigoByIdAsync(int id)
    {
        var artigos = await GetArtigosAsync();
        return artigos.FirstOrDefault(a => a.Id == id);
    }
}