import { Attributes, Span, SpanKind, SpanStatusCode, trace, Tracer } from '@opentelemetry/api';

export const BROWSER_TRACER = 'react-client';

export type AsyncFn = () => Promise<unknown>;

// Export the tracer for custom instrumentation
export const getTracer = (): Tracer => {
  return trace.getTracer(BROWSER_TRACER);
};

export const startTrace = (spanName: string, additionalAttributes?: Attributes) => {
  const span = getTracer().startActiveSpan(spanName, { kind: SpanKind.CLIENT }, span => span);

  if (additionalAttributes) {
    span.setAttributes(additionalAttributes);
  }
  return span;
};

export const runWithSpan = async (spanName: string, fn: AsyncFn) => {
  const asyncCallback = wrapExternalCallInSpan(fn);
  return getTracer().startActiveSpan(spanName, { kind: SpanKind.CLIENT }, asyncCallback);
};

const wrapExternalCallInSpan = (fn: AsyncFn): ((span: Span) => ReturnType<AsyncFn>) => {
  return async span => {
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
  };
};
