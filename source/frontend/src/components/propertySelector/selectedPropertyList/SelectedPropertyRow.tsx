import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { RiDragMove2Line } from 'react-icons/ri';
import styled from 'styled-components';

import { RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { Select } from '@/components/common/form';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import OverflowTip from '@/components/common/OverflowTip';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { withNameSpace } from '@/utils/formUtils';
import { featuresetToMapProperty, getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

import DisabledDraftCircleNumber from './DisabledDraftCircleNumber';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  showDisable?: boolean;
  property: SelectedFeatureDataset;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  showDisable,
  property,
}) => {
  const mapMachine = useMapStateMachine();
  const { setFieldTouched, touched } = useFormikContext();
  useEffect(() => {
    if (getIn(touched, `${nameSpace}.name`) !== true) {
      setFieldTouched(`${nameSpace}.name`);
    }
  }, [nameSpace, setFieldTouched, touched]);

  const propertyName = getPropertyName(featuresetToMapProperty(property));
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
      propertyIdentifier = '';
      break;
  }
  return (
    <StyledRow className="align-items-center mb-3 no-gutters">
      <Col md={3}>
        <div className="mb-0 d-flex align-items-center">
          {property.isActive === false ? (
            <DisabledDraftCircleNumber text={(index + 1).toString()} />
          ) : (
            <DraftCircleNumber text={(index + 1).toString()} />
          )}
          <OverflowTip fullText={propertyIdentifier} className="pl-3"></OverflowTip>
        </div>
      </Col>
      <Col md={5}>
        <InlineInput
          className="mb-0 w-100"
          label=""
          field={withNameSpace(nameSpace, 'name')}
          displayErrorTooltips={true}
          defaultValue=""
          errorKeys={[withNameSpace(nameSpace, 'isRetired')]}
        />
      </Col>
      {showDisable && (
        <Col md={2}>
          <Select
            className="mb-0 ml-4"
            field={withNameSpace(nameSpace, 'isActive')}
            options={[
              { label: 'Inactive', value: 'false' },
              { label: 'Active', value: 'true' },
            ]}
          ></Select>
        </Col>
      )}
      <Col md={1} className="pl-3">
        <StyledIconButton
          title="move-pin-location"
          onClick={() => {
            mapMachine.startReposition(property, index);
          }}
        >
          <RiDragMove2Line size={22} />
        </StyledIconButton>
      </Col>
      <Col md={1}>
        <RemoveButton onRemove={onRemove} fontSize="1.4rem" />
      </Col>
    </StyledRow>
  );
};

const StyledRow = styled(Row)`
  min-height: 4.5rem;
`;

export default SelectedPropertyRow;
