import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { Api_ResearchFile } from '@/models/api/ResearchFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitForElementToBeRemoved } from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import ResearchContainer, { IResearchContainerProps } from './ResearchContainer';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
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

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

const onClose = jest.fn();

describe('ResearchContainer component', () => {
  // render component under test
  const setup = (props: Partial<IResearchContainerProps>, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider>
        <ResearchContainer researchFileId={getMockResearchFile().id as number} onClose={onClose} />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        history,
        claims: [Claims.RESEARCH_VIEW, Claims.RESEARCH_ADD],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockAxios.reset();
    mockAxios.onGet(/users\/info.*?/).reply(200, {});
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const mockResearchFile = getMockResearchFile();
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, getMockResearchFile());
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}/properties`).reply(200, []);
    const { getByTestId } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(document.body).toMatchSnapshot();
  });

  it('displays a properties pid if that is the only valid identifier', async () => {
    const mockResearchFile = getMockResearchFile();
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, {
      ...mockResearchFile,
      fileProperties: [{ id: 1 }],
    } as Api_ResearchFile);
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { pid: '123-456-789' } }]);
    const { getByTestId, findByText } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(await findByText('123-456-789')).toBeVisible();
  });

  it('displays a properties pin if that is the only valid identifier', async () => {
    const mockResearchFile = getMockResearchFile();
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, {
      ...mockResearchFile,
      fileProperties: [{ id: 1 }],
    } as Api_ResearchFile);
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { pin: 123456 } }]);
    const { getByTestId, findByText } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(await findByText('123456')).toBeVisible();
  });

  it('displays a properties plan number if that is the only valid identifier', async () => {
    const mockResearchFile = getMockResearchFile();
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, {
      ...mockResearchFile,
      fileProperties: [{ id: 1 }],
    } as Api_ResearchFile);
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { planNumber: 'EPP92028' } }]);
    const { getByTestId, findByText } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(await findByText('EPP92028')).toBeVisible();
  });

  it('displays a properties lat/lng if that is the only valid identifier', async () => {
    const mockResearchFile = getMockResearchFile();
    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, {
      ...mockResearchFile,
      fileProperties: [{ id: 1 }],
    } as Api_ResearchFile);
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { latitude: 1, longitude: 2 } }]);
    const { getByTestId, findByText } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(await findByText('2.000000, 1.000000')).toBeVisible();
  });
});
