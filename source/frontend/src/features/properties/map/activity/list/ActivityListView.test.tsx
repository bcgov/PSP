import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import Claims from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks';
import { mockActivitiesResponse } from 'mocks/mockActivities';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import {
  act,
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from 'utils/test-utils';

import { SideBarContextProvider } from '../../context/sidebarContext';
import ActivityListView, { IActivityListViewProps } from './ActivityListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockTemplateTypes = [
  { id: 1, activityTemplateTypeCode: { id: 'GENERAL', description: 'General' } },
];

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

describe('Activity List View', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IActivityListViewProps>) => {
    // render component under test
    const component = render(
      <SideBarContextProvider>
        <ActivityListView fileId={1} fileType={FileTypes.Research} />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [],
      },
    );

    return {
      ...component,
    };
  };

  beforeAll(() => {
    mockAxios.onGet(new RegExp(`/activities/templates`)).reply(200, mockTemplateTypes);
    mockAxios.onGet(new RegExp(`/activities/research/*`)).reply(200, mockActivitiesResponse());
    mockAxios.onAny().reply(200, {});
  });
  it('renders as expected', async () => {
    const { asFragment, getByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should have the activity type in the component', async () => {
    const { getByTestId, getByTitle } = setup({ claims: [Claims.ACTIVITY_ADD] });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(getByTestId('add-activity-type')).toBeInTheDocument();
  });

  it('should not display the view icon/link unless the user has the correct claim', async () => {
    const { getByTitle, queryAllByTitle, queryAllByText } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const icon = queryAllByTitle('View Activity');
    const link = queryAllByText('Survey')[0];
    expect(icon).toHaveLength(0);
    expect(link).not.toHaveClass('btn-link');
  });

  it('should allow an activity to be viewed by clicking the icon', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const link = getAllByTitle('View Activity')[0];
    act(() => userEvent.click(link));

    //note: extra '/' caused by the test path not containing an id.
    expect(history.location.pathname).toBe('//activity/1');
  });

  it('should allow an activity to be viewed by clicking the link', async () => {
    const { getByTitle, getAllByText } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const link = getAllByText('Survey')[0];

    act(() => userEvent.click(link));

    //note: extra '/' caused by the test path not containing an id.
    expect(history.location.pathname).toBe('//activity/1');
  });

  it('should not show the delete icon unless the user has the correct claim', async () => {
    const { getByTitle, queryAllByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const deleteButton = queryAllByTitle('Delete Activity');

    expect(deleteButton).toHaveLength(0);
  });

  it('should show the delete modal when an activity is deleted', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const deleteButton = getAllByTitle('Delete Activity')[0];

    act(() => userEvent.click(deleteButton));
    expect(await screen.findByText(/You have chosen to delete this activity/)).toBeVisible();
  });

  it('should delete the activity if the delete modal is confirmed', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const deleteButton = getAllByTitle('Delete Activity')[0];
    act(() => userEvent.click(deleteButton));
    expect(await screen.findByText(/You have chosen to delete this activity/)).toBeVisible();
    const continueButton = await screen.findByText('Continue');

    act(() => userEvent.click(continueButton));
    await waitFor(async () => {
      expect(mockAxios.history.delete[0].url).toBe('/activities/1');
    });
  });

  it('should hide the view activity screen if deleting the currently viewed activity', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const viewButton = getAllByTitle('View Activity')[0];
    await act(() => userEvent.click(viewButton));
    const deleteButton = getAllByTitle('Delete Activity')[0];
    await act(() => userEvent.click(deleteButton));
    expect(await screen.findByText(/You have chosen to delete this activity/)).toBeVisible();
    const continueButton = await screen.findByText('Continue');
    await act(() => userEvent.click(continueButton));

    expect(history.location.pathname).toBe('/');
  });

  it('should not hide the view activity screen if deleting an activity that is not being viewed', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const viewButton = getAllByTitle('View Activity')[1];
    act(() => userEvent.click(viewButton));
    const deleteButton = getAllByTitle('Delete Activity')[0];
    act(() => userEvent.click(deleteButton));
    expect(await screen.findByText(/You have chosen to delete this activity/)).toBeVisible();
    const continueButton = await screen.findByText('Continue');
    act(() => userEvent.click(continueButton));

    // //note, the extra '/' is an artifact of no research file id being present in the test logic.
    expect(history.location.pathname).toBe('//activity/2');
  });

  it('should close the delete modal if deletion is cancelled', async () => {
    const { getByTitle, getAllByTitle } = setup({
      claims: [Claims.ACTIVITY_VIEW, Claims.ACTIVITY_DELETE],
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const deleteButton = getAllByTitle('Delete Activity')[0];
    act(() => userEvent.click(deleteButton));
    expect(await screen.findByText(/You have chosen to delete this activity/)).toBeVisible();
    const cancelButton = await screen.findByText('Cancel');

    act(() => userEvent.click(cancelButton));
    await waitFor(async () => {
      expect(screen.queryByText(/You have chosen to delete this activity/)).toBeNull();
    });
  });
});
