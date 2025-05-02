namespace Titan.Website.Models;

public class Depoimento
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Texto { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public int Avaliacao { get; set; } = 5; 

    public string? Imagem { get; set; }

    public string? Cargo { get; set; }
}
