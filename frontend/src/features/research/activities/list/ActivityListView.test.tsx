import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockLookups } from 'mocks';
import { mockActivitiesResponse } from 'mocks/mockActivities';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import ActivityListView, { IActivityListViewProps } from './ActivityListView';

const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockTemplateTypes = [
  { id: 1, activityTemplateTypeCode: { id: 'GENERAL', description: 'General' } },
];

describe('Activity List View', () => {
  const setup = (renderOptions?: RenderOptions & IActivityListViewProps) => {
    // render component under test
    const component = render(<ActivityListView fileId={1} />, {
      ...renderOptions,
      store: storeState,
    });

    return {
      ...component,
    };
  };

  beforeAll(() => {
    mockAxios
      .onGet(new RegExp(`/researchFiles/activity-templates/*`))
      .reply(200, mockTemplateTypes);
    mockAxios.onGet(new RegExp(`/researchFiles/1/activities/*`)).reply(200, mockActivitiesResponse);
  });
  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should have the Activity type in the component', () => {
    const { getByTestId } = setup();
    expect(getByTestId('add-activity-type')).toBeInTheDocument();
  });
});
