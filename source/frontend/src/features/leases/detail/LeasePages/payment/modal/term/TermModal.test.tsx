import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { LeaseTermStatusTypes } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import { defaultFormLeaseTerm } from '../../models';
import TermModal, { ITermModalProps } from './TermModal';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const onCancel = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('TermModal component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<ITermModalProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <TermModal onSave={onSave} onCancel={onCancel} displayModal={true} />,
      {
        ...renderOptions,
        history,
        store: storeState,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    onSave.mockReset();
    onCancel.mockReset();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultFormLeaseTerm },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const {
      component: { getByText },
    } = await setup({});

    await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
    await fillInput(document.body, 'expiryDate', '2020-01-02', 'datepicker');
    await fillInput(document.body, 'leasePmyFreqTypeCode.id', 'ANNUAL', 'select');
    await fillInput(document.body, 'paymentAmount', '1,000.00');
    await fillInput(document.body, 'paymentDueDate', 'A due date');
    await fillInput(document.body, 'statusTypeCode.id', 'NEXER', 'select');
    const saveButton = getByText('Save term');
    act(() => userEvent.click(saveButton));
    await waitFor(() => expect(onSave).toHaveBeenCalled());
    expect(onSave).toHaveBeenCalledWith({
      effectiveDateHist: '',
      endDateHist: '',
      expiryDate: '2020-01-02',
      gstAmount: '',
      isGstEligible: false,
      isTermExercised: false,
      leaseId: 0,
      id: null,
      leasePmtFreqTypeCode: {
        description: '',
        id: '',
        isDisabled: false,
      },
      paymentAmount: 1000,
      paymentDueDate: 'A due date',
      paymentNote: '',
      payments: [],
      renewalDate: '',
      startDate: '2020-01-01',
      statusTypeCode: {
        id: LeaseTermStatusTypes.NOT_EXERCISED,
      },
    });
  });

  it('calls onCancel when cancel button clicked', async () => {
    const {
      component: { getByText },
    } = await setup({});
    const cancelButton = getByText('Cancel');
    act(() => userEvent.click(cancelButton));
    expect(onCancel).toHaveBeenCalled();
  });
});
