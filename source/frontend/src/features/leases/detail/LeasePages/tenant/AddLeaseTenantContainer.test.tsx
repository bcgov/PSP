import { RenderOptions, waitFor } from '@testing-library/react';
import { useFormikContext } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';
import { act } from 'react-test-renderer';

import { Claims } from '@/constants';
import { LeaseContextProvider } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import {
  getEmptyPerson,
  getMockContactOrganizationWithMultiplePeople,
  getMockContactOrganizationWithOnePerson,
} from '@/mocks/contacts.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { defaultApiLease, getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockKeycloak, renderAsync } from '@/utils/test-utils';

import AddLeaseTenantContainer from './AddLeaseTenantContainer';
import { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { FormTenant } from './models';
import { IPrimaryContactWarningModalProps } from './PrimaryContactWarningModal';

// mock auth library
jest.mock('@react-keycloak/web');
jest.mock('@/hooks/pims-api/useApiContacts');
jest.mock('@/features/leases/hooks/useUpdateLease');
jest.mock('@/hooks/repositories/useLeaseTenantRepository');

const getPersonConcept = jest.fn();
const updateTenants = jest.fn().mockResolvedValue({ ...defaultApiLease(), id: 1 });
const onEdit = jest.fn();
const onSuccess = jest.fn();

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const SaveButton = () => {
  const { submitForm } = useFormikContext();
  return <button onClick={submitForm}>Save</button>;
};
let viewProps: IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps;

const View = (props: IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps) => {
  viewProps = props;
  return <></>;
};

const getLeaseTenantsObj = {
  execute: jest.fn().mockResolvedValue(defaultApiLease().tenants),
  loading: false,
  error: undefined,
  response: [],
};

describe('AddLeaseTenantContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & { lease?: ApiGen_Concepts_Lease; tenants?: FormTenant[] } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseContextProvider initialLease={renderOptions.lease ?? { ...defaultApiLease(), id: 1 }}>
        <AddLeaseTenantContainer
          formikRef={React.createRef()}
          View={View}
          onEdit={onEdit}
          tenants={renderOptions.tenants ?? []}
          onSuccess={onSuccess}
        >
          <SaveButton />
        </AddLeaseTenantContainer>
      </LeaseContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );
    return { component };
  };

  beforeEach(() => {
    jest.resetAllMocks();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
    (useApiContacts as jest.Mock).mockReturnValue({ getPersonConcept: getPersonConcept });
    (useLeaseTenantRepository as jest.Mock).mockReturnValue({
      updateLeaseTenants: { execute: updateTenants.mockResolvedValue([]) },
      getLeaseTenants: getLeaseTenantsObj,
    });
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('makes requests for primary contacts', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setSelectedTenants([getMockContactOrganizationWithOnePerson()]);

      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.selectedTenants[0].organizationPersons).toHaveLength(1);
    });
  });

  it('does not request duplicate person ids', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setSelectedTenants([
        getMockContactOrganizationWithOnePerson(),
        getMockContactOrganizationWithOnePerson(),
      ]);
      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.selectedTenants[0].organizationPersons).toHaveLength(1);
    });
  });

  it('does not request previously requested data', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setSelectedTenants([
        {
          ...getMockContactOrganizationWithOnePerson(),
          organization: {
            ...getEmptyOrganization(),
            organizationPersons: [
              { person: getEmptyPerson(), organizationId: 1, personId: 2, rowVersion: null },
            ],
          },
        },
      ]);
      expect(getPersonConcept).not.toHaveBeenCalled();
    });
  });

  it('does not overwrite an existing primary contact if the selection does not change', async () => {
    await setup({});

    //setup
    await waitFor(() => {
      viewProps.setSelectedTenants([getMockContactOrganizationWithOnePerson()]);
      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.selectedTenants).toHaveLength(1);
    });

    await waitFor(() => {
      //act
      viewProps.setSelectedTenants([getMockContactOrganizationWithOnePerson()]);
    });
    //assert
    expect(getPersonConcept).toHaveBeenCalledTimes(1);
  });

  it('sets a callback function when a tenant does not have a selected a primary contact', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setSelectedTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.selectedTenants).toHaveLength(1);
    });
    viewProps.onSubmit({
      ...LeaseFormModel.fromApi(getMockApiLease()),
      tenants: viewProps.selectedTenants,
    });
    await waitFor(() => {
      expect(viewProps.saveCallback).not.toBeUndefined();
    });
  });

  it('callback submits data with a valid empty primary contact', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setSelectedTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.selectedTenants).toHaveLength(1);
    });
    viewProps.onSubmit({
      ...LeaseFormModel.fromApi(getMockApiLease()),
      tenants: viewProps.selectedTenants,
    });
    await waitFor(() => {
      expect(viewProps.saveCallback).not.toBeUndefined();
    });

    viewProps.saveCallback && viewProps.saveCallback();
    await waitFor(() => {
      expect(updateTenants).toHaveBeenCalledTimes(1);
      expect(updateTenants.mock.calls[0][0].primaryContactId).toBeUndefined();
    });
  });

  it('onSubmit calls api with expected data', async () => {
    await setup({});
    viewProps.onSubmit({ ...new LeaseFormModel(), id: 1 });
    await waitFor(() => {
      expect(updateTenants).toHaveBeenCalledTimes(1);
      expect(onEdit).toHaveBeenCalledWith(false);
    });
  });

  it('onSubmit calls api with expected data when a person with organization is populated', async () => {
    await setup({});
    viewProps.onSubmit({
      ...new LeaseFormModel(),
      tenants: [{ personId: 1, organizationId: 2 }],
      id: 1,
    });
    await waitFor(async () => {
      expect(updateTenants).toHaveBeenCalledTimes(1);
      expect(onEdit).toHaveBeenCalledWith(false);
      expect(updateTenants.mock.calls[0][1][0]).toStrictEqual<ApiGen_Concepts_LeaseTenant>({
        personId: 1,
        person: null,
        organizationId: null,
        organization: null,
        lessorType: null,
        tenantTypeCode: null,
        primaryContactId: null,
        note: null,
        leaseId: 0,
        leaseTenantId: null,
        primaryContact: null,
        ...getEmptyBaseAudit(),
      });
    });
  });

  it('onCancel removes the callback', async () => {
    await setup({});

    //setup
    await waitFor(() => {
      viewProps.setSelectedTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.selectedTenants).toHaveLength(1);
      expect(viewProps.saveCallback).not.toBeNull();
    });
    //act
    act(() => viewProps.onCancel && viewProps.onCancel());
    //assert
    await waitFor(() => {
      expect(viewProps.saveCallback).toBeUndefined();
    });
  });
});
