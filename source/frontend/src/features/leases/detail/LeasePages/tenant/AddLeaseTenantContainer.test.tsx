import { waitFor } from '@testing-library/react';
import { Claims } from 'constants/claims';
import { LeaseContextProvider } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { apiLeaseToFormLease } from 'features/leases/leaseUtils';
import { useFormikContext } from 'formik';
import { createMemoryHistory } from 'history';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { defaultFormLease, defaultLease, IFormLease, ILease } from 'interfaces';
import { mockLookups } from 'mocks';
import {
  getMockContactOrganizationWithMultiplePeople,
  getMockContactOrganizationWithOnePerson,
} from 'mocks/mockContacts';
import { getMockLease } from 'mocks/mockLease';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { mockKeycloak, renderAsync, RenderOptions } from 'utils/test-utils';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';
import { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { IPrimaryContactWarningModalProps } from './PrimaryContactWarningModal';

// mock auth library
jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api/useApiContacts');
jest.mock('features/leases/hooks/useUpdateLease');

const getPersonConcept = jest.fn();
const updateLease = jest.fn().mockResolvedValue({ ...defaultLease, id: 1 });
const onEdit = jest.fn();

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

describe('AddLeaseTenantContainer component', () => {
  const setup = async (renderOptions: RenderOptions & { lease?: ILease } = {}) => {
    // render component under test
    const component = await renderAsync(
      <LeaseContextProvider initialLease={renderOptions.lease ?? { ...defaultLease, id: 1 }}>
        <AddLeaseTenantContainer formikRef={React.createRef()} View={View} onEdit={onEdit}>
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
    (useUpdateLease as jest.Mock).mockReturnValue({ updateLease: updateLease });
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('makes requests for primary contacts', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setTenants([getMockContactOrganizationWithOnePerson()]);

      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.tenants[0].organizationPersons).toHaveLength(1);
    });
  });

  it('does not request duplicate person ids', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setTenants([
        getMockContactOrganizationWithOnePerson(),
        getMockContactOrganizationWithOnePerson(),
      ]);
      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.tenants[0].organizationPersons).toHaveLength(1);
    });
  });

  it('does not request previously requested data', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setTenants([
        {
          ...getMockContactOrganizationWithOnePerson(),
          organization: { organizationPersons: [{ person: {} }] },
        },
      ]);
      expect(getPersonConcept).not.toHaveBeenCalled();
    });
  });

  it('does not overwrite an existing primary contact if the selection does not change', async () => {
    await setup({});

    //setup
    await waitFor(() => {
      viewProps.setTenants([getMockContactOrganizationWithOnePerson()]);
      expect(getPersonConcept).toHaveBeenCalledTimes(1);
      expect(viewProps.tenants).toHaveLength(1);
    });

    await waitFor(() => {
      //act
      viewProps.setTenants([getMockContactOrganizationWithOnePerson()]);
    });
    //assert
    expect(getPersonConcept).toHaveBeenCalledTimes(1);
  });

  it('sets a callback function when a tenant does not have a selected a primary contact', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.tenants).toHaveLength(1);
    });
    viewProps.onSubmit({
      ...(apiLeaseToFormLease(getMockLease()) as IFormLease),
      tenants: viewProps.tenants,
    });
    await waitFor(() => {
      expect(viewProps.saveCallback).not.toBeUndefined();
    });
  });

  it('callback submits data with a valid empty primary contact', async () => {
    await setup({});

    await waitFor(() => {
      viewProps.setTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.tenants).toHaveLength(1);
    });
    viewProps.onSubmit({
      ...(apiLeaseToFormLease(getMockLease()) as IFormLease),
      tenants: viewProps.tenants,
    });
    await waitFor(() => {
      expect(viewProps.saveCallback).not.toBeUndefined();
    });

    viewProps.saveCallback && viewProps.saveCallback();
    await waitFor(() => {
      expect(updateLease).toHaveBeenCalledTimes(1);
      expect(updateLease.mock.calls[0][0].tenants[0].primaryContactId).toBeUndefined();
    });
  });

  it('onSubmit calls api with expected data', async () => {
    await setup({});
    viewProps.onSubmit(defaultFormLease);
    await waitFor(() => {
      expect(updateLease).toHaveBeenCalledTimes(1);
      expect(onEdit).toHaveBeenCalledWith(false);
    });
  });

  it('onCancel removes the callback', async () => {
    await setup({});

    //setup
    await waitFor(() => {
      viewProps.setTenants([getMockContactOrganizationWithMultiplePeople()]);
      expect(viewProps.tenants).toHaveLength(1);
      expect(viewProps.saveCallback).not.toBeNull();
    });
    //act
    viewProps.onCancel && viewProps.onCancel();
    //assert
    await waitFor(() => {
      expect(viewProps.saveCallback).toBeUndefined();
    });
  });
});
