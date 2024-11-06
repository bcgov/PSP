import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { Claims } from '@/constants';
import { LeaseContextProvider } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { fromContactSummary, IContactSearchResult } from '@/interfaces';
import {
  getEmptyPerson,
  getMockContactOrganizationWithMultiplePeople,
  getMockContactOrganizationWithOnePerson,
} from '@/mocks/contacts.mock';
import { mockApiOrganization, mockApiPerson } from '@/mocks/filterData.mock';
import { getEmptyLeaseStakeholder, getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { defaultApiLease, getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  createAxiosError,
  render,
  RenderOptions,
  screen,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';

import AddLeaseStakeholderContainer from './AddLeaseStakeholderContainer';
import { IAddLeaseStakeholderFormProps } from './AddLeaseStakeholderForm';
import { FormStakeholder } from './models';
import { IPrimaryContactWarningModalProps } from './PrimaryContactWarningModal';

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mock('@/features/leases/hooks/useUpdateLease');
vi.mock('@/hooks/repositories/useLeaseStakeholderRepository');
vi.mock('@/hooks/repositories/useLeaseRepository');

const getPersonConcept = vi.fn();
const onEdit = vi.fn();
const onSuccess = vi.fn();
const refreshLease = vi.fn();

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const getLeaseTenantTypesObj = {
  execute: vi.fn().mockResolvedValue([]),
  loading: false,
  error: undefined,
  response: [],
};

vi.mocked(useApiContacts).mockReturnValue({
  getPersonConcept: getPersonConcept,
} as unknown as ReturnType<typeof useApiContacts>);

const mockLeaseStakeholderApi = {
  getLeaseStakeholders: {
    execute: vi.fn(),
    loading: false,
    error: undefined,
    response: [],
    status: 200,
  },
  updateLeaseStakeholders: {
    execute: vi.fn(),
    loading: false,
    error: undefined,
    response: [],
    status: 200,
  },
};
vi.mocked(useLeaseStakeholderRepository).mockReturnValue(mockLeaseStakeholderApi);

vi.mocked(useLeaseRepository).mockReturnValue({
  getLeaseStakeholderTypes: getLeaseTenantTypesObj,
} as unknown as ReturnType<typeof useLeaseRepository>);

describe('AddLeaseTenantContainer component', () => {
  let viewProps: IAddLeaseStakeholderFormProps & IPrimaryContactWarningModalProps;

  const View = (props: IAddLeaseStakeholderFormProps & IPrimaryContactWarningModalProps) => {
    viewProps = props;
    return <></>;
  };

  // render component under test
  const setup = (
    renderOptions: RenderOptions & {
      props?: {
        lease?: ApiGen_Concepts_Lease;
        stakeholders?: FormStakeholder[];
      };
    } = {},
  ) => {
    const formikRef = createRef<FormikProps<LeaseFormModel>>();
    const utils = render(
      <LeaseContextProvider
        initialLease={renderOptions?.props?.lease ?? { ...defaultApiLease(), id: 1 }}
      >
        <AddLeaseStakeholderContainer
          formikRef={formikRef}
          View={View}
          onEdit={onEdit}
          stakeholders={renderOptions?.props?.stakeholders ?? []}
          onSuccess={onSuccess}
          refreshLease={refreshLease}
          isPayableLease={false}
        ></AddLeaseStakeholderContainer>
      </LeaseContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history,
        claims: renderOptions?.claims ?? [Claims.CONTACT_VIEW],
      },
    );
    return { ...utils, formikRef };
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockLeaseStakeholderApi.getLeaseStakeholders.execute.mockResolvedValue(
      defaultApiLease().stakeholders,
    );
    mockLeaseStakeholderApi.updateLeaseStakeholders.execute.mockResolvedValue([]);
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    await waitForEffects();

    expect(asFragment()).toMatchSnapshot();
  });

  it('makes requests for primary contacts', async () => {
    setup();
    await waitForEffects();

    await act(async () => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithOnePerson()),
      ]);
    });
    expect(getPersonConcept).toHaveBeenCalledTimes(1);
    expect(viewProps.selectedStakeholders[0].organizationPersons).toHaveLength(1);
  });

  it('does not request duplicate person ids', async () => {
    setup();
    await waitForEffects();

    await waitFor(() => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithOnePerson()),
        fromContactSummary(getMockContactOrganizationWithOnePerson()),
      ]);
      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.selectedStakeholders[0].organizationPersons).toHaveLength(1);
    });
  });

  it('does not request previously requested data', async () => {
    setup();
    await waitForEffects();

    const contact: IContactSearchResult = {
      ...getMockContactOrganizationWithOnePerson(),
      organization: {
        ...getEmptyOrganization(),
        organizationPersons: [
          {
            id: 0,
            person: getEmptyPerson(),
            organizationId: 1,
            organization: null,
            personId: 2,
            rowVersion: null,
          },
        ],
      },
      person: undefined,
      personId: undefined,
      surname: undefined,
      firstName: undefined,
      middleNames: undefined,
    } as unknown as IContactSearchResult;

    await act(async () => {
      viewProps.setSelectedStakeholders([contact]);
    });
    expect(getPersonConcept).not.toHaveBeenCalled();
  });

  it('does not overwrite an existing primary contact if the selection does not change', async () => {
    setup();
    await waitForEffects();

    //setup
    await act(() => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithOnePerson()),
      ]);
    });

    expect(getPersonConcept).toHaveBeenCalledTimes(1);
    expect(viewProps.selectedStakeholders).toHaveLength(1);

    await act(async () => {
      //act
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithOnePerson()),
      ]);
    });

    //assert
    expect(getPersonConcept).toHaveBeenCalledTimes(1);
  });

  it('sets a callback function when a tenant does not have a selected a primary contact', async () => {
    setup();
    await waitForEffects();

    await act(() => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithMultiplePeople()),
      ]);
    });

    expect(viewProps.selectedStakeholders).toHaveLength(1);

    await act(async () => {
      viewProps.onSubmit({
        ...LeaseFormModel.fromApi(getMockApiLease()),
        stakeholders: viewProps.selectedStakeholders,
      });
    });
    expect(viewProps.saveCallback).not.toBeUndefined();
  });

  it('callback submits data with a valid empty primary contact', async () => {
    setup();
    await waitForEffects();

    await act(async () => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithMultiplePeople()),
      ]);
    });
    expect(viewProps.selectedStakeholders).toHaveLength(1);

    await act(async () => {
      viewProps.onSubmit({
        ...LeaseFormModel.fromApi(getMockApiLease()),
        stakeholders: viewProps.selectedStakeholders,
      });
    });

    expect(viewProps.saveCallback).not.toBeUndefined();

    await act(async () => {
      viewProps.saveCallback && viewProps.saveCallback();
    });

    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledTimes(1);
    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledWith(
      1, // lease Id
      [
        expect.objectContaining<Partial<ApiGen_Concepts_LeaseStakeholder>>({
          organizationId: 2,
          primaryContactId: null,
        }),
      ], // array of stakeholders
    );
  });

  it('onSubmit calls api with expected data', async () => {
    setup();
    await waitForEffects();

    await act(async () => {
      viewProps.onSubmit({
        ...new LeaseFormModel(),
        stakeholders: [{ personId: 1 }],
        id: 1,
      });
    });

    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledTimes(1);
    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledWith(
      1, // lease Id
      [
        expect.objectContaining<Partial<ApiGen_Concepts_LeaseStakeholder>>({
          personId: 1,
          organizationId: null,
        }),
      ], // array of stakeholders
    );
    expect(onEdit).toHaveBeenCalledWith(false);
    expect(refreshLease).toHaveBeenCalled();
  });

  it('onSubmit calls api with expected data when a person with organization is populated', async () => {
    setup();
    await waitForEffects();

    await act(async () => {
      viewProps.onSubmit({
        ...new LeaseFormModel(),
        stakeholders: [{ personId: 1, organizationId: 2 }],
        id: 1,
      });
    });

    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledTimes(1);
    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledWith(
      1, // lease Id
      [
        {
          personId: 1,
          person: null,
          organizationId: null,
          organization: null,
          lessorType: null,
          stakeholderTypeCode: null,
          primaryContactId: null,
          note: null,
          leaseId: 0,
          leaseStakeholderId: 0,
          primaryContact: null,
          ...getEmptyBaseAudit(),
        },
      ], // array of stakeholders
    );
    expect(onEdit).toHaveBeenCalledWith(false);
    expect(refreshLease).toHaveBeenCalled();
  });

  it('shows a friendly error message when user attempts to delete a stakeholder that is associated to a compensation', async () => {
    mockLeaseStakeholderApi.updateLeaseStakeholders.execute.mockRejectedValue(
      createAxiosError(
        409,
        `Lease File Stakeholder can not be removed since it's assigned as a payee for a compensation requisition`,
      ),
    );
    setup({
      props: {
        stakeholders: [
          new FormStakeholder({ ...getEmptyLeaseStakeholder(), leaseId: 1, person: mockApiPerson }),
          new FormStakeholder({
            ...getEmptyLeaseStakeholder(),
            leaseId: 1,
            organization: mockApiOrganization,
          }),
        ],
      },
    });
    await waitForEffects();

    await act(async () => {
      viewProps.onSubmit({
        ...new LeaseFormModel(),
        stakeholders: [{ personId: 1 }],
        id: 1,
      });
    });

    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledTimes(1);
    expect(onEdit).not.toHaveBeenCalled();
    expect(
      await screen.findByText(
        `Lease File Stakeholder can not be removed since it's assigned as a payee for a compensation requisition`,
      ),
    ).toBeVisible();

    // stakeholder form values should be reset back to initial values
    expect(viewProps.selectedStakeholders).toStrictEqual([
      new FormStakeholder({ ...getEmptyLeaseStakeholder(), leaseId: 1, person: mockApiPerson }),
      new FormStakeholder({
        ...getEmptyLeaseStakeholder(),
        leaseId: 1,
        organization: mockApiOrganization,
      }),
    ]);
  });

  it('shows generic error message for server errors', async () => {
    mockLeaseStakeholderApi.updateLeaseStakeholders.execute.mockRejectedValue(
      createAxiosError(400, 'test error message'),
    );
    setup();
    await waitForEffects();

    await act(async () => {
      viewProps.onSubmit({
        ...new LeaseFormModel(),
        stakeholders: [{ personId: 1 }],
        id: 1,
      });
    });

    expect(mockLeaseStakeholderApi.updateLeaseStakeholders.execute).toHaveBeenCalledTimes(1);
    expect(onEdit).not.toHaveBeenCalled();
    expect(await screen.findByText('test error message')).toBeVisible();
  });

  it('onCancel removes the callback', async () => {
    setup();
    await waitForEffects();

    //setup
    await act(() => {
      viewProps.setSelectedStakeholders([
        fromContactSummary(getMockContactOrganizationWithMultiplePeople()),
      ]);
    });

    expect(viewProps.selectedStakeholders).toHaveLength(1);
    expect(viewProps.saveCallback).not.toBeNull();

    //act
    act(() => viewProps.onCancel && viewProps.onCancel());

    //assert
    expect(viewProps.saveCallback).toBeUndefined();
  });
});
