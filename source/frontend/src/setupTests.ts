// jest-dom adds custom jest matchers for asserting on DOM nodes.
// allows you to do things like:
// expect(element).toHaveTextContent(/react/i)
// learn more: https://github.com/testing-library/jest-dom
import '@testing-library/jest-dom';
import 'jest-styled-components';

import noop from 'lodash/noop';
import moment from 'moment';
import { MockedRequest } from 'msw';
import failOnConsole from 'vitest-fail-on-console';

import { server } from '@/mocks/msw/server';

const localStorageMock = (function () {
  let store: any = {};

  return {
    getKeys: function () {
      return store;
    },
    getItem: function (key: string) {
      return store[key] || null;
    },
    setItem: function (key: string, value: any) {
      store[key] = value.toString();
    },
    removeItem: function (key: string) {
      store[key] = undefined;
    },
    clear: function () {
      store = {};
    },
  };
})();
Object.defineProperty(window, 'localStorage', {
  value: localStorageMock,
});

// workaround to allow polyline and other svg map renderers to function correctly in tests.
const createElementNSOrig = (global as any).document.createElementNS;
(global as any).document.createElementNS = function (namespaceURI: any, qualifiedName: any) {
  if (namespaceURI === 'http://www.w3.org/2000/svg' && qualifiedName === 'svg') {
    const element = createElementNSOrig.apply(this, arguments);
    element.createSVGRect = noop;
    return element;
  }
  return createElementNSOrig.apply(this, arguments);
};

// This allows to run unit tests on GitHub Actions which are in GMT timezone by default
['Date', 'Day', 'FullYear', 'Hours', 'Minutes', 'Month', 'Seconds'].forEach(prop => {
  (Date.prototype as any)[`get${prop}`] = function () {
    return (new Date(this.getTime() + moment(this.getTime()).utcOffset() * 60000) as any)[
      `getUTC${prop}`
    ]();
  };
});

window.scrollTo = vi.fn() as unknown as any; // not implemented by jsdom.

// Set default tenant for unit tests
import.meta.env.VITE_TENANT = 'MOTI';

const onUnhandledRequest = vi.fn();

// Establish API mocking before all tests.
beforeAll(() => server.listen({ onUnhandledRequest }));

// Reset any request handlers that we may add during the tests,
// so they don't affect other tests.
afterEach(() => {
  server.resetHandlers();
  try {
    expect(onUnhandledRequest).not.toHaveBeenCalled();
  } catch (e) {
    const req = onUnhandledRequest.mock.calls[0][0] as MockedRequest;
    const messageTemplate = [
      `[MSW] Error: captured a request without a matching request handler:`,
      `  \u2022 ${req.method} ${req.url.href}`,
      `If you still wish to intercept this unhandled request, please create a request handler for it.`,
    ];

    throw new Error(messageTemplate.join('\n\n'));
  } finally {
    onUnhandledRequest.mockClear();
  }
});

// Clean up after the tests are finished.
afterAll(() => server.close());

failOnConsole({ shouldFailOnWarn: false, shouldFailOnError: false });
