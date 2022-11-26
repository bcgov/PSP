import * as React from 'react';

import { LeaseContainer } from '..';
import { LeaseContextProvider } from '../context/LeaseContext';

interface ILeaseContainerWrapperProps {
  match?: any;
}

export const LeaseContainerWrapper: React.FunctionComponent<
  React.PropsWithChildren<ILeaseContainerWrapperProps>
> = ({ match }) => {
  return (
    <LeaseContextProvider>
      <LeaseContainer match={match} />
    </LeaseContextProvider>
  );
};

export default LeaseContainerWrapper;
