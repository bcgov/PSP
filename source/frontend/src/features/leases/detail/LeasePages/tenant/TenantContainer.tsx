import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import * as React from 'react';

import { AddLeaseTenantContainer } from './AddLeaseTenantContainer';
import { Tenant } from './Tenant';
interface ITenantContainerProps {
  isEditing?: boolean;
}

const TenantContainer: React.FunctionComponent<
  React.PropsWithChildren<ITenantContainerProps>
> = props => {
  return !!props.isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddLeaseTenantContainer />
    </ProtectedComponent>
  ) : (
    <Tenant />
  );
};

export default TenantContainer;
