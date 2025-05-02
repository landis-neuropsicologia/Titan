namespace Titan.Website.Models;

public class ArtigoBlog
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Resumo { get; set; } = string.Empty;

    public string Conteudo { get; set; } = string.Empty;

    public string Imagem { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public List<string>? Tags { get; set; }

    public List<Comentario>? Comentarios { get; set; }
}
