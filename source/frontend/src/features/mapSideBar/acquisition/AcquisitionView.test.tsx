import { createMemoryHistory } from 'history';
import React from 'react';
import { Route } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { FileTypes } from '@/constants/index';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockApiPerson } from '@/mocks/filterData.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLastUpdatedBy } from '@/mocks/lastUpdatedBy.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { rest, server } from '@/mocks/msw/server';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { prettyFormatUTCDate } from '@/utils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import AcquisitionView, { IAcquisitionViewProps } from './AcquisitionView';

// mock auth library
jest.mock('@react-keycloak/web');

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
jest.mock('@/hooks/repositories/useComposedProperties', () => {
  return {
    useComposedProperties: jest.fn().mockResolvedValue({ apiWrapper: { response: {} } }),
    PROPERTY_TYPES: {},
  };
});
jest.mock('@/features/mapSideBar/hooks/usePropertyDetails', () => {
  return {
    usePropertyDetails: jest.fn(),
  };
});

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
    isEditing: false,
    selectedMenuIndex: 0,
    defaultFileTab: FileTabType.FILE_DETAILS,
    defaultPropertyTab: InventoryTabNames.property,
  },
  formikRef: React.createRef(),
  isFormValid: true,
  error: undefined,
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
        lastUpdatedBy={{
          ...mockLastUpdatedBy(1),
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
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

    history.replace(`/mapview/sidebar/acquisition/1`);
    server.use(
      rest.get('/api/users/info/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock())),
      ),
      rest.get('/api/notes/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockNotesResponse())),
      ),
      rest.get('/api/acquisitionfiles/:id/owners', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockAcquisitionFileOwnersResponse())),
      ),
      rest.get('/api/persons/concept/:id', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockApiPerson)),
      ),
      rest.get('/api/acquisitionfiles/:id/properties', (req, res, ctx) =>
        res(
          ctx.delay(500),
          ctx.status(200),
          ctx.json(mockAcquisitionFileResponse().fileProperties),
        ),
      ),
      rest.get('/api/acquisitionfiles/:id/interestholders', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getMockApiInterestHolders())),
      ),
    );
  });

  afterEach(() => {
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
    expect(getByText(prettyFormatUTCDate(testAcquisitionFile.appCreateTimestamp))).toBeVisible();
    expect(
      getByText(prettyFormatUTCDate(mockLastUpdatedBy(1).appLastUpdateTimestamp)),
    ).toBeVisible();
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
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should show a toast and redirect to the File Details page when accessing a non-existing property index`, async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/99999`);
    const { getByRole, findByText } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
    // toast
    expect(await findByText(/Could not find property in the file/i)).toBeVisible();
  });

  it('should display the Property Selector according to routing', async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/selector`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Locate on Map/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/1`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the File Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/acquisition/1/blahblahtab?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the Property Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/1/unknownTabWhatIsThis?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display an error message when the error prop is set.`, async () => {
    const { getByText } = await setup({ ...DEFAULT_PROPS, error: {} } as any);
    expect(
      getByText(
        'Failed to load Acquisition File. Check the detailed error in the top right for more details.',
      ),
    ).toBeVisible();
  });
});
