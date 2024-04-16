import * as React from 'react';
import styled from 'styled-components';

/**
 * layout wrapper for leases, applies styling.
 * @param {IAddLeaseLayoutProps} param0
 */
export const AddLeaseLayout: React.FunctionComponent<React.PropsWithChildren<unknown>> = ({
  children,
}) => {
  return <DetailScreenGrid>{children}</DetailScreenGrid>;
};

const DetailScreenGrid = styled.div`
  width: 100%;
  grid: 4rem 7.5rem 1fr / 22rem 1fr;
  grid-template-areas:
    'breadcrumb breadcrumb'
    'leaseheader  leaseheader'
    'leaseindex leasecontent';
  display: grid;
  @media (max-width: 560px) {
    grid-template-rows: minmax(4rem, 1fr) minmax(7.5rem, 2fr) minmax(5rem, 1fr) 10fr;
    grid-template-columns: 22rem 1fr;
    .lease-header {
      flex-wrap: wrap;
      max-height: unset;
    }
  }
  height: 100%;
  overflow: auto;
  padding: 1rem;
`;

export default AddLeaseLayout;
