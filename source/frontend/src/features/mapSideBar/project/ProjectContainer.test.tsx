import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mockLookups } from '@/mocks/index.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import ProjectContainer, { IProjectContainerViewProps } from './ProjectContainer';

const mockAxios = new MockAdapter(axios);
// mock auth library

let viewProps: IProjectContainerViewProps | undefined = undefined;
const TestView: React.FC<IProjectContainerViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

describe('ProjectContainer component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider
        project={{
          ...mockProjectGetResponse(),
        }}
      >
        <ProjectContainer projectId={1} View={TestView} onClose={vi.fn()} />
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
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  beforeEach(() => {
    vi.resetAllMocks();
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios.onGet(new RegExp('projects/*')).reply(200, mockProjectGetResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    vi.clearAllMocks();
  });

  it('renders the underlying form', () => {
    const { getByText } = setup();
    waitFor(() => {
      expect(viewProps?.loadingProject).toBeFalsy();
      expect(viewProps?.project).not.toBeNull();
    });
    expect(getByText(/Content Rendered/)).toBeVisible();
  });
});
