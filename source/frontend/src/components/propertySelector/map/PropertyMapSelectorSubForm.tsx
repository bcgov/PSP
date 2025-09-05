import { Col, Form, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { SelectProperty } from '@/components/common/mapping/SelectProperty';
import { SectionField } from '@/components/common/Section/SectionField';
import { DistrictCodes, RegionCodes } from '@/constants';
import {
  addressFromFeatureSet,
  isNumber,
  pidFormatter,
  pidFromFeatureSet,
  pinFromFeatureSet,
  planFromFeatureSet,
} from '@/utils';

export interface IPropertyMapSelectorSubFormProps {
  onClickDraftMarker: () => void;
  selectedProperty?: SelectedFeatureDataset;
}

export const PropertyMapSelectorSubForm: React.FunctionComponent<
  React.PropsWithChildren<IPropertyMapSelectorSubFormProps>
> = ({ onClickDraftMarker, selectedProperty }) => {
  const pid = pidFromFeatureSet(selectedProperty);
  const pin = pinFromFeatureSet(selectedProperty);
  const planNumber = planFromFeatureSet(selectedProperty);
  const legalDescription =
    selectedProperty?.pimsFeature?.properties?.LAND_LEGAL_DESCRIPTION ??
    selectedProperty?.parcelFeature?.properties?.LEGAL_DESCRIPTION ??
    '';
  const address = addressFromFeatureSet(selectedProperty);
  const region = isNumber(selectedProperty?.regionFeature?.properties?.REGION_NUMBER)
    ? selectedProperty?.regionFeature?.properties?.REGION_NUMBER
    : RegionCodes.Unknown;
  const regionName = selectedProperty?.regionFeature?.properties?.REGION_NAME ?? 'Cannot determine';
  const district = isNumber(selectedProperty?.districtFeature?.properties?.DISTRICT_NUMBER)
    ? selectedProperty?.districtFeature?.properties?.DISTRICT_NUMBER
    : DistrictCodes.Unknown;
  const districtName =
    selectedProperty?.districtFeature?.properties?.DISTRICT_NAME ?? 'Cannot determine';

  return (
    <StyledFormRow>
      <Col md={4}>
        <SelectProperty onClick={onClickDraftMarker} />
      </Col>
      <Col md={8}>
        <Row>
          <GroupHeader>Selected property attributes</GroupHeader>
        </Row>
        <SectionField label="PID">{pidFormatter(pid)}</SectionField>
        <SectionField label="PIN">{pin}</SectionField>
        <SectionField label="Plan #">{planNumber}</SectionField>
        <SectionField label="Address">{address}</SectionField>
        <SectionField label="Region">
          {[region, regionName].filter(Boolean).join(' - ')}
        </SectionField>
        <SectionField label="District">
          {[district, districtName].filter(Boolean).join(' - ')}
        </SectionField>
        <Row>
          <Col md={12}>
            <Form.Label>Legal Description:</Form.Label>
          </Col>
          <Col md={12}>{legalDescription}</Col>
        </Row>
      </Col>
    </StyledFormRow>
  );
};

export const StyledFormRow = styled(Row)`
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
      color: ${props => props.theme.bcTokens.typographyColorSecondary};
    }
  }
`;

const GroupHeader = styled(Col)`
  color: ${props => props.theme.css.headerTextColor};
  font-family: 'BcSans-Bold';
  margin-bottom: 1rem;
`;

export default PropertyMapSelectorSubForm;
