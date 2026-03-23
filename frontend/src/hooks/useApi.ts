import { useState } from 'react';
import { ApiError, handleApiError, getErrorMessage } from '../services/errorHandler';
import { logger } from '../services/logger';

interface UseApiState<T> {
    data: T | null;
    loading: boolean;
    error: string | null;
}

interface UseApiReturn<T> extends UseApiState<T> {
    execute: <R = T>(apiCall: Promise<any>, context?: string) => Promise<R | null>;
    reset: () => void;
}

/**
 * Hook customizado para chamadas à API
 * Gerencia loading, error e data de forma centralizada
 * 
 * @param initialData - Dados iniciais (opcional)
 * @returns Estado e função execute para chamar a API
 */
export const useApi = <T,>(initialData: T | null = null): UseApiReturn<T> => {
    const [state, setState] = useState<UseApiState<T>>({
        data: initialData,
        loading: false,
        error: null
    });

    /**
     * Executa uma chamada à API
     */
    const execute = async <R = T,>(
        apiCall: Promise<any>,
        context: string = 'API Call'
    ): Promise<R | null> => {
        setState({ data: state.data, loading: true, error: null });

        try {
            logger.debug(`Iniciando ${context}`);
            const response = await apiCall;
            
            logger.info(`✅ ${context} - Sucesso`);
            setState({ data: response.data as T, loading: false, error: null });
            
            return response.data as R;
        } catch (err) {
            const error = handleApiError(err);
            const message = getErrorMessage(error);
            
            logger.error(`❌ ${context} - Erro`, {
                statusCode: error.statusCode,
                message: error.message
            });
            
            setState({ data: state.data, loading: false, error: message });
            return null;
        }
    };

    /**
     * Reseta o estado
     */
    const reset = () => {
        setState({ data: initialData, loading: false, error: null });
    };

    return { ...state, execute, reset };
};
