import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import { UpdateLeaseContainer } from 'features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import queryString from 'query-string';
import * as React from 'react';
import { useLocation } from 'react-router-dom';

import Details from './Details';

interface IDetailContainerProps {}

const DetailContainer: React.FunctionComponent<IDetailContainerProps> = props => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  return !!edit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer />
    </ProtectedComponent>
  ) : (
    <Details />
  );
};

export default DetailContainer;
