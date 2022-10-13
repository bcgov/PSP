import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/index';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { prettyFormatDate } from 'utils';
import {
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from 'utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import { AcquisitionContainer, IAcquisitionContainerProps } from './AcquisitionContainer';

const mockAxios = new MockAdapter(axios);

// mock auth library
jest.mock('@react-keycloak/web');

const onClose = jest.fn();

const DEFAULT_PROPS: IAcquisitionContainerProps = {
  acquisitionFileId: 1,
  onClose,
};
jest.mock('@react-keycloak/web');

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('AcquisitionContainer component', () => {
  // render component under test
  const setup = (
    props: IAcquisitionContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider
        file={{
          ...mockAcquisitionFileResponse(),
          fileType: FileTypes.Acquisition,
        }}
      >
        <AcquisitionContainer {...props} />
      </SideBarContextProvider>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios.onGet(new RegExp('acquisitionfiles/*')).reply(200, mockAcquisitionFileResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(asFragment()).toMatchSnapshot();
  });

  it('renders a spinner while loading', async () => {
    const { getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();

    await waitForElementToBeRemoved(spinner);
  });

  it('renders the underlying form', async () => {
    const { getByText, getByTestId } = setup();
    const testAcquisitionFile = mockAcquisitionFileResponse();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(getByText('Acquisition File')).toBeVisible();

    expect(getByText(testAcquisitionFile.fileNumber as string)).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appLastUpdateTimestamp))).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText, getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(getByText('Acquisition File')).toBeVisible();
    await waitFor(() => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle, getByTestId } = setup(undefined, { claims: [Claims.ACQUISITION_EDIT] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(getByTitle(/Edit acquisition properties/g)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle, getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(queryByTitle('Edit acquisition properties')).toBeNull();
  });
});
