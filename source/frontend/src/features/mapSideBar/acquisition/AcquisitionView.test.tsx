import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Route } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { FileTypes } from '@/constants/index';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { prettyFormatDate } from '@/utils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import AcquisitionView, { IAcquisitionViewProps } from './AcquisitionView';

const mockAxios = new MockAdapter(axios);

// mock auth library
jest.mock('@react-keycloak/web');

const onClose = jest.fn();
const onSave = jest.fn();
const onCancel = jest.fn();
const onMenuChange = jest.fn();
const onSuccess = jest.fn();
const onCancelConfirm = jest.fn();
const onUpdateProperties = jest.fn();
const canRemove = jest.fn();
const setContainerState = jest.fn();
const setIsEditing = jest.fn();
const onEditFileProperties = jest.fn();

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

const DEFAULT_PROPS: IAcquisitionViewProps = {
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onSuccess,
  onCancelConfirm,
  onUpdateProperties,
  canRemove,
  isEditing: false,
  setIsEditing,
  onShowPropertySelector: onEditFileProperties,
  setContainerState,
  containerState: {
    acquisitionFile: mockAcquisitionFileResponse(),
    isEditing: false,
    selectedMenuIndex: 0,
    showConfirmModal: false,
    defaultFileTab: FileTabType.FILE_DETAILS,
    defaultPropertyTab: InventoryTabNames.property,
  },
  formikRef: React.createRef(),
};

const history = createMemoryHistory();

describe('AcquisitionView component', () => {
  // render component under test
  const setup = async (
    props: IAcquisitionViewProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider
        file={{
          ...mockAcquisitionFileResponse(),
          fileType: FileTypes.Acquisition,
        }}
      >
        <Route path="/mapview/sidebar/acquisition/:id">
          <AcquisitionView {...props} />
        </Route>
      </SideBarContextProvider>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        history,
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    history.replace(`/mapview/sidebar/acquisition/1`);
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    const testAcquisitionFile = mockAcquisitionFileResponse();

    expect(getByText('Acquisition File')).toBeVisible();

    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appLastUpdateTimestamp))).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = await setup();

    expect(getByText('Acquisition File')).toBeVisible();
    await waitFor(() => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = await setup(undefined, { claims: [Claims.ACQUISITION_EDIT] });
    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle } = await setup(undefined, { claims: [] });
    expect(queryByTitle('Change properties')).toBeNull();
  });

  it('should display the notes tab if the user has permissions', async () => {
    const { getAllByText } = await setup(undefined, { claims: [Claims.NOTE_VIEW] });
    expect(getAllByText(/Notes/)[0]).toBeVisible();
  });

  it('should not display the notes tab if the user does not have permissions', async () => {
    const { queryByText } = await setup(undefined, { claims: [] });
    expect(queryByText('Notes')).toBeNull();
  });

  it('should display the File Details tab by default', async () => {
    const { getByRole } = await act(() => setup());
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Selector according to routing', async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/selector`);
    const { getByRole } = await act(() => setup());
    const tab = getByRole('tab', { name: /Locate on Map/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/1`);
    const { getByRole } = await act(() => setup());
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });
});
