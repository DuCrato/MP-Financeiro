import PessoaList from './components/PessoaList';

function App() {
  return (
    <div className="bg-light min-vh-100">
      {/* Barra de Navegação Simples */}
      <nav className="navbar navbar-dark bg-dark mb-3">
        <div className="container">
          <span className="navbar-brand mb-0 h1">MP Financeiro</span>
        </div>
      </nav>

      {/* Tela de Listagem Pessoa */}
      <PessoaList />
    </div>
  );
}

export default App;