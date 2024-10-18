import { findAllByTestId, getByTestId, mockKeycloak, render, RenderOptions } from '@/utils/test-utils';
import SubFileListView, { ISubFileListViewProps } from './SubFileListView';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { mockAcquisitionFileResponse, mockAcquisitionFileSubFilesResponse } from '@/mocks/acquisitionFiles.mock';
import Claims from '@/constants/claims';

const onAdd = vi.fn();

describe('DocumentDetailForm component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ISubFileListViewProps> },
  ) => {
    const utils = render(
      <SubFileListView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        acquisitionFile={
          renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse(64, 'ACQ-200-01')
        }
        subFiles={renderOptions.props?.subFiles ?? mockAcquisitionFileSubFilesResponse()}
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

    expect(linkedHeader).toHaveTextContent('51');
  });

  it('renders the correct amount of rows', async () => {
    const { container } = await setup({});
    const tableRows = container.querySelectorAll('.table .tbody .tr-wrapper');

    expect(tableRows.length).toEqual(2);
  });

  it('displays the link for the relatives', async () => {
    const { getByTestId } = await setup({});
    const link = getByTestId('sub-file-link-65');

    expect(link).toBeInTheDocument();
  });
});
