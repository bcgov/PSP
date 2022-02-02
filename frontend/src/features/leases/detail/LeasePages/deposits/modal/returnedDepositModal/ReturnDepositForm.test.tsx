import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { FormLeaseDepositReturn, ILeaseSecurityDeposit } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import ReturnDepositForm, { IReturnDepositFormProps } from './ReturnDepositForm';

const mockDeposit: ILeaseSecurityDeposit = {
  id: 7,
  description: 'Test deposit 1',
  amountPaid: 1234.0,
  depositDate: '2022-02-09',
  depositType: {
    id: 'PET',
    description: 'Pet deposit',
    isDisabled: false,
  },
  rowVersion: 1,
};

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const submitForm = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReturnDepositForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IReturnDepositFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <ReturnDepositForm
          onSave={onSave}
          formikRef={{ current: { submitForm } } as any}
          initialValues={FormLeaseDepositReturn.createEmpty(mockDeposit)}
        />
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: FormLeaseDepositReturn.createEmpty(mockDeposit),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('The return date date is required', async () => {
    const {
      component: { container, findByDisplayValue },
    } = await setup({});

    const { input } = await fillInput(container, 'returnDate', '2020-01-02', 'datepicker');
    await findByDisplayValue('01/02/2020');
    expect(input).toHaveProperty('required');
  });
});
