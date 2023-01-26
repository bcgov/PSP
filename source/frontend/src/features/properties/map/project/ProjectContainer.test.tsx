import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockLookups } from 'mocks';
import { mockProjectGetResponse } from 'mocks/mockProjects';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitForElementToBeRemoved } from 'utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import ProjectContainer, { IProjectContainerProps } from './ProjectContainer';

const mockAxios = new MockAdapter(axios);
// mock auth library
jest.mock('@react-keycloak/web');
const onClose = jest.fn();

const DEFAULT_PROPS: IProjectContainerProps = {
  projectId: 1,
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

describe('ProjectContainer component', () => {
  // render component under test
  const setup = (
    props: IProjectContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider
        project={{
          ...mockProjectGetResponse(),
        }}
      >
        <ProjectContainer {...props} />
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
    mockAxios.onGet(new RegExp('projects/*')).reply(200, mockProjectGetResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders a spinner while loading', async () => {
    const { getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();

    await waitForElementToBeRemoved(spinner);
  });
});
