import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import {
  mockAcquisitionFileResponse,
  mockAcquisitionFileSubFilesResponse,
} from '@/mocks/acquisitionFiles.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { act, render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';

import SubFileListContainer, { ISubFileListContainerProps } from './SubFileListContainer';
import { ISubFileListViewProps } from './SubFileListView';

const history = createMemoryHistory();

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
  const setup = (
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
        history,
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
    const { getByText } = setup();
    await act(async () => {});
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes the request to get the Acquisition Sub-Files', async () => {
    mockGetAcquisitionSubFilesApi.execute.mockResolvedValue(mockAcquisitionFileSubFilesResponse());

    setup();
    await waitForEffects();

    expect(mockGetAcquisitionSubFilesApi.execute).toHaveBeenCalledWith(1);
  });

  it('makes the request to get the Parent and siblings when current is sub-file', async () => {
    const mockCurrentAcquisitionFile: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(64),
      parentAcquisitionFileId: 1,
    };
    mockGetAcquisitionFileApi.execute.mockResolvedValue(mockCurrentAcquisitionFile);
    mockGetAcquisitionSubFilesApi.execute.mockResolvedValue(mockAcquisitionFileSubFilesResponse());

    setup({
      props: {
        acquisitionFile: mockCurrentAcquisitionFile,
      },
    });
    await waitForEffects();

    expect(mockGetAcquisitionFileApi.execute).toHaveBeenCalledWith(1);
    expect(mockGetAcquisitionSubFilesApi.execute).toHaveBeenCalledWith(1);
  });

  it('redirects to create acquisition form for sub-files', async () => {
    setup({ props: { acquisitionFile: mockAcquisitionFileResponse(100) } });
    await act(async () => {
      viewProps.onAdd();
    });

    expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    expect(history.location.search).toBe('?parentId=100'); // parentId should be appended to the route
  });
});
