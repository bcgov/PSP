import { ProtectedComponent } from 'components/common/ProtectedComponent';
import { SidebarStateContext } from 'components/layout/SideNavBar/SideNavbarContext';
import { SidebarContextType } from 'components/layout/SideNavBar/SideTray';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/claims';
import { getIn } from 'formik';
import * as React from 'react';
import { ReactNode } from 'react';
import { useContext } from 'react';
import { useLocation } from 'react-router-dom';
import * as Yup from 'yup';

import {
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
import DepositsContainer from './LeasePages/deposits/DepositsContainer';
import DetailContainer from './LeasePages/details/DetailContainer';
import DocumentsPage from './LeasePages/documents/DocumentsPage';
import ImprovementsContainer from './LeasePages/improvements/ImprovementsContainer';
import InsuranceContainer from './LeasePages/insurance/InsuranceContainer';
import PaymentsContainer from './LeasePages/payment/TermPaymentsContainer';
import { TermPaymentsYupSchema } from './LeasePages/payment/TermPaymentsYupSchema';
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
  claims?: string[] | string;
}

export enum LeasePageNames {
  DETAILS = 'details',
  TENANT = 'tenant',
  EDIT_TENANT = 'edit-tenant',
  PAYMENTS = 'payments',
  IMPROVEMENTS = 'improvements',
  INSURANCE = 'insurance',
  DEPOSIT = 'deposit',
  SURPLUS = 'surplus',
  DOCUMENTS = 'documents',
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
          Tenant
          <LeaseEditButton linkTo="?edit=true" />
        </>
      ),
      description: 'The following is information related to the leasee or licensee',
    },
  ],
  [
    LeasePageNames.PAYMENTS,
    {
      component: PaymentsContainer,
      title: 'Payments',
      validation: TermPaymentsYupSchema,
    },
  ],
  [
    LeasePageNames.IMPROVEMENTS,
    {
      component: ImprovementsContainer,
      title: 'Improvements',
      header: (
        <>
          Improvements
          <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
            <LeaseEditButton linkTo="?edit=true" />
          </ProtectedComponent>
        </>
      ),
    },
  ],
  [
    LeasePageNames.INSURANCE,
    {
      component: InsuranceContainer,
      title: 'Insurance',
      header: (
        <>
          Insurance
          <ProtectedComponent hideIfNotAuthorized claims={[Claims.LEASE_EDIT]}>
            <LeaseEditButton linkTo="?edit=true" />
          </ProtectedComponent>
        </>
      ),
    },
  ],
  [LeasePageNames.DEPOSIT, { component: DepositsContainer, title: 'Deposit' }],
  [LeasePageNames.SURPLUS, { component: Surplus, title: 'Surplus Declaration' }],
  [
    LeasePageNames.DOCUMENTS,
    { component: DocumentsPage, title: 'Documents', claims: Claims.DOCUMENT_VIEW },
  ],
]);

/**
 * Top level container for lease details, provides logic for loading and controlling lease display
 * @param {ILeaseAndLicenseContainerProps} props
 */
export const LeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<ILeaseAndLicenseContainerProps>
> = props => {
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
        <LeaseIndex currentPageName={leasePageName} leaseId={lease?.id}></LeaseIndex>
        <LeasePageForm
          refreshLease={refresh}
          leasePage={leasePage}
          lease={lease}
          setLease={setLease}
        >
          <LeaseRouter />
        </LeasePageForm>
        <LoadingBackdrop show={!!props?.match?.params?.leaseId && !lease} />
      </LeaseLayout>
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
