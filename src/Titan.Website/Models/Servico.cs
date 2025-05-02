namespace Titan.Website.Models;

public class Servico
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    
    public string Descricao { get; set; } = string.Empty;
    
    public string DescricaoDetalhada { get; set; } = string.Empty;
    
    public string Imagem { get; set; } = string.Empty;
    
    public List<string>? Indicacoes { get; set; }
    
    public List<string>? Etapas { get; set; }
    
    public decimal Preco { get; set; }
    
    public bool DestaquePrincipal { get; set; }
}
