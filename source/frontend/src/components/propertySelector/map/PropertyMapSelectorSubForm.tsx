import { Col, Form as BsForm, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { SelectProperty } from '@/components/common/mapping/SelectProperty';
import { SectionField } from '@/components/common/Section/SectionField';
import { featuresetToMapProperty, pidFormatter } from '@/utils';

export interface IPropertyMapSelectorSubFormProps {
  onClickDraftMarker: () => void;
  selectedProperty?: LocationFeatureDataset;
}

export const PropertyMapSelectorSubForm: React.FunctionComponent<
  React.PropsWithChildren<IPropertyMapSelectorSubFormProps>
> = ({ onClickDraftMarker, selectedProperty }) => {
  const selectedMapProperty = featuresetToMapProperty(selectedProperty);
  const pid = selectedMapProperty?.pid;
  const pin = selectedMapProperty?.pin;
  const planNumber = selectedMapProperty?.planNumber;
  const legalDescription = selectedMapProperty?.legalDescription;
  const address = selectedMapProperty?.address;
  const region = selectedMapProperty?.region;
  const regionName = selectedMapProperty?.regionName;
  const district = selectedMapProperty?.district;
  const districtName = selectedMapProperty?.districtName;
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
            <BsForm.Label>Legal Description:</BsForm.Label>
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
