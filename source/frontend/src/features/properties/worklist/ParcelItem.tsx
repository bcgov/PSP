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
      <Col>
        <OverflowTip fullText={propertyIdentifier}></OverflowTip>
      </Col>
      <Col xs="auto">
        <LinkButton
          title="Zoom to worklist parcel"
          onClick={e => {
            e.preventDefault();
            onZoomToParcel(parcel);
          }}
        >
          <FaSearchPlus size={18} />
        </LinkButton>
      </Col>
      <Col xs="auto">
        <RemoveIconButton
          title="Delete worklist parcel"
          data-testId={`delete-worklist-parcel-${parcel.id ?? 'unknown'}`}
          onRemove={e => {
            e.stopPropagation();
            onRemove(parcel.id);
          }}
        />
      </Col>
    </StyledRow>
  );
}

export default ParcelItem;

const StyledRow = styled(Row)`
  width: 100%;
  min-height: 4.5rem;
`;
