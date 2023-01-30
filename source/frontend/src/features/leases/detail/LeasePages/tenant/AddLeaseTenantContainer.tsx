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

import { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import {
  getOrgsWithNoPrimaryContact,
  IPrimaryContactWarningModalProps,
} from './PrimaryContactWarningModal';
import { FormTenant } from './ViewTenantForm';

interface IAddLeaseTenantContainerProps {
  formikRef: React.RefObject<FormikProps<IFormLease>>;
  onEdit?: (isEditing: boolean) => void;
  View: React.FunctionComponent<
    React.PropsWithChildren<IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps>
  >;
}

export const AddLeaseTenantContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantContainerProps>
> = ({ formikRef, onEdit, children, View }) => {
  const { lease, setLease } = useContext(LeaseStateContext);
  const [tenants, setTenants] = useState<FormTenant[]>(apiLeaseToFormLease(lease)?.tenants || []);
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>(
    apiLeaseToFormLease(lease)?.tenants.map(t => t.toContactSearchResult()) || [],
  );
  const [showContactManager, setShowContactManager] = React.useState<boolean>(false);
  const [handleSubmit, setHandleSubmit] = useState<Function | undefined>(undefined);
  const { updateLease } = useUpdateLease();
  const { getPersonConcept } = useApiContacts();
  const { execute } = useApiRequestWrapper({
    requestFunction: getPersonConcept,
    requestName: 'get person by id',
  });

  const setSelectedTenantsWithPersonData = async (updatedTenants?: IContactSearchResult[]) => {
    const allExistingTenantIds = tenants.map(t => t.id);
    const updatedTenantIds = updatedTenants?.map(t => t.id) ?? [];
    const newTenants = updatedTenants?.filter(t => !allExistingTenantIds.includes(t.id));
    const matchingExistingTenants = tenants?.filter(t => updatedTenantIds.includes(t?.id ?? ''));

    const personPersonIdList = getTenantOrganizationPersonList(newTenants);
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
    const tenantsWithPersons =
      newTenants?.map(tenant => {
        tenant?.organization?.organizationPersons?.forEach(op => {
          const matchingPerson = find(allPersons, p => p?.id === op.personId);
          if (!!matchingPerson) {
            op.person = matchingPerson;
          }
        });
        tenant.tenantType = tenant.tenantType ? tenant.tenantType : 'TEN';
        return tenant;
      }) ?? [];
    const formTenants = tenantsWithPersons?.map(t => new FormTenant(undefined, t)) ?? [];
    setTenants([...formTenants, ...matchingExistingTenants]);
  };

  const submit = async (leaseToUpdate: ILease) => {
    try {
      const updatedLease = await updateLease(leaseToUpdate, 'tenants');
      if (!!updatedLease?.id) {
        formikRef?.current?.resetForm({ values: apiLeaseToFormLease(updatedLease) });
        setLease(updatedLease);
      }
    } finally {
      formikRef?.current?.setSubmitting(false);
      onEdit && onEdit(false);
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
    <View
      initialValues={{ ...defaultFormLease, ...apiLeaseToFormLease(lease as ILease) }}
      selectedContacts={selectedContacts}
      setTenants={setSelectedTenantsWithPersonData}
      tenants={tenants}
      setSelectedContacts={setSelectedContacts}
      onSubmit={onSubmit}
      formikRef={formikRef}
      showContactManager={showContactManager}
      setShowContactManager={setShowContactManager}
      saveCallback={handleSubmit}
      onCancel={() => setHandleSubmit(undefined)}
      lease={formikRef?.current?.values}
    >
      {children}
    </View>
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
