import Claims from '@/constants/claims';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import AcquisitionSummaryView, { IAcquisitionSummaryViewProps } from './AcquisitionSummaryView';

// mock auth library
jest.mock('@react-keycloak/web');

const onEdit = jest.fn();

describe('AcquisitionSummaryView component', () => {
  // render component under test
  const setup = (props: IAcquisitionSummaryViewProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionSummaryView acquisitionFile={props.acquisitionFile} onEdit={props.onEdit} />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      onEdit,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with acquisition edit permissions', () => {
    const { getByTitle } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        onEdit,
      },
      { claims: [Claims.ACQUISITION_EDIT] },
    );

    const editButton = getByTitle('Edit acquisition file');
    expect(editButton).toBeVisible();
    userEvent.click(editButton);
    expect(onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have acquisition edit permissions', () => {
    const { queryByTitle } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        onEdit,
      },
      { claims: [] },
    );

    const editButton = queryByTitle('Edit acquisition file');
    expect(editButton).toBeNull();
  });

  it('renders historical file number', () => {
    const mockResponse = mockAcquisitionFileResponse();
    const { getByText } = setup(
      {
        acquisitionFile: mockResponse,
        onEdit,
      },
      { claims: [] },
    );
    expect(getByText('legacy file number')).toBeVisible();
  });

  it('renders owner solicitor information', () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        onEdit,
      },
      { claims: [] },
    );
    expect(getByText('Millennium Inc')).toBeVisible();
  });

  it('renders owner representative information', () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        onEdit,
      },
      { claims: [] },
    );
    expect(getByText('Han Solo')).toBeVisible();
    expect(getByText('test representative comment')).toBeVisible();
  });
});
