import { createMemoryHistory } from 'history';
import { http, HttpResponse } from 'msw';
import { createRef } from 'react';
import { Route } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useLtsa } from '@/hooks/useLtsa';
import { mockLookups } from '@/mocks/lookups.mock';
import {
  mockManagementFilePropertiesResponse,
  mockManagementFileResponse,
} from '@/mocks/managementFiles.mock';
import { server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { firstOrNull, prettyFormatUTCDate } from '@/utils';
import {
  act,
  cleanup,
  getMockRepositoryObj,
  render,
  RenderOptions,
  userEvent,
  waitForEffects,
} from '@/utils/test-utils';

import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import { toTypeCode } from '@/utils/formUtils';
import ManagementView, { IManagementViewProps } from './ManagementView';

// mock auth library

vi.mock('@/hooks/repositories/useNoteRepository');
vi.mock('@/hooks/pims-api/useApiNotes');

const getNotes = vi.fn().mockResolvedValue([]);
const onClose = vi.fn();
const onSave = vi.fn();
const onCancel = vi.fn();
const onSuccess = vi.fn();
const onUpdateProperties = vi.fn();
const confirmBeforeAdd = vi.fn();
const canRemove = vi.fn();
const setIsEditing = vi.fn();
const onSelectFileSummary = vi.fn();
const onSelectProperty = vi.fn();
const onEditProperties = vi.fn();

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

vi.mock('@/hooks/repositories/useComposedProperties', () => {
  return {
    useComposedProperties: vi.fn().mockResolvedValue({ apiWrapper: { response: {} } }),
    PROPERTY_TYPES: {},
  };
});

vi.mock('@/hooks/pims-api/useApiProperties');
vi.mocked(useApiProperties).mockReturnValue({
  getPropertiesViewPagedApi: vi
    .fn()
    .mockResolvedValue({ data: {} as ApiGen_Base_Page<ApiGen_Concepts_Property> }),
  getMatchingPropertiesApi: vi.fn(),
  getPropertyAssociationsApi: vi.fn(),
  exportPropertiesApi: vi.fn(),
  getPropertiesApi: vi.fn(),
  getPropertyConceptWithIdApi: vi.fn(),
  putPropertyConceptApi: vi.fn(),
  getPropertyConceptWithPidApi: vi.fn(),
  getPropertyConceptWithPinApi: vi.fn(),
});

vi.mock('@/hooks/useLtsa');
vi.mocked(useLtsa, { partial: true }).mockReturnValue({
  ltsaRequestWrapper: getMockRepositoryObj(),
  getStrataPlanCommonProperty: getMockRepositoryObj(),
});

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider, { partial: true }).mockReturnValue({
  retrieveProjectProducts: vi.fn(),
});

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');

vi.mock('@/hooks/pims-api/useApiManagementFile');
vi.mock('@/hooks/pims-api/useApiManagementFileContact');
vi.mock('@/hooks/repositories/useManagementFileRepository');

const DEFAULT_PROPS: IManagementViewProps = {
  onClose,
  onSave,
  onCancel,
  onSelectFileSummary,
  onSelectProperty,
  onEditProperties,
  onSuccess,
  onUpdateProperties,
  confirmBeforeAdd,
  canRemove,
  isEditing: false,
  setIsEditing,
  formikRef: createRef(),
  isFormValid: true,
  error: undefined,
};

const history = createMemoryHistory();

describe('ManagementView component', () => {
  // render component under test
  const setup = async (
    props: IManagementViewProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/mapview/sidebar/management/:id">
        <ManagementView
          {...{
            ...props,
            managementFile: props.managementFile ?? {
              ...mockManagementFileResponse(),
              fileProperties: mockManagementFilePropertiesResponse(),
            },
          }}
        />
      </Route>,
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

    await act(async () => {});

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    server.use(
      http.get('/api/users/info/*', () => HttpResponse.json(getUserMock(), { status: 200 })),
    );

    vi.mocked(useNoteRepository, { partial: true }).mockReturnValue({
      addNote: getMockRepositoryObj(),
      getNote: getMockRepositoryObj(),
      updateNote: getMockRepositoryObj(),
    });

    vi.mocked(useApiNotes, { partial: true }).mockReturnValue({
      getNotes,
    });

    vi.mocked(useHistoricalNumberRepository).mockReturnValue({
      getPropertyHistoricalNumbers: getMockRepositoryObj([]),
      updatePropertyHistoricalNumbers: getMockRepositoryObj([]),
    });

    vi.mocked(useManagementFileRepository, { partial: true }).mockReturnValue({
      putManagementFile: getMockRepositoryObj(),
      getAllManagementFileContacts: getMockRepositoryObj([]),
      deleteManagementContact: getMockRepositoryObj(),
    });

    history.replace(`/mapview/sidebar/management/1`);
  });

  afterEach(() => {
    vi.clearAllMocks();
    cleanup();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getAllByText, getByText } = await setup();
    await waitForEffects();
    const testManagementFile = mockManagementFileResponse();

    expect(getByText('Management File')).toBeVisible();
    expect(getAllByText(testManagementFile.fileName, { exact: false })[0]).toBeVisible();
    expect(
      getByText(new RegExp(prettyFormatUTCDate(testManagementFile.appCreateTimestamp))),
    ).toBeVisible();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = await setup(undefined, { claims: [Claims.MANAGEMENT_EDIT] });
    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('renders the warning icon instead of the edit button when file in final/archived state', async () => {
    const { getAllByTestId } = await setup(
      {
        ...DEFAULT_PROPS,
        managementFile: {
          ...mockManagementFileResponse(),
          fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED),
        },
      },
      { claims: [Claims.MANAGEMENT_EDIT] },
    );
    expect(firstOrNull(getAllByTestId('tooltip-icon-1-summary-cannot-edit-tooltip'))).toBeVisible();
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

  it(`should redirect to the File Details page when accessing a non-existing property index`, async () => {
    history.replace(`/mapview/sidebar/management/1/property/99999`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Selector according to routing', async () => {
    history.replace(`/mapview/sidebar/management/1/property/selector`);
    const { getByText } = await setup();
    expect(getByText(/Property selection/i)).toBeVisible();
  });

  it('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/management/1/property/1`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the File Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/management/1/blahblahtab?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the Property Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/management/1/property/1/unknownTabWhatIsThis?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display an error message when the error prop is set.`, async () => {
    const { getByText } = await setup({ ...DEFAULT_PROPS, error: {} } as any);
    expect(
      getByText(
        'Failed to load Management File. Check the detailed error in the top right for more details.',
      ),
    ).toBeVisible();
  });

  it(`should display property edit title when editing`, async () => {
    history.replace(`/mapview/sidebar/management/1?edit=true`);

    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    await waitForEffects();
    expect(getByText('Update Management File')).toBeVisible();
  });

  it(`should display property edit title when editing properties`, async () => {
    history.replace(`/mapview/sidebar/management/4/property/selector`);
    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    expect(getByText('Property selection')).toBeVisible();
  });

  it(`should display property edit title when editing and on property tab`, async () => {
    history.replace(`/mapview/sidebar/management/1/property/1?edit=true`);
    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    expect(getByText('Update Property File Data')).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = await setup();
    expect(getByText('Management File')).toBeVisible();
    await act(async () => userEvent.click(getCloseButton()));
    expect(onClose).toHaveBeenCalled();
  });
});
