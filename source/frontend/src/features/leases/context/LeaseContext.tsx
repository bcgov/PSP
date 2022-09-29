import { ILease } from 'interfaces';
import { noop } from 'lodash';
import * as React from 'react';
import { useState } from 'react';

export interface ILeaseState {
  lease?: ILease;
  setLease: (lease: ILease) => void;
}

export const LeaseStateContext = React.createContext<ILeaseState>({
  lease: undefined,
  setLease: noop,
});

export const LeaseContextProvider = (props: { children?: any; initialLease?: ILease }) => {
  const [lease, setLease] = useState<ILease | undefined>(props.initialLease);

  return (
    <LeaseStateContext.Provider value={{ lease, setLease }}>
      {props.children}
    </LeaseStateContext.Provider>
  );
};
