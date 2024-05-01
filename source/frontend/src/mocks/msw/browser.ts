// 1. Import the library.
import { setupWorker } from 'msw/browser';

import { handlers } from './handlers';

// 2. Describe network behavior with request handlers.
export const worker = setupWorker(...handlers);
