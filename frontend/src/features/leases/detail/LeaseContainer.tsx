import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import queryString from 'query-string';
import * as React from 'react';
import { ReactElement, useContext } from 'react';
import { useLocation } from 'react-router-dom';

import {
  BackToSearchButton,
  LeaseBreadCrumb,
  LeaseHeader,
  LeaseIndex,
  LeaseLayout,
  LeasePageForm,
  useLeaseDetail,
} from '..';
import Details from './LeasePages/details/Details';
import Tenant from './LeasePages/tenant/Tenant';

export interface ILeaseAndLicenseContainerProps {
  match?: any;
}

export interface ILeasePage {
  component: ReactElement;
  title: string;
  description?: string;
}

export enum LeasePageNames {
  DETAILS = 'details',
  TENANT = 'tenant',
  PAYMENTS = 'payments',
  IMPROVEMENTS = 'improvements',
  INSURANCE = 'insurance',
  DEPOSIT = 'deposit',
  SECURITY = 'security',
  SURPLUS = 'surplus',
}

export const leasePages: Map<LeasePageNames, ILeasePage> = new Map<LeasePageNames, ILeasePage>([
  [LeasePageNames.DETAILS, { component: <Details />, title: 'Details' }],
  [
    LeasePageNames.TENANT,
    {
      component: <Tenant />,
      title: 'Tenant',
      description: 'The following is information related to the leasee or licensee',
    },
  ],
  [LeasePageNames.PAYMENTS, { component: <></>, title: 'Payments' }],
  [LeasePageNames.IMPROVEMENTS, { component: <></>, title: 'Improvements' }],
  [LeasePageNames.INSURANCE, { component: <></>, title: 'Insurance' }],
  [LeasePageNames.DEPOSIT, { component: <></>, title: 'Deposit' }],
  [LeasePageNames.SECURITY, { component: <></>, title: 'Physical Security' }],
  [LeasePageNames.SURPLUS, { component: <></>, title: 'Surplus Declaration' }],
]);

/**
 * Top level container for lease details, provides logic for loading and controlling lease display
 * @param {ILeaseAndLicenseContainerProps} props
 */
export const LeaseContainer: React.FunctionComponent<ILeaseAndLicenseContainerProps> = props => {
  const { setTrayPage } = useContext(SidebarStateContext);
  const onClickManagement = () => setTrayPage(SidebarContextType.LEASE);

  const { search } = useLocation();
  let { leasePageName } = queryString.parse(search);
  if (leasePageName === undefined) {
    leasePageName = LeasePageNames.DETAILS;
  }
  const leasePage = leasePages.get(leasePageName as LeasePageNames);
  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }

  const { lease } = useLeaseDetail(props?.match?.params?.leaseId);

  return (
    <>
      <LeaseLayout>
        <LeaseBreadCrumb
          leaseId={props.match.leaseId}
          leasePage={leasePage}
          onClickManagement={onClickManagement}
        />
        <LeaseHeader lease={lease} />
        <BackToSearchButton />
        <LeaseIndex currentPageName={leasePageName} leaseId={lease?.id}></LeaseIndex>
        <LeasePageForm leasePage={leasePage} lease={lease}></LeasePageForm>
      </LeaseLayout>
      <LoadingBackdrop show={!lease} />
    </>
  );
};

export default LeaseContainer;
