import { useState } from 'react';
import { Modal, Button, Form, Alert } from 'react-bootstrap';
import api from '../services/api';
import { handleApiError, getErrorMessage } from '../services/errorHandler';
import { logger } from '../services/logger';

/**
 * Propriedades recebidas do componente pai
 */
interface Props {
    show: boolean;              // Controla a visibilidade do modal
    handleClose: () => void;    // Função para fechar o modal
    aoSalvar: () => void;       // Callback para atualizar a listagem após salvar
}

/**
 * Modal responsável pelo cadastro de uma nova pessoa
 */
const NovaPessoaModal = ({ show, handleClose, aoSalvar }: Props) => {
    // Estados do formulário
    const [nome, setNome] = useState('');
    const [idade, setIdade] = useState<number>(0);
    const [erro, setErro] = useState('');
    const [salvando, setSalvando] = useState(false);

    /**
     * Valida os dados do formulário
     */
    const validar = (): boolean => {
        if (!nome?.trim()) {
            setErro('Nome é obrigatório');
            return false;
        }

        if (nome.trim().length < 3) {
            setErro('Nome deve ter pelo menos 3 caracteres');
            return false;
        }

        if (idade < 1 || idade > 150) {
            setErro('Idade deve estar entre 1 e 150 anos');
            return false;
        }

        return true;
    };

    /**
     * Realiza a validação dos dados e envia a requisição
     * para criar uma nova pessoa no backend
     */
    const salvar = async () => {
        setErro('');

        // Validação
        if (!validar()) {
            return;
        }

        setSalvando(true);
        logger.info('Salvando nova pessoa', { nome, idade });

        try {
            // Envia os dados para a API
            await api.post('/pessoas', { nome, idade });

            logger.info('✅ Pessoa criada com sucesso');

            // Limpa o formulário após sucesso
            setNome('');
            setIdade(0);
            setErro('');

            // Notifica o componente pai para atualizar a lista
            aoSalvar();

            // Fecha o modal
            handleClose();
        } catch (error) {
            const apiError = handleApiError(error);
            const mensagem = getErrorMessage(apiError);

            logger.error('❌ Erro ao salvar pessoa', apiError);
            setErro(mensagem);
        } finally {
            setSalvando(false);
        }
    };

    /**
     * Trata o Enter no formulário
     */
    const handleKeyPress = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter' && !salvando) {
            salvar();
        }
    };

    return (
        <Modal show={show} onHide={handleClose} backdrop="static">
            <Modal.Header closeButton disabled={salvando}>
                <Modal.Title>👤 Cadastrar Nova Pessoa</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                {/* Exibe mensagem de erro caso exista */}
                {erro && <Alert variant="danger">{erro}</Alert>}

                <Form onKeyPress={handleKeyPress}>
                    {/* Campo Nome */}
                    <Form.Group className="mb-3">
                        <Form.Label>Nome <span className="text-danger">*</span></Form.Label>
                        <Form.Control 
                            type="text"
                            placeholder="Ex: João Silva"
                            value={nome}
                            onChange={e => setNome(e.target.value)}
                            disabled={salvando}
                            autoFocus
                        />
                        <Form.Text className="text-muted">
                            Mínimo 3 caracteres
                        </Form.Text>
                    </Form.Group>

                    {/* Campo Idade */}
                    <Form.Group className="mb-3">
                        <Form.Label>Idade <span className="text-danger">*</span></Form.Label>
                        <Form.Control 
                            type="number"
                            placeholder="Ex: 25"
                            value={idade}
                            onChange={e => setIdade(Number(e.target.value))}
                            disabled={salvando}
                            min="1"
                            max="150"
                        />
                        <Form.Text className="text-muted">
                            Entre 1 e 150 anos
                        </Form.Text>
                    </Form.Group>
                </Form>
            </Modal.Body>

            <Modal.Footer>
                <Button 
                    variant="secondary" 
                    onClick={handleClose}
                    disabled={salvando}
                >
                    Cancelar
                </Button>
                <Button 
                    variant="primary" 
                    onClick={salvar}
                    disabled={salvando}
                >
                    {salvando ? '⏳ Salvando...' : '✅ Salvar'}
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default NovaPessoaModal;
