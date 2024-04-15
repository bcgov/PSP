import { createMemoryHistory } from 'history';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import {
  getMockApiCompensationList,
  getMockApiDefaultCompensation,
  getMockDefaultCreateCompenReq,
} from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
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

import CompensationListContainer, {
  ICompensationListContainerProps,
} from './CompensationListContainer';
import { ICompensationListViewProps } from './CompensationListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
const mockGetApi = {
  error: undefined,
  response: getMockApiCompensationList(),
  execute: vi.fn(),
  loading: false,
};

const mockPutApi = {
  error: undefined,
  response: { ...mockAcquisitionFileResponse(), totalAllowableCompensation: 1000 },
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useRequisitionCompensationRepository');

const history = createMemoryHistory();

vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionCompensationRequisitions: mockGetApi,
      postAcquisitionCompensationRequisition: mockPostApi,
      updateAcquisitionFile: mockPutApi,
    };
  },
}));

let viewProps: ICompensationListViewProps;
const CompensationListView = (props: ICompensationListViewProps) => {
  viewProps = props;
  return <></>;
};

describe('compensation list view container', () => {
  const setup = async (
    renderOptions?: RenderOptions & Partial<ICompensationListContainerProps>,
  ) => {
    // render component under test
    const component = render(
      <SideBarContextProvider>
        <CompensationListContainer
          View={CompensationListView}
          fileId={renderOptions?.fileId ?? 1}
          file={renderOptions?.file ?? ({} as any)}
        />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    vi.mocked(useCompensationRequisitionRepository).mockImplementation(
      () =>
        ({
          deleteCompensation: mockPostApi,
        } as unknown as ReturnType<typeof useCompensationRequisitionRepository>),
    );
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({
      claims: [],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('Delete compensation calls displays delete modal', async () => {
    setup({
      claims: [],
    });
    await act(async () => {
      viewProps.onDelete(1);
    });
    const modal = await screen.findByText('Confirm Delete');

    expect(modal).toBeVisible();
  });

  it('confirming delete modal sends delete call', async () => {
    setup({
      claims: [],
    });
    await act(async () => {
      viewProps.onDelete(1);
    });
    const continueButton = await screen.findByText('Yes');
    await act(async () => userEvent.click(continueButton));

    expect(mockPostApi.execute).toHaveBeenCalledWith(1);
  });

  it('fetchs data when no data is currently available in container', async () => {
    setup({
      claims: [],
    });

    expect(mockGetApi.execute).toHaveBeenCalledTimes(0);
  });

  it('Creates the Compensation Requisition with the default data', async () => {
    mockPostApi.execute.mockResolvedValue(getMockApiDefaultCompensation());

    await setup({});
    await act(async () => {
      viewProps?.onAdd();
    });

    expect(mockPostApi.execute).toHaveBeenCalledWith(1, getMockDefaultCreateCompenReq());
  });

  it('returns an updated total allowable compensation if the update operation was successful', async () => {
    await setup({});
    await act(async () => {
      viewProps?.onUpdateTotalCompensation(1000);
    });

    expect(mockPutApi.execute).toHaveBeenCalledWith({ totalAllowableCompensation: 1000 }, []);
  });

  it('displays an error modal and throws an error if the api call fails', async () => {
    mockPutApi.execute.mockRejectedValue(createAxiosError(400, 'total allowable update error'));

    await setup({});
    await act(async () => {
      expect(async () => await viewProps?.onUpdateTotalCompensation(1000)).rejects.toThrowError(
        'total allowable update error',
      );
    });
    await screen.findByText('total allowable update error');
  });
});
