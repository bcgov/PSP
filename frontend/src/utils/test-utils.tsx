import { useKeycloak } from '@react-keycloak/web';
import {
  fireEvent,
  render as rtlRender,
  RenderOptions as RtlRenderOptions,
  RenderResult,
} from '@testing-library/react';
import { createMemoryHistory, MemoryHistory } from 'history';
import { IRole } from 'interfaces';
import { IAgency } from 'interfaces/agency';
import { KeycloakInstance } from 'keycloak-js';
import noop from 'lodash/noop';
import React, { ReactNode } from 'react';
import { MapContainer } from 'react-leaflet';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

import TestProviderWrapper from './TestProviderWrapper';
import TestRouterWrapper from './TestRouterWrapper';

export const mockKeycloak = (claims: string[], agencies: number[]) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        agencies: agencies,
        roles: claims,
      },
      subject: 'test',
    },
  });
};

export const fillInput = async (
  container: HTMLElement,
  name: string,
  value: any,
  type: string = 'input',
) => {
  let input: Element | null = null;

  if (type === 'radio') {
    input = container.querySelector(`#input-${name}`);
  } else {
    if (type === 'typeahead' || type === 'datepicker') {
      input = container.querySelector(`input[name="${name}"]`);
    } else {
      input = container.querySelector(`${type}[name="${name}"]`);
    }
  }

  // abort early if no input field found
  if (!input) return { input };

  if (type === 'input') {
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    fireEvent.focusOut(input);
  } else if (type === 'typeahead') {
    fireEvent.focus(input);
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    const select = container.querySelector(`[aria-label="${value}"]`);
    fireEvent.click(select!);
    fireEvent.focusOut(input);
  } else if (type === 'datepicker') {
    fireEvent.mouseDown(input);
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    fireEvent.keyPress(input, { key: 'Enter', code: 'Enter' });
  } else if (type === 'radio') {
    fireEvent.click(input);
    fireEvent.focusOut(input);
  } else {
    fireEvent.change(input!, {
      target: {
        value: value,
      },
    });
    fireEvent.focusOut(input);
  }
  fireEvent.blur(input!);

  return { input };
};

export const flushPromises = () => new Promise(setImmediate);

export const deferred = () => {
  let resolve: (value?: unknown) => void = noop;
  const promise = new Promise(_resolve => {
    resolve = _resolve;
  });
  return {
    resolve,
    promise,
  };
};

/**
 * Utility type for generic props of component testing
 */
export interface PropsWithChildren {
  children?: ReactNode;
}

const defaultHistory = createMemoryHistory({
  getUserConfirmation: (message, callback) => callback(true),
});

/**
 * Creates a in-memory router for unit testing
 * @param history (optional) a memory history to use instead of default
 * @returns The route provider
 */
export function createRouteProvider(history?: MemoryHistory) {
  return function Wrapper({ children }: PropsWithChildren) {
    return <Router history={history ?? defaultHistory}>{children}</Router>;
  };
}

/**
 * Creates a Map wrapper for unit testing
 * @param done A callback that will be called when the map has finished rendering
 * @returns The map container instance
 *
 * @example
 *    const { promise, resolve } = deferred();
 *    render(<TestComponent/>, { wrapper: createMapContainer(resolve) })
 *    await waitFor(() => promise)
 *    // the map is fully initialized here...
 */
export function createMapContainer(
  done: () => void = noop,
  whenCreated: (map: L.Map) => void = noop,
) {
  return function Container({ children }: PropsWithChildren) {
    return (
      <div id="mapid" style={{ width: 500, height: 500 }}>
        <MapContainer
          center={[48.43, -123.37]}
          zoom={14}
          whenReady={done}
          whenCreated={whenCreated}
        >
          {children}
        </MapContainer>
      </div>
    );
  };
}

// re-export everything from RTL
export * from '@testing-library/react';

// override RenderOptions interface
export interface RenderOptions extends RtlRenderOptions {
  store?: any;
  history?: MemoryHistory;
  authenticated?: boolean;
  agencies?: number[];
  roles?: string[];
}

function render(
  ui: React.ReactElement,
  options: Omit<RenderOptions, 'wrapper'> = {},
): RenderResult {
  const { store, history, authenticated = false, agencies, roles, ...renderOptions } = options;
  let auth: {
    initialized: boolean;
    keycloak: Partial<KeycloakInstance>;
  } = {
    initialized: true,
    keycloak: {},
  };

  // mock authentication state prior to rendering
  if (!authenticated) {
    auth.keycloak = { authenticated: false };
  }
  if (!!authenticated || !!roles || !!agencies) {
    auth.keycloak = {
      userInfo: {
        agencies: agencies ?? [1],
        roles: roles ?? [],
        email: 'test@test.com',
        name: 'Chester Tester',
      },
      subject: 'test',
      authenticated: true,
    };
  }
  (useKeycloak as jest.Mock).mockReturnValue(auth);

  // new providers will need to be added here
  function AllTheProviders({ children }: PropsWithChildren) {
    return (
      <TestProviderWrapper store={store}>
        <TestRouterWrapper history={history}>
          <ToastContainer
            autoClose={5000}
            hideProgressBar
            newestOnTop={false}
            closeOnClick={false}
            rtl={false}
            pauseOnFocusLoss={false}
          />
          {children}
        </TestRouterWrapper>
      </TestProviderWrapper>
    );
  }
  return rtlRender(ui, { wrapper: AllTheProviders, ...renderOptions });
}

// override render method
export { render };
