import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/index.mock';
import {
  mockGetPropertyManagementActivityList,
  mockGetPropertyManagementActivityNotStarted,
} from '@/mocks/PropertyManagementActivity.mock';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fireEvent, render, RenderOptions, waitFor } from '@/utils/test-utils';

import ManagementActivitiesListView, {
  IManagementActivitiesListViewProps,
} from './ManagementActivitiesListView';
import { PropertyActivityRow } from './models/PropertyActivityRow';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

const onCreate = vi.fn();
const onView = vi.fn();
const onDelete = vi.fn();

describe('Activities list view', () => {
  const setup = async (
    renderOptions?: RenderOptions & Partial<IManagementActivitiesListViewProps>,
  ) => {
    // render component under test
    const component = render(
      <ManagementActivitiesListView
        isLoading={renderOptions?.isLoading ?? false}
        propertyActivities={renderOptions?.propertyActivities ?? []}
        onCreate={onCreate}
        onDelete={onDelete}
        onView={onView}
      />,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [Claims.MANAGEMENT_VIEW],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const apiModelList = mockGetPropertyManagementActivityList();
    const { asFragment } = await setup({
      claims: [Claims.MANAGEMENT_VIEW],
      propertyActivities: [
        ...apiModelList.map((x: ApiGen_Concepts_PropertyActivity) =>
          PropertyActivityRow.fromApi(x),
        ),
      ],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('Hides the delete activity button when user has only read claims', async () => {
    const apiModelList = mockGetPropertyManagementActivityList();
    const { queryByTestId } = await setup({
      claims: [Claims.MANAGEMENT_VIEW],
      propertyActivities: [
        ...apiModelList.map((x: ApiGen_Concepts_PropertyActivity) =>
          PropertyActivityRow.fromApi(x),
        ),
      ],
    });

    const firstRowDelete = queryByTestId(`activity-delete-${apiModelList[0].id}`);
    expect(firstRowDelete).not.toBeInTheDocument();
  });

  it('Shows the delete activity button when user has delete claims', async () => {
    const propertyActivity = mockGetPropertyManagementActivityNotStarted();
    const { queryByTestId } = await setup({
      claims: [Claims.MANAGEMENT_VIEW, Claims.MANAGEMENT_DELETE],
      propertyActivities: [PropertyActivityRow.fromApi(propertyActivity)],
    });

    const firstRowDelete = queryByTestId(`activity-delete-${propertyActivity.id}`);
    expect(firstRowDelete).toBeInTheDocument();
  });

  it('Hides the delete activity button when activit has already started', async () => {
    const apiModelList = mockGetPropertyManagementActivityList();
    const { queryByTestId } = await setup({
      claims: [Claims.MANAGEMENT_VIEW, Claims.MANAGEMENT_DELETE],
      propertyActivities: [
        ...apiModelList.map((x: ApiGen_Concepts_PropertyActivity) =>
          PropertyActivityRow.fromApi(x),
        ),
      ],
    });
    const firstRowDelete = queryByTestId(`activity-delete-${apiModelList[0].id}`);
    expect(firstRowDelete).not.toBeInTheDocument();
  });

  it('Calls the delete function when clicked Delete Button', async () => {
    const propertyActivity = mockGetPropertyManagementActivityNotStarted();
    const { queryByTestId } = await setup({
      claims: [Claims.MANAGEMENT_VIEW, Claims.MANAGEMENT_DELETE],
      propertyActivities: [PropertyActivityRow.fromApi(propertyActivity)],
    });

    const firstRowDelete = queryByTestId(`activity-delete-${propertyActivity.id}`);
    expect(firstRowDelete).toBeInTheDocument();

    await waitFor(() => {
      fireEvent.click(firstRowDelete as HTMLElement);
    });

    expect(onDelete).toHaveBeenCalledWith(1);
  });
});
