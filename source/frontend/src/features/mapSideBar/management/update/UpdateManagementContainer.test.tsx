import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  createAxiosError,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import UpdateManagementContainer from './UpdateManagementContainer';
import { IUpdateManagementFormProps } from './UpdateManagementForm';
import { ManagementFormModel } from '../models/ManagementFormModel';

// mock API service calls
vi.mock('@/hooks/repositories/useManagementFileRepository');

type Provider = typeof useManagementFileRepository;
const mockUpdateManagementFile = vi.fn();

vi.mocked(useManagementFileRepository).mockReturnValue({
  putManagementFile: {
    error: undefined,
    response: undefined,
    execute: mockUpdateManagementFile,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateManagementFormProps | undefined;

const TestView: React.FC<IUpdateManagementFormProps> = props => {
  viewProps = props;
  return (
    <Formik<ManagementFormModel>
      enableReinitialize
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.fileName}</>}
    </Formik>
  );
};

describe('UpdateManagement container', () => {
  let managementFile: ApiGen_Concepts_ManagementFile;
  const onSuccess = vi.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<ManagementFormModel>>();
    const utils = render(
      <UpdateManagementContainer
        ref={formikRef}
        managementFile={managementFile}
        onSuccess={onSuccess}
        View={TestView}
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
    viewProps = undefined;
    managementFile = mockManagementFileResponse();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Test Management File/)).toBeVisible();
  });

  it('calls the API to update the disposition file when form is saved', async () => {
    mockUpdateManagementFile.mockResolvedValue(mockManagementFileResponse());
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateManagementFile).toHaveBeenCalledWith(1, fileData, []);
    expect(onSuccess).toHaveBeenCalled();
  });

  it('displays a toast with server-returned error responses', async () => {
    mockUpdateManagementFile.mockRejectedValue(createAxiosError(400, 'Lorem ipsum error'));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Lorem ipsum error')).toBeVisible();
  });

  it('displays a toast for generic server errors', async () => {
    mockUpdateManagementFile.mockRejectedValue(createAxiosError(500));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Failed to update Management File')).toBeVisible();
  });

  it(`triggers the confirm popup when user changes file status to completed`, async () => {
    mockUpdateManagementFile.mockRejectedValue(
      createAxiosError(
        409,
        'You are changing this file to a non-editable state. Only system administrators can edit the file when set to Archived, Cancelled or Completed state). Do you wish to continue?',
        {
          errorCode: UserOverrideCode.DISPOSITION_FILE_FINAL_STATUS,
        },
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(/You are changing this file to a non-editable state/i);
    expect(popup).toBeVisible();
  });

  it(`saves the form when clicking 'Continue Save' in the properties popup`, async () => {
    mockUpdateManagementFile.mockRejectedValue(
      createAxiosError(
        409,
        'You are changing this file to a non-editable state. Only system administrators can edit the file when set to Archived, Cancelled or Completed state). Do you wish to continue?',
        { errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY },
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(/You are changing this file to a non-editable state/i);
    expect(popup).toBeVisible();

    mockUpdateManagementFile.mockResolvedValue(mockManagementFileResponse());

    await act(async () => userEvent.click(await screen.findByText('Yes')));

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateManagementFile).toHaveBeenCalledTimes(2);
    expect(mockUpdateManagementFile).toHaveBeenNthCalledWith(1, 1, fileData, []);
    expect(mockUpdateManagementFile).toHaveBeenNthCalledWith(2, 1, fileData, [
      'PROPERTY_OF_INTEREST_TO_INVENTORY',
    ]);
    expect(onSuccess).toHaveBeenCalled();
  });

  it(`displays custom 400 errors in a modal`, async () => {
    mockUpdateManagementFile.mockRejectedValue(
      createAxiosError(400, 'test error', {
        errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
      }),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(/test error/i);
    expect(popup).toBeVisible();
  });
});
