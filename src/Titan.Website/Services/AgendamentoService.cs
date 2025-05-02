using Titan.Website.Models;

namespace Titan.Website.Services;

public class AgendamentoService(HttpClient http)
{
    private readonly HttpClient _http = http;

    public async Task<bool> SolicitarAgendamentoAsync(Agendamento agendamento)
    {
        try
        {
            // Em uma implementação real, envie para a API
            // await _http.PostAsJsonAsync("api/agendamentos", agendamento);

            // Por enquanto, apenas simulamos sucesso
            await Task.Delay(1000); // Simular tempo de processamento
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<DateTime>> GetHorariosDisponiveisAsync(int profissionalId, DateTime data)
    {
        // Em uma implementação real, busque da API
        // return await _http.GetFromJsonAsync<List<DateTime>>($"api/agendamentos/horarios-disponiveis?profissionalId={profissionalId}&data={data:yyyy-MM-dd}") ?? new List<DateTime>();

        // Por enquanto, retornamos dados mockados
        var horarios = new List<DateTime>();
        var baseTime = new DateTime(data.Year, data.Month, data.Day, 8, 0, 0);

        // Horários a cada 1 hora, das 8h às 17h
        for (int i = 0; i < 10; i++)
        {
            horarios.Add(baseTime.AddHours(i));
        }

        // Simular alguns horários já ocupados
        if (profissionalId == 1)
        {
            horarios.RemoveAt(2); // 10h
            horarios.RemoveAt(5); // 14h
        }
        else
        {
            horarios.RemoveAt(1); // 9h
            horarios.RemoveAt(6); // 15h
        }

        return horarios;
    }
}
