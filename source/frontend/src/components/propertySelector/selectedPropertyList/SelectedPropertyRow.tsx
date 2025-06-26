import { getIn, useFormikContext } from 'formik';
import { geoJSON } from 'leaflet';
import { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaSearchPlus } from 'react-icons/fa';
import { RiDragMove2Line } from 'react-icons/ri';

import { LinkButton, RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import OverflowTip from '@/components/common/OverflowTip';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
import { featuresetToMapProperty, getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: SelectedFeatureDataset;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  property,
}) => {
  const mapMachine = useMapStateMachine();
  const { setFieldTouched, touched } = useFormikContext();
  useEffect(() => {
    if (getIn(touched, `${nameSpace}.name`) !== true) {
      setFieldTouched(`${nameSpace}.name`);
    }
  }, [nameSpace, setFieldTouched, touched]);

  const onZoomToProperty = useCallback(() => {
    const geom = property.pimsFeature.geometry;
    const bounds = geoJSON(geom).getBounds();

    if (exists(bounds) && bounds.isValid()) {
      mapMachine.requestFlyToBounds(bounds);
    }
  }, [mapMachine, property.pimsFeature.geometry]);

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
    <Row className="align-items-center mb-3 no-gutters">
      <Col md={3}>
        <div className="mb-0 d-flex align-items-center">
          <DraftCircleNumber text={(index + 1).toString()} />
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
      <Col xs="auto">
        <LinkButton onClick={onZoomToProperty}>
          <FaSearchPlus size={18} className="ml-4" />
        </LinkButton>
      </Col>
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
      <Col md={2}>
        <RemoveButton onRemove={onRemove} fontSize="1.4rem" />
      </Col>
    </Row>
  );
};

export default SelectedPropertyRow;
