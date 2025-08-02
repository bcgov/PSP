import styled from 'styled-components';

import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { exists } from '@/utils';

import { ParcelFeature } from './models';
import MoreOptionsDropdown from './MoreOptionsDropdown';
import ParcelItem from './ParcelItem';

export interface IWorklistViewProps {
  parcels: ParcelFeature[];
  selectedId: string | null;
  onSelect: (id: string) => void;
  onRemove: (id: string) => void;
  onZoomToParcel: (parcel: ParcelFeature) => void;
  onClearAll: () => void;
  onCreateResearchFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateAcquisitionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateDispositionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateLeaseFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateManagementFile: (event: React.MouseEvent<HTMLElement>) => void;
}

export const WorklistView: React.FC<IWorklistViewProps> = ({
  parcels,
  selectedId,
  onSelect,
  onRemove,
  onZoomToParcel,
  onClearAll,
  onCreateResearchFile,
  onCreateAcquisitionFile,
  onCreateDispositionFile,
  onCreateLeaseFile,
  onCreateManagementFile,
}) => {
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
        <MoreOptionsDropdown
          canClearAll={parcels?.length > 0}
          onClearAll={onClearAll}
          onCreateResearchFile={onCreateResearchFile}
          onCreateAcquisitionFile={onCreateAcquisitionFile}
          onCreateDispositionFile={onCreateDispositionFile}
          onCreateLeaseFile={onCreateLeaseFile}
          onCreateManagementFile={onCreateManagementFile}
        />
      </StyledHeader>
      <ScrollArea>
        {parcels.map(p => (
          <ParcelItem
            key={p.id}
            parcel={p}
            isSelected={exists(p.id) && p.id === selectedId}
            onSelect={onSelect}
            onRemove={onRemove}
            onZoomToParcel={onZoomToParcel}
          ></ParcelItem>
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
