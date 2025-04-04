import { Span } from '@opentelemetry/api';
import { SpanProcessor } from '@opentelemetry/sdk-trace-web';
import {
  ATTR_USER_FULL_NAME,
  ATTR_USER_NAME,
} from '@opentelemetry/semantic-conventions/incubating';

import { user } from '..';

/**
 * A {@link SpanProcessor} that adds user information to each span.
 */
export class UserInfoSpanProcessor implements SpanProcessor {
  onStart(span: Span) {
    const userInfo = user.getUserManager().getUser();

    span.setAttributes({
      [ATTR_USER_FULL_NAME]: userInfo?.displayName ?? '',
      [ATTR_USER_NAME]: userInfo?.idir ?? '',
    });
  }

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  onEnd() {}

  forceFlush() {
    return Promise.resolve();
  }

  shutdown() {
    return Promise.resolve();
  }
}
