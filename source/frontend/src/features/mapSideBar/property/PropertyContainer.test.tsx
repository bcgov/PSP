import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { cleanup, render, RenderOptions } from '@/utils/test-utils';

import PropertyContainer, { IPropertyContainerProps } from './PropertyContainer';

// mock keycloak auth library

describe('PropertyContainer component', () => {
  const history = createMemoryHistory();
  const storeState = {
    [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  };

  const setup = (renderOptions: RenderOptions & IPropertyContainerProps) => {
    // render component under test
    const utils = render(<PropertyContainer {...renderOptions} />, {
      ...renderOptions,
      history,
      store: { ...renderOptions.store, ...storeState },
      claims: renderOptions.claims ?? [],
    });

    return { ...utils };
  };

  afterEach(() => {
    vi.restoreAllMocks();
    cleanup();
  });

  it('hides the management tab if the user does not have permission', async () => {
    const { queryByText } = setup({
      claims: [],
      composedPropertyState: { apiWrapper: { response: {} }, id: 1 } as any,
    });

    expect(queryByText('Management')).toBeNull();
  });

  it('displays the management tab if the user has permission', async () => {
    const { getByText } = setup({
      claims: [Claims.MANAGEMENT_VIEW],
      composedPropertyState: { apiWrapper: { response: {} }, id: 1 } as any,
    });

    expect(getByText('Management')).toBeVisible();
  });
});
