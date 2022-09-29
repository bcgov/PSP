import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLeaseTerm } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { ITermFormProps, TermForm } from './TermForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const submitForm = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('TermForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<ITermFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <TermForm onSave={onSave} formikRef={{ current: { submitForm } } as any} />
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
      initialValues: { ...defaultFormLeaseTerm },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });
  it('The end date must be after the start date', async () => {
    const {
      component: { container, findByText },
    } = await setup({});

    await fillInput(container, 'startDate', '2020-01-02', 'datepicker');
    await fillInput(container, 'expiryDate', '2020-01-01', 'datepicker');
    const error = await findByText('Expiry Date must be after Start Date');
    expect(error).toBeVisible();
  });

  it('The start date is required', async () => {
    const {
      component: { container, findByDisplayValue },
    } = await setup({});

    const { input } = await fillInput(container, 'expiryDate', '2020-01-02', 'datepicker');
    await findByDisplayValue('01/02/2020');
    expect(input).toHaveProperty('required');
  });

  it('The default term status is NEXER', async () => {
    const {
      component: { findByDisplayValue },
    } = await setup({});

    const termStatus = await findByDisplayValue('Not Exercised');
    expect(termStatus).toBeVisible();
  });
});
