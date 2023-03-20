import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import FormListView, { IFormListViewProps } from './FormListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');
const saveForm = jest.fn();

describe('form list view', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IFormListViewProps>) => {
    // render component under test
    const component = render(<FormListView saveForm={saveForm} />, {
      ...renderOptions,
      store: storeState,
      history: history,
      claims: renderOptions?.claims ?? [Claims.ACQUISITION_VIEW],
    });

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({
      claims: [],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('plus button is disabled by default', async () => {
    const { getByTitle } = setup();

    const plusButton = getByTitle('add form');

    expect(plusButton).toBeDisabled();
  });

  it('it calls saveForm form with appropriate type code value when plus button is clicked', async () => {
    const { getByTitle, container } = setup();

    await fillInput(container, 'formTypeId', 'H120', 'select');
    const plusButton = getByTitle('add form');
    await waitFor(() => {
      expect(plusButton).not.toBeDisabled();
    });
    act(() => userEvent.click(getByTitle('add form')));

    await waitFor(() => {
      expect(saveForm).toHaveBeenCalledWith('H120');
    });
  });
});
