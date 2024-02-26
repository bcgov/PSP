import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FileTypes } from '@/constants';
import { Claims } from '@/constants/claims';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitForElementToBeRemoved } from '@/utils/test-utils';

import { SideBarContextProvider, TypedFile } from '../context/sidebarContext';
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
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IResearchContainerProps> } & {
      context?: { file?: TypedFile };
    } = {},
  ) => {
    const utils = render(
      <SideBarContextProvider {...renderOptions?.context}>
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

  const mockResearchFile = getMockResearchFile();

  beforeEach(() => {
    mockAxios.resetHistory();
    mockAxios.reset();
    mockAxios.onGet(/users\/info.*?/).reply(200, {});

    mockAxios.onGet(`/researchFiles/${mockResearchFile?.id}`).reply(200, {
      ...mockResearchFile,
      fileProperties: [{ id: 1 }],
    } as ApiGen_Concepts_ResearchFile);
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { pid: '123-456-789' } }]);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { getByTestId } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(document.body).toMatchSnapshot();
  });

  it('renders a spinner while loading', async () => {
    const { findByTestId } = setup();
    const spinner = await findByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders research details when file is in context', async () => {
    const typedFile: TypedFile = { ...getMockResearchFile(), fileType: FileTypes.Research };
    const { findByText } = setup({ context: { file: typedFile } });
    expect(await findByText('File Summary')).toBeVisible();
  });

  it(`doesn't render research details when wrong file type is in context`, async () => {
    const typedFile: TypedFile = {
      ...mockAcquisitionFileResponse(),
      fileType: FileTypes.Acquisition,
    };
    const { findByTestId } = setup({ context: { file: typedFile } });
    expect(await findByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('displays a properties pid if that is the only valid identifier', async () => {
    const { getByTestId, findByText } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(await findByText('123-456-789')).toBeVisible();
  });

  it('displays a properties pin if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { pin: 123456 } }]);

    const { getByTestId, findByText } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(await findByText('123456')).toBeVisible();
  });

  it('displays a properties plan number if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { planNumber: 'EPP92028' } }]);

    const { getByTestId, findByText } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(await findByText('EPP92028')).toBeVisible();
  });

  it('displays a properties lat/lng if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { latitude: 1, longitude: 2 } }]);

    const { getByTestId, findByText } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(await findByText('2.000000, 1.000000')).toBeVisible();
  });
});
