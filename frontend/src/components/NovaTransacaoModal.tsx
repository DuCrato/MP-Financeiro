import { useEffect, useState } from 'react';
import { Modal, Button, Form, Alert, Row, Col, InputGroup } from 'react-bootstrap';
import api from '../services/api';
import type { Categoria } from '../types';

/**
 * Propriedades recebidas do componente pai
 */
interface Props {
    show: boolean;              // Controla visibilidade do modal
    handleClose: () => void;    // Fecha o modal
    pessoaId: number | null;    // Pessoa vinculada à transação
    aoSalvar: () => void;       // Callback para atualizar a listagem após salvar
}

/**
 * Modal responsável pelo cadastro de uma nova transação financeira
 */
const NovaTransacaoModal = ({ show, handleClose, pessoaId, aoSalvar }: Props) => {

    // Estados do formulário
    const [descricao, setDescricao] = useState('');

    /**
     * valorString → valor formatado para exibição (ex: 1.234,56)
     * valorNumerico → valor real enviado para o backend
     */
    const [valorString, setValorString] = useState('');
    const [valorNumerico, setValorNumerico] = useState(0);
    const [tipo, setTipo] = useState(0);
    const [categoriaId, setCategoriaId] = useState<number>(0);
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [erro, setErro] = useState('');

    /**
     * Ao abrir o modal:
     * - Carrega categorias
     * - Limpa formulário e mensagens de erro
     */
    useEffect(() => {
        if (show) {
            carregarCategorias();
            setErro('');
            setDescricao('');
            setValorString('');
            setValorNumerico(0);
        }
    }, [show]);

    /**
     * Busca as categorias cadastradas no backend
     */
    const carregarCategorias = async () => {
        try {
            const response = await api.get('/categorias');
            setCategorias(response.data);

            // Define a primeira categoria como padrão
            if (response.data.length > 0) {
                setCategoriaId(response.data[0].id);
            }
        } catch (error) {
            console.error("Erro ao carregar categorias", error);
        }
    };

    /**
     * Trata o valor digitado:
     * - Remove caracteres não numéricos
     * - Converte para decimal
     * - Formata no padrão brasileiro (R$)
     */
    const handleValorChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const valorDigitado = e.target.value.replaceAll(/\D/g, "");

        if (valorDigitado === "") {
            setValorString("");
            setValorNumerico(0);
            return;
        }

        const numero = Number.parseFloat(valorDigitado) / 100;

        const formatado = numero.toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });

        setValorString(formatado);
        setValorNumerico(numero);
    };

    /**
     * Valida os dados e envia a transação para o backend
     * As regras de negócio (menor de idade, categoria x tipo, etc.)
     * são validadas no servidor
     */
    const salvar = async () => {
        if (!pessoaId || valorNumerico <= 0 || !descricao || !categoriaId) {
            setErro('Preencha todos os campos corretamente.');
            return;
        }

        try {
            await api.post('/transacoes', {
                descricao,
                valor: valorNumerico,
                tipo,
                pessoaId,
                categoriaId
            });

            // Atualiza listagem e fecha modal
            aoSalvar();
            handleClose();
        } catch (error) {
            console.error(error);
            setErro('Erro ao salvar transação. Verifique as regras de negócio.');
        }
    };

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Nova Transação</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                {/* Exibição de mensagem de erro */}
                {erro && <Alert variant="danger">{erro}</Alert>}

                <Form>
                    {/* Descrição da transação */}
                    <Form.Group className="mb-3">
                        <Form.Label>Descrição</Form.Label>
                        <Form.Control 
                            type="text"
                            placeholder="Ex: Salário, Aluguel..."
                            value={descricao}
                            onChange={e => setDescricao(e.target.value)}
                        />
                    </Form.Group>

                    <Row>
                        {/* Campo Valor */}
                        <Col>
                            <Form.Group className="mb-3">
                                <Form.Label>Valor</Form.Label>
                                <InputGroup>
                                    <InputGroup.Text>R$</InputGroup.Text>
                                    <Form.Control 
                                        type="text"
                                        placeholder="0,00"
                                        value={valorString}
                                        onChange={handleValorChange}
                                    />
                                </InputGroup>
                            </Form.Group>
                        </Col>

                        {/* Tipo da transação */}
                        <Col>
                            <Form.Group className="mb-3">
                                <Form.Label>Tipo</Form.Label>
                                <Form.Select 
                                    value={tipo}
                                    onChange={e => setTipo(Number(e.target.value))}
                                >
                                    <option value={0}>Despesa</option>
                                    <option value={1}>Receita</option>
                                </Form.Select>
                            </Form.Group>
                        </Col>
                    </Row>

                    {/* Categoria */}
                    <Form.Group className="mb-3">
                        <Form.Label>Categoria</Form.Label>
                        <Form.Select 
                            value={categoriaId}
                            onChange={e => setCategoriaId(Number(e.target.value))}
                        >
                            {categorias.map(cat => (
                                <option key={cat.id} value={cat.id}>
                                    {cat.descricao}
                                </option>
                            ))}
                        </Form.Select>

                        {categorias.length === 0 && (
                            <Form.Text className="text-muted">
                                Nenhuma categoria cadastrada.
                            </Form.Text>
                        )}
                    </Form.Group>
                </Form>
            </Modal.Body>

            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Cancelar
                </Button>
                <Button variant="success" onClick={salvar}>
                    Salvar Transação
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default NovaTransacaoModal;
