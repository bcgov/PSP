import { mockProjectGetResponse } from 'mocks/mockProjects';
import { rest, server } from 'mocks/msw/server';
import { getUserMock } from 'mocks/userMock';
import { Api_Project } from 'models/api/Project';
import { prettyFormatDate } from 'utils';
import { render, RenderOptions } from 'utils/test-utils';

import ProjectHeader, { IProjectHeaderProps } from './ProjectHeader';

type TestProps = Pick<IProjectHeaderProps, 'project'>;

describe('ProjectHeader component', () => {
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<ProjectHeader project={props.project} />, { ...renderOptions });

    return { ...utils };
  };

  let project: Api_Project;
  beforeEach(() => {
    project = mockProjectGetResponse();
    server.use(
      rest.get('/api/users/info/:userId', (req, res, ctx) => {
        return res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock()));
      }),
    );
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ project });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders values as expected', () => {
    const { getByText } = setup({ project: project });

    const createDateString = prettyFormatDate(project?.appCreateTimestamp);
    const updateDateString = prettyFormatDate(project?.appLastUpdateTimestamp);

    expect(getByText('771 Project Cool A')).toBeVisible();
    expect(getByText('USER_A')).toBeVisible();
    expect(getByText(createDateString)).toBeVisible();
    expect(getByText('USER_B')).toBeVisible();
    expect(getByText(updateDateString)).toBeVisible();
    expect(getByText('Active (AC)')).toBeVisible();
  });
});
