import { FormikProps } from 'formik';
import { filter, find, orderBy, some } from 'lodash';
import React, { useContext, useEffect, useState } from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IContactSearchResult } from '@/interfaces';
import { Api_Lease } from '@/models/api/Lease';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';
import { Api_Person } from '@/models/api/Person';

import { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';
import { FormTenant } from './models';
import {
  getOrgsWithNoPrimaryContact,
  IPrimaryContactWarningModalProps,
} from './PrimaryContactWarningModal';

interface IAddLeaseTenantContainerProps {
  formikRef: React.RefObject<FormikProps<LeaseFormModel>>;
  onEdit?: (isEditing: boolean) => void;
  tenants: FormTenant[];
  View: React.FunctionComponent<
    React.PropsWithChildren<IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps>
  >;
}

export const AddLeaseTenantContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantContainerProps>
> = ({ formikRef, onEdit, children, View, tenants: initialTenants }) => {
  const { lease } = useContext(LeaseStateContext);
  const [tenants, setTenants] = useState<FormTenant[]>(initialTenants);
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>(
    tenants.map(t => FormTenant.toContactSearchResult(t)) || [],
  );
  const [showContactManager, setShowContactManager] = React.useState<boolean>(false);
  const [handleSubmit, setHandleSubmit] = useState<Function | undefined>(undefined);
  const {
    updateLeaseTenants,
    getLeaseTenants: { execute: getLeaseTenants, loading },
  } = useLeaseTenantRepository();
  const leaseId = lease?.id;
  useEffect(() => {
    const tenantFunc = async () => {
      const tenants = await getLeaseTenants(leaseId ?? 0);
      if (tenants !== undefined) {
        setTenants(tenants.map((t: Api_LeaseTenant) => new FormTenant(t)));
        setSelectedContacts(
          tenants.map((t: Api_LeaseTenant) =>
            FormTenant.toContactSearchResult(new FormTenant(t)),
          ) || [],
        );
      }
    };
    tenantFunc();
  }, [leaseId, getLeaseTenants]);

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

  const submit = async (leaseToUpdate: Api_Lease) => {
    if (leaseToUpdate?.id) {
      try {
        const updatedTenants = await updateLeaseTenants.execute(
          leaseToUpdate?.id,
          leaseToUpdate.tenants,
        );
        if (!!updatedTenants) {
          formikRef?.current?.resetForm({
            values: LeaseFormModel.fromApi({
              ...leaseToUpdate,
              tenants: updatedTenants,
            }),
          });
          onEdit && onEdit(false);
        }
      } finally {
        formikRef?.current?.setSubmitting(false);
      }
    }
  };

  const onSubmit = async (lease: LeaseFormModel) => {
    const leaseToUpdate = LeaseFormModel.toApi(lease);
    if (getOrgsWithNoPrimaryContact(lease.tenants)?.length > 0) {
      setHandleSubmit(() => () => submit(leaseToUpdate));
    } else {
      submit(leaseToUpdate);
    }
  };

  return lease ? (
    <View
      initialValues={{ ...new LeaseFormModel(), ...LeaseFormModel.fromApi(lease) }}
      selectedContacts={selectedContacts}
      setSelectedTenants={setSelectedTenantsWithPersonData}
      selectedTenants={tenants}
      setSelectedContacts={setSelectedContacts}
      onSubmit={onSubmit}
      formikRef={formikRef}
      showContactManager={showContactManager}
      setShowContactManager={setShowContactManager}
      saveCallback={handleSubmit}
      onCancel={() => setHandleSubmit(undefined)}
      loading={loading}
    >
      {children}
    </View>
  ) : (
    <></>
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
