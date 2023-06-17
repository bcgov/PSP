import { screen } from '@testing-library/react';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { ResearchFileNameGuide } from './ResearchFileNameGuide';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ResearchFileNameGuide', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<ResearchFileNameGuide />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...component,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should have the Help with choosing a name text in the component', async () => {
    setup({});
    expect(screen.getByText(`Help with choosing a name`)).toBeInTheDocument();
  });

  it('should have the Ministry project name text in the component', async () => {
    setup({});
    expect(screen.getByText(`Ministry project name`)).toBeInTheDocument();
  });
});
