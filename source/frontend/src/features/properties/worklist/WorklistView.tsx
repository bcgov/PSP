import styled from 'styled-components';

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
  onClearAll: () => void;
  onZoomToParcel: (parcel: ParcelFeature) => void;
}

export const WorklistView: React.FC<IWorklistViewProps> = ({
  parcels,
  selectedId,
  onSelect,
  onRemove,
  onClearAll,
  onZoomToParcel,
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
        <MoreOptionsDropdown canClearAll={parcels?.length > 0} onClearAll={onClearAll} />
      </StyledHeader>
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
    </StyledContainer>
  );
};

const StyledContainer = styled.div`
  display: flex;
  flex-direction: column;
`;

const StyledHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding-bottom: 1rem;
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
