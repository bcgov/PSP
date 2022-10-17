import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import { mockLookups } from 'mocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { prettyFormatDate } from 'utils';
import { render, RenderOptions } from 'utils/test-utils';

import ActivityHeader, { IActivityHeaderProps } from './ActivityHeader';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
jest.mock('@react-keycloak/web');

describe('ActivityHeader component', () => {
  // render component under test
  const setup = (props: Partial<IActivityHeaderProps>, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <Formik onSubmit={noop} initialValues={props?.activity ?? getMockActivityResponse()}>
        <ActivityHeader
          editMode={props?.editMode ?? false}
          file={props.file ?? { ...getMockResearchFile(), id: 1 }}
          activity={props?.activity ?? { ...getMockActivityResponse(), id: 2 }}
        />
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        claims: renderOptions.claims ?? [Claims.ACTIVITY_EDIT],
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
    const { asFragment } = setup({ file: getMockResearchFile(), editMode: false });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when an acquisition file is provided', async () => {
    const testAcquisitionFile = { ...mockAcquisitionFileResponse(), id: 1 };
    const testActivity = getMockActivityResponse();
    const { getByText } = setup({
      file: { ...mockAcquisitionFileResponse(), id: 1 },
      activity: testActivity,
      editMode: false,
    });

    expect(
      getByText(`${testAcquisitionFile.fileNumber as string} - ${testAcquisitionFile.fileName}`),
    ).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appLastUpdateTimestamp))).toBeVisible();
    expect(getByText(testActivity?.activityStatusTypeCode?.description ?? '')).toBeVisible();
  });

  it('renders the activity status select', async () => {
    const testActivity = getMockActivityResponse();
    const { queryByLabelText } = setup({
      file: { ...mockAcquisitionFileResponse(), id: 1 },
      activity: testActivity,
      editMode: true,
    });
    expect(queryByLabelText('Status')).toBeNull();
  });

  it('populates status field as expected', async () => {
    const testActivity = getMockActivityResponse();
    const { getByRole } = setup({
      file: { ...mockAcquisitionFileResponse(), id: 1 },
      activity: testActivity,
      editMode: true,
    });
    expect((getByRole('option', { name: 'In Progress' }) as any).selected).toBe(true);
  });
  it('renders as expected when a research file is provided', async () => {
    const testResearchFile = getMockResearchFile();
    const testActivity = getMockActivityResponse();
    const { getByText } = setup({
      file: testResearchFile,
      activity: testActivity,
      editMode: false,
    });

    expect(
      getByText(`${testResearchFile.fileNumber as string} - ${testResearchFile.fileName}`),
    ).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testActivity.appLastUpdateTimestamp))).toBeVisible();
    expect(getByText(testActivity?.activityStatusTypeCode?.description ?? '')).toBeVisible();
  });
});
