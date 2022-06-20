import * as React from 'react';
import styled from 'styled-components';

interface ILeaseLayoutProps {}

/**
 * layout wrapper for leases, applies styling.
 * @param {ILeaseLayoutProps} param0
 */
export const LeaseLayout: React.FunctionComponent<ILeaseLayoutProps> = ({ children }) => {
  return <DetailScreenGrid>{children}</DetailScreenGrid>;
};

const DetailScreenGrid = styled.div`
  width: 100%;
  grid: 4rem 7.5rem 5rem 1fr / minmax(22rem, fit-content) 1fr;
  grid-template-areas:
    'breadcrumb breadcrumb'
    'leaseheader  leaseheader'
    'backbutton .'
    'leaseindex leasecontent';
  display: grid;
  height: 100%;
  overflow: auto;
  padding: 1rem;
`;

export default LeaseLayout;
