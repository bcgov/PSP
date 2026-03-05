import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { useResearchRepository } from '@/hooks/repositories/useResearchRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLastUpdatedBy } from '@/mocks/lastUpdatedBy.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getMockRepositoryObj, render, RenderOptions } from '@/utils/test-utils';

import { SideBarContextProvider, TypedFile } from '../context/sidebarContext';
import ResearchContainer, { IResearchContainerProps } from './ResearchContainer';
import ResearchView from './ResearchView';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onClose = vi.fn();

vi.mock('@/hooks/repositories/useResearchRepository');
vi.mocked(useResearchRepository, { partial: true }).mockReturnValue({
  getLastUpdatedBy: getMockRepositoryObj(mockLastUpdatedBy(1)),
});

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');
vi.mocked(useHistoricalNumberRepository, { partial: true }).mockReturnValue({
  getPropertyHistoricalNumbers: getMockRepositoryObj([]),
});

// Mock ConfirmNavigation to avoid Prompt issues in jsdom
vi.mock('@/components/common/ConfirmNavigation', () => {
  return {
    default: () => null,
  };
});

describe('ResearchContainer component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IResearchContainerProps> } & {
      context?: { file?: TypedFile };
    } = {},
  ) => {
    const utils = render(
      <SideBarContextProvider {...renderOptions?.context}>
        <ResearchContainer
          researchFileId={getMockResearchFile().id}
          onClose={onClose}
          View={ResearchView}
        />
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

    // wait for any useEffects to resolve and component to update with mocked data
    await act(async () => {});

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
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { getByTestId } = await setup();
    expect(document.body).toMatchSnapshot();
  });

  it('renders research details when file is in context', async () => {
    const typedFile: TypedFile = {
      ...getMockResearchFile(),
      fileType: ApiGen_CodeTypes_FileTypes.Research,
    };
    const { findByText } = await setup({ context: { file: typedFile } });
    await act(async () => {});
    expect(await findByText('File Summary')).toBeVisible();
  });

  it(`doesn't render research details when wrong file type is in context`, async () => {
    const typedFile: TypedFile = {
      ...mockAcquisitionFileResponse(),
      fileType: ApiGen_CodeTypes_FileTypes.Acquisition,
    };
    const { getByTestId } = await setup({ context: { file: typedFile } });
  });

  it('displays a properties pid if that is the only valid identifier', async () => {
    const { getByTestId, findByText } = await setup();
    expect(await findByText('123-456-789')).toBeVisible();
  });

  it('displays a properties pin if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { pin: 123456 } }]);

    const { getByTestId, findByText } = await setup();
    expect(await findByText('123456')).toBeVisible();
  });

  it('displays a properties plan number if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { planNumber: 'EPP92028' } }]);

    const { getByTestId, findByText } = await setup();
    expect(await findByText('EPP92028')).toBeVisible();
  });

  it('displays a properties lat/lng if that is the only valid identifier', async () => {
    mockAxios
      .onGet(`/researchFiles/${mockResearchFile?.id}/properties`)
      .reply(200, [{ id: 1, property: { latitude: 1, longitude: 2 } }]);

    const { getByTestId, findByText } = await setup();
    expect(await findByText('1.000000, 2.000000')).toBeVisible();
  });
});
