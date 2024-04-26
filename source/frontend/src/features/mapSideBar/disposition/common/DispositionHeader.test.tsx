import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { rest, server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { prettyFormatUTCDate } from '@/utils/dateUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import DispositionHeader, { IDispositionHeaderProps } from './DispositionHeader';

describe('DispositionHeader component', () => {
  // render component under test
  const setup = (props: IDispositionHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <DispositionHeader
        dispositionFile={props.dispositionFile}
        lastUpdatedBy={props.lastUpdatedBy}
      />,
      {
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    server.use(
      rest.get('/api/users/info/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock())),
      ),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when no data is provided', () => {
    const { asFragment } = setup({ lastUpdatedBy: null });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when a disposition file is provided', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = setup({
      dispositionFile: testDispositionFile as unknown as ApiGen_Concepts_DispositionFile,
      lastUpdatedBy: {
        parentId: testDispositionFile.id || 0,
        appLastUpdateUserid: testDispositionFile.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testDispositionFile.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testDispositionFile.appLastUpdateTimestamp || '',
      },
    });

    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
    expect(
      getByText(prettyFormatUTCDate(testDispositionFile.appLastUpdateTimestamp)),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = setup({ dispositionFile: testDispositionFile, lastUpdatedBy: null });

    expect(getByText('File:')).toBeVisible();
    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
  });

  it('renders the last-update-time when provided', async () => {
    const testDate = new Date().toISOString();
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = setup({
      dispositionFile: testDispositionFile,
      lastUpdatedBy: {
        parentId: testDispositionFile.id || 0,
        appLastUpdateUserid: 'Test User Id',
        appLastUpdateUserGuid: 'TEST GUID',
        appLastUpdateTimestamp: testDate,
      },
    });

    expect(getByText(/FILE_NUMBER 3A8F46B/)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDate))).toBeVisible();
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
    const { getByText } = setup({ dispositionFile: testDispositionFile, lastUpdatedBy: null });

    expect(getByText('Status:')).toBeVisible();
    expect(getByText(/mock file status/i)).toBeVisible();
  });
});
