import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { getMockDeposits } from '@/mocks/deposits.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from '@/utils/test-utils';

import { FormLeaseDepositReturn } from '../../models/FormLeaseDepositReturn';
import ReturnDepositForm, { IReturnDepositFormProps } from './ReturnDepositForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = vi.fn();
const submitForm = vi.fn();
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
          initialValues={FormLeaseDepositReturn.createEmpty(getMockDeposits()[0])}
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
      initialValues: FormLeaseDepositReturn.createEmpty(getMockDeposits()[0]),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('The return date date is required', async () => {
    const {
      component: { container, findByDisplayValue },
    } = await setup({});

    await act(async () => {
      await fillInput(container, 'returnDate', '2020-01-02', 'datepicker');
    });
    const input = await findByDisplayValue('Jan 02, 2020');
    expect(input).toHaveProperty('required');
  });
});
