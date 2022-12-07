import { ILeasePage, LeasePageForm } from 'features/leases';
import { ILease } from 'interfaces';
import React from 'react';

export interface ILeaseTabProps {
  lease?: ILease;
  leasePage?: ILeasePage;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
}

export const LeaseTab: React.FC<ILeaseTabProps> = ({
  lease,
  leasePage,
  refreshLease,
  setLease,
}) => {
  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }

  const Component = leasePage.component;

  return (
    <LeasePageForm
      refreshLease={refreshLease}
      leasePage={leasePage}
      lease={lease}
      setLease={setLease}
    >
      <Component />
    </LeasePageForm>
  );
};
