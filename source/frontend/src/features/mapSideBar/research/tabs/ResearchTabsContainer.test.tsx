import { createMemoryHistory } from 'history';
import { act } from 'react-test-renderer';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import Claims from '@/constants/claims';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { SideBarContextProvider } from '../../context/sidebarContext';
import ResearchTabsContainer, { IResearchTabsContainerProps } from './ResearchTabsContainer';

// mock auth library
jest.mock('@react-keycloak/web');

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

const setIsEditing = jest.fn();
const history = createMemoryHistory();

describe('ResearchFileTabs component', () => {
  // render component under test
  const setup = (props: IResearchTabsContainerProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider>
        <ResearchTabsContainer
          researchFile={props.researchFile}
          setIsEditing={props.setIsEditing}
        />
      </SideBarContextProvider>,
      {
        useMockAuthentication: true,
        history,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    expect(editButton).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    act(() => {
      userEvent.click(editButton);
    });
    await waitFor(() => {
      expect(history.location.pathname).toBe('/documents');
    });
  });

  it('notes tab can be changed to', async () => {
    const { getAllByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setIsEditing,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const editButton = getAllByText('Notes')[0];
    act(() => {
      userEvent.click(editButton);
    });
    await waitFor(() => {
      expect(history.location.pathname).toBe('/notes');
    });
  });
});
