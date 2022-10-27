import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import queryString from 'query-string';
import * as React from 'react';
import { useLocation } from 'react-router-dom';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';
import { Tenant } from './Tenant';
interface ITenantContainerProps {}

const TenantContainer: React.FunctionComponent<ITenantContainerProps> = props => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  return !!edit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddLeaseTenantContainer />
    </ProtectedComponent>
  ) : (
    <Tenant />
  );
};

export default TenantContainer;
