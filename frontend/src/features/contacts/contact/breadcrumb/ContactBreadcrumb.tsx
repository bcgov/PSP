import * as CommonStyled from 'components/common/styles';
import * as React from 'react';
import { Breadcrumb } from 'react-bootstrap';
import { Link } from 'react-router-dom';

interface IContactAndLicenseBreadCrumbProps {}

/**
 * breadcrumb navigation for Contacts pages.
 * @param {IContactAndLicenseBreadCrumbProps} param0
 */
export const ContactBreadcrumb: React.FunctionComponent<IContactAndLicenseBreadCrumbProps> = props => {
  return (
    <CommonStyled.Breadcrumb>
      <Breadcrumb.Item linkAs={Link} linkProps={{ to: '/contact/list' }}>
        Contact Search
      </Breadcrumb.Item>
      <Breadcrumb.Item active>Contact Details</Breadcrumb.Item>
    </CommonStyled.Breadcrumb>
  );
};

export default ContactBreadcrumb;
