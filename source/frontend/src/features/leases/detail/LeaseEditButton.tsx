import * as React from 'react';
import { FaEdit } from 'react-icons/fa';

import { LinkButton } from '@/components/common/buttons';
import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';

interface ILeaseEditButtonProps {
  onEdit?: () => void;
  isEditing: boolean;
}

export const LeaseEditButton: React.FunctionComponent<
  React.PropsWithChildren<ILeaseEditButtonProps>
> = ({ onEdit, isEditing }) => {
  return (
    <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
      {!isEditing && !!onEdit && (
        <LinkButton onClick={onEdit} className="float-right">
          <FaEdit size={'2rem'} />
        </LinkButton>
      )}
    </ProtectedComponent>
  );
};

export default LeaseEditButton;
