import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';

import { ParcelDataset } from './models';
import ParcelItem from './ParcelItem';

export interface IParcelListViewProps {
  parcels: ParcelDataset[];
}

export const ParcelListView: React.FC<IParcelListViewProps> = ({ parcels }) => {
  if (parcels.length > 0) {
    return (
      <StyledContainer>
        <StyledHeader>
          <StyledSpan>
            {parcels.length}
            {parcels.length > 1 ? ' properties' : ' property'}
          </StyledSpan>
        </StyledHeader>
        {parcels.map((p, index) => (
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
  } else {
    return <StyledSection className="p-0">No properties to show</StyledSection>;
  }
};

const StyledContainer = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const StyledHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;

  position: sticky;
  top: 0;
  z-index: 1;
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
