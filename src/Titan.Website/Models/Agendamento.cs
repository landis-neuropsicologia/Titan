namespace Titan.Website.Models;

public class Agendamento
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime DataConsulta { get; set; }

    public TimeSpan HorarioConsulta { get; set; }

    public int ServicoId { get; set; }

    public string NomeServico { get; set; } = string.Empty;

    public int ProfissionalId { get; set; }

    public string NomeProfissional { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public string Status { get; set; } = "Pendente"; // Pendente, Confirmado, Cancelado, Realizado

    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
