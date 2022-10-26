import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { prettyFormatDate } from 'utils';
import { render, RenderOptions } from 'utils/test-utils';

import ActivityHeader, { IActivityHeaderProps } from './ActivityHeader';

const mockAxios = new MockAdapter(axios);

describe('ActivityHeader component', () => {
  // render component under test
  const setup = (props: Partial<IActivityHeaderProps>, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ActivityHeader
        file={props.file ?? { ...getMockResearchFile(), id: 1 }}
        activity={props?.activity ?? { ...getMockActivityResponse(), id: 2 }}
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

  it('renders as expected', () => {
    const { asFragment } = setup({ file: getMockResearchFile() });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when an acquisition file is provided', async () => {
    const testAcquisitionFile = { ...mockAcquisitionFileResponse(), id: 1 };
    const testActivity = getMockActivityResponse();
    const { getByText } = setup({
      file: { ...mockAcquisitionFileResponse(), id: 1 },
      activity: testActivity,
    });

    expect(
      getByText(`${testAcquisitionFile.fileNumber as string} - ${testAcquisitionFile.fileName}`),
    ).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appLastUpdateTimestamp))).toBeVisible();
    expect(getByText(testActivity?.activityStatusTypeCode?.description ?? '')).toBeVisible();
  });

  it('renders as expected when a research file is provided', async () => {
    const testResearchFile = getMockResearchFile();
    const testActivity = getMockActivityResponse();
    const { getByText } = setup({ file: testResearchFile, activity: testActivity });

    expect(
      getByText(`${testResearchFile.fileNumber as string} - ${testResearchFile.fileName}`),
    ).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appLastUpdateTimestamp))).toBeVisible();
    expect(getByText(testActivity?.activityStatusTypeCode?.description ?? '')).toBeVisible();
  });
});
