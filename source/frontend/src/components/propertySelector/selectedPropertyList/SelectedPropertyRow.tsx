import { getIn, useFormikContext } from 'formik';
import { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { ImUpload } from 'react-icons/im';
import { RiDragMove2Line } from 'react-icons/ri';
import styled from 'styled-components';

import RemoveShapeIcon from '@/assets/images/remove-shape-icon.svg?react';
import { RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { Select } from '@/components/common/form';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import OverflowTip from '@/components/common/OverflowTip';
import { TooltipWrapper } from '@/components/common/TooltipWrapper';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import { ShapeUploadModal } from '@/features/properties/shapeUpload/ShapeUploadModal';
import { exists } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
import { getPropertyNameFromSelectedFeatureSet, NameSourceType } from '@/utils/mapPropertyUtils';

import DisabledDraftCircleNumber from './DisabledDraftCircleNumber';

export interface ISelectedPropertyRowProps {
  property: PropertyForm;
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  showDisable?: boolean;
  canUploadShapefile?: boolean;
  onUploadShapefile?: (result: UploadResponseModel | null) => void;
  onRemoveShapefile?: () => void;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  property,
  index,
  nameSpace,
  onRemove,
  showDisable,
  canUploadShapefile,
  onUploadShapefile,
  onRemoveShapefile,
}) => {
  const hasCustomBoundary = exists(property.fileBoundary);
  const featureSet = property.toFeatureDataset();
  const mapMachine = useMapStateMachine();
  const { setFieldTouched, touched } = useFormikContext();
  const [isUploadVisible, setIsUploadVisible] = useState(false);

  useEffect(() => {
    if (getIn(touched, `${nameSpace}.name`) !== true) {
      setFieldTouched(`${nameSpace}.name`);
    }
  }, [nameSpace, setFieldTouched, touched]);

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
      break;
  }

  const handleModalUploadClose = (result: UploadResponseModel | null) => {
    setIsUploadVisible(false);
    onUploadShapefile?.(result);
  };

  return (
    <>
      <StyledRow className="align-items-center mb-3 no-gutters">
        <Col md={3}>
          <div className="mb-0 d-flex align-items-center">
            {exists(index) &&
              (featureSet.isActive === false ? (
                <DisabledDraftCircleNumber text={(index! + 1).toString()} />
              ) : (
                <DraftCircleNumber text={(index! + 1).toString()} />
              ))}
            <OverflowTip
              fullText={propertyIdentifier}
              className={exists(index) ? 'pl-3' : ''}
            ></OverflowTip>
          </div>
        </Col>
        <Col md={showDisable ? 4 : 5}>
          <InlineInput
            className="mb-0 w-100"
            label=""
            field={withNameSpace(nameSpace, 'name')}
            displayErrorTooltips={true}
            defaultValue=""
            errorKeys={[withNameSpace(nameSpace, 'isRetired')]}
          />
        </Col>
        <RestrictedWidthCol xs="auto" className="ml-5">
          <ZoomToLocation geometry={featureSet.pimsFeature.geometry} icon={ZoomIconType.single} />
        </RestrictedWidthCol>
        {showDisable && (
          <Col md={2} className="mr-3">
            {!featureSet?.pimsFeature?.properties?.IS_RETIRED ? (
              <div className="mb-0 ml-7">Retired</div>
            ) : (
              <Select
                className="mb-0 ml-3"
                field={withNameSpace(nameSpace, 'isActive')}
                options={[
                  { label: 'Inactive', value: 'false' },
                  { label: 'Active', value: 'true' },
                ]}
              ></Select>
            )}
          </Col>
        )}
        <StyledActionsCol xs="auto">
          <StyledIconButton
            title="move-pin-location"
            onClick={() => {
              mapMachine.startReposition(featureSet, index);
            }}
            data-testid={'move-pin-location-' + index}
          >
            <RiDragMove2Line size={22} />
          </StyledIconButton>
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
      </StyledRow>
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

const StyledRow = styled(Row)`
  min-height: 4.5rem;
`;

const StyledActionsCol = styled(Col)`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-end;
`;

const RestrictedWidthCol = styled(Col)`
  max-width: 3rem;
  min-width: 3rem;
`;

const StyledSpacingWrapper = styled.div`
  padding-left: 1.2rem;
`;

export default SelectedPropertyRow;
