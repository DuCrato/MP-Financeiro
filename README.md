# 💰 MP-Financeiro - Sistema de Gestão Financeira Pessoal

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/) 
[![React](https://img.shields.io/badge/React-18-blue)](https://react.dev/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Latest-red)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

> **API REST profissional + Frontend em React** para gestão de finanças pessoais com regras de negócio robustas, tratamento robusto de exceções e testes unitários.

## 📖 Sobre o Projeto

**MP-Financeiro** é um sistema Full Stack que permite gerenciar receitas, despesas e visualizar relatórios financeiros consolidados. Desenvolvido com **arquitetura em camadas**, **tratamento global de erros**, **logging estruturado** e **testes unitários** — demonstrando boas práticas profissionais de desenvolvimento.

## ✨ Destaques Técnicos

- ✅ **Clean Architecture** - Camadas bem definidas (Controllers → Services → Data)
- ✅ **Global Exception Handling** - Middleware customizado com exceções específicas
- ✅ **Logging Estruturado** - ILogger em toda a aplicação
- ✅ **Testes Unitários** - xUnit + Moq com cobertura de casos de sucesso/erro
- ✅ **CORS Seguro** - Configurado com origens específicas (não AllowAny)
- ✅ **DTOs Validados** - Request/Response separados com validações robustas
- ✅ **Entity Framework Core** - Migrations e relacionamentos bem definidos
- ✅ **API RESTful** - Endpoints com respostas HTTP padronizadas

---

## 🏗️ Arquitetura

```
MP-Financeiro/
├── backend/
│   ├── MPTeste.API/
│   │   ├── Controllers/          # Endpoints HTTP
│   │   ├── Services/             # Lógica de negócio
│   │   ├── Models/               # Entidades do domínio
│   │   ├── DTOs/                 # Request/Response
│   │   ├── Exceptions/           # Exceções customizadas
│   │   ├── Middleware/           # Tratamento global
│   │   ├── Data/                 # DbContext e Migrations
│   │   └── Program.cs            # Configuração DI
│   │
│   └── MPTeste.Tests/            # Testes unitários com xUnit
│
└── frontend/
    └── React + TypeScript + Vite
```

---

## 🛠️ Tecnologias

### Backend
- **Runtime:** .NET 9
- **Framework:** ASP.NET Core
- **Database:** SQL Server + Entity Framework Core
- **Testes:** xUnit, Moq, InMemoryDatabase
- **Logging:** Microsoft.Extensions.Logging

### Frontend
- **Framework:** React 18
- **Linguagem:** TypeScript
- **Build Tool:** Vite
- **Styling:** Bootstrap

---

## 📋 Funcionalidades

### 👥 Pessoas
- Cadastro de pessoas com validação de idade
- Listagem com relatório consolidado
- Cálculo automático de receitas/despesas/saldo
- Exclusão em cascata com suas transações

### 💳 Categorias
- Categorias para Receitas, Despesas ou Ambas
- Restrição de deleção se houver transações

### 💰 Transações
- Lançamento de receitas e despesas
- **Regra de Negócio:** Menores de idade só podem ter despesas
- **Regra de Negócio:** Tipo de transação deve corresponder à categoria
- Relatório consolidado por pessoa

---

## ⚙️ Pré-requisitos

- **Backend:** .NET SDK 9.0+
- **Frontend:** Node.js 18+
- **Database:** SQL Server (local ou container)
- **IDE:** Visual Studio 2022+ ou VSCode

---

## 🚀 Como Executar

### 1️⃣ Backend (.NET)

```bash
# Navegar para o diretório
cd backend

# Restaurar dependências
dotnet restore

# Aplicar migrations (banco de dados)
dotnet ef database update -p MPTeste.API

# Executar a API
dotnet run --project MPTeste.API
```

**Swagger disponível em:** `http://localhost:5000/swagger`

### 2️⃣ Frontend (React)

```bash
# Navegar para o diretório
cd frontend

# Instalar dependências
npm install

# Executar em desenvolvimento
npm run dev
```

**Aplicação disponível em:** `http://localhost:5173`

### 3️⃣ Executar Testes

```bash
# Testes do Backend
dotnet test MPTeste.Tests

# Testes com cobertura
dotnet test MPTeste.Tests /p:CollectCoverage=true
```

---

## 📚 Endpoints da API

### Pessoas (`/api/pessoas`)
```
GET    /api/pessoas              - Listar todas as pessoas
GET    /api/pessoas/totais       - Relatório financeiro consolidado
POST   /api/pessoas              - Criar nova pessoa
DELETE /api/pessoas/{id}         - Deletar pessoa
```

**Exemplo Request:**
```json
POST /api/pessoas
{
  "nome": "João Silva",
  "idade": 25
}
```

**Exemplo Response:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "idade": 25
}
```

### Categorias (`/api/categorias`)
```
GET  /api/categorias  - Listar todas
POST /api/categorias  - Criar categoria
```

### Transações (`/api/transacoes`)
```
GET  /api/transacoes  - Listar todas
POST /api/transacoes  - Registrar transação
```

---

## 💡 Regras de Negócio Implementadas

| Regra | Descrição |
|-------|-----------|
| 👶 **Menores de Idade** | Só podem registrar **Despesas**, nunca Receitas |
| 📊 **Tipo de Transação** | Deve corresponder à **Finalidade** da Categoria |
| 🗑️ **Exclusão de Categoria** | Bloqueada se houver transações associadas (Restrict) |
| 👤 **Exclusão de Pessoa** | Remove transações em cascata (Cascade) |

---

## 🧠 Decisões Técnicas Importantes

### 1. **Exceções Customizadas**
Criadas 3 exceções específicas para melhor tratamento:
- `NotFoundException` → HTTP 404
- `ValidationException` → HTTP 400
- `BusinessRuleViolationException` → HTTP 422

### 2. **Middleware Global**
Middleware `GlobalExceptionHandlerMiddleware` captura todas as exceções não tratadas e retorna respostas padronizadas.

### 3. **DTOs Separados**
Request e Response separados para maior flexibilidade e segurança (não exposição de entidades).

### 4. **Logging Estruturado**
Logs em diferentes níveis (Information, Error, Debug) para facilitar debugging em produção.

### 5. **Injeção de Dependência**
Services injetados via DI container, facilitando testes e manutenção.

### 6. **CORS Seguro**
CORS configurado com origens específicas (leitura do `appsettings.json`), não permite `AllowAnyOrigin`.

---

## 🧪 Testes Unitários

Cobertura de testes com xUnit + Moq:

```bash
# Executar todos os testes
dotnet test

# Testes com padrão de nomenclatura
dotnet test --filter "ClassName"
```

**Exemplos de testes:**
- ✓ `CriarAsync_DeveSalvarPessoaNoBanco`
- ✓ `CriarAsync_DeveThrowValidationException_QuandoIdadeNegativa`
- ✓ `ExcluirAsync_DeveThrowNotFoundException_QuandoIdNaoExiste`

---

## 📊 Estrutura de Resposta de Erro

Todos os erros retornam no padrão:

```json
{
  "statusCode": 404,
  "message": "Pessoa com ID 999 não encontrada.",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

---

## 🚀 Melhorias Futuras

- [ ] Autenticação com JWT
- [ ] Paginação em endpoints de listagem
- [ ] Filtros avançados (data, valor, categoria)
- [ ] Versionamento de API (v1, v2)
- [ ] GitHub Actions (CI/CD automático)
- [ ] Soft Delete (exclusão lógica)
- [ ] Relatórios em PDF

---

## 📁 Estrutura de Pastas Detalhada

```
backend/
├── MPTeste.API/
│   ├── Controllers/
│   │   ├── CategoriasController.cs
│   │   ├── PessoasController.cs
│   │   └── TransacoesController.cs
│   │
│   ├── Services/
│   │   ├── CategoriaService.cs
│   │   ├── PessoaService.cs
│   │   └── TransacaoService.cs
│   │
│   ├── Models/
│   │   ├── Pessoa.cs
│   │   ├── Categoria.cs
│   │   └── Transacao.cs
│   │
│   ├── DTOs/
│   │   ├── PessoaDtos.cs
│   │   ├── TransacaoDtos.cs
│   │   └── CategoriaDto.cs
│   │
│   ├── Exceptions/
│   │   ├── NotFoundException.cs
│   │   ├── ValidationException.cs
│   │   └── BusinessRuleViolationException.cs
│   │
│   ├── Middleware/
│   │   └── GlobalExceptionHandlerMiddleware.cs
│   │
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   │
│   └── Program.cs
│
└── MPTeste.Tests/
    ├── PessoaServiceTests.cs
    ├── TransacaoServiceTests.cs
    └── CategoriaServiceTests.cs
```

---

## 🤝 Como Contribuir

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Add MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## 📝 Convenção de Commits

Este projeto segue **Conventional Commits**:

- `feat(escopo): descrição` - Nova funcionalidade
- `refactor(escopo): descrição` - Refatoração
- `fix(escopo): descrição` - Correção de bug
- `test(escopo): descrição` - Testes
- `docs(escopo): descrição` - Documentação

---

## 👨‍💻 Autor

**Willian Mateus**

- GitHub: [@DuCrato](https://github.com/DuCrato)
- Email: williamanderson1994@hotmail.com

---

## 📄 Licença

Este projeto está licenciado sob a **Licença MIT** - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 🙏 Agradecimentos

- Microsoft .NET Team
- Entity Framework Core Community
- React Community
- Stack Overflow Community

---

**⭐ Se este projeto foi útil para você, deixe uma star!**
   
