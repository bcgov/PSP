import { Span } from '@opentelemetry/api';
import {
  ATTR_USER_FULL_NAME,
  ATTR_USER_NAME,
} from '@opentelemetry/semantic-conventions/incubating';

import { exists } from '@/utils';

export interface TraceUser {
  displayName: string;
  idir: string;
}

export abstract class SpanEnrichment {
  public static enrichWithUser(span: Span | undefined, userInfo: TraceUser) {
    if (exists(span)) {
      span.setAttribute(ATTR_USER_FULL_NAME, userInfo.displayName);
      span.setAttribute(ATTR_USER_NAME, userInfo.idir);
    }
    return span;
  }

  public static enrichWithRoute(span: Span | undefined, route: string) {
    if (exists(span)) {
      span.setAttribute('page.route', route);
    }
    return span;
  }
}
