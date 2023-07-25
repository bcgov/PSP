import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { getMockApiFileForms } from '@/mocks/form.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import FormListView, { IFormListViewProps } from './FormListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');
const saveForm = jest.fn();
const setFormFilter = jest.fn();
const setSort = jest.fn();
const onDelete = jest.fn();

describe('form list view', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IFormListViewProps>) => {
    // render component under test
    const component = render(
      <FormListView
        saveForm={saveForm}
        setFormFilter={setFormFilter}
        setSort={setSort}
        sort={{}}
        forms={renderOptions?.forms ?? []}
        onDelete={onDelete}
      />,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [Claims.FORM_ADD, Claims.FORM_VIEW],
      },
    );

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

  it('plus button is not visible if user does not have add claim', async () => {
    const { queryByTitle } = setup({ claims: [] });

    const plusButton = queryByTitle('add form');

    expect(plusButton).toBeNull();
  });

  it('it calls saveForm form with appropriate type code value when plus button is clicked', async () => {
    const { getByTitle, container } = setup();

    await fillInput(container, 'formTypeCode', 'H120', 'select');
    const plusButton = getByTitle('add form');
    await waitFor(() => {
      expect(plusButton).not.toBeDisabled();
    });
    act(() => userEvent.click(getByTitle('add form')));

    await waitFor(() => {
      expect(saveForm).toHaveBeenCalledWith('H120');
    });
  });

  it('displays the expected message if no forms are available', async () => {
    const { getByText } = setup();

    await waitFor(() => {
      expect(getByText('No matching Form(s) found')).toBeVisible();
    });
  });

  it('displays the expected results when results are provided', async () => {
    const { getAllByText } = setup({
      forms: getMockApiFileForms(),
    });

    await waitFor(() => {
      expect(getAllByText('Offer agreement - Total (H179 T)')[1]).toBeVisible();
      expect(getAllByText('Offer agreement - Section 3 (H179 A)')[1]).toBeVisible();
      expect(getAllByText('Payment requisition (H120)')[1]).toBeVisible();
    });
  });

  it('can click the view action on a given row', async () => {
    const { findAllByTitle } = setup({
      forms: getMockApiFileForms(),
      claims: [Claims.FORM_VIEW],
    });

    const viewButton = (await findAllByTitle('View Template Form'))[0];
    act(() => userEvent.click(viewButton));
    await waitFor(() => {
      expect(history.location.pathname).toBe('//popup/form/2');
    });
  });

  it('view action hidden if view claim missing', async () => {
    const { queryByTitle } = setup({
      forms: getMockApiFileForms(),
      claims: [],
    });

    const viewButton = queryByTitle('View Template Form');
    await waitFor(() => {
      expect(viewButton).toBeNull();
    });
  });

  it('can click the delete action on a given row', async () => {
    const { findAllByTitle } = setup({
      forms: getMockApiFileForms(),
      claims: [Claims.FORM_DELETE],
    });

    const deleteButton = (await findAllByTitle('Delete Template Form'))[0];
    act(() => userEvent.click(deleteButton));
    await waitFor(() => {
      expect(onDelete).toHaveBeenCalledWith(2);
    });
  });

  it('delete action hidden if delete claim missing', async () => {
    const { queryByTitle } = setup({
      forms: getMockApiFileForms(),
      claims: [],
    });

    const deleteButton = queryByTitle('Delete Template Form');
    await waitFor(() => {
      expect(deleteButton).toBeNull();
    });
  });
});
