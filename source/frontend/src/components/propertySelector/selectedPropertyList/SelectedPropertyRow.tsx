import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { ImUpload } from 'react-icons/im';
import { RiDragMove2Line } from 'react-icons/ri';
import styled from 'styled-components';

import { RemoveButton, StyledIconButton } from '@/components/common/buttons';
import { Select } from '@/components/common/form';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import OverflowTip from '@/components/common/OverflowTip';
import { TooltipWrapper } from '@/components/common/TooltipWrapper';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { withNameSpace } from '@/utils/formUtils';
import { getPropertyNameFromSelectedFeatureSet, NameSourceType } from '@/utils/mapPropertyUtils';

import DisabledDraftCircleNumber from './DisabledDraftCircleNumber';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  showDisable?: boolean;
  showUploadShapefile?: boolean;
  property: SelectedFeatureDataset;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  showDisable,
  showUploadShapefile,
  property,
}) => {
  const mapMachine = useMapStateMachine();
  const { setFieldTouched, touched } = useFormikContext();
  useEffect(() => {
    if (getIn(touched, `${nameSpace}.name`) !== true) {
      setFieldTouched(`${nameSpace}.name`);
    }
  }, [nameSpace, setFieldTouched, touched]);

  const propertyName = getPropertyNameFromSelectedFeatureSet(property);
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
      <Col xs="auto" className="ml-5">
        <ZoomToLocation geometry={property.pimsFeature.geometry} icon={ZoomIconType.single} />
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
      <StyledActionsCol xs="auto" className="pl-3">
        <StyledIconButton
          title="move-pin-location"
          onClick={() => {
            mapMachine.startReposition(property, index);
          }}
          data-testid={'move-pin-location-' + index}
        >
          <RiDragMove2Line size={22} />
        </StyledIconButton>
        {showUploadShapefile && (
          <TooltipWrapper tooltip="Upload shapefile" tooltipId={'upload-shapefile-' + index}>
            <StyledIconButton
              onClick={() => {
                // TODO: implement shapefile upload
              }}
              data-testid={'upload-shapefile-' + index}
            >
              <ImUpload size={18} />
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

const StyledSpacingWrapper = styled.div`
  padding-left: 1.2rem;
`;

export default SelectedPropertyRow;
