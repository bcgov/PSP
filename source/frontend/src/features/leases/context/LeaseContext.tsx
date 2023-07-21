import { noop } from 'lodash';
import * as React from 'react';
import { useState } from 'react';

import { Api_Lease } from '@/models/api/Lease';

export interface ILeaseState {
  lease?: Api_Lease;
  setLease: (lease: Api_Lease) => void;
}

export const LeaseStateContext = React.createContext<ILeaseState>({
  lease: undefined,
  setLease: noop,
});

export const LeaseContextProvider = (props: { children?: any; initialLease?: Api_Lease }) => {
  const [lease, setLease] = useState<Api_Lease | undefined>(props.initialLease);

  return (
    <LeaseStateContext.Provider value={{ lease, setLease }}>
      {props.children}
    </LeaseStateContext.Provider>
  );
};
