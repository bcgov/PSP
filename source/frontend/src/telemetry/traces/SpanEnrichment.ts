import { Attributes, Span } from '@opentelemetry/api';
import { parseUrl } from '@opentelemetry/sdk-trace-web';
import {
  ATTR_HTTP_RESPONSE_STATUS_CODE,
  ATTR_SERVER_ADDRESS,
  ATTR_URL_SCHEME,
} from '@opentelemetry/semantic-conventions';
import { AxiosResponse } from 'axios';
import Keycloak from 'keycloak-js';

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

  public static enrichWithKeycloakToken(attributes: Attributes, keycloak: Keycloak) {
    if (!exists(attributes)) {
      attributes = {};
    }
    // JWT token dates are stored in SECONDS since January 1, 1970 (as opposed to milliseconds in JS dates)
    const accessTokenExpiry = (keycloak?.tokenParsed?.exp ?? 0) * 1000;
    // access token stats
    attributes['access.token.expiration'] = accessTokenExpiry;
    attributes['access.token.expiration_date'] = new Date(accessTokenExpiry).toISOString();
    attributes['access.token.is_expired'] = keycloak?.isTokenExpired() ?? false;
    // refresh token stats
    const refreshTokenExpiry = (keycloak?.refreshTokenParsed?.exp ?? 0) * 1000;
    attributes['refresh.token.expiration'] = refreshTokenExpiry;
    attributes['refresh.token.expiration_date'] = new Date(refreshTokenExpiry).toISOString();
  }
}
