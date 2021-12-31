import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { getIn } from 'formik';
import * as React from 'react';
import { ReactNode } from 'react';
import { useContext } from 'react';
import { useLocation } from 'react-router-dom';
import * as Yup from 'yup';

import {
  BackToSearchButton,
  LeaseBreadCrumb,
  LeaseHeader,
  LeaseIndex,
  LeaseLayout,
  LeasePageForm,
  LeaseRouter,
  useLeaseDetail,
} from '..';
import { LeaseSchema } from '../add/AddLeaseYupSchema';
import LeaseEditButton from './LeaseEditButton';
import Deposits from './LeasePages/deposits/Deposits';
import DetailContainer from './LeasePages/details/DetailContainer';
import Improvements from './LeasePages/improvements/Improvements';
import InsuranceContainer from './LeasePages/insurance/InsuranceContainer';
import Surplus from './LeasePages/surplus/Surplus';
import TenantContainer from './LeasePages/tenant/TenantContainer';

export interface ILeaseAndLicenseContainerProps {
  match?: any;
}

export interface ILeasePage {
  component: any;
  title: string;
  header?: ReactNode;
  description?: string;
  validation?: Yup.ObjectSchema<any>;
}

export enum LeasePageNames {
  DETAILS = 'details',
  TENANT = 'tenant',
  EDIT_TENANT = 'edit-tenant',
  PAYMENTS = 'payments',
  IMPROVEMENTS = 'improvements',
  INSURANCE = 'insurance',
  DEPOSIT = 'deposit',
  SECURITY = 'security',
  SURPLUS = 'surplus',
}

export const leasePages: Map<LeasePageNames, ILeasePage> = new Map<LeasePageNames, ILeasePage>([
  [
    LeasePageNames.DETAILS,
    {
      component: DetailContainer,
      title: 'Details',
      header: (
        <>
          Details&nbsp;
          <LeaseEditButton linkTo="?edit=true" />
        </>
      ),
      validation: LeaseSchema,
    },
  ],
  [
    LeasePageNames.TENANT,
    {
      component: TenantContainer,
      title: 'Tenant',
      header: (
        <>
          Tenant&nbsp;
          <LeaseEditButton linkTo="?edit=true" />
        </>
      ),
      description: 'The following is information related to the leasee or licensee',
    },
  ],
  [LeasePageNames.PAYMENTS, { component: undefined, title: 'Payments' }],
  [LeasePageNames.IMPROVEMENTS, { component: Improvements, title: 'Improvements' }],
  [LeasePageNames.INSURANCE, { component: InsuranceContainer, title: 'Insurance' }],
  [LeasePageNames.DEPOSIT, { component: Deposits, title: 'Deposit' }],
  [LeasePageNames.SECURITY, { component: undefined, title: 'Physical Security' }],
  [LeasePageNames.SURPLUS, { component: Surplus, title: 'Surplus Declaration' }],
]);

/**
 * Top level container for lease details, provides logic for loading and controlling lease display
 * @param {ILeaseAndLicenseContainerProps} props
 */
export const LeaseContainer: React.FunctionComponent<ILeaseAndLicenseContainerProps> = props => {
  const { setTrayPage } = useContext(SidebarStateContext);
  const onClickManagement = () => setTrayPage(SidebarContextType.LEASE);

  const { pathname } = useLocation();
  const { lease, setLease, refresh } = useLeaseDetail(props?.match?.params?.leaseId);
  const leasePageName = getLeasePageFromPath(pathname, props.match.url);
  const leasePage = leasePages.get(leasePageName);

  if (!leasePage) {
    throw Error('The requested lease page does not exist');
  }
  return (
    <>
      <LeaseLayout>
        <LeaseBreadCrumb
          leaseId={lease?.id}
          leasePage={leasePage}
          onClickManagement={onClickManagement}
        />
        <LeaseHeader lease={lease} />
        <BackToSearchButton />
        <LeaseIndex currentPageName={leasePageName} leaseId={lease?.id}></LeaseIndex>
        <LeasePageForm
          refreshLease={refresh}
          leasePage={leasePage}
          lease={lease}
          setLease={setLease}
        >
          <LeaseRouter />
        </LeasePageForm>
      </LeaseLayout>
      <LoadingBackdrop show={!!props?.match?.params?.leaseId && !lease} />
    </>
  );
};

const getLeasePageFromPath = (pathname: string, url: string) => {
  const leasePageName = getIn(pathname.match(/\/lease\/.*?\/(.*)/), '1');
  if (!leasePageName) {
    return LeasePageNames.DETAILS;
  }
  return leasePageName;
};

export default LeaseContainer;
