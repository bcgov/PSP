import { FormikProps } from 'formik';
import { createRef } from 'react';

import * as API from '@/constants/API';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import {
  mockAcquisitionFileChecklistResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions } from '@/utils/test-utils';

import { AcquisitionChecklistFormModel } from './models';
import {
  IUpdateAcquisitionChecklistFormProps,
  UpdateAcquisitionChecklistForm,
} from './UpdateAcquisitionChecklistForm';

// mock API service calls
jest.mock('@/hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const sectionTypes = mockLookups.filter(
  c => c.type === API.ACQUISITION_CHECKLIST_SECTION_TYPES && c.isDisabled !== true,
) as ILookupCode[];

let mockViewProps: IUpdateAcquisitionChecklistFormProps = {
  formikRef: null as any,
  initialValues: new AcquisitionChecklistFormModel(),
  onSave: jest.fn(),
  onError: jest.fn(),
  onSuccess: jest.fn(),
};

describe('UpdateAcquisitionChecklist form', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<AcquisitionChecklistFormModel>>();
    const utils = render(
      <UpdateAcquisitionChecklistForm
        formikRef={formikRef}
        initialValues={mockViewProps.initialValues}
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

    mockViewProps.initialValues = AcquisitionChecklistFormModel.fromApi(
      apiAcquisitionFile,
      sectionTypes,
    );
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
