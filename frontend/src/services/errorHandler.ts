import axios, { AxiosError } from 'axios';

export interface ApiError {
    statusCode: number;
    message: string;
    details?: unknown;
    timestamp?: string;
}


export const handleApiError = (error: unknown): ApiError => {
    if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<any>;
        return {
            statusCode: axiosError.response?.status || 500,
            message: axiosError.response?.data?.message || axiosError.message || 'Erro na requisição',
            details: axiosError.response?.data,
            timestamp: new Date().toISOString()
        };
    }

    if (error instanceof Error) {
        return {
            statusCode: 500,
            message: error.message,
            timestamp: new Date().toISOString()
        };
    }

    return {
        statusCode: 500,
        message: 'Erro desconhecido',
        timestamp: new Date().toISOString()
    };
};

/**
 * Converte erro da API em mensagem amigável para o usuário
 */
export const getErrorMessage = (error: ApiError): string => {
    switch (error.statusCode) {
        case 400:
            return `❌ Dados inválidos: ${error.message}`;
        case 404:
            return `❌ Recurso não encontrado: ${error.message}`;
        case 422:
            return `⚠️ Regra violada: ${error.message}`;
        case 500:
            return `🔴 Erro no servidor: ${error.message}`;
        case 503:
            return `⏱️ Servidor indisponível. Tente novamente mais tarde.`;
        case 0:
            return '❌ Falha de conexão. Verifique se o backend está rodando.';
        default:
            return `❌ Erro: ${error.message}`;
    }
};


export const logError = (context: string, error: ApiError, additionalInfo?: any) => {
    const logLevel = import.meta.env.VITE_LOG_LEVEL || 'info';
    
    if (logLevel === 'debug' || logLevel === 'info') {
        console.error(`[${context}]`, {
            statusCode: error.statusCode,
            message: error.message,
            details: error.details,
            timestamp: error.timestamp,
            ...additionalInfo
        });
    }
};
