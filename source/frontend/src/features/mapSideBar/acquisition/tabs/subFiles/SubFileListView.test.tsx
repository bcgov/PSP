import {
  mockKeycloak,
  render,
  RenderOptions,
} from '@/utils/test-utils';
import SubFileListView, { ISubFileListViewProps } from './SubFileListView';
import {
  mockAcquisitionFileResponse,
  mockAcquisitionFileSubFilesResponse,
} from '@/mocks/acquisitionFiles.mock';
import Claims from '@/constants/claims';

const onAdd = vi.fn();

describe('DocumentDetailForm component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ISubFileListViewProps> },
  ) => {
    const mockCurrentAcquisitionFile = mockAcquisitionFileResponse(1, 'ACQ-200-01');
    const utils = render(
      <SubFileListView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockCurrentAcquisitionFile}
        subFiles={
          renderOptions.props?.subFiles ?? [
            mockCurrentAcquisitionFile,
            ...mockAcquisitionFileSubFilesResponse(),
          ]
        }
        onAdd={onAdd}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
      useMockAuthentication: true,
    };
  };

  beforeEach(() => {
    mockKeycloak({ claims: [Claims.ACQUISITION_VIEW] });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('renders the parent file name in the header', async () => {
    const { getByTestId } = await setup({});
    const linkedHeader = getByTestId('linked-files-header');

    expect(linkedHeader).toHaveTextContent('1');
  });

  it('renders the correct amount of rows', async () => {
    const { container } = await setup({});
    const tableRows = container.querySelectorAll('.table .tbody .tr-wrapper');

    expect(tableRows.length).toEqual(3);
  });

  it('displays the link for the relatives', async () => {
    const { getByTestId } = await setup({});
    const link = getByTestId('sub-file-link-65');

    expect(link).toBeInTheDocument();
  });
});
