import { noop } from 'lodash';
import * as React from 'react';
import { useState } from 'react';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

export interface ILeaseState {
  lease?: ApiGen_Concepts_Lease;
  setLease: (lease: ApiGen_Concepts_Lease) => void;
}

export const LeaseStateContext = React.createContext<ILeaseState>({
  lease: undefined,
  setLease: noop,
});

export const LeaseContextProvider = (props: {
  children?: any;
  initialLease?: ApiGen_Concepts_Lease;
}) => {
  const [lease, setLease] = useState<ApiGen_Concepts_Lease | undefined>(props.initialLease);

  return (
    <LeaseStateContext.Provider value={{ lease, setLease }}>
      {props.children}
    </LeaseStateContext.Provider>
  );
};
