import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { prettyFormatUTCDate } from '@/utils/dateUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import DispositionHeader, { IDispositionHeaderProps } from './DispositionHeader';

const mockAxios = new MockAdapter(axios);

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
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    jest.clearAllMocks();
  });

  it('renders as expected when no data is provided', () => {
    const { asFragment } = setup({ lastUpdatedBy: null });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when an acquisition file is provided', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = setup({
      dispositionFile: testDispositionFile,
      lastUpdatedBy: {
        parentId: testDispositionFile.id || 0,
        appLastUpdateUserid: testDispositionFile.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testDispositionFile.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testDispositionFile.appLastUpdateTimestamp || '',
      },
    });

    expect(getByText(/FILE_NUMBER 3A8F46B/g)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
    expect(
      getByText(prettyFormatUTCDate(testDispositionFile.appLastUpdateTimestamp)),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testDispositionFile = mockDispositionFileResponse();
    const { getByText } = setup({ dispositionFile: testDispositionFile, lastUpdatedBy: null });

    expect(getByText('File:')).toBeVisible();
    expect(getByText(/FILE_NUMBER 3A8F46B/g)).toBeVisible();
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

    expect(getByText(/FILE_NUMBER 3A8F46B/g)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDate))).toBeVisible();
  });
});
