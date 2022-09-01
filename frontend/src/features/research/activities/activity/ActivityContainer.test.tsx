import { waitForElementToBeRemoved } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockLookups } from 'mocks';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import ActivityContainer, { IActivityContainerProps } from './ActivityContainer';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');
const onClose = jest.fn();
const mockAxios = new MockAdapter(axios);

describe('Activity Container', () => {
  const setup = (renderOptions?: RenderOptions & IActivityContainerProps) => {
    // render component under test
    const component = render(
      <ActivityContainer onClose={onClose} activityId={renderOptions?.activityId ?? 1} />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...component,
    };
  };

  beforeAll(() => {
    mockAxios.onGet(new RegExp(`activities/1`)).reply(200, getMockActivityResponse());
    mockAxios.onAny().reply(200, {});
  });
  it('renders as expected', async () => {
    const { asFragment, getByTestId } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays the activity title based on its type', async () => {
    const { getByTestId, getByText } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(getByText('Activity - General')).toBeVisible();
  });

  it('allows the activity window to be closed', async () => {
    const { getByTestId, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    const closeButton = getByTitle('close');
    userEvent.click(closeButton);
    expect(onClose).toHaveBeenCalled();
  });
});
