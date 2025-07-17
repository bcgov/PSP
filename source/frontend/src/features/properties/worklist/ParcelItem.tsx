import { Col, Row } from 'react-bootstrap';
import { FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, RemoveIconButton } from '@/components/common/buttons';
import OverflowTip from '@/components/common/OverflowTip';
import { getWorklistPropertyName, NameSourceType } from '@/utils';

import { ParcelFeature } from './models';

export interface IParcelItemProps {
  parcel: ParcelFeature;
  isSelected: boolean;
  onSelect: (id: string) => void;
  onRemove: (id: string) => void;
  onZoomToParcel: (parcel: ParcelFeature) => void;
}

export function ParcelItem({ parcel, onSelect, onRemove, onZoomToParcel }: IParcelItemProps) {
  const propertyName = getWorklistPropertyName(parcel);
  let propertyIdentifier = '';
  switch (propertyName.label) {
    case NameSourceType.PID:
    case NameSourceType.PIN:
    case NameSourceType.PLAN:
    case NameSourceType.ADDRESS:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.LOCATION:
      propertyIdentifier = `${propertyName.value}`;
      break;
    default:
      break;
  }

  return (
    <StyledRow onClick={() => onSelect(parcel.id)}>
      <StyledPidCol>
        <StyledOverflowTip fullText={propertyIdentifier}></StyledOverflowTip>
      </StyledPidCol>
      <StyledButtonCol>
        <ButtonContainer>
          <LinkButton
            title="Zoom to worklist parcel"
            onClick={e => {
              e.preventDefault();
              onZoomToParcel(parcel);
            }}
          >
            <FaSearchPlus size={18} />
          </LinkButton>
          <RemoveIconButton
            title="Delete worklist parcel"
            data-testId={`delete-worklist-parcel-${parcel.id ?? 'unknown'}`}
            onRemove={e => {
              e.stopPropagation();
              onRemove(parcel.id);
            }}
          />
        </ButtonContainer>
      </StyledButtonCol>
    </StyledRow>
  );
}

export default ParcelItem;

const StyledRow = styled(Row)`
  display: flex;
  align-items: center;
  margin-left: 0;
  margin-right: 0;
  min-height: 4.5rem;

  &:hover {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'};
  }
`;

const StyledOverflowTip = styled(OverflowTip)`
  font-size: 1.4rem;
  font-weight: 700;
  color: ${props => props.theme.css.pimsBlue200};
`;

const StyledPidCol = styled(Col)`
  display: flex;
  justify-content: flex-start;
  padding-left: 3rem;
  padding-right: 0;
`;

const StyledButtonCol = styled(Col)`
  width: 10rem;
  flex: 0 0 10rem; /* Prevents shrinking/growing */
  display: flex;
  justify-content: flex-end;
`;

const ButtonContainer = styled.div`
  display: none;
  gap: 0.5rem;
  align-items: center;

  ${StyledRow}:hover & {
    display: flex;
  }
`;
