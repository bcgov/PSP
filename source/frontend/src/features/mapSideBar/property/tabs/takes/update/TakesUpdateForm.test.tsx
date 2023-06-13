import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { TakeModel } from './models';
import TakesUpdateForm, { ITakesUpdateFormProps } from './TakesUpdateForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = jest.fn();

describe('TakesUpdateForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<ITakesUpdateFormProps> }) => {
    const utils = render(
      <TakesUpdateForm
        {...renderOptions.props}
        onSubmit={onSubmit}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        takes={renderOptions.props?.takes ?? getMockApiTakes().map(t => new TakeModel(t))}
        loading={renderOptions.props?.loading ?? false}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays a title using name from the property name', () => {
    const { getByText } = setup({});
    const header = getByText(/007-723-385/);
    expect(header).toBeVisible();
  });

  it('adds a take if the create a take button is clicked', async () => {
    const { getByText } = setup({});
    const createButton = getByText('Create a Take');
    await act(async () => userEvent.click(createButton));

    const takeTwo = getByText('Take 2');
    expect(takeTwo).toBeVisible();
  });

  it('displays modal when delete take button clicked', async () => {
    const { getAllByTitle } = setup({});
    const deleteButton = getAllByTitle('delete take')[0];
    await act(async () => userEvent.click(deleteButton));

    const confirmModal = await screen.findByText('Confirm Delete');
    expect(confirmModal).toBeVisible();
  });

  it('removes take when delete is clicked and modal confirmed', async () => {
    const { getAllByTitle, queryByText } = setup({});
    const deleteButton = getAllByTitle('delete take')[0];
    await act(async () => userEvent.click(deleteButton));

    const continueButton = await screen.findByText('Continue');
    await act(async () => userEvent.click(continueButton));

    expect(queryByText('Take 1')).toBeNull();
  });

  it('does not remove take when delete is clicked and modal cancelled', async () => {
    const { getAllByTitle, queryByText } = setup({});
    const deleteButton = getAllByTitle('delete take')[0];
    await act(async () => userEvent.click(deleteButton));

    const cancelButton = await screen.findByTitle('cancel-modal');
    await act(async () => userEvent.click(cancelButton));

    expect(queryByText('Take 1')).toBeVisible();
  });

  it('displays a warning if radio button toggled from yes to no', async () => {
    const { getByTestId } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewrightofway-no');

    await act(async () => userEvent.click(noButton));

    const confirmModal = await screen.findByText('Confirm change');
    expect(confirmModal).toBeVisible();
  });

  it('resets is new rightofway values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewrightofway-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('4046.86')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('4046.86')).toBeNull();
  });

  it('resets isstatutoryrightofway values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isstatutoryrightofway-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('8093.71')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('8093.71')).toBeNull();
  });

  it('resets islandAct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.islandact-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('12140.57')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('12140.57')).toBeNull();
  });

  it('resets isLicenseToConstruct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.islicensetoconstruct-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('16187.43')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('16187.43')).toBeNull();
  });

  it('resets isSurplus values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.issurplus-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('20234.28')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('20234.28')).toBeNull();
  });
});
