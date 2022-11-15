import { useKeycloak } from '@react-keycloak/web';
import {
  fireEvent,
  render as rtlRender,
  RenderOptions as RtlRenderOptions,
  RenderResult,
} from '@testing-library/react';
import { FilterProvider } from 'components/maps/providers/FIlterProvider';
import { createMemoryHistory, MemoryHistory } from 'history';
import noop from 'lodash/noop';
import React, { ReactNode } from 'react';
import { MapContainer } from 'react-leaflet';
import { Router } from 'react-router-dom';

import TestCommonWrapper from './TestCommonWrapper';

// re-export everything from RTL
export * from '@testing-library/react';
export { default as userEvent } from '@testing-library/user-event';

export const mockKeycloak = (
  props: {
    claims?: string[];
    roles?: string[];
    organizations?: number[];
    authenticated?: boolean;
  } = {},
) => {
  const { claims, roles, organizations, authenticated = true } = props;
  // mock what would be returned by keycloak userinfo endpoint
  const userInfo = {
    organizations: organizations ?? [1],
    roles: claims ?? [],
    groups: roles ?? [],
    email: 'test@test.com',
    name: 'Chester Tester',
  };

  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo,
      subject: 'test',
      authenticated,
      loadUserInfo: jest.fn().mockResolvedValue(userInfo),
    },
  });
};

/**
 * Generates fake text data to aid testing input form validation.
 * @param length The length of the text to generate.
 * @returns A string with as many 'x' as the supplied length
 */
export function fakeText(length = 50): string {
  return 'x'.repeat(length);
}

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
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
  }
  fireEvent.blur(input);

  return { input };
};

window.setImmediate = window.setTimeout as any;
export const flushPromises = () => new Promise(window.setImmediate);

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
        <MapContainer center={[48.43, -123.37]} zoom={14} whenReady={done} ref={whenCreated}>
          {children}
        </MapContainer>
      </div>
    );
  };
}

// override RenderOptions interface
export interface RenderOptions extends RtlRenderOptions {
  store?: any;
  history?: MemoryHistory;
  useMockAuthentication?: boolean;
  organizations?: number[];
  claims?: string[];
  roles?: string[];
}

function render(
  ui: React.ReactElement,
  options: Omit<RenderOptions, 'wrapper'> = {},
): RenderResult {
  const {
    store,
    history,
    useMockAuthentication = false,
    organizations,
    claims,
    roles,
    ...renderOptions
  } = options;

  // mock authentication state prior to rendering. Check first that keycloak has been mocked!
  if (!!useMockAuthentication || !!claims || !!roles || !!organizations) {
    if (typeof (useKeycloak as jest.Mock).mockReturnValue === 'function') {
      mockKeycloak({
        claims: claims ?? [],
        roles: roles ?? [],
        organizations: organizations ?? [1],
        authenticated: true,
      });
    }
  }

  function AllTheProviders({ children }: PropsWithChildren) {
    return (
      <TestCommonWrapper store={store} history={history}>
        <FilterProvider>{children}</FilterProvider>
      </TestCommonWrapper>
    );
  }
  return rtlRender(ui, { wrapper: AllTheProviders, ...renderOptions });
}

async function renderAsync(
  ui: React.ReactElement,
  options: Omit<RenderOptions, 'wrapper'> = {},
): Promise<RenderResult> {
  const {
    store,
    history,
    useMockAuthentication = false,
    organizations,
    claims,
    roles,
    ...renderOptions
  } = options;

  // mock authentication state prior to rendering. Check first that keycloak has been mocked!
  if (!!useMockAuthentication || !!claims || !!roles || !!organizations) {
    if (typeof (useKeycloak as jest.Mock).mockReturnValue === 'function') {
      mockKeycloak({
        claims: claims ?? [],
        roles: roles ?? [],
        organizations: organizations ?? [1],
        authenticated: true,
      });
    }
  }

  function AllTheProviders({ children }: PropsWithChildren) {
    return (
      <TestCommonWrapper store={store} history={history}>
        <FilterProvider>{children}</FilterProvider>
      </TestCommonWrapper>
    );
  }
  return await rtlRender(ui, { wrapper: AllTheProviders, ...renderOptions });
}

// override render method
export { render, renderAsync };
