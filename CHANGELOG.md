# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e este projeto segue [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2024-01-15

### ✨ Added (Adicionado)

#### Backend
- ✅ Sistema completo de gestão de pessoas
  - Cadastro com validação de idade
  - Listagem simples e com relatório consolidado
  - Exclusão com cascade de transações
  
- ✅ Gerenciamento de categorias
  - Categorias para Receita, Despesa ou Ambas
  - Proteção contra exclusão se houver transações
  
- ✅ Registro de transações financeiras
  - Validação de regras de negócio
  - Regra: Menores de idade só podem ter despesas
  - Regra: Tipo deve corresponder à categoria
  
- ✅ Tratamento robusto de erros
  - Exceções customizadas (NotFoundException, ValidationException, BusinessRuleViolationException)
  - Middleware global `GlobalExceptionHandlerMiddleware`
  - Respostas HTTP padronizadas
  
- ✅ Logging estruturado
  - ILogger integrado em todos os serviços
  - Rastreamento de operações em diferentes níveis
  
- ✅ Testes unitários
  - xUnit + Moq para testes de services
  - InMemoryDatabase para testes isolados
  - Cobertura de casos de sucesso e erro
  
- ✅ Swagger/OpenAPI
  - Documentação automática dos endpoints
  - Interface interativa para testes

#### Frontend
- ✅ Interface React com TypeScript
- ✅ Tela de gestão de pessoas
- ✅ Tela de categorias
- ✅ Tela de transações
- ✅ Cálculo e visualização de saldos

### 🛠️ Technical (Técnico)

- ✅ Arquitetura em camadas (Controllers → Services → Data)
- ✅ Injeção de dependência configurada
- ✅ Entity Framework Core com migrations
- ✅ CORS seguro com origens específicas
- ✅ Primary Constructors (.NET 9)
- ✅ Async/Await em todas operações

---

## [0.9.0] - 2024-01-14

### 🔧 Changed (Modificado)

- Refatoração de Services para usar exceções customizadas
- Melhoria em validações de Controllers
- CORS configurado com origens específicas
- Testes unitários adaptados para novo sistema de exceções

### 📚 Documentation (Documentação)

- README melhorado com detalhes técnicos
- Guia de contribuição adicionado
- Estrutura de arquivos documentada

---

## [0.8.0] - 2024-01-13

### ✨ Added

- Infraestrutura de exceções customizadas
  - `NotFoundException`
  - `ValidationException`
  - `BusinessRuleViolationException`
  
- Middleware global `GlobalExceptionHandlerMiddleware`
  - Captura e trata exceções não tratadas
  - Logging centralizado de erros

---

## [0.7.0] - 2024-01-12

### 🔧 Changed

- Logging estruturado adicionado aos services
- Tratamento de erros melhorado em PessoaService
- Validações mais robustas

---

## [0.1.0] - 2024-01-10

### ✨ Added

- Projeto inicial criado
  - Estrutura base de Controllers, Services, Models
  - Configuração de Entity Framework Core
  - Primeiro conjunto de migrations

---

## 📝 Notas de Versão

### Como Contribuir

Para sugerir mudanças que devem ser documentadas no Changelog:

1. Siga o padrão do arquivo
2. Agrupe mudanças por tipo (Added, Changed, Deprecated, Removed, Fixed, Security)
3. Use linguagem clara e concisa
4. Referencie issues quando aplicável

### Tipos de Mudanças

- **Added** (✨): Nova funcionalidade
- **Changed** (🔧): Mudança em funcionalidade existente
- **Deprecated** (⚠️): Funcionalidade que será removida em breve
- **Removed** (🗑️): Funcionalidade removida
- **Fixed** (🐛): Correção de bug
- **Security** (🔐): Correção de vulnerabilidade

---

## 🔮 Roadmap Futuro

### v1.1.0 (Próxima)
- [ ] Autenticação com JWT
- [ ] Paginação em endpoints
- [ ] Filtros avançados por data e valor

### v1.2.0
- [ ] Versionamento de API
- [ ] GitHub Actions (CI/CD)
- [ ] Soft Delete para entidades

### v2.0.0
- [ ] Relatórios em PDF
- [ ] Integração com serviços externos
- [ ] Sincronização em tempo real com WebSockets

---

**[Voltar ao README](README.md)**
