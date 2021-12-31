import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import * as React from 'react';
import { FaEdit } from 'react-icons/fa';
import { Link } from 'react-router-dom';

interface ILeaseEditButtonProps {
  linkTo: string;
}

export const LeaseEditButton: React.FunctionComponent<ILeaseEditButtonProps> = ({ linkTo }) => {
  return (
    <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
      <Link to={linkTo} className="float-right">
        <FaEdit />
      </Link>
    </ProtectedComponent>
  );
};

export default LeaseEditButton;
