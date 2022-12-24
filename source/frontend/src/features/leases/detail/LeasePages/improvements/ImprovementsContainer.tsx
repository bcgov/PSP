import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import * as React from 'react';

import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

interface IImprovementsContainerProps {
  isEditing?: boolean;
}

export const ImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<IImprovementsContainerProps>
> = props => {
  return !!props.isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer />
    </ProtectedComponent>
  ) : (
    <Improvements disabled={true} />
  );
};

export default ImprovementsContainer;
