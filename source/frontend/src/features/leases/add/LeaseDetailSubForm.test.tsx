import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { getDefaultFormLease } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import LeaseDetailSubForm, { ILeaseDetailsSubFormProps } from './LeaseDetailSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('LeaseDetailSubForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<ILeaseDetailsSubFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <Formik
        onSubmit={noop}
        initialValues={getDefaultFormLease()}
        validationSchema={AddLeaseYupSchema}
      >
        {formikProps => <LeaseDetailSubForm formikProps={formikProps} />}
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
