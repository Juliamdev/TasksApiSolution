# TasksAPI
 
API REST de gerenciamento de tarefas em **C# / .NET 8 (Minimal API)**, com **Entity Framework Core**, **PostgreSQL** e **Docker**. Sobe inteira com um único comando: o banco, a API, as migrations e o Swagger.
 
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791?logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)
![Tests](https://img.shields.io/badge/tests-xUnit-brightgreen)
 
## Destaques
 
- **CRUD completo** de tarefas com Minimal APIs.
- **Migrations automáticas**: o banco é criado/atualizado na inicialização da API.
- **Validação de entrada**: título vazio retorna `400 Bad Request` com mensagem de erro.
- **Respostas HTTP corretas**: `201 Created` com `Location`, `204 No Content`, `404 Not Found`.
- **Health check** em `/health`, usado para monitoramento.
- **Docker Compose** com healthcheck do PostgreSQL: a API só sobe quando o banco está pronto.
- **Build multi-stage** no Dockerfile (imagem final só com o runtime).
- **Testes unitários com xUnit** para as regras do modelo.
## Como executar
 
Pré-requisito: **Docker** e **Docker Compose**. Não é preciso instalar o .NET nem o PostgreSQL.
 
```bash
git clone https://github.com/Juliamdev/TasksApiSolution.git
cd TasksApiSolution
docker compose up --build
```
 
- Swagger: http://localhost:8080/swagger
- Health check: http://localhost:8080/health
## Endpoints
 
| Método | Rota | Descrição | Respostas |
|---|---|---|---|
| `GET` | `/api/tasks` | Lista todas as tarefas | `200` |
| `GET` | `/api/tasks/{id}` | Busca uma tarefa pelo ID (GUID) | `200`, `404` |
| `POST` | `/api/tasks` | Cria uma tarefa | `201`, `400` |
| `PUT` | `/api/tasks/{id}` | Atualiza título, descrição e status | `200`, `400`, `404` |
| `DELETE` | `/api/tasks/{id}` | Remove uma tarefa | `204`, `404` |
| `GET` | `/health` | Verifica se a API está no ar | `200` |
 
### Exemplo
 
```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Estudar .NET 8", "description": "Aprender Minimal APIs"}'
```
 
```json
{
  "id": "3f1c2a5e-8b0d-4c1e-9a77-2d6f5b8e4c10",
  "title": "Estudar .NET 8",
  "description": "Aprender Minimal APIs",
  "isCompleted": false,
  "createdAt": "2026-10-16T01:45:08Z"
}
```
 
## Testes
 
```bash
dotnet test
```
 
Os testes cobrem a criação de tarefas com valores padrão e a validação de título (nulo, vazio e só espaços).
 
## Estrutura
 
```text
TasksApiSolution/
├── docker-compose.yml          # PostgreSQL + API
├── src/TaskApi/
│   ├── Program.cs              # configuração, migrations e endpoints
│   ├── Models/TaskItem.cs      # entidade + regra de validação
│   ├── Dtos/                   # contratos de entrada (Create/Update)
│   ├── Data/AppDbContext.cs    # EF Core
│   ├── Migrations/
│   └── Dockerfile              # build multi-stage
└── tests/TasksApi.Tests/       # xUnit
```
 
## Tecnologias
 
C# · .NET 8 · ASP.NET Core Minimal API · Entity Framework Core · Npgsql · PostgreSQL 15 · Swagger · Docker · xUnit
 
## Licença
 
MIT. Veja [LICENSE](LICENSE).
 
