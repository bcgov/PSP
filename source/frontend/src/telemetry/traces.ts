import { Attributes, Span, SpanKind, SpanStatusCode, trace, Tracer } from '@opentelemetry/api';

export const BROWSER_TRACER = 'react-client';

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

export const runWithSpan = async <F extends (span: Span) => unknown>(
  spanName: string,
  additionalAttributes: Attributes,
  fn: F,
) => {
  const asyncCallback = wrapExternalCallInSpan(fn, additionalAttributes);
  return getTracer().startActiveSpan(spanName, { kind: SpanKind.CLIENT }, asyncCallback);
};

const wrapExternalCallInSpan = <F extends (span: Span) => unknown>(
  fn: F,
  additionalAttributes: Attributes,
): ((span: Span) => unknown) => {
  return async span => {
    try {
      span.setAttributes(additionalAttributes);
      const result = await fn(span);
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
