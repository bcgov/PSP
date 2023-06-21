import { act } from 'react-test-renderer';

import Claims from '@/constants/claims';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { FileTabType } from '../../shared/detail/FileTabs';
import AcquisitionFileTabs, { IAcquisitionFileTabsProps } from './AcquisitionFileTabs';

// mock auth library
jest.mock('@react-keycloak/web');

const setIsEditing = jest.fn();

describe('AcquisitionFileTabs component', () => {
  // render component under test
  const setup = (
    props: Omit<IAcquisitionFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <AcquisitionFileTabs
        acquisitionFile={props.acquisitionFile}
        defaultTab={props.defaultTab}
        setIsEditing={setIsEditing}
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

    const editButton = getByText('Documents');
    expect(editButton).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
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
