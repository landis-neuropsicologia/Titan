﻿namespace Titan.Website.Models;

public class Contato
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Assunto { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public DateTime DataEnvio { get; set; } = DateTime.Now;

    public bool Lido { get; set; } = false;
}
