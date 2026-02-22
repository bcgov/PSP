import { Middleware } from '@reduxjs/toolkit';

import { runWithSpan } from '@/telemetry/traces';
import { exists } from '@/utils/utils';

import { logError } from './slices/network/networkSlice';

export const telemetryMiddleware: Middleware = _storeAPI => next => async action => {
  const result = await next(action);

  if (logError.match(action)) {
    const { name, status, error } = action.payload;

    await runWithSpan(
      'network.error',
      {
        'network.request.name': name,
        'network.response.status': status,
        'network.error.message': error?.message ?? 'Unknown error',
        'network.error.code': error?.code,
        'network.error.config.url': error?.config?.url,
        'network.error.config.method': error?.config?.method,
      },
      async span => {
        if (exists(error)) {
          span.recordException(error);
        }
      },
    );
  }

  return result;
};
