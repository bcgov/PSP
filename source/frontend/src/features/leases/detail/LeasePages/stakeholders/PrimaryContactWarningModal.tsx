import filter from 'lodash/filter';

import { GenericModal } from '@/components/common/GenericModal';

import { FormStakeholder } from './models';

export interface IPrimaryContactWarningModalProps {
  saveCallback?: () => void;
  selectedStakeholders: FormStakeholder[];
  onCancel?: () => void;
}

const PrimaryContactWarningModal: React.FunctionComponent<
  React.PropsWithChildren<IPrimaryContactWarningModalProps>
> = ({ saveCallback, selectedStakeholders: selectedTenants, onCancel }) => {
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

export const getOrgsWithNoPrimaryContact = (tenants: FormStakeholder[]): FormStakeholder[] => {
  return filter(
    tenants?.filter((tenant: FormStakeholder) => {
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
