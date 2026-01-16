export interface Pessoa {
    id: number;
    nome: string;
    idade: number;
}

export interface PessoaTotal {
    id: number;
    nome: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface RelatorioPessoas {
    pessoas: PessoaTotal[];
    totalGeralReceitas: number;
    totalGeralDespesas: number;
    saldoGeralLiquido: number;
}

export interface Categoria {
    id: number;
    descricao: string;
    finalidade: number;
}

export interface Transacao {
    id: number;
    descricao: string;
    valor: number;
    tipo: number;
    pessoaId: number;
    pessoaNome: string;
    categoriaId: number;
    categoriaDescricao: string;
}