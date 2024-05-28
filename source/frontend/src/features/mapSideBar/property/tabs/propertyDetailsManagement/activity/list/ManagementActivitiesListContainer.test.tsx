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
  execute: vi.fn(),
  loading: false,
};

const mockDeleteApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyActivityRepository', () => ({
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
    vi.resetAllMocks();
  });

  it('renders as expected', async () => {
    mockGetApi.execute.mockResolvedValue(mockGetPropertyManagementActivityList());
    const { asFragment } = await setup({});
    await act(async () => {});
    const fragment = await waitFor(() => asFragment());

    expect(fragment).toMatchSnapshot();
    expect(mockGetApi.execute).toHaveBeenCalledWith(1);
  });

  it('Delete activity calls displays delete modal', async () => {
    mockGetApi.execute.mockResolvedValue(mockGetPropertyManagementActivityList());
    await setup({});

    await act(async () => {
      viewProps.onDelete(1);
    });

    const modal = await screen.findByText('Confirm Delete');
    expect(modal).toBeVisible();
  });

  it('confirming delete modal sends delete call', async () => {
    mockGetApi.execute.mockResolvedValue([mockGetPropertyManagementActivityNotStarted()]);
    setup({
      claims: [Claims.MANAGEMENT_DELETE],
    });

    await act(async () => {
      viewProps.onDelete(1);
    });
    const continueButton = await screen.findByText('Yes');
    await act(async () => userEvent.click(continueButton));

    expect(mockDeleteApi.execute).toHaveBeenCalledTimes(1);
  });
});
