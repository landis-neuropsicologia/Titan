namespace Titan.Website.Models;

public class Profissional
{
    public int Id { get; set; }
   
    public string Nome { get; set; } = string.Empty;
    
    public string Especialidade { get; set; } = string.Empty;
    
    public string Descricao { get; set; } = string.Empty;
    
    public string Imagem { get; set; } = string.Empty;
    
    public string Curriculo { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public List<string>? Formacoes { get; set; }
    
    public List<string>? Certificacoes { get; set; }
}
