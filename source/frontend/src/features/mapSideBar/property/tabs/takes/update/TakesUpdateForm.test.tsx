import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { TakeModel } from './models';
import TakesUpdateForm, { ITakesUpdateFormProps } from './TakesUpdateForm';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { Claims, Roles } from '@/constants';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();

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
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
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

    const takeTwo = getByText('New Take');
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

    const continueButton = await screen.findAllByText('Yes');
    await act(async () => userEvent.click(continueButton[continueButton.length - 1]));

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
    const noButton = getByTestId('radio-takes.0.isnewhighwaydedication-no');

    await act(async () => userEvent.click(noButton));

    const confirmModal = await screen.findByText('Confirm change');
    expect(confirmModal).toBeVisible();
  });

  it('displays a warning if lease payable radio button toggled from no to yes', async () => {
    const { getByTestId } = setup({});
    const noButton = getByTestId('radio-takes.0.isleasepayable-no');
    const yesButton = getByTestId('radio-takes.0.isleasepayable-yes');

    await act(async () => userEvent.click(noButton));

    const confirmModal = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmModal));

    await act(async () => userEvent.click(yesButton));

    const closeModal = await screen.findByText('Close');
    expect(closeModal).toBeVisible();
  });

  it('resets is new isNewHighwayDedication values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewhighwaydedication-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('4046.86')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('4046.86')).toBeNull();
  });

  it('resets isNewInterestInSrw values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewinterestinsrw-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('8093.71')).not.toBeNull();
    expect(queryByDisplayValue('Nov 20, 2022')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('8093.71')).toBeNull();
    expect(queryByDisplayValue('Nov 20, 2022')).toBeNull();
  });

  it('resets isNewlandAct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewlandact-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('12140.57')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('12140.57')).toBeNull();
  });

  it('hides landActEndDt value if radio button toggled from yes to no', async () => {
    const { queryByTestId } = setup({});
    await act(async () =>
      userEvent.selectOptions(
        getByName('takes.0.landActTypeCode'),
        screen.getByTestId('select-option-Crown Grant'),
      ),
    );

    expect(queryByTestId('takes.0.landActEndDt', { exact: false })).toBeNull();
  });

  it('resets isNewLicenseToConstruct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isnewlicensetoconstruct-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('16187.43')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('16187.43')).toBeNull();
  });

  it('resets isThereSurplus values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.istheresurplus-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('20234.28')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('20234.28')).toBeNull();
  });

  it('hides the delete button when the take has been completed', () => {
    let completeTake = getMockApiTakes()[0];
    const takeModel = new TakeModel(completeTake);
    takeModel.takeStatusTypeCode = ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE;

    const { queryByTitle, getByTestId } = setup({
      props: {
        takes: [takeModel],
      },
    });

    const deleteButton = queryByTitle('delete take');
    expect(deleteButton).toBeNull();

    const noButton = getByTestId('radio-takes.0.istheresurplus-no');
    expect(noButton).toBeDisabled();
  });

  it('resets isLeasePayable values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-takes.0.isleasepayable-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('20231.28')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('20231.28')).toBeNull();
  });

  it('shows the edit button when the take has been completed for Admin users', () => {
    let completeTake = getMockApiTakes()[0];
    const takeModel = new TakeModel(completeTake);
    takeModel.takeStatusTypeCode = ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE;

    const { queryByTitle } = setup({
      props: {
        takes: [takeModel],
      },
      claims: [Claims.ACQUISITION_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });

    const deleteButton = queryByTitle('delete take');
    expect(deleteButton).toBeInTheDocument();
  });
});
