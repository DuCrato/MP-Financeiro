# Gestão Financeira - Teste Técnico

Projeto Full Stack desenvolvido para o processo seletivo. O sistema permite o cadastro de pessoas, transações financeiras (receitas/despesas) e visualização de saldos consolidados.

## 🚀 Tecnologias Utilizadas

- **Backend:** .NET 9, Entity Framework Core, SQL Server.
- **Frontend:** React, TypeScript, Vite, Bootstrap.
- **Testes:** xUnit, InMemory Database.

## ⚙️ Pré-requisitos

- .NET SDK 9.0
- Node.js (v18 ou superior)
- SQL Server (Instância Local ou Container)

## 🔧 Como Rodar o Projeto

### 1. Backend (.NET)

   ```bash
   cd backend/MPTeste.API
   ```
   ```bash
   dotnet restore
   ```
   ```bash
   dotnet run
   ```

### 2. Frontend (React)

   ```bash
   cd frontend
   ```
   ```bash
   npm install
   ```
   ```bash
   npm run dev
   ```

## 📋 Funcionalidades
- Cadastro de pessoas
- Cadastro de categorias
- Lançamento de receitas e despesas
- Cálculo automático de saldo por pessoa
- Visualização de totais consolidados

## 🧠 Decisões Técnicas
- Utilização de DTOs para evitar exposição direta das entidades
- Relatórios agregados calculados no backend para reduzir lógica no frontend
- Separação clara entre Controller, Service e Repository
   
