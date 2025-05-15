import { trace } from '@opentelemetry/api';
import React, { useEffect } from 'react';
import { useRouteMatch } from 'react-router-dom';

import { SpanEnrichment } from './SpanEnrichment';

/**
 * SpanProcessor that adds attributes to spans based on the state of the React Router.
 * Sets the `page.route` attribute to the generic dynamic route.
 */
export const ReactRouterSpanProcessor: React.FunctionComponent<unknown> = () => {
  const { path } = useRouteMatch();

  useEffect(() => {
    const activeSpan = trace.getActiveSpan();
    SpanEnrichment.enrichWithRoute(activeSpan, path);
  }, [path]);

  return null;
};
