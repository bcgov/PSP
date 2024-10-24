import Claims from '@/constants/claims';
import {
  mockAcquisitionFileResponse,
  mockAcquisitionFileSubFilesResponse,
} from '@/mocks/acquisitionFiles.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import SubFileListView, { ISubFileListViewProps } from './SubFileListView';

const onAdd = vi.fn();

describe('SubFileListView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ISubFileListViewProps> } = {},
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
        useMockAuthentication: true,
        claims: renderOptions.claims ?? [Claims.ACQUISITION_VIEW],
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
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the parent file name in the header', () => {
    const { getByTestId } = setup();
    const linkedHeader = getByTestId('linked-files-header');

    expect(linkedHeader).toHaveTextContent('1');
  });

  it('renders the correct amount of rows', () => {
    const { container } = setup();
    const tableRows = container.querySelectorAll('.table .tbody .tr-wrapper');

    expect(tableRows.length).toEqual(3);
  });

  it('displays the link for the relatives', async () => {
    const { getByTestId } = setup();
    const link = getByTestId('sub-file-link-65');

    expect(link).toBeInTheDocument();
  });

  it('displays the "Add Sub-file" button only for main files', () => {
    const { getByText } = setup({
      props: {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(1, 'ACQ-200-01'),
          parentAcquisitionFileId: null,
        },
      },
      claims: [Claims.ACQUISITION_ADD, Claims.ACQUISITION_VIEW],
    });

    const addButton = getByText(/Add Sub-interest File/i);
    expect(addButton).toBeInTheDocument();
  });

  it('calls onAdd when "Add Sub-file" button is clicked', async () => {
    const { getByText } = setup({
      props: {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(1, 'ACQ-200-01'),
          parentAcquisitionFileId: null,
        },
      },
      claims: [Claims.ACQUISITION_ADD, Claims.ACQUISITION_VIEW],
    });

    const addButton = getByText(/Add Sub-interest File/i);
    expect(addButton).toBeInTheDocument();
    await act(() => userEvent.click(addButton));
    expect(onAdd).toHaveBeenCalled();
  });

  it('hides the "Add Sub-file" button when looking at a sub-file', () => {
    const { queryByText } = setup({
      props: {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(1, 'ACQ-200-01'),
          parentAcquisitionFileId: 100,
        },
      },
      claims: [Claims.ACQUISITION_ADD, Claims.ACQUISITION_VIEW],
    });

    const addButton = queryByText(/Add Sub-interest File/i);
    expect(addButton).toBeNull();
  });
});
