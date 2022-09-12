import { waitForElementToBeRemoved } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileTypes } from 'constants/fileTypes';
import { SideBarContextProvider } from 'features/properties/map/context/sidebarContext';
import { mockLookups } from 'mocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { Api_Activity } from 'models/api/Activity';
import { Api_File } from 'models/api/File';
import { act } from 'react-dom/test-utils';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import { IActivityTrayProps } from '../ActivityTray/ActivityTray';
import ActivityContainer, { IActivityContainerProps } from './ActivityContainer';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');
const onClose = jest.fn();
const mockAxios = new MockAdapter(axios);
let viewProps: IActivityTrayProps | undefined;

const ActivityView = (props: IActivityTrayProps) => {
  viewProps = props;
  return (
    <>
      <LoadingBackdrop show={props.loading} />
    </>
  );
};

describe('Activity Container', () => {
  const setup = (
    renderOptions?: RenderOptions & Partial<IActivityContainerProps> & { file?: Api_File },
  ) => {
    // render component under test
    const component = render(
      <SideBarContextProvider
        file={{
          ...(renderOptions?.file ?? mockAcquisitionFileResponse()),
          fileType: FileTypes.Acquisition,
        }}
      >
        <ActivityContainer
          View={ActivityView}
          onClose={onClose}
          activityId={renderOptions?.activityId ?? 1}
        />
      </SideBarContextProvider>,
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
    mockAxios
      .onPut(new RegExp(`activities/2`))
      .reply(200, { ...getMockActivityResponse(), rowVersion: 2 });
    mockAxios.onAny().reply(200, {});
    jest.restoreAllMocks();
    viewProps = undefined;
  });

  it('throws an error if the file is not set', async () => {
    expect(() => setup({ file: { id: undefined } })).toThrowError(
      'Unable to determine id of current file.',
    );
  });

  it('fetches the activity and passes it to the view', async () => {
    const { getByTestId } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(viewProps?.activity).toStrictEqual(getMockActivityResponse());
  });

  it('calls update activity and returns the response', async () => {
    const { getByTestId } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    let updatedActivity: Api_Activity | undefined;
    await act(async () => {
      updatedActivity = await viewProps?.onSave(getMockActivityResponse());
    });
    expect(await waitFor(() => viewProps?.updateLoading)).toBeFalsy();
    expect(mockAxios.history.put[0].data).toStrictEqual(
      JSON.stringify({
        ...getMockActivityResponse(),
      }),
    );
    expect(updatedActivity?.rowVersion).toBe(2);
  });
});
