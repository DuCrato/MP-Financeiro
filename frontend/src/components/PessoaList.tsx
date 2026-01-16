import { useEffect, useState } from "react";
import api from "../services/api";
import type { RelatorioPessoas } from "../types";
import {
    Table,
    Container,
    Alert,
    Spinner,
    Badge,
    Card,
    Row,
    Col,
} from "react-bootstrap";
import NovaPessoaModal from "./NovaPessoaModal";
import NovaTransacaoModal from "./NovaTransacaoModal";

/**
 * Componente responsável por:
 * - Exibir o relatório financeiro por pessoa
 * - Mostrar totais gerais (receitas, despesas e saldo)
 * - Permitir cadastro de pessoas e transações
 */
const PessoaList = () => {

    const [relatorio, setRelatorio] = useState<RelatorioPessoas | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [showModal, setShowModal] = useState(false);
    const [showTransacaoModal, setShowTransacaoModal] = useState(false);
    const [selectedPessoaId, setSelectedPessoaId] = useState<number | null>(null);

    /**
     * Carrega os dados ao montar o componente
     */
    useEffect(() => {
        carregarDados();
    }, []);

    /**
     * Busca o relatório financeiro por pessoa no backend
     */
    const carregarDados = () => {
        api
            .get<RelatorioPessoas>("/pessoas/totais")
            .then((response) => {
                setRelatorio(response.data);
                setLoading(false);
            })
            .catch((err) => {
                console.error(err);
                setError(
                    "Erro ao carregar dados. Verifique se o Backend está rodando."
                );
                setLoading(false);
            });
    };

    /**
     * Formata valores monetários no padrão brasileiro (R$)
     */
    const formatarValor = (valor: number) => {
        return new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
        }).format(valor);
    };

    /**
     * Estado de carregamento
     */
    if (loading) {
        return (
            <Container className="mt-5 text-center">
                <Spinner animation="border" variant="primary" />
                <p>Carregando dados...</p>
            </Container>
        );
    }

    /**
     * Estado de erro
     */
    if (error) {
        return (
            <Container className="mt-5">
                <Alert variant="danger">{error}</Alert>
            </Container>
        );
    }

    return (
        <Container className="mt-4">

            {/* Cabeçalho da tela */}
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2 className="mb-0">Gestão Financeira</h2>
                <button
                    className="btn btn-primary"
                    onClick={() => setShowModal(true)}
                >
                    + Nova Pessoa
                </button>
            </div>

            {/* Cards de resumo geral */}
            {relatorio && (
                <Row className="mb-4 text-center">
                    <Col>
                        <Card className="text-white bg-success mb-3">
                            <Card.Header>Receitas Totais</Card.Header>
                            <Card.Body>
                                <Card.Title>
                                    {formatarValor(relatorio.totalGeralReceitas)}
                                </Card.Title>
                            </Card.Body>
                        </Card>
                    </Col>

                    <Col>
                        <Card className="text-white bg-danger mb-3">
                            <Card.Header>Despesas Totais</Card.Header>
                            <Card.Body>
                                <Card.Title>
                                    {formatarValor(relatorio.totalGeralDespesas)}
                                </Card.Title>
                            </Card.Body>
                        </Card>
                    </Col>

                    <Col>
                        <Card
                            className={`text-white mb-3 ${
                                relatorio.saldoGeralLiquido >= 0
                                    ? "bg-primary"
                                    : "bg-warning"
                            }`}
                        >
                            <Card.Header>Saldo Geral</Card.Header>
                            <Card.Body>
                                <Card.Title>
                                    {formatarValor(relatorio.saldoGeralLiquido)}
                                </Card.Title>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>
            )}

            {/* Tabela de pessoas */}
            <Card className="shadow-sm">
                <Card.Header>Pessoas Cadastradas</Card.Header>

                <Table hover responsive className="mb-0">
                    <thead className="table-light">
                        <tr>
                            <th>Nome</th>
                            <th className="text-success">Receitas</th>
                            <th className="text-danger">Despesas</th>
                            <th>Saldo</th>
                            <th>Ações</th>
                        </tr>
                    </thead>

                    <tbody>
                        {relatorio?.pessoas?.map((pessoa) => (
                            <tr key={pessoa.id}>
                                <td className="fw-bold">{pessoa.nome}</td>

                                <td className="text-success">
                                    {formatarValor(pessoa.totalReceitas)}
                                </td>

                                <td className="text-danger">
                                    {formatarValor(pessoa.totalDespesas)}
                                </td>

                                <td>
                                    <Badge bg={pessoa.saldo >= 0 ? "success" : "danger"}>
                                        {formatarValor(pessoa.saldo)}
                                    </Badge>
                                </td>

                                <td>
                                    <button
                                        className="btn btn-sm btn-outline-primary"
                                        onClick={() => {
                                            setSelectedPessoaId(pessoa.id);
                                            setShowTransacaoModal(true);
                                        }}
                                    >
                                        + Transação
                                    </button>
                                </td>
                            </tr>
                        ))}

                        {/* Estado vazio */}
                        {relatorio?.pessoas?.length === 0 && (
                            <tr>
                                <td colSpan={5} className="text-center text-muted">
                                    Nenhuma pessoa cadastrada.
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </Card>

            {/* Modal de cadastro de pessoa */}
            <NovaPessoaModal
                show={showModal}
                handleClose={() => setShowModal(false)}
                aoSalvar={carregarDados}
            />

            {/* Modal de cadastro de transação */}
            <NovaTransacaoModal
                show={showTransacaoModal}
                handleClose={() => setShowTransacaoModal(false)}
                pessoaId={selectedPessoaId}
                aoSalvar={carregarDados}
            />
        </Container>
    );
};

export default PessoaList;
