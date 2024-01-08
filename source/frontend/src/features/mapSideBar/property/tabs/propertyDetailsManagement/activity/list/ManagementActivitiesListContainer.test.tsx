import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import {
  mockGetPropertyManagementActivityList,
  mockGetPropertyManagementActivityNotStarted,
} from '@/mocks/PropertyManagementActivity.mock';
import { act, render, RenderOptions, screen, userEvent, waitFor } from '@/utils/test-utils';

import PropertyManagementActivitiesListContainer, {
  IPropertyManagementActivitiesListContainerProps,
} from './ManagementActivitiesListContainer';
import { IManagementActivitiesListViewProps } from './ManagementActivitiesListView';

const history = createMemoryHistory();

let viewProps: IManagementActivitiesListViewProps;
const TestView: React.FC<IManagementActivitiesListViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockDeleteApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/usePropertyActivityRepository', () => ({
  usePropertyActivityRepository: () => {
    return {
      getActivities: mockGetApi,
      deleteActivity: mockDeleteApi,
    };
  },
}));

describe('ManagementActivitiesListContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertyManagementActivitiesListContainerProps>;
    } = {},
  ) => {
    const component = render(
      <PropertyManagementActivitiesListContainer
        propertyId={renderOptions?.props?.propertyId ?? 1}
        View={TestView}
      />,
      {
        history,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.MANAGEMENT_VIEW],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected', async () => {
    mockGetApi.execute.mockResolvedValue(mockGetPropertyManagementActivityList());
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());

    expect(fragment).toMatchSnapshot();
    expect(mockGetApi.execute).toHaveBeenCalledWith(1);
  });

  it('Delete activity calls displays delete modal', async () => {
    mockGetApi.execute.mockResolvedValue(mockGetPropertyManagementActivityList());
    await setup({});

    viewProps.onDelete(1);
    const modal = await screen.findByText('Confirm Delete');

    expect(modal).toBeVisible();
  });

  it('confirming delete modal sends delete call', async () => {
    mockGetApi.execute.mockResolvedValue([mockGetPropertyManagementActivityNotStarted()]);
    setup({
      claims: [Claims.MANAGEMENT_DELETE],
    });

    viewProps.onDelete(1);
    const continueButton = await screen.findByText('Yes');
    act(() => userEvent.click(continueButton));

    expect(mockDeleteApi.execute).toHaveBeenCalledTimes(1);
  });
});
