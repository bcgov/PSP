import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { FormLeaseDeposit } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import ReceivedDepositForm, { IReceivedDepositFormProps } from './ReceivedDepositForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const submitForm = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReceivedDepositForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IReceivedDepositFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <ReceivedDepositForm
          onSave={onSave}
          formikRef={{ current: { submitForm } } as any}
          initialValues={FormLeaseDeposit.createEmpty()}
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
      initialValues: FormLeaseDeposit.createEmpty(),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('The deposit date is required', async () => {
    const {
      component: { container, findByDisplayValue },
    } = await setup({});

    const { input } = await fillInput(container, 'depositDate', '2020-01-02', 'datepicker');
    await findByDisplayValue('01/02/2020');
    expect(input).toHaveProperty('required');
  });
});
