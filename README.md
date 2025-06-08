## Descrição do Projeto

Esta API RESTful gerencia três entidades principais: usuários, rotas seguras e riscos. Implementa padrões modernos como:

## Integrantes:
- Igor Akira Bortolini Tateishi RM:554227 
- Nicola Monte Cravo Garofalo RM:553991 
- Willyam Santos Souza RM:554244 

- Arquitetura MVC com Repository e Service  
- HATEOAS para navegação de API  
- Rate limiting para proteção contra abuso  
- Integração com RabbitMQ para mensageria assíncrona  
- Predição de níveis de risco com ML.NET  
- Documentação completa via Swagger  
- Testes unitários com xUnit  

## Tecnologias Utilizadas

- **Linguagem:** C# (.NET 6)  
- **Banco de Dados:** Oracle  
- **ORM:** Entity Framework Core  
- **Mensageria:** RabbitMQ  
- **Machine Learning:** ML.NET  
- **Testes:** xUnit
- **Documentação:** Swagger (OpenAPI)  

**Outras Bibliotecas:**

- ASP.NET Core Rate Limit  
- Newtonsoft.Json  
- Oracle.EntityFrameworkCore  

---

## Como Executar o Projeto

### Pré-requisitos

- .NET 6 SDK  
- Oracle Database (ou Docker para rodar um container Oracle)  
- RabbitMQ (pode ser via Docker)  
- Git (opcional)  

### Passos para Execução

**Clonar o repositório:**


# Documentação da API - Guia de Execução e Testes

## Executar o Projeto

### Execução Local
```bash
dotnet run
```

### Acessar a Documentação
Abra o navegador em: `http://localhost:5000/swagger`

## Rodar com Docker

### Subir Containers
```bash
docker-compose up -d
```

### Executar a Aplicação
```bash
dotnet run
```

## Documentação dos Endpoints

### Usuários (`/api/usuarios`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET    | `/api/usuarios`     | Lista todos os usuários |
| GET    | `/api/usuarios/{id}` | Obtém detalhes de um usuário |
| POST   | `/api/usuarios`     | Cria um novo usuário |
| PUT    | `/api/usuarios/{id}` | Atualiza um usuário |
| DELETE | `/api/usuarios/{id}` | Exclui um usuário |

### Rotas Seguras (`/api/rotas-seguras`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET    | `/api/rotas-seguras`     | Lista todas as rotas |
| GET    | `/api/rotas-seguras/{id}` | Obtém detalhes de uma rota |
| POST   | `/api/rotas-seguras`     | Cria uma nova rota |
| PUT    | `/api/rotas-seguras/{id}` | Atualiza uma rota |
| DELETE | `/api/rotas-seguras/{id}` | Exclui uma rota |

### Riscos (`/api/riscos`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET    | `/api/riscos`     | Lista todos os riscos |
| GET    | `/api/riscos/{id}` | Obtém detalhes de um risco |
| POST   | `/api/riscos`     | Cria um novo risco (com predição automática) |
| PUT    | `/api/riscos/{id}` | Atualiza um risco |
| DELETE | `/api/riscos/{id}` | Exclui um risco |

## Características Especiais

- **HATEOAS**: Respostas incluem links HATEOAS para ações relacionadas
- **Rate Limiting**: 5 requisições/segundo em endpoints POST
- **Machine Learning**: Predição automática de nível de risco usando ML.NET

## Instruções de Testes

### Testes Unitários

Execute todos os testes:
```bash
dotnet test
```

### Testes Manuais via Swagger

1. Acesse `http://localhost:5000/swagger`
2. Selecione um endpoint
3. Clique em "Try it out"
4. Preencha os dados
5. Execute e analise a resposta

## Exemplos de Payload para Teste

### Criar Usuário
```json
{
  "nome": "Maria Silva",
  "email": "maria.silva@empresa.com"
}
```

### Criar Rota Segura
```json
{
  "localizacao": "Centro da cidade",
  "coordenadas": "-23.550520, -46.633308"
}
```

### Criar Risco (sem nível)
```json
{
  "descricao": "Queda de árvores na avenida principal"
}
```
