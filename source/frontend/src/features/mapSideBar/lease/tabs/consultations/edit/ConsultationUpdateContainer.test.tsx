import { FormikHelpers, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { vi } from 'vitest';
import { useConsultationProvider } from '@/hooks/repositories/useConsultationProvider';
import { getMockApiConsultation } from '@/mocks/consultations.mock';
import { IConsultationEditFormProps } from './ConsultationEditForm';
import { IConsultationAddProps } from './ConsultationAddContainer';
import { ConsultationFormModel } from './models';
import ConsultationUpdateContainer from './ConsultationUpdateContainer';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockGetPersonApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockGetOrganizationApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/features/contacts/repositories/usePersonRepository');
vi.mocked(usePersonRepository).mockImplementation(() => ({
  getPersonDetail: mockGetPersonApi,
}));

vi.mock('@/features/contacts/repositories/useOrganizationRepository');
vi.mocked(useOrganizationRepository).mockImplementation(() => ({
  getOrganizationDetail: mockGetOrganizationApi,
}));

vi.mock('@/hooks/repositories/useConsultationProvider');
vi.mocked(useConsultationProvider).mockImplementation(() => ({
  getLeaseConsultationById: mockGetApi,
  updateLeaseConsultation: mockUpdateApi,
  addLeaseConsultation: {} as any, //unused
  deleteLeaseConsultation: {} as any,
  getLeaseConsultations: {} as any,
}));

const onSuccess = vi.fn();

describe('ConsultationUpdateContainer component', () => {
  // render component under test

  let viewProps: IConsultationEditFormProps;
  const View = forwardRef<FormikProps<any>, IConsultationEditFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IConsultationAddProps> }) => {
    const utils = render(
      <ConsultationUpdateContainer
        {...renderOptions.props}
        leaseId={renderOptions?.props?.leaseId ?? 1}
        consultationId={renderOptions?.props?.leaseId ?? 1}
        View={View}
        onSuccess={onSuccess}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    history.push('/lease/1/consultations/1/edit');
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls expected route when cancelled', async () => {
    setup({});

    await act(async () => {
      viewProps.onCancel();
    });

    expect(history.location.pathname).toEqual('/lease/1');
  });

  it('calls getOrganization method for returned consultation', async () => {
    mockGetApi.execute.mockResolvedValue({
      ...getMockApiConsultation(),
      personId: null,
      organizationId: 1,
    });
    setup({});

    await act(async () => {});

    expect(mockGetOrganizationApi.execute).toHaveBeenCalledTimes(1);
  });

  it('calls getPerson method for returned consultation', async () => {
    mockGetApi.execute.mockResolvedValue(getMockApiConsultation());
    setup({});

    await act(async () => {});

    expect(mockGetPersonApi.execute).toHaveBeenCalledTimes(1);
  });

  it('submits data when onSubmit called', async () => {
    setup({});

    const formikHelpers: Partial<FormikHelpers<ConsultationFormModel>> = {
      setSubmitting: vi.fn(),
      resetForm: vi.fn(),
    };
    await act(async () => {
      viewProps.onSubmit(
        ConsultationFormModel.fromApi(getMockApiConsultation(), null, null),
        formikHelpers as FormikHelpers<ConsultationFormModel>,
      );
    });

    await waitFor(() => {
      expect(mockUpdateApi.execute).toHaveBeenCalledTimes(1);
    });
  });

  it('submits data when onSubmit called history is updated', async () => {
    mockUpdateApi.execute.mockResolvedValue({});
    setup({});

    const formikHelpers: Partial<FormikHelpers<ConsultationFormModel>> = {
      setSubmitting: vi.fn(),
      resetForm: vi.fn(),
    };
    await act(async () => {
      viewProps.onSubmit(
        ConsultationFormModel.fromApi(getMockApiConsultation(), null, null),
        formikHelpers as FormikHelpers<ConsultationFormModel>,
      );
    });

    expect(onSuccess).toHaveBeenCalledTimes(1);
  });

  it('displays error when add fails', async () => {
    mockUpdateApi.execute.mockRejectedValue(createAxiosError(500));
    const { getByText } = setup({});

    const formikHelpers: Partial<FormikHelpers<ConsultationFormModel>> = {
      setSubmitting: vi.fn(),
      resetForm: vi.fn(),
    };
    await act(async () => {
      viewProps.onSubmit(
        ConsultationFormModel.fromApi(getMockApiConsultation(), null, null),
        formikHelpers as FormikHelpers<ConsultationFormModel>,
      );
    });

    await waitFor(() => {
      expect(getByText('Unable to save. Please try again.')).toBeVisible();
    });
  });
});
