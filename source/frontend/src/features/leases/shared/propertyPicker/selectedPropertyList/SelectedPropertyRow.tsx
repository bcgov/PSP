import { FormikProps, getIn } from 'formik';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { ImUpload } from 'react-icons/im';
import { RiDragMove2Line } from 'react-icons/ri';
import styled from 'styled-components';

import RemoveShapeIcon from '@/assets/images/remove-shape-icon.svg?react';
import { RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import OverflowTip from '@/components/common/OverflowTip';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import AreaContainer from '@/components/measurements/AreaContainer';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { FormLeaseProperty, LeaseFormModel } from '@/features/leases/models';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import { ShapeUploadModal } from '@/features/properties/shapeUpload/ShapeUploadModal';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
import { getPropertyNameFromSelectedFeatureSet, NameSourceType } from '@/utils/mapPropertyUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: PropertyForm;
  formikProps: FormikProps<LeaseFormModel>;
  showSeparator?: boolean;
  canUploadShapefile?: boolean;
  onUploadShapefile?: (result: UploadResponseModel | null) => void;
  onRemoveShapefile?: () => void;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  property,
  formikProps,
  showSeparator = false,
  canUploadShapefile,
  onUploadShapefile,
  onRemoveShapefile,
}) => {
  const hasCustomBoundary = exists(property.fileBoundary);
  const featureSet = property.toFeatureDataset();
  const mapMachine = useMapStateMachine();
  const [isUploadVisible, setIsUploadVisible] = useState(false);
  const propertyName = getPropertyNameFromSelectedFeatureSet(featureSet);
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

  const handleModalUploadClose = (result: UploadResponseModel | null) => {
    setIsUploadVisible(false);
    onUploadShapefile?.(result);
  };

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
        <StyledActionsCol xs="auto" className="pl-3">
          <StyledIconButton
            title="move-pin-location"
            onClick={() => {
              mapMachine.startReposition(featureSet, index);
            }}
          >
            <RiDragMove2Line size={22} />
          </StyledIconButton>
          <ZoomToLocation geometry={featureSet.pimsFeature.geometry} icon={ZoomIconType.single} />
          {canUploadShapefile && !hasCustomBoundary && (
            <TooltipWrapper tooltip="Upload shapefile" tooltipId={'upload-shapefile-' + index}>
              <StyledIconButton
                data-testid={'upload-shapefile-' + index}
                onClick={() => setIsUploadVisible(true)}
              >
                <ImUpload size={18} />
              </StyledIconButton>
            </TooltipWrapper>
          )}
          {canUploadShapefile && hasCustomBoundary && (
            <TooltipWrapper tooltip="Remove shape" tooltipId={'remove-shape-' + index}>
              <StyledIconButton data-testid={'remove-shape-' + index} onClick={onRemoveShapefile}>
                <RemoveShapeIcon width="1.8rem" height="1.8rem" />
              </StyledIconButton>
            </TooltipWrapper>
          )}
          <StyledSpacingWrapper>
            <RemoveButton
              onRemove={onRemove}
              fontSize="1.4rem"
              data-testId={'delete-property-' + index}
            />
          </StyledSpacingWrapper>
        </StyledActionsCol>
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

      {canUploadShapefile && (
        <ShapeUploadModal
          display={isUploadVisible}
          setDisplay={setIsUploadVisible}
          onClose={handleModalUploadClose}
          propertyIdentifier={propertyIdentifier}
        />
      )}
    </>
  );
};

export default SelectedPropertyRow;

const StyledActionsCol = styled(Col)`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-end;
`;

const StyledSpacingWrapper = styled.div`
  padding-left: 1.2rem;
`;
