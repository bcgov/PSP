import { FormikProps, getIn } from 'formik';
import { geoJSON } from 'leaflet';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaSearchPlus } from 'react-icons/fa';
import { RiDragMove2Line } from 'react-icons/ri';

import { LinkButton, RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import OverflowTip from '@/components/common/OverflowTip';
import AreaContainer from '@/components/measurements/AreaContainer';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { FormLeaseProperty, LeaseFormModel } from '@/features/leases/models';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
import { featuresetToMapProperty, getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: SelectedFeatureDataset;
  formikProps: FormikProps<LeaseFormModel>;
  showSeparator?: boolean;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  property,
  formikProps,
  showSeparator = false,
}) => {
  const mapMachine = useMapStateMachine();
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

  const currentLeaseProperty: FormLeaseProperty = getIn(
    formikProps.values,
    withNameSpace(nameSpace),
  );

  const onZoomToProperty = useCallback(
    (property: SelectedFeatureDataset) => {
      const geom = property?.parcelFeature?.geometry ?? property?.pimsFeature?.geometry;
      const bounds = geoJSON(geom).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        mapMachine.requestFlyToBounds(bounds);
      }
    },
    [mapMachine],
  );

  return (
    <>
      <Row className="align-items-center my-3 no-gutters">
        <Col md={3} className="mb-0 d-flex align-items-center">
          <DraftCircleNumber text={(index + 1).toString()} />
          <OverflowTip fullText={propertyIdentifier} className="pl-3" />
        </Col>
        <Col md={5}>
          <InlineInput
            className="mb-0 w-100 pr-3"
            label=""
            field={withNameSpace(nameSpace, 'name')}
            displayErrorTooltips={true}
            errorKeys={[
              withNameSpace(nameSpace, 'property.isRetired'),
              withNameSpace(nameSpace, 'property.isDisposed'),
            ]}
          />
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
        <Col xs="auto">
          <LinkButton onClick={() => onZoomToProperty(property)}>
            <FaSearchPlus size={18} className="mr-5" />
          </LinkButton>
        </Col>
        <Col md={2}>
          <RemoveButton
            onRemove={onRemove}
            fontSize="1.4rem"
            data-testId={'delete-property-' + index}
          />
        </Col>
      </Row>
      <Row className="align-items-center mb-3 no-gutters">
        <Col md={{ span: 9, offset: 3 }}>
          <AreaContainer
            isEditable
            field={withNameSpace(nameSpace, 'landArea')}
            landArea={currentLeaseProperty.landArea}
            unitCode={currentLeaseProperty.areaUnitTypeCode}
            onChange={(landArea, areaUnitTypeCode) => {
              formikProps.setFieldValue(withNameSpace(nameSpace, 'landArea'), landArea);
              formikProps.setFieldValue(
                withNameSpace(nameSpace, 'areaUnitTypeCode'),
                areaUnitTypeCode,
              );
            }}
          />
        </Col>
      </Row>
      {showSeparator && <hr className="my-3"></hr>}
    </>
  );
};

export default SelectedPropertyRow;
