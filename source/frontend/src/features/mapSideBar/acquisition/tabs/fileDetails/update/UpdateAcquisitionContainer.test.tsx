import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { mockAcquisitionFileResponse, mockLookups } from '@/mocks/index.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
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

import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionContainer } from './UpdateAcquisitionContainer';
import { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

// mock API service calls
vi.mock('@/hooks/repositories/useAcquisitionProvider');

type Provider = typeof useAcquisitionProvider;
const mockUpdateAcquisitionFile = vi.fn();

vi.mocked(useAcquisitionProvider).mockReturnValue({
  updateAcquisitionFile: {
    error: undefined,
    response: undefined,
    execute: mockUpdateAcquisitionFile,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateAcquisitionFormProps | undefined;

const TestView: React.FC<IUpdateAcquisitionFormProps> = props => {
  viewProps = props;
  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.fileName}</>}
    </Formik>
  );
};

describe('UpdateAcquisition container', () => {
  let acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  const onSuccess = vi.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<UpdateAcquisitionSummaryFormModel>>();
    const utils = render(
      <UpdateAcquisitionContainer
        ref={formikRef}
        acquisitionFile={acquisitionFile}
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
    acquisitionFile = mockAcquisitionFileResponse();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered - Test ACQ File/)).toBeVisible();
  });

  it('calls the API to update the acquisition file when form is saved', async () => {
    mockUpdateAcquisitionFile.mockResolvedValue(mockAcquisitionFileResponse());
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateAcquisitionFile).toHaveBeenCalledWith(fileData, []);
    expect(onSuccess).toHaveBeenCalled();
  });

  it('displays a toast with server-returned error responses', async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(createAxiosError(400, 'Lorem ipsum error'));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Lorem ipsum error')).toBeVisible();
  });

  it('displays a toast for generic server errors', async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(createAxiosError(500));
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(await screen.findByText('Failed to update Acquisition File')).toBeVisible();
  });

  it(`triggers the confirm popup when region doesn't match`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
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

  it(`triggers the modal for contractor self-removal`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(
        409,
        'Contractors cannot remove themselves from a file. Please contact the admin at pims@gov.bc.ca',
        {
          errorCode: UserOverrideCode.CONTRACTOR_SELFREMOVED,
        },
      ),
    );
    const { formikRef, findByText } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await findByText(
      /Contractors cannot remove themselves from a file. Please contact the admin/i,
    );
    expect(popup).toBeVisible();
  });

  it(`triggers popup when deleting a selected payee`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(409, 'Acquisition File Owner Reperesentative can not be removed', {
        errorCode: null,
      }),
    );
    const { formikRef, findByText } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await findByText(/Acquisition File Owner Reperesentative can not be removed/i);
    expect(popup).toBeVisible();
  });

  it(`saves the form when clicking 'Continue Save' in the region popup`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(409, 'The Ministry region has been changed', {
        errorCode: UserOverrideCode.UPDATE_REGION,
      }),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(/The Ministry region has been changed/i);
    expect(popup).toBeVisible();

    mockUpdateAcquisitionFile.mockResolvedValue(mockAcquisitionFileResponse());

    await act(async () => userEvent.click(await screen.findByText('Yes')));

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateAcquisitionFile).toHaveBeenCalledTimes(2);
    expect(mockUpdateAcquisitionFile).toHaveBeenNthCalledWith(1, fileData, []);
    expect(mockUpdateAcquisitionFile).toHaveBeenNthCalledWith(2, fileData, [
      UserOverrideCode.UPDATE_REGION,
    ]);
    expect(onSuccess).toHaveBeenCalled();
  });

  it(`dismisses the region popup when clicking 'No'`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
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
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(
        409,
        'The properties of interest will be added to the inventory as acquired properties',
        {
          errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
        },
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(
      /The properties of interest will be added to the inventory as acquired properties/i,
    );
    expect(popup).toBeVisible();
  });

  it(`saves the form when clicking 'Continue Save' in the properties popup`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(
        409,
        'The properties of interest will be added to the inventory as acquired properties',
        { errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY },
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const popup = await screen.findByText(
      /The properties of interest will be added to the inventory as acquired properties/i,
    );
    expect(popup).toBeVisible();

    mockUpdateAcquisitionFile.mockResolvedValue(mockAcquisitionFileResponse());

    await act(async () => userEvent.click(await screen.findByText('Yes')));

    const fileData = viewProps?.initialValues.toApi();
    expect(mockUpdateAcquisitionFile).toHaveBeenCalledTimes(2);
    expect(mockUpdateAcquisitionFile).toHaveBeenNthCalledWith(1, fileData, []);
    expect(mockUpdateAcquisitionFile).toHaveBeenNthCalledWith(2, fileData, [
      'PROPERTY_OF_INTEREST_TO_INVENTORY',
    ]);
    expect(onSuccess).toHaveBeenCalled();
  });

  it(`displays custom 400 errors in a modal`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
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

  it(`dismisses the properties popup when clicking 'No'`, async () => {
    mockUpdateAcquisitionFile.mockRejectedValue(
      createAxiosError(
        409,
        'The properties of interest will be added to the inventory as acquired properties',
        {
          errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
        },
      ),
    );
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(
      await screen.findByText(
        /The properties of interest will be added to the inventory as acquired properties/i,
      ),
    ).toBeVisible();

    await act(async () => userEvent.click(await screen.findByText('No')));

    await waitFor(() =>
      expect(
        screen.queryByText(
          /The properties of interest will be added to the inventory as acquired properties/i,
        ),
      ).toBeNull(),
    );
    expect(onSuccess).not.toHaveBeenCalled();
  });
});
