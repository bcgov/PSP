import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { prettyFormatUTCDate } from '@/utils/dateUtils';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { http, HttpResponse } from 'msw';
import ManagementHeader, { IManagementHeaderProps } from './ManagementHeader';

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');
vi.mocked(useHistoricalNumberRepository).mockReturnValue({
  getPropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
  updatePropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
});

describe('ManagementHeader component', () => {
  // render component under test
  const setup = async (props: IManagementHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ManagementHeader
        managementFile={props.managementFile}
        lastUpdatedBy={props.lastUpdatedBy}
      />,
      {
        ...renderOptions,
      },
    );

    await act(async () => {});

    return { ...utils };
  };

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock(), { status: 200 })),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when no data is provided', async () => {
    const { asFragment } = await setup({ lastUpdatedBy: null });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when a management file is provided', async () => {
    const testManagementFile = mockManagementFileResponse();
    const { getByText } = await setup({
      managementFile: testManagementFile as unknown as ApiGen_Concepts_ManagementFile,
      lastUpdatedBy: {
        parentId: testManagementFile.id || 0,
        appLastUpdateUserid: testManagementFile.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testManagementFile.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testManagementFile.appLastUpdateTimestamp || '',
      },
    });

    expect(getByText(testManagementFile.fileName, { exact: false })).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testManagementFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testManagementFile.appLastUpdateTimestamp))),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testManagementFile = mockManagementFileResponse();
    const { getByText, getAllByText } = await setup({
      managementFile: testManagementFile,
      lastUpdatedBy: null,
    });

    expect(getAllByText('File:')[0]).toBeVisible();
    expect(getByText(testManagementFile.fileName, { exact: false })).toBeVisible();
  });

  it('renders the last-update-time when provided', async () => {
    const testDate = new Date().toISOString();
    const testManagementFile = mockManagementFileResponse();
    const { getByText } = await setup({
      managementFile: testManagementFile,
      lastUpdatedBy: {
        parentId: testManagementFile.id || 0,
        appLastUpdateUserid: 'Test User Id',
        appLastUpdateUserGuid: 'TEST GUID',
        appLastUpdateTimestamp: testDate,
      },
    });

    expect(getByText(testManagementFile.fileName, { exact: false })).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testManagementFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(getByText(new RegExp(prettyFormatUTCDate(testDate)))).toBeVisible();
  });

  it('renders the file status when provided', async () => {
    const testManagementFile: ApiGen_Concepts_ManagementFile = {
      ...mockManagementFileResponse(),
      fileStatusTypeCode: {
        id: 'TEST',
        description: 'mock file status',
        displayOrder: null,
        isDisabled: false,
      },
    };
    const { getByText } = await setup({
      managementFile: testManagementFile,
      lastUpdatedBy: null,
    });

    expect(getByText(/mock file status/i)).toBeVisible();
  });
});
