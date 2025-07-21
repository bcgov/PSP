import { createMemoryHistory } from 'history';
import { Mock } from 'vitest';

import { Claims } from '@/constants';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { useManagementActivityPropertyRepository } from '@/hooks/repositories/useManagementActivityPropertyRepository';
import { getMockPropertyManagementActivity } from '@/mocks/PropertyManagementActivity.mock';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import useActivityContactRetriever from '../hooks';
import {
  IPropertyActivityEditContainerProps,
  PropertyActivityEditContainer,
} from './PropertyActivityEditContainer';
import { IPropertyActivityEditFormProps } from './PropertyActivityEditForm';
import { PropertyActivityFormModel } from './models';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';

const history = createMemoryHistory();

const onClose = vi.fn();

const mockContactApi: ReturnType<typeof useActivityContactRetriever> = {
  fetchMinistryContacts: vi.fn(),
  fetchPartiesContact: vi.fn(),
  fetchProviderContact: vi.fn(),
  isLoading: false,
};

vi.mock('../hooks');
vi.mocked(useActivityContactRetriever).mockReturnValue(mockContactApi);

const mockPropertyActivityApi: ReturnType<typeof useManagementActivityPropertyRepository> = {
  createActivity: {
    error: undefined,
    response: undefined,
    execute: vi.fn().mockResolvedValue(getMockPropertyManagementActivity(1)),
    loading: false,
    status: 200,
  },
  deleteActivity: {
    error: undefined,
    response: undefined,
    execute: vi.fn(),
    loading: false,
    status: 200,
  },
  getActivities: {
    error: undefined,
    response: undefined,
    execute: vi.fn(),
    loading: false,
    status: 200,
  },
  getActivity: {
    error: undefined,
    response: undefined,
    execute: vi.fn(),
    loading: false,
    status: 200,
  },
  updateActivity: {
    error: undefined,
    response: undefined,
    execute: vi.fn().mockResolvedValue(getMockPropertyManagementActivity(1)),
    loading: false,
    status: 200,
  },
};

vi.mock('@/hooks/repositories/useManagementActivityPropertyRepository');
vi.mocked(useManagementActivityPropertyRepository).mockReturnValue(mockPropertyActivityApi);

describe('PropertyActivityEditContainer component', () => {
  let viewProps: IPropertyActivityEditFormProps;

  const TestView: React.FC<IPropertyActivityEditFormProps> = props => {
    viewProps = props;
    return <span>Content Rendered</span>;
  };

  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertyActivityEditContainerProps>;
    } = {},
  ) => {
    const result = render(
      <SideBarContextProvider>
        <PropertyActivityEditContainer
          propertyId={renderOptions?.props?.propertyId ?? 1}
          managementActivityId={renderOptions?.props?.managementActivityId ?? 1}
          viewEnabled={renderOptions?.props?.viewEnabled ?? true}
          onClose={renderOptions?.props?.onClose ?? onClose}
          View={TestView}
        />
      </SideBarContextProvider>,
      {
        history,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [
          Claims.MANAGEMENT_VIEW,
          Claims.MANAGEMENT_ADD,
          Claims.MANAGEMENT_EDIT,
        ],
        ...renderOptions,
      },
    );

    // wait for effect to settle
    await act(async () => {});

    return { ...result };
  };

  beforeEach(() => {
    viewProps = undefined;
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup();

    expect(await screen.findByText(/Content Rendered/i)).toBeVisible();
    expect(mockPropertyActivityApi.getActivity.execute).toHaveBeenCalledWith(1, 1);
  });

  it('loads the management activity and passes as props to the view', async () => {
    (mockPropertyActivityApi.getActivity.execute as Mock).mockResolvedValue(
      getMockPropertyManagementActivity(1),
    );

    await setup();

    expect(await screen.findByText(/Content Rendered/i)).toBeVisible();
    expect(viewProps.propertyId).toBe(1);
    expect(viewProps.loading).toBe(false);
    expect(viewProps.initialValues).toEqual(
      expect.objectContaining({
        id: 1,
        activityTypeCode: 'APPLICPERMIT',
        activityStatusCode: 'NOTSTARTED',
        activitySubtypeCodes: [
          {
            id: 100,
            managementActivityId: 1,
            rowVersion: 1,
            subTypeCode: 'ACCESS',
            subTypeCodeDescription: 'Access',
          },
        ],
        requestedDate: '2023-10-17T00:00:00',
        completionDate: '',
        rowNumber: 1,
      } as PropertyActivityFormModel),
    );
  });

  it('loads related contact information for person and organizations', async () => {
    const apiManagement: ApiGen_Concepts_ManagementActivity = {
      ...getMockPropertyManagementActivity(1),
      ministryContacts: [
        {
          id: 1,
          personId: 1,
          person: null,
          managementActivityId: 1,
          managementActivity: null,
          ...getEmptyBaseAudit(),
        },
      ],
      involvedParties: [
        {
          id: 2,
          personId: 0,
          person: null,
          organizationId: 1,
          organization: null,
          managementActivityId: 1,
          managementActivity: null,
          ...getEmptyBaseAudit(),
        },
      ],
    };
    (mockPropertyActivityApi.getActivity.execute as Mock).mockResolvedValue(apiManagement);

    await setup();

    expect(mockContactApi.fetchMinistryContacts).toHaveBeenCalledTimes(1);
    expect(mockContactApi.fetchPartiesContact).toHaveBeenCalledTimes(1);
    expect(mockContactApi.fetchProviderContact).toHaveBeenCalledTimes(1);
  });

  it('calls API to create new management activity and redirects to view screen', async () => {
    const apiManagement = getMockPropertyManagementActivity(0);
    await setup();
    await act(async () => viewProps.onSave(apiManagement));

    expect(mockPropertyActivityApi.createActivity.execute).toHaveBeenCalledWith(1, apiManagement);
    expect(history.location.pathname).toBe('/mapview/sidebar/property/1/management/activity/1');
  });

  it('calls API to update an existing management activity and redirects to view screen', async () => {
    await setup();
    const apiManagement = getMockPropertyManagementActivity(1);
    await act(async () => viewProps.onSave(apiManagement));

    expect(mockPropertyActivityApi.updateActivity.execute).toHaveBeenCalledWith(1, apiManagement);
    expect(history.location.pathname).toBe('/mapview/sidebar/property/1/management/activity/1');
  });

  it('navigates back to the property management view screen when form is cancelled', async () => {
    await setup();
    await act(async () => viewProps.onCancel());

    expect(mockPropertyActivityApi.createActivity.execute).not.toHaveBeenCalled();
    expect(mockPropertyActivityApi.updateActivity.execute).not.toHaveBeenCalled();
    expect(history.location.pathname).toBe('/mapview/sidebar/property/1/management/activity/1');
  });
});
