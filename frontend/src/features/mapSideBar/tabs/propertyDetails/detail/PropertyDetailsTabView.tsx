import { EditButton } from 'components/common/EditButton';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import { Claims, PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import Api_TypeCode from 'models/api/TypeCode';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { booleanToYesNoUnknownString, stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { InlineContainer, LeftBorderCol } from '../../SectionStyles';
import { LandMeasurementTable } from './components/LandMeasurementTable';
import { VolumetricMeasurementTable } from './components/VolumetricMeasurementTable';
import { IPropertyDetailsForm, readOnlyMultiSelectStyle } from './PropertyDetailsTabView.helpers';

export interface IPropertyDetailsTabView {
  property?: IPropertyDetailsForm;
  loading: boolean;
  setEditMode?: (isEditing: boolean) => void;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyDetailsTabView: React.FunctionComponent<IPropertyDetailsTabView> = ({
  property,
  loading,
  setEditMode,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const { hasClaim } = useKeycloakWrapper();

  const pphTypeOptions = getOptionsByType(API.PPH_STATUS_TYPES);

  const pphStatusTypeCodeDesc = pphTypeOptions.find(
    pph => pph.value === property?.pphStatusTypeCode,
  )?.label;

  const anomalies = property?.anomalies as Api_TypeCode<string>[];
  const tenureStatus = property?.tenure as Api_TypeCode<string>[];
  const roadType = property?.roadType as Api_TypeCode<string>[];
  const adjacentLand = property?.adjacentLand as Api_TypeCode<string>[];

  // measurement tables
  const landMeasurement = property?.landMeasurementTable;
  const volumeMeasurement = property?.volumetricMeasurementTable;

  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.AdjacentLand);
  const isIndianReserve =
    isAdjacentLand && adjacentLand?.some(obj => obj.id === PropertyAdjacentLandTypes.IndianReserve);

  const isVolumetricParcel = stringToBoolean(property?.isVolumetricParcel || '');
  return (
    <>
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <StyledEditWrapper className="mr-3 my-1">
          {setEditMode !== undefined && hasClaim(Claims.PROPERTY_EDIT) && (
            <EditButton
              title="Edit research file"
              onClick={() => {
                setEditMode(true);
              }}
            />
          )}
        </StyledEditWrapper>
        <Section header="Property Attributes">
          <SectionField label="MOTI region">{property?.regionType?.description}</SectionField>
          <SectionField label="Highways district">
            <InlineContainer>
              {property?.districtType?.description !== 'Cannot determine' && (
                <>{property?.districtType?.id}-</>
              )}
              {property?.districtType?.description}
            </InlineContainer>
          </SectionField>
          <SectionField label="Electoral district">
            {property?.electoralDistrict?.ED_NAME}
          </SectionField>
          <SectionField label="Agricultural Land Reserve">
            {property?.isALR ? 'Yes' : 'No'}
          </SectionField>
          <SectionField label="Railway belt / Dominion patent">
            {booleanToYesNoUnknownString(property?.isRwyBeltDomPatent)}
          </SectionField>
          <SectionField label="Land parcel type">
            {property?.propertyType?.description}
          </SectionField>
          <SectionField label="Municipal zoning">{property?.municipalZoning}</SectionField>
          <SectionField label="Anomalies">
            <Multiselect
              disable
              disablePreSelectedValues
              hidePlaceholder
              placeholder=""
              selectedValues={anomalies}
              displayValue="description"
              style={readOnlyMultiSelectStyle}
            />
          </SectionField>
        </Section>

        <Section header="Tenure Status">
          <SectionField label="Tenure status">
            <Multiselect
              disable
              disablePreSelectedValues
              hidePlaceholder
              placeholder=""
              selectedValues={tenureStatus}
              displayValue="description"
              style={readOnlyMultiSelectStyle}
            />
          </SectionField>
          <SectionField label="Provincial Public Hwy">
            {pphStatusTypeCodeDesc ?? 'Unknown'}
          </SectionField>
          {isHighwayRoad && (
            <SectionField label="Highway / Road">
              <Multiselect
                disable
                disablePreSelectedValues
                hidePlaceholder
                placeholder=""
                selectedValues={roadType}
                displayValue="description"
                style={readOnlyMultiSelectStyle}
              />
            </SectionField>
          )}
          {isAdjacentLand && (
            <SectionField label="Adjacent land">
              <Multiselect
                disable
                disablePreSelectedValues
                hidePlaceholder
                placeholder=""
                selectedValues={adjacentLand}
                displayValue="description"
                style={readOnlyMultiSelectStyle}
              />
            </SectionField>
          )}
        </Section>

        {isIndianReserve && (
          <Section header="First Nations Information">
            <SectionField label="Band name">{property?.firstNations?.bandName}</SectionField>
            <SectionField label="Reserve name">{property?.firstNations?.reserveName}</SectionField>
          </Section>
        )}

        <Section header="Area">
          <Row>
            <Col>
              <Row>
                <Col className="col-10">
                  <LandMeasurementTable data={landMeasurement} />
                </Col>
              </Row>
            </Col>
            <LeftBorderCol>
              <StyledFieldLabel>Is this a volumetric parcel?</StyledFieldLabel>
              <Row className="pb-3">
                <Col>
                  <Form.Check
                    inline
                    label="Yes"
                    name="is-volumetric-radio"
                    type="radio"
                    id={`inline-isVolumetric-yes`}
                    checked={property?.isVolumetricParcel === 'true'}
                    disabled
                  />
                  <Form.Check
                    inline
                    label="No"
                    name="is-volumetric-radio"
                    type="radio"
                    id={`inline-isVolumetric-no`}
                    value={'yes'}
                    checked={property?.isVolumetricParcel === 'false'}
                    disabled
                  />
                </Col>
              </Row>

              {isVolumetricParcel && (
                <>
                  <SectionField label="Type">{property?.volumetricType?.description}</SectionField>

                  <Row>
                    <Col className="col-10">
                      <VolumetricMeasurementTable data={volumeMeasurement} />
                    </Col>
                  </Row>
                </>
              )}
            </LeftBorderCol>
          </Row>
        </Section>

        <Section header="Notes">
          <p>{property?.notes}</p>
        </Section>
      </StyledSummarySection>
    </>
  );
};

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;
