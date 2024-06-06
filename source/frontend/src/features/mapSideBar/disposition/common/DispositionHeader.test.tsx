import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { prettyFormatUTCDate } from '@/utils/dateUtils';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { http, HttpResponse } from 'msw';
import DispositionHeader, { IDispositionHeaderProps } from './DispositionHeader';

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

describe('DispositionHeader component', () => {
  // render component under test
  const setup = async (props: IDispositionHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <DispositionHeader
        dispositionFile={props.dispositionFile}
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

  it('renders as expected when a disposition file is provided', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = await setup({
      dispositionFile: testDispositionFile as unknown as ApiGen_Concepts_DispositionFile,
      lastUpdatedBy: {
        parentId: testDispositionFile.id || 0,
        appLastUpdateUserid: testDispositionFile.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testDispositionFile.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testDispositionFile.appLastUpdateTimestamp || '',
      },
    });

    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testDispositionFile.appLastUpdateTimestamp))),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText, getAllByText } = await setup({
      dispositionFile: testDispositionFile,
      lastUpdatedBy: null,
    });

    expect(getAllByText('File:')[0]).toBeVisible();
    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
  });

  it('renders the last-update-time when provided', async () => {
    const testDate = new Date().toISOString();
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = await setup({
      dispositionFile: testDispositionFile,
      lastUpdatedBy: {
        parentId: testDispositionFile.id || 0,
        appLastUpdateUserid: 'Test User Id',
        appLastUpdateUserGuid: 'TEST GUID',
        appLastUpdateTimestamp: testDate,
      },
    });

    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(getByText(new RegExp(prettyFormatUTCDate(testDate)))).toBeVisible();
  });

  it('renders the file status when provided', async () => {
    const testDispositionFile: ApiGen_Concepts_DispositionFile = {
      ...mockDispositionFileResponse(),
      fileStatusTypeCode: {
        id: 'TEST',
        description: 'mock file status',
        displayOrder: null,
        isDisabled: false,
      },
    };
    const { getByText } = await setup({
      dispositionFile: testDispositionFile,
      lastUpdatedBy: null,
    });

    expect(getByText(/mock file status/i)).toBeVisible();
  });
});
