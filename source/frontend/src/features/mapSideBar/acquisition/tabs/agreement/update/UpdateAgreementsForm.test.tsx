import { FormikProps } from 'formik';
import { createRef } from 'react';

import { AGREEMENT_TYPES } from '@/constants/API';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { AgreementsFormModel } from './models';
import { IUpdateAgreementsFormProps, UpdateAgreementsForm } from './UpdateAgreementsForm';

// mock API service calls
jest.mock('@/hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const agreementTypes: ILookupCode[] = mockLookups.filter(x => x.type === AGREEMENT_TYPES);

let mockViewProps: IUpdateAgreementsFormProps = {
  isLoading: false,
  formikRef: null as any,
  initialValues: new AgreementsFormModel(0),
  agreementTypes: agreementTypes,
  onSave: jest.fn(),
};

describe('UpdateAgreementsForm component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<AgreementsFormModel>>();
    const utils = render(
      <UpdateAgreementsForm
        isLoading={false}
        formikRef={formikRef}
        initialValues={mockViewProps.initialValues}
        agreementTypes={mockViewProps.agreementTypes}
        onSave={mockViewProps.onSave}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      formikRef,
    };
  };

  beforeEach(() => {
    const agreements = mockAgreementsResponse();

    mockViewProps.initialValues = AgreementsFormModel.fromApi(1, agreements);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('saves the form with minimal data', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockAgreementsResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });
    expect(mockViewProps.onSave).toHaveBeenCalled();
  });
});
