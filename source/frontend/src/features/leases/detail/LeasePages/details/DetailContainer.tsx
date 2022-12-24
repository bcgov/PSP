import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import { UpdateLeaseContainer } from 'features/leases/detail/LeasePages/details/UpdateLeaseContainer';
import * as React from 'react';

import Details from './Details';

interface IDetailContainerProps {
  isEditing?: boolean;
}

const DetailContainer: React.FunctionComponent<
  React.PropsWithChildren<IDetailContainerProps>
> = props => {
  return !!props.isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <UpdateLeaseContainer />
    </ProtectedComponent>
  ) : (
    <Details />
  );
};

export default DetailContainer;
