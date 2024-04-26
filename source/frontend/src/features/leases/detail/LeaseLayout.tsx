import styled from 'styled-components';
/**
 * layout wrapper for leases, applies styling.
 * @param {ILeaseLayoutProps} param0
 */
export const LeaseLayout: React.FunctionComponent<React.PropsWithChildren<unknown>> = ({
  children,
}) => {
  return <DetailScreenGrid>{children}</DetailScreenGrid>;
};

const DetailScreenGrid = styled.div`
  width: 100%;
  grid: 4rem minmax(7.5rem, max-content) 5rem 1fr / 22rem 1fr;
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
