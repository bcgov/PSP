import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import { AddLeaseLayout, LeaseBreadCrumb, LeaseHeader, LeaseIndex } from 'features/leases';
import * as React from 'react';
import { useContext } from 'react';

import { LeasePageNames, leasePages } from '../detail/LeaseContainer';

interface ILeaseDetailContainerProps {}

export const LeaseDetailContainer: React.FunctionComponent<ILeaseDetailContainerProps> = props => {
  const leasePage = leasePages.get(LeasePageNames.DETAILS);
  const { setTrayPage } = useContext(SidebarStateContext);
  const onClickManagement = () => setTrayPage(SidebarContextType.LEASE);
  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }

  return (
    <>
      <AddLeaseLayout>
        <LeaseBreadCrumb leasePage={leasePage} onClickManagement={onClickManagement} />
        <LeaseHeader />
        <LeaseIndex currentPageName={LeasePageNames.DETAILS}></LeaseIndex>
        <p>Add Lease Placeholder</p>
      </AddLeaseLayout>
    </>
  );
};

export default LeaseDetailContainer;
