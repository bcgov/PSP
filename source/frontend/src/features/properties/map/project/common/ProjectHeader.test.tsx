import { mockProjectGetResponse } from 'mocks/mockProjects';
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
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ project });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders values as expected', async () => {
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
