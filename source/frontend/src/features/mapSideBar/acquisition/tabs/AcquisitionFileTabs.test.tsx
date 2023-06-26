import { act } from 'react-test-renderer';

import Claims from '@/constants/claims';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { FileTabType } from '../../shared/detail/FileTabs';
import AcquisitionFileTabs, { IAcquisitionFileTabsProps } from './AcquisitionFileTabs';

// mock auth library
jest.mock('@react-keycloak/web');

const setContainerState = jest.fn();

describe('AcquisitionFileTabs component', () => {
  // render component under test
  const setup = (props: IAcquisitionFileTabsProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionFileTabs
        acquisitionFile={props.acquisitionFile}
        defaultTab={props.defaultTab}
        setContainerState={setContainerState}
      />,
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
    const { asFragment } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
        setContainerState,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
        setContainerState,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    expect(editButton).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
        setContainerState,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    await act(async () => {
      userEvent.click(editButton);
    });
    await waitFor(() => {
      expect(getByText('Documents')).toHaveClass('active');
    });
  });

  it('hides the expropiation tab when the Acquisition file type is "Consensual Agreement"', () => {
    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      defaultTab: FileTabType.FILE_DETAILS,
      setContainerState,
    });

    const expropiationButton = queryByText('Expropiation');
    expect(expropiationButton).not.toBeInTheDocument();
  });

  it('shows the expropiation tab when the Acquisition file type is "Section 3"', () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN3',
      description: 'Section 3 Agreement',
      isDisabled: false,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
      setContainerState,
    });

    const editButton = queryByText('Expropiation');
    expect(editButton).toBeInTheDocument();
  });

  it('shows the expropiation tab when the Acquisition file type is "Section 6"', () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN6',
      description: 'Section 6 Expropriation',
      isDisabled: false,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
      setContainerState,
    });

    const editButton = queryByText('Expropiation');
    expect(editButton).toBeInTheDocument();
  });
});
