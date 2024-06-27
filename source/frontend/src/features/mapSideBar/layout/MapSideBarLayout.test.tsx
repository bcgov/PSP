import Claims from '@/constants/claims';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { toTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import Roles from '@/constants/roles';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import MapSideBarLayout, { IMapSideBarLayoutProps } from './MapSideBarLayout';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { SideBarType } from '@/components/common/mapFSM/machineDefinition/types';

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

// mock auth library

const onClose = vi.fn();
const toggleSidebarDisplay = vi.fn();

describe('MapSideBarLayout component', () => {
  // render component under test
  const setup = (
    props: Partial<IMapSideBarLayoutProps & { collapsed: boolean }>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <MapSideBarLayout
        title={props.title}
        icon={props.icon}
        showCloseButton={props.showCloseButton}
        onClose={onClose}
      />,
      {
        useMockAuthentication: true,
        mockMapMachine: {
          ...mapMachineBaseMock,
          mapSideBarViewState: {
            isCollapsed: props.collapsed,
            type: SideBarType.NOT_DEFINED,
            isFullWidth: false,
            isOpen: true,
          },
          toggleSidebarDisplay: toggleSidebarDisplay,
        },
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {});

  afterEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      title: 'title',
      icon: 'icon',
    });
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls close method when closed', async () => {
    const { getByTitle } = setup({
      title: 'title',
      icon: 'icon',
      collapsed: true,
      showCloseButton: true,
    });
    await waitForEffects();
    await act(async () => userEvent.click(getByTitle('close')));
    expect(onClose).toHaveBeenCalled();
  });

  it('displays as expected when collapsed', async () => {
    const { queryByText } = setup({
      title: 'title',
      icon: 'icon',
      collapsed: true,
    });
    await waitForEffects();
    expect(queryByText('title')).toBeNull();
  });

  it('expands the sidebar when collapsed', async () => {
    const { getByTitle } = setup({
      title: 'title',
      icon: 'icon',
      collapsed: true,
    });
    await waitForEffects();
    await act(async () => userEvent.click(getByTitle('expand')));
    expect(toggleSidebarDisplay).toHaveBeenCalled();
  });

  it('collapses the sidebar when expanded', async () => {
    const { getByTitle } = setup({
      title: 'title',
      icon: 'icon',
      collapsed: false,
    });
    await waitForEffects();
    await act(async () => userEvent.click(getByTitle('collapse')));
    expect(toggleSidebarDisplay).toHaveBeenCalled();
  });
});
