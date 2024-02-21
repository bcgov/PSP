import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { DispositionFormModel } from '@/features/mapSideBar/disposition/models/DispositionFormModel';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
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

import UpdateDispositionContainer from './UpdateDispositionContainer';
import { IUpdateDispositionFormProps } from './UpdateDispositionForm';

// mock API service calls
jest.mock('@/hooks/repositories/useDispositionProvider');

type Provider = typeof useDispositionProvider;
const mockUpdateDispositionFile = jest.fn();

(useDispositionProvider as jest.MockedFunction<Provider>).mockReturnValue({
  putDispositionFile: {
    error: undefined,
    response: undefined,
    execute: mockUpdateDispositionFile,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateDispositionFormProps | undefined;

const TestView: React.FC<IUpdateDispositionFormProps> = props => {
  viewProps = props;
  return (
    <Formik<DispositionFormModel>
      enableReinitialize
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.fileName}</>}
    </Formik>
  );
};

describe('UpdateDisposition container', () => {
  let dispositionFile: ApiGen_Concepts_DispositionFile;
  const onSuccess = jest.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<DispositionFormModel>>();
    const utils = render(
      <UpdateDispositionContainer
        ref={formikRef}
        dispositionFile={dispositionFile}
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
    dispositionFile = mockDispositionFileResponse();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Test Disposition File/)).toBeVisible();
  });

  it('calls the API to update the disposition file when form is saved', async () => {
    mockUpdateDispositionFile.mockResolvedValue(mockDispositionFileResponse());
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateDispositionFile).toHaveBeenCalledWith(1, fileData, []);
    expect(onSuccess).toHaveBeenCalled();
  });

  it('displays a toast with server-returned error responses', async () => {
    mockUpdateDispositionFile.mockRejectedValue(createAxiosError(400, 'Lorem ipsum error'));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Lorem ipsum error')).toBeVisible();
  });

  it('displays a toast for generic server errors', async () => {
    mockUpdateDispositionFile.mockRejectedValue(createAxiosError(500));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Failed to update Disposition File')).toBeVisible();
  });

  it(`triggers the confirm popup when region doesn't match`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
      createAxiosError(409, 'The Ministry region has been changed', {
        errorCode: UserOverrideCode.UPDATE_REGION,
      }),
    );
    const { formikRef, findByText } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await findByText(/The Ministry region has been changed/i);
    expect(popup).toBeVisible();
  });

  it(`saves the form when clicking 'Continue Save' in the region popup`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
      createAxiosError(409, 'The Ministry region has been changed', {
        errorCode: UserOverrideCode.UPDATE_REGION,
      }),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(/The Ministry region has been changed/i);
    expect(popup).toBeVisible();

    mockUpdateDispositionFile.mockResolvedValue(mockDispositionFileResponse());

    await act(async () => userEvent.click(await screen.findByText('Yes')));

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateDispositionFile).toHaveBeenCalledTimes(2);
    expect(mockUpdateDispositionFile).toHaveBeenNthCalledWith(1, 1, fileData, []);
    expect(mockUpdateDispositionFile).toHaveBeenNthCalledWith(2, 1, fileData, [
      UserOverrideCode.UPDATE_REGION,
    ]);
    expect(onSuccess).toHaveBeenCalled();
  });

  it(`dismisses the region popup when clicking 'No'`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
      createAxiosError(409, 'The Ministry region has been changed', {
        errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
      }),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText(/The Ministry region has been changed/i)).toBeVisible();

    await act(async () => userEvent.click(await screen.findByText('No')));

    await waitFor(() =>
      expect(screen.queryByText(/The Ministry region has been changed/i)).toBeNull(),
    );
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it(`triggers the confirm popup when user changes file status to completed`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
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
    mockUpdateDispositionFile.mockRejectedValue(
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

    mockUpdateDispositionFile.mockResolvedValue(mockDispositionFileResponse());

    await act(async () => userEvent.click(await screen.findByText('Yes')));

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateDispositionFile).toHaveBeenCalledTimes(2);
    expect(mockUpdateDispositionFile).toHaveBeenNthCalledWith(1, 1, fileData, []);
    expect(mockUpdateDispositionFile).toHaveBeenNthCalledWith(2, 1, fileData, [
      'PROPERTY_OF_INTEREST_TO_INVENTORY',
    ]);
    expect(onSuccess).toHaveBeenCalled();
  });

  it(`displays Contactor cannot remove itself from Team`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
      createAxiosError(
        400,
        'test error',
        {
          errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
        },
        'ContractorNotInTeamException',
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(
      /Contractors cannot remove themselves from a Disposition file./i,
    );
    expect(popup).toBeVisible();
  });

  it(`displays custom 400 errors in a modal`, async () => {
    mockUpdateDispositionFile.mockRejectedValue(
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
