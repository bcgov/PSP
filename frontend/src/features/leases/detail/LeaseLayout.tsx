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
  grid: 4rem 7.5rem 5rem 1fr / 22rem 1fr;
  grid-template-areas:
    'breadcrumb breadcrumb'
    'leaseheader  leaseheader'
    'backbutton .'
    'leaseindex leasecontent';
  display: grid;
  @media (max-width: 35rem) {
    grid-template-rows: minmax(4rem, 1fr) minmax(7.5rem, 2fr) minmax(5rem, 1fr) 10fr;
    grid-template-columns: 22rem 1fr;
    .lease-header {
      flex-wrap: wrap;
      max-height: unset;
    }
  }
  height: 100%;
  padding: 1rem;
`;

export default LeaseLayout;
