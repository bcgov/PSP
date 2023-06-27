import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';
import { act } from 'react-test-renderer';

import Claims from '@/constants/claims';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import AcquisitionFileTabs, { IAcquisitionFileTabsProps } from './AcquisitionFileTabs';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const setIsEditing = jest.fn();

describe('AcquisitionFileTabs component', () => {
  // render component under test
  const setup = (
    props: Omit<IAcquisitionFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/blah/:tab">
        <AcquisitionFileTabs
          acquisitionFile={props.acquisitionFile}
          defaultTab={props.defaultTab}
          setIsEditing={setIsEditing}
        />
      </Route>,
      {
        useMockAuthentication: true,
        history,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    history.replace(`/blah/${FileTabType.FILE_DETAILS}`);
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
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
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    expect(tab).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    await act(async () => userEvent.click(tab));

    expect(getByText('Documents')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.DOCUMENTS}`);
  });

  it('hides the expropriation tab when the Acquisition file type is "Consensual Agreement"', () => {
    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      defaultTab: FileTabType.FILE_DETAILS,
    });

    const expropriationButton = queryByText('Expropriation');
    expect(expropriationButton).not.toBeInTheDocument();
  });

  it('shows the expropriation tab when the Acquisition file type is "Section 3"', () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN3',
      description: 'Section 3 Agreement',
      isDisabled: false,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
    });

    const editButton = queryByText('Expropriation');
    expect(editButton).toBeInTheDocument();
  });

  it('shows the expropriation tab when the Acquisition file type is "Section 6"', () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN6',
      description: 'Section 6 Expropriation',
      isDisabled: false,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
    });

    const editButton = queryByText('Expropriation');
    expect(editButton).toBeInTheDocument();
  });
});
