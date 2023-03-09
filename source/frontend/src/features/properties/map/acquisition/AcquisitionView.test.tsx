import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/index';
import { InventoryTabNames } from 'features/mapSideBar/tabs/InventoryTabs';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { mockNotesResponse } from 'mocks/mockNoteResponses';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { prettyFormatDate } from 'utils';
import { act, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
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
  setContainerState,
  containerState: {
    acquisitionFile: mockAcquisitionFileResponse(),
    isEditing: false,
    selectedMenuIndex: 0,
    showConfirmModal: false,
    defaultPropertyTab: InventoryTabNames.property,
  },
  formikRef: React.createRef(),
};

describe('AcquisitionView component', () => {
  // render component under test
  const setup = (
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
        <AcquisitionView {...props} />
      </SideBarContextProvider>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();

    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    const testAcquisitionFile = mockAcquisitionFileResponse();

    expect(getByText('Acquisition File')).toBeVisible();

    expect(getByText('1-12345-01 - Test ACQ File')).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appCreateTimestamp))).toBeVisible();
    expect(getByText(prettyFormatDate(testAcquisitionFile.appLastUpdateTimestamp))).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = setup();

    expect(getByText('Acquisition File')).toBeVisible();
    await waitFor(() => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = setup(undefined, { claims: [Claims.ACQUISITION_EDIT] });

    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle } = setup(undefined, { claims: [] });

    expect(queryByTitle('Change properties')).toBeNull();
  });

  it('should display the notes tab if the user has permissions', async () => {
    const { getAllByText } = setup(undefined, { claims: [Claims.NOTE_VIEW] });
    await act(async () => {
      expect(getAllByText(/Notes/)[0]).toBeVisible();
    });
  });

  it('should not display the notes tab if the user does not have permissions', async () => {
    const { queryByText } = setup(undefined, { claims: [] });

    expect(queryByText('Notes')).toBeNull();
  });
});
