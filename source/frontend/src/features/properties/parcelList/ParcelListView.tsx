import { useMemo } from 'react';
import styled from 'styled-components';
import { v4 as uuidv4 } from 'uuid';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';

import { LocationDatasetWithId } from '../worklist/context/WorklistContext';
import ParcelItem from './ParcelItem';

export interface ISearchItemListViewProps {
  parcels: LocationFeatureDataset[];
}

export const SearchItemListView: React.FC<ISearchItemListViewProps> = ({ parcels }) => {
  //TODO: This is kind of awkward. The Id is only used for the react key
  const parcelsWithId = useMemo<LocationDatasetWithId[]>(
    () => parcels.map(x => ({ ...x, id: uuidv4() })),
    [parcels],
  );

  if (parcelsWithId.length === 0) {
    return <StyledSection className="p-0">No properties to show</StyledSection>;
  }

  return (
    <StyledContainer>
      <StyledHeader>
        <StyledSpan>
          {parcelsWithId.length}
          {parcelsWithId.length > 1 ? ' properties' : ' property'}
        </StyledSpan>
      </StyledHeader>
      {parcelsWithId.map((p, index) => (
        <ParcelItem
          key={p.id}
          parcel={p}
          onRemove={null}
          canAddToWorklist={true}
          parcelIndex={index}
        />
      ))}
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

  position: sticky;
  top: 0; /* pin to the top of the scrolling container   */
  z-index: 1; /* sit above the rows                           */
  background: '#fff';
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
