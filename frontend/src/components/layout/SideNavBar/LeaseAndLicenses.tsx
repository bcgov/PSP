import { Claims } from 'constants/claims';
import { LeaseH3 } from 'features/leases/detail/styles';
import ExportAggregatedLeasesContainer from 'features/leases/reports/aggregated/ExportAggregatedLeasesContainer';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { FaRegFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

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
          <Link onClick={onLinkClick} to="/lease/new">
            Create a Lease/License File
          </Link>
        )}
      </HalfHeightDiv>
      {hasClaim(Claims.LEASE_VIEW) && (
        <HalfHeightDiv>
          <LeaseH3>
            <FaRegFileExcel /> Exports
          </LeaseH3>
          <p>Aggregated Lease & License Payments</p>
          <ExportAggregatedLeasesContainer />
        </HalfHeightDiv>
      )}
    </>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
