import { useState } from 'react';
import { Modal, Button, Form, Alert } from 'react-bootstrap';
import api from '../services/api';

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

    /**
     * Realiza a validação dos dados e envia a requisição
     * para criar uma nova pessoa no backend
     */
    const salvar = async () => {
        // Validação básica de formulário
        if (!nome || idade <= 0) {
            setErro('Preencha nome e idade corretamente.');
            return;
        }

        try {
            // Envia os dados para a API
            await api.post('/pessoas', { nome, idade });

            // Limpa o formulário após sucesso
            setNome('');
            setIdade(0);
            setErro('');

            // Notifica o componente pai para atualizar a lista
            aoSalvar();

            // Fecha o modal
            handleClose();
        } catch (error) {
            console.error(error);
            setErro('Erro ao salvar pessoa.');
        }
    };

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Cadastrar Nova Pessoa</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                {/* Exibe mensagem de erro caso exista */}
                {erro && <Alert variant="danger">{erro}</Alert>}

                <Form>
                    {/* Campo Nome */}
                    <Form.Group className="mb-3">
                        <Form.Label>Nome</Form.Label>
                        <Form.Control 
                            type="text"
                            placeholder="Ex: João Silva"
                            value={nome}
                            onChange={e => setNome(e.target.value)}
                        />
                    </Form.Group>

                    {/* Campo Idade */}
                    <Form.Group className="mb-3">
                        <Form.Label>Idade</Form.Label>
                        <Form.Control 
                            type="number"
                            placeholder="Ex: 25"
                            value={idade}
                            onChange={e => setIdade(Number(e.target.value))}
                        />
                    </Form.Group>
                </Form>
            </Modal.Body>

            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Cancelar
                </Button>
                <Button variant="primary" onClick={salvar}>
                    Salvar
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default NovaPessoaModal;
