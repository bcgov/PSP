import { Span } from '@opentelemetry/api';
import { parseUrl } from '@opentelemetry/sdk-trace-web';
import {
  ATTR_HTTP_RESPONSE_STATUS_CODE,
  ATTR_SERVER_ADDRESS,
  ATTR_URL_SCHEME,
} from '@opentelemetry/semantic-conventions';
import { AxiosResponse } from 'axios';

import { exists } from '@/utils';

export abstract class SpanEnrichment {
  public static enrichWithRoute(span: Span | undefined, route: string) {
    if (exists(span)) {
      span.setAttribute('page.route', route);
    }
    return span;
  }

  public static enrichWithXhrResponse(span: Span | undefined, response: AxiosResponse) {
    if (exists(span)) {
      const parsedUrl = parseUrl(response.config.url);
      span.setAttribute(ATTR_HTTP_RESPONSE_STATUS_CODE, response.status);
      if (exists(response.statusText)) {
        span.setAttribute('http.status_text', response.statusText);
      }
      span.setAttribute(ATTR_SERVER_ADDRESS, parsedUrl.host);
      span.setAttribute(ATTR_URL_SCHEME, parsedUrl.protocol.replace(':', ''));
    }
    return span;
  }
}
