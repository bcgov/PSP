import { trace } from '@opentelemetry/api';
import React, { useEffect } from 'react';

import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { getUserFromSession } from '@/utils';

import { SpanEnrichment } from './SpanEnrichment';

/**
 * SpanProcessor that adds attributes to spans based on logged-in user info.
 * Sets the `user.full_name`, `user.name` attributes
 */
export const UserInfoSpanProcessor: React.FunctionComponent<unknown> = () => {
  const session = useKeycloakWrapper();

  useEffect(() => {
    const activeSpan = trace.getActiveSpan();
    const user = getUserFromSession(session);
    SpanEnrichment.enrichWithUser(activeSpan, user);
  }, [session]);

  return null;
};
