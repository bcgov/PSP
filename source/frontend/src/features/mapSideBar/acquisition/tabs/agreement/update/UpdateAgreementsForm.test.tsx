import { FormikProps } from 'formik';
import { createRef } from 'react';

import { AGREEMENT_TYPES } from '@/constants/API';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  selectOptions,
  userEvent,
} from '@/utils/test-utils';

import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';
import { AgreementsFormModel } from './models';
import { IUpdateAgreementsFormProps, UpdateAgreementsForm } from './UpdateAgreementsForm';

jest.mock('../../fileDetails/detail/statusUpdateSolver');

export const organizerMock = {
  canEditOrDeleteAgreement: jest.fn(),
};

// mock API service calls
jest.mock('@/hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const agreementTypes: ILookupCode[] = mockLookups.filter(x => x.type === AGREEMENT_TYPES);

const mockViewProps: IUpdateAgreementsFormProps = {
  acquistionFile: undefined,
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
        acquistionFile={mockViewProps.acquistionFile}
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
    (StatusUpdateSolver as jest.Mock).mockImplementation(() => organizerMock);
    organizerMock.canEditOrDeleteAgreement.mockReturnValue(true);
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

  it('does not display cancellation note by default', async () => {
    const { queryByText } = setup();

    expect(queryByText(/Cancellation reason/i)).toBeNull();
  });

  it('displays cancellation note if status is cancelled', async () => {
    const { getByText } = setup();

    await act(async () => {
      await act(() => selectOptions('agreements.0.agreementStatusTypeCode', 'CANCELLED'));
    });
    expect(getByText(/Cancellation reason/i)).toBeVisible();
  });

  it('displays a popup if status is changed from cancelled and there is a cancellation note', async () => {
    const { getByText, container } = setup();

    await act(async () => selectOptions('agreements.0.agreementStatusTypeCode', 'CANCELLED'));
    await act(async () => {
      fillInput(container, 'agreements.0.cancellationNote', 'this is a test cancellation note');
    });
    await act(async () => selectOptions('agreements.0.agreementStatusTypeCode', 'DRAFT'));
    expect(
      getByText(
        'Changing status to a status other than "Cancelled" will remove your "Cancellation reason". Are you sure you want to continue?',
      ),
    ).toBeVisible();
  });

  it('hides cancellation note if status is changed from cancelled and there is a cancellation note, and the displayed popup is confirmed', async () => {
    const { container, getByText, formikRef, queryByText } = setup();

    await act(async () => selectOptions('agreements.0.agreementStatusTypeCode', 'CANCELLED'));
    await act(async () => {
      fillInput(container, 'agreements.0.cancellationNote', 'this is a test cancellation note');
    });
    await act(async () => selectOptions('agreements.0.agreementStatusTypeCode', 'DRAFT'));
    await act(async () => userEvent.click(getByText('Yes')));

    expect(queryByText(/Cancellation reason/i)).toBeNull();
    expect(formikRef.current?.values.agreements[0].cancellationNote).toBe('');
  });

  it('Cannot edit if not allowed', async () => {
    organizerMock.canEditOrDeleteAgreement.mockReturnValue(false);
    const { getByTestId } = setup();

    const element: HTMLSelectElement | null = document.querySelector(
      `select[name="agreements.0.agreementStatusTypeCode"]`,
    );

    expect(element).toHaveAttribute('disabled');
    expect(getByTestId('tooltip-icon-1-agreement-cannot-edit-tooltip')).toBeVisible();
  });
});
