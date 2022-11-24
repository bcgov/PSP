import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { useUpdateLease } from 'features/leases/hooks/useUpdateLease';
import { apiLeaseToFormLease, formLeaseToApiLease } from 'features/leases/leaseUtils';
import { FormikProps } from 'formik';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { defaultFormLease, IContactSearchResult, IFormLease, ILease } from 'interfaces';
import { filter, find, orderBy, some } from 'lodash';
import { Api_Person } from 'models/api/Person';
import * as React from 'react';
import { useContext } from 'react';
import { useState } from 'react';
import { useHistory } from 'react-router';

import AddLeaseTenantForm from './AddLeaseTenantForm';
import PrimaryContactWarningModal, {
  getOrgsWithNoPrimaryContact,
} from './PrimaryContactWarningModal';
import { FormTenant } from './Tenant';

interface IAddLeaseTenantContainerProps {}

export const AddLeaseTenantContainer: React.FunctionComponent<
  IAddLeaseTenantContainerProps
> = () => {
  const formikRef = React.useRef<FormikProps<IFormLease>>(null);
  const { lease, setLease } = useContext(LeaseStateContext);
  const [selectedTenants, setSelectedTenants] = useState<FormTenant[]>([]);
  const [handleSubmit, setHandleSubmit] = useState<Function | undefined>(undefined);
  const { updateLease } = useUpdateLease();
  const history = useHistory();
  const { getPersonConcept } = useApiContacts();
  const { execute } = useApiRequestWrapper({
    requestFunction: getPersonConcept,
    requestName: 'get person by id',
  });

  const onCancel = () => {
    history.push(`/lease/${lease?.id}/tenant`);
  };

  const setSelectedTenantsWithPersonData = async (tenants?: IContactSearchResult[]) => {
    const personPersonIdList = getTenantOrganizationPersonList(tenants);
    // break the list up into the parts that have already been fetched and the parts that haven't been fetched.
    const unprocessedPersons = filter(personPersonIdList, p => p.person === undefined);
    const processedPersons = filter(personPersonIdList, p => p.person !== undefined).map(
      p => p.person,
    );

    // fetch any person ids that we do not have person information for.
    const personQueries = unprocessedPersons.map(person => execute(person.personId));
    const personResponses = await Promise.all(personQueries);
    const allPersons = personResponses.concat(processedPersons);

    // append the fetched person data onto the selected tenant list.
    const tenantsWithPersons = tenants?.map(tenant => {
      tenant?.organization?.organizationPersons?.forEach(op => {
        const matchingPerson = find(allPersons, p => p?.id === op.personId);
        if (!!matchingPerson) {
          op.person = matchingPerson;
        }
      });
      return tenant;
    });
    setSelectedTenants(tenantsWithPersons?.map(t => new FormTenant(undefined, t)) ?? []);
  };

  const submit = async (leaseToUpdate: ILease) => {
    try {
      const updatedLease = await updateLease(leaseToUpdate, 'tenants');
      if (!!updatedLease?.id) {
        formikRef?.current?.resetForm({ values: apiLeaseToFormLease(updatedLease) });
        setLease(updatedLease);
        history.push(`/lease/${updatedLease?.id}/tenant`);
      }
    } finally {
      formikRef?.current?.setSubmitting(false);
    }
  };

  const onSubmit = async (lease: IFormLease) => {
    const leaseToUpdate = formLeaseToApiLease(lease);
    if (getOrgsWithNoPrimaryContact(lease)?.length > 0) {
      setHandleSubmit(() => () => submit(leaseToUpdate));
    } else {
      submit(leaseToUpdate);
    }
  };

  return (
    <>
      <AddLeaseTenantForm
        initialValues={{ ...defaultFormLease, ...apiLeaseToFormLease(lease as ILease) }}
        selectedTenants={selectedTenants}
        setSelectedTenants={setSelectedTenantsWithPersonData}
        onCancel={onCancel}
        onSubmit={onSubmit}
        formikRef={formikRef}
      />
      <PrimaryContactWarningModal
        saveCallback={handleSubmit}
        onCancel={() => setHandleSubmit(undefined)}
        lease={formikRef?.current?.values}
      />
    </>
  );
};
// get a unique list of all tenant organization person-ids that are associated to organization tenants.
// in the case of a duplicate organization person, prefers tenants that have the person field non-null.
const getTenantOrganizationPersonList = (tenants?: IContactSearchResult[]) => {
  const personList: { person?: Api_Person; personId: number }[] = [];
  // put any tenants that have non-null organization person first to ensure that the de-duplication logic below will maintain that value.
  tenants = orderBy(
    tenants,
    t => some(t?.organization?.organizationPersons, op => op.person !== undefined),
    'desc',
  );
  tenants?.forEach(tenant =>
    tenant?.organization?.organizationPersons?.forEach(op => {
      if (op.personId !== undefined && !find(personList, p => p.personId === op.personId)) {
        personList.push({ person: op?.person, personId: op?.personId });
      }
    }),
  );
  return personList;
};

export default AddLeaseTenantContainer;
