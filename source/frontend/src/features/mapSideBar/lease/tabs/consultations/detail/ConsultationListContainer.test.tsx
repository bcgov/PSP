import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { vi } from 'vitest';
import { useConsultationProvider } from '@/hooks/repositories/useConsultationProvider';
import { IConsultationListViewProps } from './ConsultationListView';
import ConsultationListContainer, { IConsultationListProps } from './ConsultationListContainer';
import { getMockApiConsultation } from '@/mocks/consultations.mock';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockDeleteApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/hooks/repositories/useConsultationProvider');
vi.mocked(useConsultationProvider).mockImplementation(() => ({
  getLeaseConsultations: mockGetApi,
  deleteLeaseConsultation: mockDeleteApi,
  addLeaseConsultation: {} as any, //unused
  getLeaseConsultationById: {} as any,
  updateLeaseConsultation: {} as any,
}));

describe('ConsultationListContainer component', () => {
  // render component under test

  let viewProps: IConsultationListViewProps;
  const View = forwardRef<FormikProps<any>, IConsultationListViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IConsultationListProps> }) => {
    const utils = render(
      <ConsultationListContainer
        {...renderOptions.props}
        leaseId={renderOptions?.props?.leaseId ?? 1}
        View={View}
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

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    mockGetApi.response = [
      { ...getMockApiConsultation(), id: 1 },
      { ...getMockApiConsultation(), id: 2 },
    ];
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls getLeaseConsultations', async () => {
    setup({});

    await waitFor(() => {
      expect(mockGetApi.execute).toHaveBeenCalledTimes(1);
    });
  });

  it('returns the lease consultations returned from the api', async () => {
    mockGetApi.execute.mockResolvedValue([
      { ...getMockApiConsultation(), id: 1 },
      { ...getMockApiConsultation(), id: 2 },
    ]);
    setup({});

    await waitFor(() => {
      expect(viewProps.consultations).toHaveLength(2);
    });
  });

  it('handles onAdd request with expected navigation', async () => {
    mockGetApi.response = [];
    setup({});

    await act(async () => {
      viewProps.onAdd();
    });

    await waitFor(() => {
      expect(history.location.pathname).toBe('//consultations/add');
    });
  });

  it('handles onEdit request with expected navigation', async () => {
    mockGetApi.response = [];
    setup({});

    await act(async () => {
      viewProps.onEdit(1);
    });

    await waitFor(() => {
      expect(history.location.pathname).toBe('//consultations/1/edit');
    });
  });

  it('handles onDelete by calling the expected API', async () => {
    mockGetApi.response = [];
    setup({});

    await act(async () => {
      viewProps.onDelete(1);
    });

    await waitFor(() => {
      expect(mockDeleteApi.execute).toHaveBeenCalled();
    });
  });

  it('after calling onDelete, refreshes list of consultations', async () => {
    mockGetApi.response = [];
    setup({});

    await act(async () => {
      viewProps.onDelete(1);
    });

    await waitFor(() => {
      expect(mockDeleteApi.execute).toHaveBeenCalled();
    });
  });

  it('throws an error for an invalid lease id', async () => {
    mockGetApi.response = [];
    vi.spyOn(console, 'error').mockImplementation(() => {});
    const act = () => setup({ props: { leaseId: 0 } });
    expect(act).toThrowError('Unable to determine id of current file.');
  });
});
