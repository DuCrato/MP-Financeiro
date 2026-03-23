import { useEffect, useState } from 'react';
import { Modal, Button, Form, Alert, Row, Col, InputGroup, Spinner } from 'react-bootstrap';
import api from '../services/api';
import { handleApiError, getErrorMessage } from '../services/errorHandler';
import { logger } from '../services/logger';
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
    const [valorString, setValorString] = useState('');
    const [valorNumerico, setValorNumerico] = useState(0);
    const [tipo, setTipo] = useState(0);
    const [categoriaId, setCategoriaId] = useState<number>(0);
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [erro, setErro] = useState('');
    const [salvando, setSalvando] = useState(false);
    const [carregandoCategorias, setCarregandoCategorias] = useState(false);

    /**
     * Ao abrir o modal:
     * - Carrega categorias
     * - Limpa formulário e mensagens de erro
     */
    useEffect(() => {
        if (show) {
            carregarCategorias();
            limparFormulario();
        }
    }, [show]);

    /**
     * Limpa o formulário
     */
    const limparFormulario = () => {
        setErro('');
        setDescricao('');
        setValorString('');
        setValorNumerico(0);
        setTipo(0);
    };

    /**
     * Busca as categorias cadastradas no backend
     */
    const carregarCategorias = async () => {
        setCarregandoCategorias(true);
        logger.info('Carregando categorias');

        try {
            const response = await api.get('/categorias');
            setCategorias(response.data);

            // Define a primeira categoria como padrão
            if (response.data.length > 0) {
                setCategoriaId(response.data[0].id);
            }
            logger.info('✅ Categorias carregadas', { count: response.data.length });
        } catch (error) {
            const apiError = handleApiError(error);
            logger.error('❌ Erro ao carregar categorias', apiError);
            setErro(getErrorMessage(apiError));
        } finally {
            setCarregandoCategorias(false);
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
     */
    const validar = (): boolean => {
        if (!descricao?.trim()) {
            setErro('Descrição é obrigatória');
            return false;
        }

        if (descricao.length < 3) {
            setErro('Descrição deve ter pelo menos 3 caracteres');
            return false;
        }

        if (valorNumerico <= 0) {
            setErro('Valor deve ser maior que zero');
            return false;
        }

        if (tipo === 0 || tipo === undefined) {
            setErro('Selecione o tipo de transação');
            return false;
        }

        if (!categoriaId) {
            setErro('Selecione uma categoria');
            return false;
        }

        if (!pessoaId) {
            setErro('Nenhuma pessoa selecionada');
            return false;
        }

        return true;
    };

    /**
     * Valida os dados e envia a transação para o backend
     */
    const salvar = async () => {
        setErro('');

        if (!validar()) {
            return;
        }

        setSalvando(true);
        logger.info('Salvando transação', { descricao, valorNumerico, tipo, pessoaId, categoriaId });

        try {
            await api.post('/transacoes', {
                descricao,
                valor: valorNumerico,
                tipo,
                pessoaId,
                categoriaId
            });

            logger.info('✅ Transação criada com sucesso');

            // Limpa e fecha
            limparFormulario();
            aoSalvar();
            handleClose();
        } catch (error) {
            const apiError = handleApiError(error);
            const mensagem = getErrorMessage(apiError);

            logger.error('❌ Erro ao salvar transação', apiError);
            setErro(mensagem);
        } finally {
            setSalvando(false);
        }
    };

    /**
     * Trata Enter no formulário
     */
    const handleKeyPress = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter' && !salvando) {
            salvar();
        }
    };

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" size="lg">
            <Modal.Header closeButton disabled={salvando || carregandoCategorias}>
                <Modal.Title>💰 Nova Transação</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                {/* Exibição de mensagem de erro */}
                {erro && <Alert variant="danger">{erro}</Alert>}

                {carregandoCategorias ? (
                    <div className="text-center">
                        <Spinner animation="border" size="sm" className="me-2" />
                        Carregando categorias...
                    </div>
                ) : (
                    <Form onKeyPress={handleKeyPress}>
                        {/* Descrição da transação */}
                        <Form.Group className="mb-3">
                            <Form.Label>Descrição <span className="text-danger">*</span></Form.Label>
                            <Form.Control 
                                type="text"
                                placeholder="Ex: Salário, Aluguel, Compras..."
                                value={descricao}
                                onChange={e => setDescricao(e.target.value)}
                                disabled={salvando}
                                autoFocus
                            />
                            <Form.Text className="text-muted">
                                Mínimo 3 caracteres
                            </Form.Text>
                        </Form.Group>

                        <Row>
                            {/* Campo Valor */}
                            <Col md={6}>
                                <Form.Group className="mb-3">
                                    <Form.Label>Valor <span className="text-danger">*</span></Form.Label>
                                    <InputGroup>
                                        <InputGroup.Text>R$</InputGroup.Text>
                                        <Form.Control 
                                            type="text"
                                            placeholder="0,00"
                                            value={valorString}
                                            onChange={handleValorChange}
                                            disabled={salvando}
                                        />
                                    </InputGroup>
                                    <Form.Text className="text-muted">
                                        Digite o valor em reais
                                    </Form.Text>
                                </Form.Group>
                            </Col>

                            {/* Tipo da transação */}
                            <Col md={6}>
                                <Form.Group className="mb-3">
                                    <Form.Label>Tipo <span className="text-danger">*</span></Form.Label>
                                    <Form.Select 
                                        value={tipo}
                                        onChange={e => setTipo(Number(e.target.value))}
                                        disabled={salvando}
                                    >
                                        <option value={0}>📉 Despesa</option>
                                        <option value={1}>📈 Receita</option>
                                    </Form.Select>
                                </Form.Group>
                            </Col>
                        </Row>

                        {/* Categoria */}
                        <Form.Group className="mb-3">
                            <Form.Label>Categoria <span className="text-danger">*</span></Form.Label>
                            {categorias.length > 0 ? (
                                <Form.Select 
                                    value={categoriaId}
                                    onChange={e => setCategoriaId(Number(e.target.value))}
                                    disabled={salvando}
                                >
                                    <option value={0}>Selecione uma categoria...</option>
                                    {categorias.map(cat => (
                                        <option key={cat.id} value={cat.id}>
                                            {cat.descricao}
                                        </option>
                                    ))}
                                </Form.Select>
                            ) : (
                                <Alert variant="warning">
                                    ⚠️ Nenhuma categoria cadastrada. Crie uma categoria primeiro.
                                </Alert>
                            )}
                        </Form.Group>
                    </Form>
                )}
            </Modal.Body>

            <Modal.Footer>
                <Button 
                    variant="secondary" 
                    onClick={handleClose}
                    disabled={salvando || carregandoCategorias}
                >
                    Cancelar
                </Button>
                <Button 
                    variant="success" 
                    onClick={salvar}
                    disabled={salvando || carregandoCategorias || categorias.length === 0}
                >
                    {salvando ? '⏳ Salvando...' : '✅ Salvar Transação'}
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default NovaTransacaoModal;
