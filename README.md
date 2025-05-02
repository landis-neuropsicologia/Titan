# Sistema de Gestão com Autenticação Multinível em C# e Blazor
## Descrição do Projeto
Este projeto implementa um sistema completo de gestão com autenticação e permissões multinível, desenvolvido em C# com interface Blazor. A aplicação oferece acesso diferenciado para usuários pessoa física, usuários corporativos e administradores internos, seguindo rigorosamente os princípios SOLID, Domain-Driven Design (DDD) e Clean Code.

## Características Principais

### Sistema de Registro e Autenticação

- Fluxo específico para pessoas físicas e jurídicas
- Suporte a registro via redes sociais para pessoas físicas
- Sistema de ativação via email com token
- Integração com Keycloak para gerenciamento de identidade
- Autenticação via tokens JWT

### Gerenciamento Multinível

- Usuários pessoa física: Gerenciam apenas o próprio perfil
- Usuários corporativos: Sistema baseado em roles, com administradores que gerenciam usuários da mesma empresa
- Funcionários internos: Acesso total ao sistema conforme roles administrativas

### Arquitetura e Tecnologias

- Backend em C# seguindo arquitetura em camadas
- Interface web responsiva com Blazor
- Versão mobile disponível (Blazor Hybrid)
- Banco de dados PostgreSQL
- Integração com APIs externas via Minimal APIs

## Estrutura do Projeto
O projeto segue uma arquitetura de monolito modular estruturado em camadas:

- Domain: Entidades de negócio, interfaces de repositórios e serviços
- Application: Serviços de aplicação, DTOs e validadores
- Infrastructure: Implementações de repositórios, acesso a banco de dados, serviços externos
- WebUI: Interface Blazor, componentes e serviços de apresentação
- Tests: Testes unitários, de integração e UI para todas as camadas

## Requisitos Técnicos

- .NET 7.0 ou superior
- PostgreSQL 14 ou superior
- Keycloak (para autenticação e gestão de identidade)
- SMTP Server (para envio de emails)

## Princípios de Desenvolvimento
O código implementa e demonstra:

- Princípios SOLID de design orientado a objetos
- Padrões e práticas de Domain-Driven Design
- Clean Code com foco em legibilidade e manutenibilidade
- Testes automáticos em múltiplos níveis
- Gestão de permissões e segurança em camadas

## Instalação e Configuração

### Clone o repositório

```markdown
https://lead7consultoria@dev.azure.com/lead7consultoria/Titan/_git/Titan
```

### Configure as strings de conexão em appsettings.json

### Configure o serviço Keycloak conforme documentação

### Execute as migrações do banco de dados (executadas automaticamente no primeiro uso)

### Execute a aplicação com dotnet run no diretório WebUI
