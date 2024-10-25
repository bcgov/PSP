import { FaFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import ExportAggregatedLeasesContainer from '@/features/leases/reports/aggregated/ExportAggregatedLeasesContainer';
import ExportLeasePaymentsContainer from '@/features/leases/reports/payments/ExportLeasePaymentsContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
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
        {hasClaim(Claims.LEASE_VIEW) && (
          <Link onClick={onLinkClick} to="/lease/list" className="nav-item pl-9 pb-3">
            Manage Lease/Licence Files
          </Link>
        )}
        {hasClaim(Claims.LEASE_ADD) && (
          <Link
            onClick={onLinkClick}
            to="/mapview/sidebar/lease/new"
            className="nav-item pl-9 pb-3"
          >
            Create a Lease/Licence File
          </Link>
        )}
      </HalfHeightDiv>
      {hasClaim(Claims.LEASE_VIEW) && (
        <HalfHeightDiv>
          <ExportH3 className="mt-5">
            <span className="mr-4">
              <FaFileExcel />
            </span>
            Exports
          </ExportH3>
          <p className="ml-9 font-weight-bold">Aggregated Lease & Licence Payments</p>
          <ExportAggregatedLeasesContainer />
          <p className="ml-9 font-weight-bold">Lease & Licence Payments by Fiscal Year</p>
          <ExportLeasePaymentsContainer />
        </HalfHeightDiv>
      )}
    </>
  );
};
