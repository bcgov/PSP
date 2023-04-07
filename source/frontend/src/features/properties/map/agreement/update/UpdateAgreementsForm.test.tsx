import * as API from 'constants/API';
import { FormikProps } from 'formik';
import { useApiUsers } from 'hooks/pims-api/useApiUsers';
import { mockLookups } from 'mocks';
import {
  mockAcquisitionFileChecklistResponse,
  mockAcquisitionFileResponse,
} from 'mocks/mockAcquisitionFiles';
import { Api_Agreement } from 'models/api/Agreement';
import { createRef } from 'react';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions } from 'utils/test-utils';

import { AgreementsFormModel } from './models';
import { IUpdateAgreementsFormProps, UpdateAgreementsForm } from './UpdateAgreementsForm';

// mock API service calls
jest.mock('hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const sectionTypes: Api_Agreement[] = [];

let mockViewProps: IUpdateAgreementsFormProps = {
  isLoading: false,
  formikRef: null as any,
  initialValues: new AgreementsFormModel(1),
  agreementTypes: [],
  onSave: jest.fn(),
  onError: jest.fn(),
  onSuccess: jest.fn(),
};

describe('UpdateAcquisitionChecklist form', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<AgreementsFormModel>>();
    const utils = render(
      <UpdateAgreementsForm
        isLoading={false}
        formikRef={formikRef}
        initialValues={mockViewProps.initialValues}
        agreementTypes={[]}
        onSave={mockViewProps.onSave}
        onSuccess={mockViewProps.onSuccess}
        onError={mockViewProps.onError}
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
    const apiAcquisitionFile = mockAcquisitionFileResponse();
    apiAcquisitionFile.acquisitionFileChecklist = mockAcquisitionFileChecklistResponse();

    mockViewProps.initialValues = AgreementsFormModel.fromApi(1, sectionTypes);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders last updated by and last updated on for the overall checklist', () => {
    const { getByText } = setup();
    expect(getByText(/This checklist was last updated Mar 17, 2023 by/i)).toBeVisible();
  });

  it('saves the form with minimal data', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockAcquisitionFileResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });
    expect(mockViewProps.onSave).toHaveBeenCalled();
  });

  it('saves the form with updated values', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockAcquisitionFileResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });
    expect(mockViewProps.onSave).toHaveBeenCalled();
  });

  it('calls onSuccess when the acquisition checklist is saved successfully', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockAcquisitionFileResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(mockViewProps.onSave).toHaveBeenCalled();
    expect(mockViewProps.onSuccess).toHaveBeenCalled();
  });

  it('calls onError when it cannot save the form', async () => {
    const { formikRef } = setup();
    const error500 = createAxiosError(500);
    (mockViewProps.onSave as jest.Mock).mockRejectedValue(error500);

    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(mockViewProps.onSave).toHaveBeenCalled();
    expect(mockViewProps.onError).toHaveBeenCalled();
    expect(mockViewProps.onSuccess).not.toHaveBeenCalled();
  });
});
