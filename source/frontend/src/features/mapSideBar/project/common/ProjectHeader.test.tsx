import { rest, server } from '@/mocks/msw/server';
import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { prettyFormatUTCDate } from '@/utils';
import { render, RenderOptions } from '@/utils/test-utils';

import ProjectHeader, { IProjectHeaderProps } from './ProjectHeader';

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
      rest.get('/api/users/info/:userId', (req, res, ctx) => {
        return res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock()));
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
    expect(getByText(createDateString)).toBeVisible();
    expect(getByText('USER_B')).toBeVisible();
    expect(getByText(updateDateString)).toBeVisible();
    expect(getByText('Active (AC)')).toBeVisible();
  });
});
