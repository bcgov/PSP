import { waitForElementToBeRemoved } from '@testing-library/react';
import { screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileTypes } from 'constants/fileTypes';
import { SideBarContextProvider } from 'features/properties/map/context/sidebarContext';
import { mockLookups } from 'mocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { getMockApiPropertyFiles } from 'mocks/mockProperties';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Activity } from 'models/api/Activity';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import { act } from 'react-dom/test-utils';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

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
    renderOptions?: RenderOptions &
      Partial<IActivityContainerProps> & {
        file?: Partial<Api_ResearchFile> | Partial<Api_AcquisitionFile>;
        fileType?: FileTypes;
      },
  ) => {
    // render component under test
    const component = render(
      <SideBarContextProvider
        file={{
          ...(renderOptions?.file ?? mockAcquisitionFileResponse()),
          fileType: renderOptions?.fileType ?? FileTypes.Acquisition,
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
    mockAxios.onGet(new RegExp(`activities/1`)).reply(200, {
      ...getMockActivityResponse(),
    });
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

  it('fetches the activity for a research file and passes it to the view', async () => {
    const { getByTestId } = setup({ fileType: FileTypes.Research });
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    expect(viewProps?.activity).toStrictEqual(getMockActivityResponse());
  });

  it('displays the select properties modal for an acquisition file', async () => {
    const { getByTestId } = setup({
      file: { ...mockAcquisitionFileResponse(), fileProperties: getMockApiPropertyFiles() },
    });
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    await act(async () => {
      viewProps?.onEditRelatedProperties();
    });

    expect(screen.getByText('Related properties')).toBeVisible();
    const checkBoxes = screen.getAllByTitle('Toggle Row Selected');
    checkBoxes.forEach(checkBox => {
      expect(checkBox).toBeChecked();
    });
  });

  it('displays the select properties modal for a research file', async () => {
    const { getByTestId } = setup({
      file: { ...getMockResearchFile(), fileProperties: getMockApiPropertyFiles() },
      fileType: FileTypes.Research,
    });
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    await act(async () => {
      viewProps?.onEditRelatedProperties();
    });

    expect(screen.getByText('Related properties')).toBeVisible();
    const checkBoxes = screen.getAllByTitle('Toggle Row Selected');
    checkBoxes.forEach(checkBox => {
      expect(checkBox).toBeChecked();
    });
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
