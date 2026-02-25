import { Middleware } from '@reduxjs/toolkit';

import { Telemetry } from '@/telemetry';

import { logError } from './slices/network/networkSlice';

// Log "bomb" errors to telemetry with relevant attributes for easier debugging and monitoring of network errors
export const telemetryMiddleware: Middleware = _storeAPI => next => async action => {
  const result = await next(action);

  if (logError.match(action)) {
    const { name, status, error } = action.payload;

    Telemetry.recordException(
      error,
      {
        'network.request.name': name,
        'network.response.status': status,
        'network.error.message': error?.message ?? 'Unknown error',
        'network.error.code': error?.code,
        'network.error.config.url': error?.config?.url,
        'network.error.config.method': error?.config?.method,
      },
      'network.error',
    );
  }

  return result;
};
