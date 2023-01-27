import { GenericModal } from 'components/common/GenericModal';
import { IFormLease } from 'interfaces/ILease';
import filter from 'lodash/filter';
import * as React from 'react';

import { FormTenant } from './Tenant';

interface IPrimaryContactWarningModalProps {
  saveCallback?: Function;
  lease: IFormLease | undefined;
  onCancel?: Function;
}

const PrimaryContactWarningModal: React.FunctionComponent<IPrimaryContactWarningModalProps> = ({
  saveCallback,
  lease,
  onCancel,
}) => {
  const warningOrgs = lease ? getOrgsWithNoPrimaryContact(lease) : [];
  const warningOrgNames = warningOrgs.map(org => org.summary);
  return (
    <GenericModal
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

export const getOrgsWithNoPrimaryContact = (lease: IFormLease): FormTenant[] => {
  return filter(
    lease.tenants.filter((tenant: FormTenant) => {
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
