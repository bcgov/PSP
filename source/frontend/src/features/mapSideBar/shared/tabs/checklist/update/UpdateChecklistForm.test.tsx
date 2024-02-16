import { FormikProps } from 'formik';
import { createRef } from 'react';

import * as API from '@/constants/API';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockFileChecklistResponse, mockLookups } from '@/mocks/index.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions } from '@/utils/test-utils';

import { ChecklistFormModel } from './models';
import { IUpdateChecklistFormProps, UpdateChecklistForm } from './UpdateChecklistForm';

// mock API service calls
jest.mock('@/hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const sectionTypes = mockLookups.filter(
  c => c.type === API.ACQUISITION_CHECKLIST_SECTION_TYPES && c.isDisabled !== true,
) as ILookupCode[];

const mockViewProps: IUpdateChecklistFormProps = {
  formikRef: null as any,
  initialValues: new ChecklistFormModel(),
  onSave: jest.fn(),
  onError: jest.fn(),
  onSuccess: jest.fn(),
  sectionTypeName: API.ACQUISITION_CHECKLIST_SECTION_TYPES,
  statusTypeName: API.ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES,
  prefix: 'acq',
};

describe('UpdateChecklist form', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<ChecklistFormModel>>();
    const utils = render(
      <UpdateChecklistForm
        formikRef={formikRef}
        initialValues={mockViewProps.initialValues}
        onSave={mockViewProps.onSave}
        onSuccess={mockViewProps.onSuccess}
        onError={mockViewProps.onError}
        sectionTypeName={mockViewProps.sectionTypeName}
        statusTypeName={mockViewProps.statusTypeName}
        prefix={mockViewProps.prefix}
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
    const apiDispositionFile = mockDispositionFileResponse();
    apiDispositionFile.fileChecklistItems = mockFileChecklistResponse();

    mockViewProps.initialValues = ChecklistFormModel.fromApi(apiDispositionFile, sectionTypes);
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
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockDispositionFileResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });
    expect(mockViewProps.onSave).toHaveBeenCalled();
  });

  it('saves the form with updated values', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockDispositionFileResponse());

    await act(async () => {
      formikRef.current?.submitForm();
    });
    expect(mockViewProps.onSave).toHaveBeenCalled();
  });

  it('calls onSuccess when the disposition checklist is saved successfully', async () => {
    const { formikRef } = setup();
    (mockViewProps.onSave as jest.Mock).mockResolvedValue(mockDispositionFileResponse());

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
