import { FormikProps } from 'formik';
import { createRef } from 'react';
import { server } from '@/mocks/msw/server';
import { http, HttpResponse } from 'msw';

import UpdateLeaseForm, { IUpdateLeaseFormProps } from './UpdateLeaseForm';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor, screen, waitForEffects } from '@/utils/test-utils';
import { LeaseFormModel } from '@/features/leases/models';
import { getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import Roles from '@/constants/roles';
import { getUserMock } from '@/mocks/user.mock';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();
const initialValues = LeaseFormModel.fromApi(getMockApiLease());

describe('UpdateLeaseForm component', () => {
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
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        roles: renderOptions?.roles ?? [Roles.SYSTEM_ADMINISTRATOR],
      },
    );

    // wait for useEffect
    await waitFor(async () => {});

    return {
      ...utils,
      getFormikRef: () => formikRef,
      getPrimaryArbitrationCity: () => {
        return utils.container.querySelector(
          `input[name="primaryArbitrationCity"]`,
        ) as HTMLInputElement;
      },
    };
  };

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock(), { status: 200 })),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    await waitForEffects();

    expect(asFragment()).toMatchSnapshot();
  });
});
