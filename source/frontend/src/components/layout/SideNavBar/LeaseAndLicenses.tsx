import { FaFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
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
        <Styled.TrayHeader>
          <span className="mr-2">
            <LeaseIcon title="Lease and Licence Icon" fill="currentColor" />
          </span>
          Leases & Licences
        </Styled.TrayHeader>
        {hasClaim(Claims.LEASE_VIEW) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/lease/list">
            Manage Lease/Licence Files
          </Link>
        )}
        {hasClaim(Claims.LEASE_ADD) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/lease/new">
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
