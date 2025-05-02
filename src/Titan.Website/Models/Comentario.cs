namespace Titan.Website.Models;

public class Comentario
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Texto { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;
}
