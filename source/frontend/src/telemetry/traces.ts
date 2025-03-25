import { SpanKind, SpanStatusCode, trace, Tracer } from '@opentelemetry/api';

export const BROWSER_TRACER = 'react-client';

export const getTracer = (): Tracer => {
  return trace.getTracer(BROWSER_TRACER);
};

export const runWithSpan = async (name: string, fn: () => Promise<unknown>) => {
  return getTracer().startActiveSpan(name, { kind: SpanKind.CLIENT }, async span => {
    try {
      const result = await fn();
      span.setStatus({ code: SpanStatusCode.OK });
      return result;
    } catch (err) {
      // Record the exception and update the span status.
      span.recordException(err);
      span.setStatus({
        code: SpanStatusCode.ERROR,
        message: err.message,
      });
      throw err;
    } finally {
      // Be sure to end the span!
      span.end();
    }
  });
};
