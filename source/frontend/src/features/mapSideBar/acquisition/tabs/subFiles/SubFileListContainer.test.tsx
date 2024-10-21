import Claims from '@/constants/claims';
import SubFileListContainer, { ISubFileListContainerProps } from './SubFileListContainer';
import { ISubFileListViewProps } from './SubFileListView';
import { act, render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';
import { mockAcquisitionFileResponse, mockAcquisitionFileSubFilesResponse } from '@/mocks/acquisitionFiles.mock';


const mockGetAcquisitionSubFilesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
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
        acquisitionFile={renderOptions?.props?.acquisitionFile ?? mockAcquisitionFileResponse(64)}
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

    expect(mockGetAcquisitionSubFilesApi.execute).toHaveBeenCalled();
  });
});
