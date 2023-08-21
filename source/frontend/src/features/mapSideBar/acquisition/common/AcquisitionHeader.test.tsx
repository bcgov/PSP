import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { prettyFormatUTCDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import { AcquisitionHeader, IAcquisitionHeaderProps } from './AcquisitionHeader';

const mockAxios = new MockAdapter(axios);

describe('AcquisitionHeader component', () => {
  // render component under test
  const setup = (props: IAcquisitionHeaderProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<AcquisitionHeader acquisitionFile={props.acquisitionFile} />, {
      ...renderOptions,
    });

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
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when an acquisition file is provided', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile });

    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testAcquisitionFile.appCreateTimestamp))).toBeVisible();
    expect(
      getByText(prettyFormatUTCDate(testAcquisitionFile.appLastUpdateTimestamp)),
    ).toBeVisible();
  });

  it('renders the file number and name concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile });

    expect(getByText('File:')).toBeVisible();
    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
  });

  it('renders the file Project Number and name concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile });

    expect(getByText('Ministry project:')).toBeVisible();
    expect(
      getByText(
        "001 - Hwy 14 Expansion - Vancouver Island but it's really long so it can wrap around if it has to",
      ),
    ).toBeVisible();
  });

  it('renders the file Product code and description concatenated', async () => {
    const testAcquisitionFile = mockAcquisitionFileResponse();
    const { getByText } = setup({ acquisitionFile: testAcquisitionFile });

    expect(getByText('Ministry product:')).toBeVisible();
    expect(getByText('00048 - MISCELLANEOUS CLAIMS')).toBeVisible();
  });

  it('renders the Product label when null', async () => {
    const { getByText, getByTestId } = setup({
      acquisitionFile: {
        ...mockAcquisitionFileResponse,
        totalAllowableCompensation: 0,
        product: undefined,
        productId: null,
        projectId: null,
      },
    });

    expect(getByText('Ministry product:')).toBeVisible();
    expect(getByTestId('acq-header-product-val')).toHaveTextContent('');
  });
});
