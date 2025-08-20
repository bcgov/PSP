import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';

import { ParcelDataset } from './models';
import ParcelItem from './ParcelItem';

export interface IParcelListViewProps {
  parcels: ParcelDataset[];
}

export const ParcelListView: React.FC<IParcelListViewProps> = ({ parcels }) => {
  if (parcels.length === 0) {
    return <StyledSection>No properties to show</StyledSection>;
  }

  return (
    <StyledContainer>
      <StyledHeader>
        <StyledSpan>
          {parcels.length}
          {parcels.length > 1 ? ' properties' : ' property'}
        </StyledSpan>
      </StyledHeader>
      {parcels.map(p => (
        <ParcelItem key={p.id} parcel={p} onRemove={null} canAddToWorklist={true} />
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
  padding-bottom: 1rem;

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
