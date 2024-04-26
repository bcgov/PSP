import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

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
import { rest, server } from '@/mocks/msw/server';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { prettyFormatUTCDate } from '@/utils';
import { RenderOptions, act, render, userEvent, waitFor } from '@/utils/test-utils';

import { getMockApiTakes } from '@/mocks/takes.mock';
import { SideBarContextProvider } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import AcquisitionView, { IAcquisitionViewProps } from './AcquisitionView';
import { createRef } from 'react';

// mock auth library

vi.mock('@/hooks/repositories/useComposedProperties', () => {
  return {
    useComposedProperties: vi.fn().mockResolvedValue({ apiWrapper: { response: {} } }),
    PROPERTY_TYPES: {},
  };
});
vi.mock('@/features/mapSideBar/hooks/usePropertyDetails', () => {
  return {
    usePropertyDetails: vi.fn(),
  };
});

const onClose = vi.fn();
const onSave = vi.fn();
const onCancel = vi.fn();
const onMenuChange = vi.fn();
const onSuccess = vi.fn();
const onCancelConfirm = vi.fn();
const onUpdateProperties = vi.fn();
const canRemove = vi.fn();
const confirmBeforeAdd = vi.fn();
const setContainerState = vi.fn();
const setIsEditing = vi.fn();
const onEditFileProperties = vi.fn();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

const DEFAULT_PROPS: IAcquisitionViewProps = {
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onSuccess,
  onCancelConfirm,
  onUpdateProperties,
  confirmBeforeAdd,
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
  formikRef: createRef(),
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
    history.replace(`/mapview/sidebar/acquisition/1`);
    server.use(
      rest.get('/users/info/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock())),
      ),
      rest.get('/notes/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockNotesResponse())),
      ),
      rest.get('/acquisitionfiles/:id/owners', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockAcquisitionFileOwnersResponse())),
      ),
      rest.get('/takes/acquisition/:id/property/:propertyId', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getMockApiTakes())),
      ),
      rest.get('/takes/property/:id/count', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(1)),
      ),
      rest.get('/persons/concept/:id', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(mockApiPerson)),
      ),
      rest.get('/acquisitionfiles/:id/properties', (req, res, ctx) =>
        res(
          ctx.delay(500),
          ctx.status(200),
          ctx.json(mockAcquisitionFileResponse().fileProperties),
        ),
      ),
      rest.get('/acquisitionfiles/:id/interestholders', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getMockApiInterestHolders())),
      ),
    );
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    await act(async () => {});
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
    await act(async () => {});

    expect(getByText('Acquisition File')).toBeVisible();
    await waitFor(() => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = await setup(undefined, { claims: [Claims.ACQUISITION_EDIT] });
    await act(async () => {});

    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle } = await setup(undefined, { claims: [] });
    await act(async () => {});

    expect(queryByTitle('Change properties')).toBeNull();
  });

  it('should display the notes tab if the user has permissions', async () => {
    const { getAllByText } = await setup(undefined, { claims: [Claims.NOTE_VIEW] });
    await act(async () => {});

    expect(getAllByText(/Notes/)[0]).toBeVisible();
  });

  it('should not display the notes tab if the user does not have permissions', async () => {
    const { queryByText } = await setup(undefined, { claims: [] });
    await act(async () => {});

    expect(queryByText('Notes')).toBeNull();
  });

  it('should display the File Details tab by default', async () => {
    const { getByRole } = await setup();
    await act(async () => {});

    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it.skip(`should show a toast and redirect to the File Details page when accessing a non-existing property index`, async () => {
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

  it.skip('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/1`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the File Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/acquisition/1/blahblahtab?edit=true`);
    const { getByRole } = await setup();
    await act(async () => {});

    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it.skip(`should display the Property Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/acquisition/1/property/1/unknownTabWhatIsThis?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display an error message when the error prop is set.`, async () => {
    const { getByText } = await setup({ ...DEFAULT_PROPS, error: {} } as any);
    await act(async () => {});

    expect(
      getByText(
        'Failed to load Acquisition File. Check the detailed error in the top right for more details.',
      ),
    ).toBeVisible();
  });
});
