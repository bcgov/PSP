// jest-dom adds custom jest matchers for asserting on DOM nodes.
// allows you to do things like:
// expect(element).toHaveTextContent(/react/i)
// learn more: https://github.com/testing-library/jest-dom
import '@testing-library/jest-dom/extend-expect';
import 'jest-styled-components';

// Unofficial adapter for React 17 for Enzyme.
// See https://github.com/enzymejs/enzyme/issues/2429 for details
import Adapter from '@wojtekmaj/enzyme-adapter-react-17';
import { configure } from 'enzyme';

configure({ adapter: new Adapter() });

var localStorageMock = (function() {
  var store: any = {};

  return {
    getKeys: function() {
      return store;
    },
    getItem: function(key: string) {
      return store[key] || null;
    },
    setItem: function(key: string, value: any) {
      store[key] = value.toString();
    },
    removeItem: function(key: string) {
      store[key] = undefined;
    },
    clear: function() {
      store = {};
    },
  };
})();
Object.defineProperty(window, 'localStorage', {
  value: localStorageMock,
});

//workaround to allow polyline and other svg map renderers to function correctly in tests.
var createElementNSOrig = (global as any).document.createElementNS;
(global as any).document.createElementNS = function(namespaceURI: any, qualifiedName: any) {
  if (namespaceURI === 'http://www.w3.org/2000/svg' && qualifiedName === 'svg') {
    var element = createElementNSOrig.apply(this, arguments);
    element.createSVGRect = function() {};
    return element;
  }
  return createElementNSOrig.apply(this, arguments);
};

window.scrollTo = jest.fn(); // not implemented by jsdom.

jest.setTimeout(10000);
