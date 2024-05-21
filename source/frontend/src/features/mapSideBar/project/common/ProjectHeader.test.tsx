import { server } from '@/mocks/msw/server';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { prettyFormatUTCDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import ProjectHeader, { IProjectHeaderProps } from './ProjectHeader';
import { http, HttpResponse } from 'msw';

type TestProps = Pick<IProjectHeaderProps, 'project'>;

describe('ProjectHeader component', () => {
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<ProjectHeader project={props.project} />, { ...renderOptions });

    return { ...utils };
  };

  let project: ApiGen_Concepts_Project;
  beforeEach(() => {
    project = mockProjectGetResponse();
    server.use(
      http.get('/api/users/info/:userId', () => {
        return HttpResponse.json(getUserMock(), { status: 200 });
      }),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ project });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders values as expected', () => {
    const { getByText } = setup({ project: project });

    const createDateString = prettyFormatUTCDate(project?.appCreateTimestamp);
    const updateDateString = prettyFormatUTCDate(project?.appLastUpdateTimestamp);

    expect(getByText('771 Project Cool A')).toBeVisible();
    expect(getByText('USER_A')).toBeVisible();
    expect(getByText(new RegExp(createDateString))).toBeVisible();
    expect(getByText('USER_B')).toBeVisible();
    expect(getByText(new RegExp(updateDateString))).toBeVisible();
    expect(getByText('ACTIVE (AC)')).toBeVisible();
  });
});
