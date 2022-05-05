import { GenericModal } from 'components/common/GenericModal';
import { IFormLease } from 'interfaces/ILease';
import filter from 'lodash/filter';
import * as React from 'react';

import { FormTenant } from './Tenant';

interface IPrimaryContactWarningModalProps {
  display?: Function;
  setDisplay: (display: Function | undefined) => void;
  lease: IFormLease | undefined;
  onSave?: Function;
  onCancel?: Function;
}

const PrimaryContactWarningModal: React.FunctionComponent<IPrimaryContactWarningModalProps> = ({
  display,
  setDisplay,
  lease,
  onCancel,
  onSave,
}) => {
  const warningOrgs = lease ? getOrgsWithNoPrimaryContact(lease) : [];
  const warningOrgNames = warningOrgs.map(org => org.summary);
  return (
    <GenericModal
      display={!!display}
      setDisplay={() => setDisplay(undefined)}
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
      handleOk={() => display && display()}
    />
  );
};

export const getOrgsWithNoPrimaryContact = (lease: IFormLease): FormTenant[] => {
  console.log(lease.tenants);
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
