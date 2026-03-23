import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import { handleApiError, logError } from './errorHandler';

/**
 * Instância Axios configurada com baseURL do .env
 * Inclui interceptadores para tratamento de erros e logging
 */
const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5028/api',
    timeout: import.meta.env.VITE_API_TIMEOUT ? parseInt(import.meta.env.VITE_API_TIMEOUT) : 5000
});

/**
 * Interceptador de Requisição
 * Adiciona headers padrão e logging
 */
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const logLevel = import.meta.env.VITE_LOG_LEVEL || 'info';
        if (logLevel === 'debug') {
            console.log(`📤 [${config.method?.toUpperCase()}] ${config.url}`, {
                data: config.data,
                headers: config.headers
            });
        }
        return config;
    },
    (error: AxiosError) => {
        logError('Interceptador Requisição', handleApiError(error));
        return Promise.reject(error);
    }
);

/**
 * Interceptador de Resposta
 * Trata erros HTTP de forma centralizada
 */
api.interceptors.response.use(
    (response) => {
        const logLevel = import.meta.env.VITE_LOG_LEVEL || 'info';
        if (logLevel === 'debug') {
            console.log(`📥 [${response.status}] ${response.config.url}`, response.data);
        }
        return response;
    },
    (error: AxiosError) => {
        const apiError = handleApiError(error);
        logError('Interceptador Resposta', apiError, {
            url: error.config?.url,
            method: error.config?.method
        });

        // Tratar erros específicos
        if (error.response?.status === 401) {
            // Redirecionar para login (se tiver autenticação)
            console.warn('⚠️ Token expirado ou acesso não autorizado');
        }

        if (error.response?.status === 403) {
            console.warn('⚠️ Acesso proibido');
        }

        if (error.response?.status === 500) {
            console.error('🔴 Erro no servidor');
        }

        return Promise.reject(apiError);
    }
);

export default api;