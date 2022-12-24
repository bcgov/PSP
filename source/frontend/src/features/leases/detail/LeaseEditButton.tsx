import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import queryString from 'query-string';
import * as React from 'react';
import { FaEdit } from 'react-icons/fa';
import { Link, useLocation } from 'react-router-dom';

interface ILeaseEditButtonProps {
  linkTo: string;
}

export const LeaseEditButton: React.FunctionComponent<
  React.PropsWithChildren<ILeaseEditButtonProps>
> = ({ linkTo }) => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  return (
    <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
      {!edit && (
        <Link to={location + '/edit'} className="float-right">
          <FaEdit />
        </Link>
      )}
    </ProtectedComponent>
  );
};

export default LeaseEditButton;
