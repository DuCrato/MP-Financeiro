/**
 * Serviço centralizado de logging
 * Facilita rastreamento de eventos e erros
 */

type LogLevel = 'debug' | 'info' | 'warn' | 'error';

const LOG_LEVELS = {
    debug: 0,
    info: 1,
    warn: 2,
    error: 3
};

const getCurrentLogLevel = (): number => {
    const level = import.meta.env.VITE_LOG_LEVEL || 'info';
    return LOG_LEVELS[level as LogLevel] || 1;
};

const formatLog = (level: LogLevel, message: string, data?: any): void => {
    const timestamp = new Date().toISOString();
    const prefix = `[${timestamp}] [${level.toUpperCase()}]`;

    switch (level) {
        case 'debug':
            if (getCurrentLogLevel() <= LOG_LEVELS.debug) {
                console.debug(`${prefix} ${message}`, data);
            }
            break;
        case 'info':
            if (getCurrentLogLevel() <= LOG_LEVELS.info) {
                console.log(`${prefix} ${message}`, data);
            }
            break;
        case 'warn':
            if (getCurrentLogLevel() <= LOG_LEVELS.warn) {
                console.warn(`${prefix} ${message}`, data);
            }
            break;
        case 'error':
            if (getCurrentLogLevel() <= LOG_LEVELS.error) {
                console.error(`${prefix} ${message}`, data);
            }
            break;
    }
};

export const logger = {
    debug: (message: string, data?: any) => formatLog('debug', message, data),
    info: (message: string, data?: any) => formatLog('info', message, data),
    warn: (message: string, data?: any) => formatLog('warn', message, data),
    error: (message: string, data?: any) => formatLog('error', message, data)
};
