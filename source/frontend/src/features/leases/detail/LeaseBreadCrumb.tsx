import * as CommonStyled from 'components/common/styles';
import * as React from 'react';
import { Breadcrumb } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import { ILeasePage } from './LeaseContainer';

interface ILeaseAndLicenseBreadCrumbProps {
  leaseId?: number;
  leasePage?: ILeasePage;
  onClickManagement: () => void;
}

/**
 * breadcrumb navigation for leases pages.
 * @param {ILeaseAndLicenseBreadCrumbProps} param0
 */
export const LeaseBreadCrumb: React.FunctionComponent<
  React.PropsWithChildren<ILeaseAndLicenseBreadCrumbProps>
> = ({ leaseId, leasePage, onClickManagement }) => {
  return (
    <CommonStyled.Breadcrumb>
      <Breadcrumb.Item onClick={onClickManagement}>Management</Breadcrumb.Item>
      {/* Render link only if leaseId is available */}
      {leaseId && (
        <Breadcrumb.Item linkAs={Link} linkProps={{ to: '/lease/list' }}>
          Lease &amp; License Search
        </Breadcrumb.Item>
      )}
      <Breadcrumb.Item active>{leasePage?.title}</Breadcrumb.Item>
    </CommonStyled.Breadcrumb>
  );
};

export default LeaseBreadCrumb;
