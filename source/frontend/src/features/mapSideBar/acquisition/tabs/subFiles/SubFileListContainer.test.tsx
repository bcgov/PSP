import Claims from '@/constants/claims';
import SubFileListContainer, { ISubFileListContainerProps } from './SubFileListContainer';
import { ISubFileListViewProps } from './SubFileListView';
import { act, render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';
import { mockAcquisitionFileResponse, mockAcquisitionFileSubFilesResponse } from '@/mocks/acquisitionFiles.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';


const mockGetAcquisitionSubFilesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
const mockGetAcquisitionFileApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionFile: mockGetAcquisitionFileApi,
      getAcquisitionSubFiles: mockGetAcquisitionSubFilesApi,
    };
  },
}));

let viewProps: ISubFileListViewProps | null;
const TestView: React.FC<ISubFileListViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};
describe('SubFileListContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<ISubFileListContainerProps>;
    } = {},
  ) => {
    const component = render(
      <SubFileListContainer
        acquisitionFile={renderOptions?.props?.acquisitionFile ?? mockAcquisitionFileResponse(1)}
        View={TestView}
      />,
      {
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_VIEW],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  it('renders the underlying view', async () => {
    const { getByText } = await setup();
    await act(async () => {});
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes the request to get the Acquisition Sub-Files', async () => {
    mockGetAcquisitionSubFilesApi.execute.mockResolvedValue(mockAcquisitionFileSubFilesResponse());

    await act(async () => {
      setup({});
    });
    await waitForEffects();

    expect(mockGetAcquisitionSubFilesApi.execute).toHaveBeenCalledWith(1);
  });

  it('makes the request to get the Parent and siblings when current is sub-file', async () => {
    const mockCurrentAcquisitionFile: ApiGen_Concepts_AcquisitionFile  = { ...mockAcquisitionFileResponse(64), parentAcquisitionFileId: 1 };
    mockGetAcquisitionFileApi.execute.mockResolvedValue(mockCurrentAcquisitionFile);
    mockGetAcquisitionSubFilesApi.execute.mockResolvedValue(mockAcquisitionFileSubFilesResponse());

    await act(async () => {
      setup({props: {
        acquisitionFile: mockCurrentAcquisitionFile
      }});
    });
    await waitForEffects();

    expect(mockGetAcquisitionFileApi.execute).toHaveBeenCalledWith(1);
    expect(mockGetAcquisitionSubFilesApi.execute).toHaveBeenCalledWith(1);
  });
});
