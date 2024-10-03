import { createMemoryHistory } from 'history';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import {
  getMockApiCompensationList,
  getMockApiDefaultCompensation,
  getMockDefaultCreateCompenReq,
} from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
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
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

const mockDeleteApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetFileCompensationsApi = {
  error: undefined,
  response: getMockApiCompensationList(),
  execute: vi.fn(),
  loading: false,
};

const mockPutAcquisitionApi = {
  error: undefined,
  response: { ...mockAcquisitionFileResponse(), totalAllowableCompensation: 1000 },
  execute: vi.fn(),
  loading: false,
};

const mockPutLeaseApi = {
  error: undefined,
  response: { ...getMockApiLease(), totalAllowableCompensation: 1000 },
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useRequisitionCompensationRepository', () => ({
  useCompensationRequisitionRepository: () => {
    return {
      postCompensationRequisition: mockPostApi,
      getFileCompensationRequisitions: mockGetFileCompensationsApi,
      deleteCompensation: mockDeleteApi,
    };
  },
}));

vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      updateAcquisitionFile: mockPutAcquisitionApi,
    };
  },
}));

vi.mock('@/hooks/repositories/useLeaseRepository', () => ({
  useLeaseRepository: () => {
    return {
      updateLease: mockPutLeaseApi,
    };
  },
}));

let viewProps: ICompensationListViewProps;
const TestView: React.FC<ICompensationListViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('compensation list view container', () => {
  const setup = async (
    renderOptions?: RenderOptions & Partial<ICompensationListContainerProps>,
  ) => {
    // render component under test
    const component = render(
      <SideBarContextProvider>
        <CompensationListContainer
          fileType={renderOptions?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
          file={renderOptions?.file ?? mockAcquisitionFileResponse(1)}
          View={TestView}
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
    vi.resetAllMocks();
    mockGetFileCompensationsApi.execute.mockResolvedValue([]);
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
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

    expect(mockDeleteApi.execute).toHaveBeenCalledWith(1);
  });

  it('fetchs data when no data is currently available in container', async () => {
    setup({
      claims: [],
    });

    expect(mockGetFileCompensationsApi.execute).toHaveBeenCalledTimes(0);
  });

  it('Creates the Compensation Requisition with the default data for Acquisition File', async () => {
    const acquisitionFileMock: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      fileProperties: [],
    };
    mockPostApi.execute.mockResolvedValue(
      getMockApiDefaultCompensation(acquisitionFileMock.id, null),
    );

    await setup({
      fileType: ApiGen_CodeTypes_FileTypes.Acquisition,
      file: acquisitionFileMock,
    });

    await act(async () => {
      viewProps?.onAdd();
    });

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Acquisition,
      expect.objectContaining({
        acquisitionFileId: acquisitionFileMock.id,
        leaseId: null,
        isDraft: true,
        compReqLeaseProperties: null,
      }),
    );
  });

  it('Creates the Compensation Requisition with the default data for Lease File', async () => {
    const leaseFileMock: ApiGen_Concepts_Lease = { ...getMockApiLease(), fileProperties: [] };
    mockPostApi.execute.mockResolvedValue(getMockApiDefaultCompensation(null, leaseFileMock.id));

    await setup({
      fileType: ApiGen_CodeTypes_FileTypes.Lease,
      file: leaseFileMock,
    });

    await act(async () => {
      viewProps?.onAdd();
    });

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Lease,
      expect.objectContaining({
        acquisitionFileId: null,
        leaseId: leaseFileMock.id,
        isDraft: true,
        compReqAcquisitionProperties: null,
      }),
    );
  });

  it('Creates the Compensation Requisition with all the properties selected from the Acquisition file', async () => {
    const acquisitionFileMock: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
    };

    mockPostApi.execute.mockResolvedValue(
      getMockApiDefaultCompensation(acquisitionFileMock.id, null),
    );

    await setup({
      fileType: ApiGen_CodeTypes_FileTypes.Acquisition,
      file: acquisitionFileMock,
    });

    await act(async () => {
      viewProps?.onAdd();
    });

    const mockNewCompensationRequisition = {
      ...getMockDefaultCreateCompenReq(acquisitionFileMock.id, null),
      compReqAcquisitionProperties: [
        {
          compensationRequisitionPropertyId: null,
          compensationRequisitionId: null,
          propertyAcquisitionFileId: 1,
          acquisitionFileProperty: null,
        },
        {
          compensationRequisitionPropertyId: null,
          compensationRequisitionId: null,
          propertyAcquisitionFileId: 2,
          acquisitionFileProperty: null,
        },
      ],
      compReqLeaseProperties: null,
      compReqLeaseStakeholder: null,
    } as ApiGen_Concepts_CompensationRequisition;

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Acquisition,
      mockNewCompensationRequisition,
    );
  });

  it('Creates the Compensation Requisition with all the properties selected from the Lease', async () => {
    const leaseFileMock: ApiGen_Concepts_Lease = {
      ...getMockApiLease(),
      fileProperties: [
        {
          id: 10,
          fileId: 1,
          file: null,
          leaseArea: 0,
          areaUnitType: undefined,
          propertyName: '',
          location: undefined,
          displayOrder: 0,
          property: undefined,
          propertyId: 0,
          rowVersion: 0,
        },
        {
          id: 20,
          fileId: 1,
          file: null,
          leaseArea: 0,
          areaUnitType: undefined,
          propertyName: '',
          location: undefined,
          displayOrder: 0,
          property: undefined,
          propertyId: 0,
          rowVersion: 0,
        },
      ],
    };
    mockPostApi.execute.mockResolvedValue(getMockApiDefaultCompensation(null, leaseFileMock.id));

    await setup({
      fileType: ApiGen_CodeTypes_FileTypes.Lease,
      file: leaseFileMock,
    });

    await act(async () => {
      viewProps?.onAdd();
    });

    const mockNewCompensationRequisition = {
      ...getMockDefaultCreateCompenReq(null, leaseFileMock.id),
      compReqLeaseProperties: [
        {
          compensationRequisitionPropertyId: null,
          compensationRequisitionId: null,
          propertyLeaseId: 10,
          leaseProperty: null,
        },
        {
          compensationRequisitionPropertyId: null,
          compensationRequisitionId: null,
          propertyLeaseId: 20,
          leaseProperty: null,
        },
      ],
      compReqAcquisitionProperties: null,
      compReqLeaseStakeholder: null,
    } as ApiGen_Concepts_CompensationRequisition;

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      ApiGen_CodeTypes_FileTypes.Lease,
      mockNewCompensationRequisition,
    );
  });

  it('returns an updated total allowable compensation for ACQUISITION if the update operation was successful', async () => {
    await setup({});
    await act(async () => {
      viewProps?.onUpdateTotalCompensation(1000);
    });

    expect(mockPutAcquisitionApi.execute).toHaveBeenCalledWith(
      expect.objectContaining({
        ...mockAcquisitionFileResponse(),
        totalAllowableCompensation: 1000,
      }),
      [],
    );
  });

  it('returns an updated total allowable compensation for LEASE if the update operation was successful', async () => {
    const leaseFileMock: ApiGen_Concepts_Lease = { ...getMockApiLease(), fileProperties: [] };

    await setup({
      fileType: ApiGen_CodeTypes_FileTypes.Lease,
      file: leaseFileMock,
    });

    await act(async () => {
      viewProps?.onUpdateTotalCompensation(1000);
    });

    expect(mockPutLeaseApi.execute).toHaveBeenCalledWith(
      expect.objectContaining({
        totalAllowableCompensation: 1000,
      }),
      [],
    );
  });

  it('displays an error modal and throws an error if the api call fails', async () => {
    mockPutAcquisitionApi.execute.mockRejectedValue(
      createAxiosError(400, 'total allowable update error'),
    );

    await setup({});
    await act(async () => {
      expect(async () => await viewProps?.onUpdateTotalCompensation(1000)).rejects.toThrowError(
        'total allowable update error',
      );
    });
    await screen.findByText('total allowable update error');
  });
});
