import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { server } from '@/mocks/msw/server';
import { prettyFormatUTCDate } from '@/utils';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { getUserMock } from '@/mocks/user.mock';
import { http, HttpResponse } from 'msw';
import { AcquisitionHeader, IAcquisitionHeaderProps } from './AcquisitionHeader';

describe('AcquisitionHeader component', () => {
  // render component under test
  const setup = (props: IAcquisitionHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionHeader
        acquisitionFile={props.acquisitionFile}
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
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock())),
      http.get('/api/properties/:id/historicalNumbers', () => HttpResponse.json([])),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when no data is provided', async () => {
    const { asFragment } = setup({ lastUpdatedBy: null });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when an acquisition file is provided', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({
      acquisitionFile: testAcquisitionFile,
      lastUpdatedBy: {
        parentId: testAcquisitionFile.id || 0,
        appLastUpdateUserid: testAcquisitionFile.appLastUpdateUserid || '',
        appLastUpdateUserGuid: testAcquisitionFile.appLastUpdateUserGuid || '',
        appLastUpdateTimestamp: testAcquisitionFile.appLastUpdateTimestamp || '',
      },
    });
    await act(async () => {});

    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testAcquisitionFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testAcquisitionFile.appLastUpdateTimestamp))),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText, getAllByText } = setup({
      acquisitionFile: testAcquisitionFile,
      lastUpdatedBy: null,
    });
    await act(async () => {});

    expect(getAllByText('File:')[0]).toBeVisible();
    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
  });

  it('renders the file Project Number and name concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile, lastUpdatedBy: null });
    await act(async () => {});

    expect(getByText('Ministry project:')).toBeVisible();
    expect(
      getByText(
        "001 - Hwy 14 Expansion - Vancouver Island but it's really long so it can wrap around if it has to",
      ),
    ).toBeVisible();
  });

  it('renders the file Product code and description concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile, lastUpdatedBy: null });
    await act(async () => {});

    expect(getByText('Ministry product:')).toBeVisible();
    expect(getByText('00048 - MISCELLANEOUS CLAIMS')).toBeVisible();
  });

  it('renders the Product label when null', async () => {
    const { getByText, getByTestId } = setup({
      acquisitionFile: {
        ...mockAcquisitionFileResponse(),
        totalAllowableCompensation: 0,
        product: null,
        productId: null,
        projectId: null,
        fileChecklistItems: [],
      },
      lastUpdatedBy: null,
    });
    await act(async () => {});

    expect(getByText('Ministry product:')).toBeVisible();
    expect(getByTestId('acq-header-product-val')).toHaveTextContent('');
  });

  it('renders the last-update-time when provided', async () => {
    const testDate = new Date().toISOString();
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({
      acquisitionFile: testAcquisitionFile,
      lastUpdatedBy: {
        parentId: testAcquisitionFile.id || 0,
        appLastUpdateUserid: 'Test User Id',
        appLastUpdateUserGuid: 'TEST GUID',
        appLastUpdateTimestamp: testDate,
      },
    });
    await act(async () => {});

    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testAcquisitionFile.appCreateTimestamp))),
    ).toBeVisible();
    expect(getByText(new RegExp(prettyFormatUTCDate(testDate)))).toBeVisible();
  });
});
