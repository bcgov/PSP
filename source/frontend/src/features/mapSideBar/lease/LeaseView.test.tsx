import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';
import { Route } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useLtsa } from '@/hooks/useLtsa';
import { getMockApiLease, getMockLeaseProperties } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
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
} from '@/utils/test-utils';

import { useLeaseDetail } from '@/features/leases';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { getUserMock } from '@/mocks/user.mock';
import { LeasePageNames } from './LeaseContainer';
import LeaseView, { ILeaseViewProps } from './LeaseView';
import { LeaseFileTabNames } from './detail/LeaseFileTabs';

vi.mock('@/hooks/repositories/useNoteRepository');
vi.mock('@/hooks/pims-api/useApiNotes');

const getNotes = vi.fn().mockResolvedValue([]);
const onClose = vi.fn();
const onSave = vi.fn();
const onCancel = vi.fn();
const onChildSuccess = vi.fn();
const onPropertyUpdateSuccess = vi.fn();
const refreshLease = vi.fn();
const setLease = vi.fn();
const setIsEditing = vi.fn();
const onSelectFileSummary = vi.fn();
const onSelectProperty = vi.fn();
const onEditProperties = vi.fn();
const setContainerState = vi.fn();

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

vi.mock('@/hooks/pims-api/useApiUsers');
vi.mocked(useApiUsers, { partial: true }).mockReturnValue({
  getUserInfo: vi.fn().mockResolvedValue({ data: getUserMock() }),
});

vi.mock('@/features/leases/hooks/useLeaseDetail');
let useLeaseDetailMock: ReturnType<typeof useLeaseDetail>;

const history = createMemoryHistory();

describe('LeaseView component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ILeaseViewProps> } = {},
  ) => {
    const formikRef = createRef<FormikProps<any>>();
    const rendered = render(
      <Route path="/mapview/sidebar/lease/:id">
        <LeaseView
          formikRef={formikRef}
          onClose={onClose}
          onSave={onSave}
          onCancel={onCancel}
          onSelectFileSummary={onSelectFileSummary}
          onSelectProperty={onSelectProperty}
          onEditProperties={onEditProperties}
          onPropertyUpdateSuccess={onPropertyUpdateSuccess}
          onChildSuccess={onChildSuccess}
          refreshLease={refreshLease}
          setLease={setLease}
          containerState={
            renderOptions.props?.containerState ?? {
              isEditing: false,
              activeEditForm: LeasePageNames.DETAILS,
              activeTab: LeaseFileTabNames.fileDetails,
            }
          }
          setContainerState={setContainerState}
          setIsEditing={setIsEditing}
          isFormValid={renderOptions.props?.isFormValid ?? true}
          lease={
            renderOptions.props?.lease ?? {
              ...getMockApiLease(1),
              fileProperties: getMockLeaseProperties(1),
            }
          }
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
      ...rendered,
      getCloseButton: () => rendered.getByTitle('close'),
    };
  };

  beforeEach(() => {
    useLeaseDetailMock = {
      lease: getMockApiLease(),
      setLease: vi.fn(),
      getCompleteLease: vi.fn().mockResolvedValue(getMockApiLease()),
      refresh: vi.fn(),
      loading: false,
    };
    vi.mocked(useLeaseDetail).mockReturnValue(useLeaseDetailMock);

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

    history.replace(`/mapview/sidebar/lease/1`);
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
    const testLease = {
      ...getMockApiLease(),
      appCreateTimestamp: '2024-08-21T19:17:26.77',
      appLastUpdateTimestamp: '2024-08-26T20:46:43.163',
    };
    const { getAllByText, getByText } = await setup({ props: { lease: testLease } });

    expect(getByText('Lease / Licence')).toBeVisible();
    expect(firstOrNull(getAllByText(testLease.lFileNo, { exact: false }))).toBeVisible();
    expect(getByText(new RegExp(prettyFormatUTCDate(testLease.appCreateTimestamp)))).toBeVisible();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = await setup({ claims: [Claims.LEASE_EDIT] });
    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle } = await setup({ claims: [] });
    expect(queryByTitle('Change properties')).toBeNull();
  });

  it('should display the notes tab if the user has permissions', async () => {
    const { getAllByText } = await setup({ claims: [Claims.NOTE_VIEW] });
    expect(getAllByText(/Notes/)[0]).toBeVisible();
  });

  it('should not display the notes tab if the user does not have permissions', async () => {
    const { queryByText } = await setup({ claims: [] });
    expect(queryByText('Notes')).toBeNull();
  });

  it('should display the File Details tab by default', async () => {
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should redirect to the File Details page when accessing a non-existing property index`, async () => {
    history.replace(`/mapview/sidebar/lease/1/property/99999`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Selector according to routing', async () => {
    history.replace(`/mapview/sidebar/lease/1/property/selector`);
    const { getByText } = await setup();
    expect(getByText(/Property selection/i)).toBeVisible();
  });

  it('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/lease/1/property/387`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the File Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/lease/1/blahblahtab?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the Property Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/lease/1/property/387/unknownTabWhatIsThis?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display Property edit title when editing properties`, async () => {
    history.replace(`/mapview/sidebar/lease/1/property/selector`);
    const { getByText } = await setup({ props: { containerState: { isEditing: true } } });
    expect(getByText('Property selection')).toBeVisible();
  });

  it(`should display property edit title when editing and on property tab`, async () => {
    history.replace(`/mapview/sidebar/lease/1/property/387?edit=true`);
    const { getByText } = await setup({ props: { containerState: { isEditing: true } } });
    expect(getByText('Update Lease / Licence')).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = await setup();
    expect(getByText('Lease / Licence')).toBeVisible();
    await act(async () => userEvent.click(getCloseButton()));
    expect(onClose).toHaveBeenCalled();
  });
});
