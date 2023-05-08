import { rest } from 'msw';
import { setupServer } from 'msw/node';

import { handlers } from './handlers';

// This configures a request mocking server with the given request handlers (for jest unit tests).
const server = setupServer(...handlers);

export { rest, server };
