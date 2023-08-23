import { FaFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import ExportAggregatedLeasesContainer from '@/features/leases/reports/aggregated/ExportAggregatedLeasesContainer';
import ExportLeasePaymentsContainer from '@/features/leases/reports/payments/ExportLeasePaymentsContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';
import { ExportH3, HalfHeightDiv } from './styles';

/**
 * Lease And Licenses Tray page with links to lis functionality.
 * Intended for use within the left side tray.
 */
export const LeaseAndLicenses = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        <Styled.TrayHeader>Management</Styled.TrayHeader>
        {hasClaim(Claims.LEASE_VIEW) && (
          <Link onClick={onLinkClick} to="/lease/list">
            Manage Lease/License Files
          </Link>
        )}
        {hasClaim(Claims.LEASE_ADD) && (
          <Link onClick={onLinkClick} to="/mapview/sidebar/lease/new">
            Create a Lease/License File
          </Link>
        )}
      </HalfHeightDiv>
      {hasClaim(Claims.LEASE_VIEW) && (
        <HalfHeightDiv>
          <ExportH3>
            <FaFileExcel /> Exports
          </ExportH3>
          <p>Aggregated Lease & License Payments</p>
          <ExportAggregatedLeasesContainer />
          <p>Lease and License Payments by Fiscal Year</p>
          <ExportLeasePaymentsContainer />
        </HalfHeightDiv>
      )}
    </>
  );
};
