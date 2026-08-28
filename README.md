# FrutNatura

Sistema de suporte técnico integrado a um ambiente de vendas simulado, desenvolvido como **Trabalho de Conclusão de Curso (TCC)**.

## 📋 Sobre o projeto

Em vez de construir apenas um sistema de chamados simples, o grupo optou por montar um **ambiente completo**: um sistema de vendas com problemas propositalmente inseridos, no qual o usuário navega, realiza processos do dia a dia e pode abrir chamados pedindo auxílio à equipe de suporte — que atende, responde e resolve através de um painel próprio.

O projeto também conta com um **bot auxiliar baseado na API da OpenAI**, usado para tirar dúvidas leves dentro do sistema.

## ✨ Funcionalidades

- Autenticação e cadastro de usuários (clientes e atendentes) com JWT
- Abertura, atribuição e acompanhamento de chamados de suporte
- Troca de mensagens em tempo real entre cliente e atendente
- Aplicativo desktop para a equipe de atendimento
- Aplicativo mobile (Android) para os clientes
- Bot auxiliar com IA para dúvidas rápidas
- Envio de notificações por e-mail

## 🏗️ Arquitetura e tecnologias

O backend segue os princípios de **Clean Architecture**, organizado em camadas:

- **Core** — entidades de domínio, contratos e regras de segurança
- **Application** — casos de uso e orquestração de regras de negócio
- **Infrastructure** — persistência (Entity Framework Core + SQL Server), envio de e-mails, IA, autenticação e comunicação em tempo real
- **Interface** — API Web, aplicativo Desktop e aplicativo mobile Android

**Principais tecnologias:**

| Camada | Tecnologia |
|---|---|
| Backend | .NET 8, ASP.NET Core, Entity Framework Core |
| Autenticação | JWT + PBKDF2 (hash de senha) |
| Banco de dados | SQL Server |
| IA | API da OpenAI (GPT-4o-mini) |
| Mobile | Android (Java) |
| Funções serverless | Azure Functions |

## 👥 Equipe

| Integrante | Área |
|---|---|
| Lucas | Backend / Banco de dados |
| Vitor | Backend |
| Vitória | Frontend |

## 🚀 Como executar

1. Clone o repositório
2. Configure a `ConnectionString` em `FrutNatura2/appsettings.json` com os dados do seu SQL Server local
3. Configure sua própria chave de API da OpenAI e credenciais de e-mail nos campos indicados (`[digite a senha de api]` / `[coloque a chave de app]`)
4. Restaure os pacotes NuGet e rode as migrations do Entity Framework
5. Execute o projeto `FrutNatura2` (API) e, em seguida, o `Desktop` ou o app Android

## 📄 Licença

Projeto acadêmico desenvolvido para fins educacionais.
