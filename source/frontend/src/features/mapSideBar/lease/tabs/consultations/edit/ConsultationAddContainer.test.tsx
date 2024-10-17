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
import ConsultationAddContainer, { IConsultationAddProps } from './ConsultationAddContainer';
import { ConsultationFormModel } from './models';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockAddApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/hooks/repositories/useConsultationProvider');
vi.mocked(useConsultationProvider).mockImplementation(() => ({
  addLeaseConsultation: mockAddApi,
  deleteLeaseConsultation: {} as any,
  getLeaseConsultations: {} as any, //unused
  getLeaseConsultationById: {} as any,
  updateLeaseConsultation: {} as any,
}));

const onSuccess = vi.fn();

describe('ConsultationAddContainer component', () => {
  // render component under test

  let viewProps: IConsultationEditFormProps;
  const View = forwardRef<FormikProps<any>, IConsultationEditFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IConsultationAddProps> }) => {
    const utils = render(
      <ConsultationAddContainer
        {...renderOptions.props}
        leaseId={renderOptions?.props?.leaseId ?? 1}
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
    history.push('/lease/1/consultations/add');
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
      expect(mockAddApi.execute).toHaveBeenCalledTimes(1);
    });
  });

  it('submits data when onSubmit called history is updated', async () => {
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
      expect(history.location.pathname).toEqual('/lease/1/consultations/add');
    });
  });

  it('displays error when add fails', async () => {
    mockAddApi.execute.mockRejectedValue(createAxiosError(500));
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
