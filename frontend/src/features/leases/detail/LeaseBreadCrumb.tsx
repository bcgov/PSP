import * as React from 'react';
import { Breadcrumb } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import { ILeasePage } from './LeaseContainer';
import * as Styled from './styles';

interface ILeaseAndLicenseBreadCrumbProps {
  leaseId?: number;
  leasePage?: ILeasePage;
  onClickManagement: () => void;
}

/**
 * breadcrumb navigation for leases pages.
 * @param {ILeaseAndLicenseBreadCrumbProps} param0
 */
export const LeaseBreadCrumb: React.FunctionComponent<ILeaseAndLicenseBreadCrumbProps> = ({
  leaseId,
  leasePage,
  onClickManagement,
}) => {
  return (
    <Styled.LeaseBreadcrumb>
      <Breadcrumb.Item onClick={onClickManagement}>Management</Breadcrumb.Item>
      <Breadcrumb.Item linkAs={Link} linkProps={{ to: '/lease/list' }}>
        Lease & License Search
      </Breadcrumb.Item>
      <Breadcrumb.Item
        active
        linkAs={Link}
        linkProps={{ to: `/lease/${leaseId}?leasePage=${leasePage?.title}` }}
      >
        {leasePage?.title}
      </Breadcrumb.Item>
    </Styled.LeaseBreadcrumb>
  );
};

export default LeaseBreadCrumb;
