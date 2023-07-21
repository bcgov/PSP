import { mockProjectGetResponse } from '@/mocks/projects.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import ProjectSummaryView, { IProjectSummaryViewProps } from './ProjectSummaryView';

// mock auth library
jest.mock('@react-keycloak/web');
const onEdit = jest.fn();

describe('ProjectSummaryView component', () => {
  // render component under test
  const setup = (props: IProjectSummaryViewProps, renderOptions: RenderOptions = {}) => {
    const utils = render(<ProjectSummaryView project={props.project} onEdit={props.onEdit} />, {
      useMockAuthentication: true,
      ...renderOptions,
    });

    return { ...utils };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup({
      project: mockProjectGetResponse(),
      onEdit,
    });
    expect(asFragment()).toMatchSnapshot();
  });
});
