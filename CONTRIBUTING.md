# 🤝 Guia de Contribuição

Obrigado por considerar contribuir para o **MP-Financeiro**! Cada contribuição é valiosa e bem-vinda.

## 📋 Código de Conduta

Esperamos que todos os contribuintes sigam um código de conduta respeitoso. Qualquer comportamento abusivo será reportado.

## 🚀 Como Contribuir

### 1. Reportar Bugs

**Abra uma issue** com as seguintes informações:
- Descrição clara do bug
- Passos para reproduzir
- Comportamento esperado
- Ambiente (SO, versão .NET, etc)
- Screenshots ou logs (se aplicável)

**Exemplo:**
```
Título: [BUG] Erro ao deletar pessoa com transações

Descrição:
Ao tentar deletar uma pessoa que possui transações, 
a API retorna erro 500 em vez de 422.

Passos para reproduzir:
1. Criar pessoa
2. Criar transação para essa pessoa
3. Tentar deletar pessoa
4. Resultado: erro 500
```

### 2. Sugerir Melhorias

Abra uma issue com:
- Descrição da melhoria sugerida
- Por que seria útil
- Exemplos de implementação (se possível)

### 3. Enviar Pull Requests

#### Configurar o Ambiente

```bash
# 1. Fork o repositório
# https://github.com/DuCrato/MP-Financeiro/fork

# 2. Clone seu fork
git clone https://github.com/SEU_USUARIO/MP-Financeiro.git
cd MP-Financeiro

# 3. Adicione o repositório original como upstream
git remote add upstream https://github.com/DuCrato/MP-Financeiro.git

# 4. Crie uma branch
git checkout -b feature/sua-feature
```

#### Fazer as Mudanças

1. **Faça mudanças pequenas e focadas** - Um PR = Uma funcionalidade
2. **Siga o padrão de código** do projeto
3. **Adicione testes** para novas funcionalidades
4. **Atualize documentação** se necessário

#### Commits Profissionais

Siga **Conventional Commits**:

```bash
# ✅ Bom
git commit -m "feat(services): adicionar paginação em ListarAsync"

# ✅ Bom
git commit -m "refactor(controllers): melhorar validação de entrada"

# ❌ Ruim
git commit -m "fix bug"

# ❌ Ruim
git commit -m "Update"
```

**Tipos permitidos:**
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `refactor`: Melhoria de código
- `test`: Testes
- `docs`: Documentação
- `chore`: Configurações
- `perf`: Performance

#### Enviar o PR

```bash
# 1. Push para seu fork
git push origin feature/sua-feature

# 2. Abra um PR no GitHub
# Compare seu branch com main
# Descreva as mudanças em detalhes
```

**Template de PR:**

```markdown
## 📝 Descrição
Descrição clara do que foi mudado e por quê.

## 🎯 Tipo de Mudança
- [ ] Correção de bug
- [ ] Nova funcionalidade
- [ ] Melhoria de documentação

## ✅ Checklist
- [ ] Código segue o estilo do projeto
- [ ] Testes foram adicionados/atualizados
- [ ] Documentação foi atualizada
- [ ] Não há warnings de compilação
- [ ] Testes passam localmente

## 🔗 Issues Relacionadas
Fecha #123
```

---

## 📊 Padrões de Código

### Backend (.NET)

#### Nomenclatura
```csharp
// ✅ Classes, Métodos, Propriedades: PascalCase
public class PessoaService { }
public async Task<List<PessoaResponseDto>> ListarAsync() { }
public string Nome { get; set; }

// ✅ Variáveis locais: camelCase
var minhaVariavel = 10;

// ✅ Constantes: UPPER_SNAKE_CASE
private const string CONNECTION_STRING = "...";
```

#### Formatação
```csharp
// ✅ Use async/await
public async Task<List<T>> ListarAsync()

// ✅ Use injeção de dependência
public class MinhaService(AppDbContext context, ILogger<MinhaService> logger)

// ✅ Validate inputs
if (id <= 0)
    throw new ValidationException("ID deve ser maior que 0");

// ❌ Evite callbacks
// ❌ Evite exceções genéricas
// ❌ Evite variáveis globais
```

#### Testes
```csharp
// ✅ Nome descritivo: MethodName_Scenario_ExpectedResult
[Fact]
public async Task CriarAsync_DeveThrowValidationException_QuandoNomeVazio()
{
    // Arrange
    var request = new PessoaRequestDto { Nome = "", Idade = 25 };
    
    // Act & Assert
    await Assert.ThrowsAsync<ValidationException>(
        () => service.CriarAsync(request));
}
```

### Frontend (React/TypeScript)

```typescript
// ✅ Components: PascalCase
export const PessoaList = () => { }

// ✅ Funções utilitárias: camelCase
export const formatarMoeda = (valor: number) => { }

// ✅ Constantes: UPPER_SNAKE_CASE
const API_BASE_URL = "http://localhost:5000/api";

// ✅ Use TypeScript para tipagem
interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}
```

---

## 🧪 Testes

### Antes de submeter PR

```bash
# Backend
dotnet test --configuration Release

# Frontend
npm test
npm run lint
```

---

## 📚 Recursos Úteis

- [Documentação .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [React Docs](https://react.dev/)
- [Conventional Commits](https://www.conventionalcommits.org/)

---

## ❓ Dúvidas?

- Abra uma **Discussion** no repositório
- Verifique **Issues existentes**
- Leia a documentação no README

---

## 📄 Licença

Ao contribuir, você concorda que suas contribuições serão licenciadas sob a licença MIT do projeto.

---

**Obrigado por contribuir para MP-Financeiro! 🙏**
