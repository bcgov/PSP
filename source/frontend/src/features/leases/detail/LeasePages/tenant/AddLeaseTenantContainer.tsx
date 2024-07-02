import { FormikProps } from 'formik/dist/types';
import { find } from 'lodash';
import React, { useContext, useEffect, useState } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { isValidId } from '@/utils/utils';

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
  onSuccess: () => void;
  View: React.FunctionComponent<
    React.PropsWithChildren<IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps>
  >;
}

export const AddLeaseTenantContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantContainerProps>
> = ({ formikRef, onEdit, children, View, tenants: initialTenants, onSuccess }) => {
  const { lease } = useContext(LeaseStateContext);
  const [tenants, setTenants] = useState<FormTenant[]>(initialTenants);
  const [selectedContacts, setSelectedContacts] = useState<IContactSearchResult[]>(
    tenants.map(t => FormTenant.toContactSearchResult(t)) || [],
  );
  const [showContactManager, setShowContactManager] = React.useState<boolean>(false);
  const [handleSubmit, setHandleSubmit] = useState<(() => void) | undefined>(undefined);

  const {
    updateLeaseTenants,
    getLeaseTenants: { execute: getLeaseTenants, loading: loadingTenants },
  } = useLeaseTenantRepository();

  const {
    getOrganizationDetail: { execute: getOrganizationDetail },
  } = useOrganizationRepository();

  const leaseId = lease?.id;
  useEffect(() => {
    const tenantFunc = async () => {
      const tenants = await getLeaseTenants(leaseId ?? 0);
      if (tenants !== undefined) {
        setTenants(tenants.map((t: ApiGen_Concepts_LeaseTenant) => new FormTenant(t)));
        setSelectedContacts(
          tenants.map((t: ApiGen_Concepts_LeaseTenant) =>
            FormTenant.toContactSearchResult(new FormTenant(t)),
          ) || [],
        );
      }
    };
    tenantFunc();
  }, [leaseId, getLeaseTenants]);

  // get a unique list of all tenant organization person-ids that are associated to organization tenants.
  // in the case of a duplicate organization person, prefers tenants that have the person field non-null.
  const getTenantOrganizationPersonList = async (tenants?: IContactSearchResult[]) => {
    const personList: ApiGen_Concepts_PersonOrganization[] = [];
    tenants?.forEach(async tenant => {
      if (tenant.id.startsWith('O') === true) {
        const organizationDetails = await getOrganizationDetail(tenant.organizationId);
        organizationDetails?.organizationPersons?.forEach(op => {
          if (isValidId(op.personId) && !find(personList, p => p.personId === op.personId)) {
            personList.push(op);
          }
        });
        tenant.organization.organizationPersons = personList;
      }
    });
    return tenants;
  };

  const setSelectedTenantsWithPersonData = async (updatedTenants?: IContactSearchResult[]) => {
    const allExistingTenantIds = tenants.map(t => t.id);
    const updatedTenantIds = updatedTenants?.map(t => t.id) ?? [];
    const newTenants = updatedTenants?.filter(t => !allExistingTenantIds.includes(t.id));
    const matchingExistingTenants = tenants?.filter(t => updatedTenantIds.includes(t?.id ?? null));

    const orgTenants = await getTenantOrganizationPersonList(newTenants);
    const formTenants = orgTenants?.map(t => new FormTenant(undefined, t)) ?? [];
    setTenants([...formTenants, ...matchingExistingTenants]);
  };

  const submit = async (leaseToUpdate: ApiGen_Concepts_Lease) => {
    if (isValidId(leaseToUpdate.id)) {
      try {
        const updatedTenants = await updateLeaseTenants.execute(
          leaseToUpdate.id,
          leaseToUpdate.tenants ?? [],
        );
        if (updatedTenants) {
          formikRef?.current?.resetForm({
            values: LeaseFormModel.fromApi({
              ...leaseToUpdate,
              tenants: updatedTenants,
            }),
          });
          onEdit && onEdit(false);
          onSuccess();
        }
      } finally {
        formikRef?.current?.setSubmitting(false);
      }
    }
  };

  const onSubmit = async (lease: LeaseFormModel) => {
    const leaseToUpdate = LeaseFormModel.toApi(lease);
    if (getOrgsWithNoPrimaryContact(lease.tenants)?.length > 0) {
      console.log(lease.tenants);
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
      loading={loadingTenants}
    >
      {children}
    </View>
  ) : (
    <></>
  );
};

export default AddLeaseTenantContainer;
