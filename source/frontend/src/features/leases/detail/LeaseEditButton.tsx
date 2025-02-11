import { FaEdit } from 'react-icons/fa';

import { LinkButton } from '@/components/common/buttons';
import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';

interface ILeaseEditButtonProps {
  onEdit?: () => void;
  isEditing: boolean;
  pageName: string;
}

export const LeaseEditButton: React.FunctionComponent<
  React.PropsWithChildren<ILeaseEditButtonProps>
> = ({ onEdit, isEditing, pageName }) => {
  return (
    <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
      {!isEditing && !!onEdit && (
        <LinkButton id={`edit-${pageName}-btn`} onClick={onEdit} className="float-right">
          <FaEdit size={22} title="lease-edit" />
        </LinkButton>
      )}
    </ProtectedComponent>
  );
};

export default LeaseEditButton;
