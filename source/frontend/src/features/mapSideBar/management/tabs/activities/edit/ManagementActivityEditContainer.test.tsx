import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import useActivityContactRetriever from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { getMockPropertyManagementActivity } from '@/mocks/PropertyManagementActivity.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { act, getMockRepositoryObj, render, RenderOptions, screen } from '@/utils/test-utils';

import {
  IManagementActivityEditContainerProps,
  ManagementActivityEditContainer,
} from './ManagementActivityEditContainer';
import { IManagementActivityEditFormProps } from './ManagementActivityEditForm';
import { ManagementActivityFormModel } from './models';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

const history = createMemoryHistory();

const onClose = vi.fn();

vi.mock('@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks');
const mockContactApi: ReturnType<typeof useActivityContactRetriever> = {
  fetchMinistryContacts: vi.fn(),
  fetchPartiesContact: vi.fn(),
  fetchProviderContact: vi.fn(),
  isLoading: false,
};

vi.mock('@/hooks/repositories/useManagementActivityRepository');
const mockGetManagementActivity = getMockRepositoryObj(getMockPropertyManagementActivity(1));
const mockAddManagementActivity = getMockRepositoryObj(getMockPropertyManagementActivity(1));
const mockUpdateManagementActivity = getMockRepositoryObj(getMockPropertyManagementActivity(1));

describe('ManagementActivityEditContainer component', () => {
  let viewProps: IManagementActivityEditFormProps;

  const TestView: React.FC<IManagementActivityEditFormProps> = props => {
    viewProps = props;
    return <span>Content Rendered</span>;
  };

  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IManagementActivityEditContainerProps>;
    } = {},
  ) => {
    const rendered = render(
      <SideBarContextProvider
        file={{ ...mockManagementFileResponse(), fileType: ApiGen_CodeTypes_FileTypes.Management }}
      >
        <ManagementActivityEditContainer
          managementFileId={renderOptions?.props?.managementFileId ?? 1}
          activityId={renderOptions?.props?.activityId ?? 1}
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

    return { ...rendered };
  };

  beforeEach(() => {
    viewProps = undefined;
    vi.mocked(useManagementActivityRepository, { partial: true }).mockReturnValue({
      getManagementActivity: mockGetManagementActivity,
      addManagementActivity: mockAddManagementActivity,
      updateManagementActivity: mockUpdateManagementActivity,
    });
    vi.mocked(useActivityContactRetriever, { partial: true }).mockReturnValue(mockContactApi);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup();

    expect(await screen.findByText(/Content Rendered/i)).toBeVisible();
    expect(mockGetManagementActivity.execute).toHaveBeenCalledWith(1, 1);
  });

  it('loads the management activity and passes as props to the view', async () => {
    await setup();

    expect(await screen.findByText(/Content Rendered/i)).toBeVisible();
    expect(viewProps.loading).toBe(false);
    expect(viewProps.managementFile).toEqual(
      expect.objectContaining(mockManagementFileResponse(1)),
    );

    expect(viewProps.initialValues).toEqual(
      expect.objectContaining({
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
        activityProperties: [
          {
            id: 15,
            managementActivityId: 1,
            propertyId: 1,
            rowVersion: 1,
          },
        ],
        selectedProperties: [
          {
            fileId: null,
            id: 0,
            property: null,
            propertyId: 1,
            rowVersion: 0,
          } as ApiGen_Concepts_FileProperty,
        ],
        rowVersion: 1,
      } as ManagementActivityFormModel),
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
    mockGetManagementActivity.execute.mockResolvedValueOnce(apiManagement);

    await setup();

    expect(mockContactApi.fetchMinistryContacts).toHaveBeenCalledTimes(1);
    expect(mockContactApi.fetchPartiesContact).toHaveBeenCalledTimes(1);
    expect(mockContactApi.fetchProviderContact).toHaveBeenCalledTimes(1);
  });

  it('calls API to create new management activity and redirects to view screen', async () => {
    const apiManagement = getMockPropertyManagementActivity(0);
    await setup();
    await act(async () => viewProps.onSave(apiManagement));

    expect(mockAddManagementActivity.execute).toHaveBeenCalledWith(1, apiManagement);
    expect(history.location.pathname).toBe('/mapview/sidebar/management/1/activities/1');
  });

  it('calls API to update an existing management activity and redirects to view screen', async () => {
    await setup();
    const apiManagement = getMockPropertyManagementActivity(1);
    await act(async () => viewProps.onSave(apiManagement));

    expect(mockUpdateManagementActivity.execute).toHaveBeenCalledWith(1, apiManagement);
    expect(history.location.pathname).toBe('/mapview/sidebar/management/1/activities/1');
  });

  it('navigates back to the property management view screen when form is cancelled', async () => {
    await setup({ props: { activityId: 1 } });
    await act(async () => viewProps.onCancel());

    expect(mockAddManagementActivity.execute).not.toHaveBeenCalled();
    expect(mockUpdateManagementActivity.execute).not.toHaveBeenCalled();
    expect(history.location.pathname).toBe('/mapview/sidebar/management/1/activities/1');
  });

  it('closes the add activity screen when the form is cancelled', async () => {
    await setup({ props: { activityId: 0 } });
    await act(async () => viewProps.onCancel());

    expect(mockAddManagementActivity.execute).not.toHaveBeenCalled();
    expect(mockUpdateManagementActivity.execute).not.toHaveBeenCalled();
    expect(onClose).toHaveBeenCalled();
  });
});
