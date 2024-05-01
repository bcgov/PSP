import { Breadcrumb } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import * as CommonStyled from '@/components/common/styles';

export const ContactBreadcrumb: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
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
