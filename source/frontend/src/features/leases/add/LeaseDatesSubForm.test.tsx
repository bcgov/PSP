import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { defaultFormLease } from '../models';
import { LeaseSchema } from './AddLeaseYupSchema';
import LeaseDatesSubForm, { ILeaseDatesSubFormProps } from './LeaseDatesSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('LeaseDatesSubForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<ILeaseDatesSubFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={defaultFormLease} validationSchema={LeaseSchema}>
        {formikProps => <LeaseDatesSubForm formikProps={formikProps} />}
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
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('expiry date must be later then start date', async () => {
    const {
      component: { findByText, container },
    } = await setup({});
    await fillInput(container, 'startDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/01/2020', 'datepicker');
    expect(await findByText('Expiry Date must be after Start Date')).toBeVisible();
  });
});
