import { useState } from 'react';
import styled from 'styled-components';

import { MoreOptionsButton } from '@/components/common/buttons/MoreOptionsButton';
import { Section } from '@/components/common/Section/Section';
import { exists } from '@/utils';

import { ParcelFeature } from './models';
import ParcelItem from './ParcelItem';

export interface IWorklistViewProps {
  parcels: ParcelFeature[];
  selectedId: string | null;
  onSelect: (id: string) => void;
  onRemove: (id: string) => void;
  onZoomToParcel: (parcel: ParcelFeature) => void;
}

export const WorklistView: React.FC<IWorklistViewProps> = ({
  parcels,
  selectedId,
  onSelect,
  onRemove,
  onZoomToParcel,
}) => {
  // open/close "more options" menu
  const [showMoreOptions, setShowMoreOptions] = useState(false);

  const handleMoreOptionsClick = () => {
    setShowMoreOptions(prev => !prev);
  };

  if (parcels.length === 0) {
    return <Section>CTRL + Click to add a property</Section>;
  }

  return (
    <StyledContainer className="p-3">
      <StyledHeader>
        <StyledSpan>
          {parcels.length}
          {parcels.length > 1 ? ' properties' : ' property'}
        </StyledSpan>
        <MoreOptionsButton onClick={handleMoreOptionsClick} />
      </StyledHeader>
      {/* TODO: Implement more-options menu UI */}
      {showMoreOptions && <div>More options go here (clear list, etc)</div>}
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
`;

const StyledSpan = styled.span`
  font-size: 1.3rem;
`;
