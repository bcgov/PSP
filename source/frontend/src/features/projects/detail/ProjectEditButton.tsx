import * as React from 'react';
import { FaEdit } from 'react-icons/fa';

import { LinkButton } from '@/components/common/buttons';
import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';

interface IProjectEditButtonProps {
  onEdit?: () => void;
  isEditing: boolean;
}

export const ProjectEditButton: React.FunctionComponent<
  React.PropsWithChildren<IProjectEditButtonProps>
> = ({ onEdit, isEditing }) => {
  return (
    <ProtectedComponent hideIfNotAuthorized claims={[Claims.PROJECT_EDIT]}>
      {!isEditing && !!onEdit && (
        <LinkButton onClick={onEdit} className="float-right">
          <FaEdit size={'2rem'} />
        </LinkButton>
      )}
    </ProtectedComponent>
  );
};

export default ProjectEditButton;
