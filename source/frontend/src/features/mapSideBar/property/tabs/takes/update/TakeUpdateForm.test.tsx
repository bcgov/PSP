import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { TakeModel } from '../models';
import TakeForm, { ITakesFormProps } from './TakeForm';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { Claims, Roles } from '@/constants';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();

describe('TakeUpdateForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<ITakesFormProps> }) => {
    const utils = render(
      <TakeForm
        {...renderOptions.props}
        onSubmit={onSubmit}
        take={renderOptions.props?.take ?? getMockApiTakes().map(t => new TakeModel(t))[0]}
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

  it('displays a warning if radio button toggled from yes to no', async () => {
    const { getByTestId } = setup({});
    const noButton = getByTestId('radio-isnewhighwaydedication-no');

    await act(async () => userEvent.click(noButton));

    const confirmModal = await screen.findByText('Confirm change');
    expect(confirmModal).toBeVisible();
  });

  it('displays a warning if lease payable radio button toggled from no to yes', async () => {
    const { getByTestId } = setup({});
    const noButton = getByTestId('radio-isleasepayable-no');
    const yesButton = getByTestId('radio-isleasepayable-yes');

    await act(async () => userEvent.click(noButton));

    const confirmModal = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmModal));

    await act(async () => userEvent.click(yesButton));

    const closeModal = await screen.findByText('Close');
    expect(closeModal).toBeVisible();
  });

  it('resets is new isNewHighwayDedication values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-isnewhighwaydedication-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('4046.8564')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('4046.8564')).toBeNull();
  });

  it('resets isNewInterestInSrw values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-isnewinterestinsrw-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('8093.713')).not.toBeNull();
    expect(queryByDisplayValue('Nov 20, 2022')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('8093.713')).toBeNull();
    expect(queryByDisplayValue('Nov 20, 2022')).toBeNull();
  });

  it('resets isNewlandAct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-isnewlandact-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('12140.569')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('12140.569')).toBeNull();
  });

  it('hides landActEndDt value if radio button toggled from yes to no', async () => {
    const { queryByTestId } = setup({});
    await act(async () =>
      userEvent.selectOptions(
        getByName('landActTypeCode'),
        screen.getByTestId('select-option-Crown Grant'),
      ),
    );

    expect(queryByTestId('landActEndDt', { exact: false })).toBeNull();
  });

  it('resets isNewLicenseToConstruct values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-isnewlicensetoconstruct-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('16187.426')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('16187.426')).toBeNull();
  });

  it('resets isThereSurplus values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-istheresurplus-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('20234.281')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('20234.281')).toBeNull();
  });

  it('hides the delete button when the take has been completed', () => {
    let completeTake = getMockApiTakes()[0];
    const takeModel = new TakeModel(completeTake);
    takeModel.takeStatusTypeCode = ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE;

    const { queryByTitle, getByTestId } = setup({
      props: {
        take: takeModel,
      },
    });

    const deleteButton = queryByTitle('delete take');
    expect(deleteButton).toBeNull();

    const noButton = getByTestId('radio-istheresurplus-no');
    expect(noButton).toBeDisabled();
  });

  it('resets isLeasePayable values if radio button toggled from yes to no', async () => {
    const { getByTestId, queryByDisplayValue } = setup({});
    const noButton = getByTestId('radio-isleasepayable-no');
    await act(async () => userEvent.click(noButton));

    expect(queryByDisplayValue('20231.281')).not.toBeNull();
    const confirmButton = await screen.findByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(queryByDisplayValue('20231.281')).toBeNull();
  });
});
