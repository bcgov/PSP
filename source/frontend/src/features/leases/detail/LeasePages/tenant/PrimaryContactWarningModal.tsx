import filter from 'lodash/filter';
import * as React from 'react';

import { GenericModal } from '@/components/common/GenericModal';

import { FormTenant } from './models';

export interface IPrimaryContactWarningModalProps {
  saveCallback?: () => void;
  selectedTenants: FormTenant[];
  onCancel?: () => void;
}

const PrimaryContactWarningModal: React.FunctionComponent<
  React.PropsWithChildren<IPrimaryContactWarningModalProps>
> = ({ saveCallback, selectedTenants, onCancel }) => {
  const warningOrgs = selectedTenants ? getOrgsWithNoPrimaryContact(selectedTenants) : [];
  const warningOrgNames = warningOrgs.map(org => org.summary);
  return (
    <GenericModal
      variant="warning"
      display={!!saveCallback}
      title="Confirm save"
      message={
        <>
          <p>
            A primary contact for <b>{warningOrgNames.join(', ')}</b> was not provided
          </p>
          <p>Do you wish to save without providing a primary contact?</p>
        </>
      }
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleCancel={onCancel}
      handleOk={() => saveCallback && saveCallback()}
    />
  );
};

export const getOrgsWithNoPrimaryContact = (tenants: FormTenant[]): FormTenant[] => {
  return filter(
    tenants?.filter((tenant: FormTenant) => {
      return (
        tenant.organizationId &&
        !tenant.personId &&
        (tenant.organizationPersons ?? []).length > 1 &&
        !tenant.primaryContactId
      );
    }),
  );
};

export default PrimaryContactWarningModal;
