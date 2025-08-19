import { useMemo } from 'react';
import { FaPlus } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ParcelDataset } from '../parcelList/models';
import ParcelItem from '../parcelList/ParcelItem';

export interface IWorklistViewProps {
  parcels: ParcelDataset[];
  canAddToOpenFile?: boolean;
  onRemove: (id: string) => void;
  onClearAll: () => void;
  onCreateResearchFile: () => void;
  onCreateAcquisitionFile: () => void;
  onCreateDispositionFile: () => void;
  onCreateLeaseFile: () => void;
  onCreateManagementFile: () => void;
  onAddToOpenFile: () => void;
}

export const WorklistView: React.FC<IWorklistViewProps> = ({
  parcels,
  canAddToOpenFile = false,
  onRemove,
  onClearAll,
  onCreateResearchFile,
  onCreateAcquisitionFile,
  onCreateDispositionFile,
  onCreateLeaseFile,
  onCreateManagementFile,
  onAddToOpenFile,
}) => {
  const keycloak = useKeycloakWrapper();

  const menuOptions: MenuOption[] = useMemo(() => {
    const options: MenuOption[] = [
      {
        label: 'Clear list',
        icon: <MdClose size="1.5rem" />,
        onClick: onClearAll,
        disabled: parcels.length === 0,
      },
    ];

    if (keycloak.hasClaim(Claims.RESEARCH_ADD)) {
      options.push({
        label: 'Create Research File',
        onClick: onCreateResearchFile,
        icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.ACQUISITION_ADD)) {
      options.push({
        label: 'Create Acquisition File',
        onClick: onCreateAcquisitionFile,
        icon: <AcquisitionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.MANAGEMENT_ADD)) {
      options.push({
        label: 'Create Management File',
        onClick: onCreateManagementFile,
        icon: <ManagementIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.LEASE_ADD)) {
      options.push({
        label: 'Create Lease File',
        onClick: onCreateLeaseFile,
        icon: <LeaseIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.DISPOSITION_ADD)) {
      options.push({
        label: 'Create Disposition File',
        onClick: onCreateDispositionFile,
        icon: <DispositionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }

    options.push({
      label: 'Add to Open File',
      onClick: onAddToOpenFile,
      icon: canAddToOpenFile ? <FaPlus size="1.5rem" /> : undefined,
      disabled: !canAddToOpenFile,
      tooltip: 'A file must be open and in "edit property" mode',
      separator: true, // Add a separator before the "Add to Open File" option
    });

    return options;
  }, [
    canAddToOpenFile,
    keycloak,
    onAddToOpenFile,
    onClearAll,
    onCreateAcquisitionFile,
    onCreateDispositionFile,
    onCreateLeaseFile,
    onCreateManagementFile,
    onCreateResearchFile,
    parcels.length,
  ]);

  if (parcels.length === 0) {
    return <StyledSection>CTRL + Click to add a property</StyledSection>;
  }

  return (
    <StyledContainer className="p-3">
      <StyledHeader>
        <StyledSpan>
          {parcels.length}
          {parcels.length > 1 ? ' properties' : ' property'}
        </StyledSpan>
        <MoreOptionsMenu options={menuOptions} ariaLabel="worklist more options" />
      </StyledHeader>
      <ScrollArea>
        {parcels.map(p => (
          <ParcelItem key={p.id} parcel={p} onRemove={onRemove} canAddToWorklist={false} />
        ))}
      </ScrollArea>
    </StyledContainer>
  );
};

const StyledContainer = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%; /* make the flex‑children measure against full height */
`;

const StyledHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding-bottom: 1rem;

  position: sticky;
  top: 0; /* pin to the top of the scrolling container   */
  z-index: 1; /* sit above the rows                           */
  background: '#fff';
`;

const ScrollArea = styled(Scrollable)`
  flex: 1 1 auto; /* consume remaining height in the column       */
  overflow-y: auto; /* provide the scrolling behaviour              */
`;

const StyledSpan = styled.span`
  font-size: 1.3rem;
`;

const StyledSection = styled(Section)`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  margin-top: auto;
  margin-bottom: auto;
`;
