import Claims from 'constants/claims';
import { FileTabType } from 'features/mapSideBar/tabs/FileTabs';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { act } from 'react-test-renderer';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

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
});
