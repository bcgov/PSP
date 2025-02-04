import { FormikProps } from 'formik';
import { createRef } from 'react';

import UpdateLeaseForm, { IUpdateLeaseFormProps } from './UpdateLeaseForm';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';
import { LeaseFormModel } from '@/features/leases/models';
import { getMockApiLease } from '@/mocks/lease.mock';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();
const initialValues = LeaseFormModel.fromApi(getMockApiLease());

describe('Compensation Requisition UpdateForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IUpdateLeaseFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<LeaseFormModel>>();
    const utils = render(
      <UpdateLeaseForm
        onSubmit={onSubmit}
        formikRef={formikRef}
        initialValues={renderOptions.props?.initialValues ?? initialValues}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    // wait for useEffect
    await waitFor(async () => {});

    return {
      ...utils,
      formikRef,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

});
