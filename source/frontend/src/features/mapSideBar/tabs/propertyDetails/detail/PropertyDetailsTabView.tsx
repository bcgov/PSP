import { EditButton } from 'components/common/EditButton';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import AreaContainer from 'components/measurements/AreaContainer';
import VolumeContainer from 'components/measurements/VolumeContainer';
import * as API from 'constants/API';
import { Claims, PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { booleanToYesNoUnknownString, stringToBoolean } from 'utils/formUtils';
import { getPrettyLatLng } from 'utils/mapPropertyUtils';

import { Section } from '../../Section';
import { SectionField } from '../../SectionField';
import { InlineContainer } from '../../SectionStyles';
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

  const anomalies = property?.anomalies;
  const tenureStatus = property?.tenures;
  const roadType = property?.roadTypes;
  const adjacentLand = property?.adjacentLands;
  const address = property?.address;

  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj?.id === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(obj => obj?.id === PropertyTenureTypes.AdjacentLand);
  const isIndianReserve =
    isAdjacentLand &&
    adjacentLand?.some(obj => obj?.id === PropertyAdjacentLandTypes.IndianReserve);

  const isVolumetricParcel = stringToBoolean(property?.isVolumetricParcel || '');

  return (
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
      <Section header="Property Address">
        {address !== undefined ? (
          <>
            <StyledSubtleText>
              This is the address stored in PIMS application for this property and will be used
              wherever this property's address is needed.
            </StyledSubtleText>
            <SectionField label="Address">
              {address?.streetAddress1 && <div>{address?.streetAddress1}</div>}
              {address?.streetAddress2 && <div>{address?.streetAddress2}</div>}
              {address?.streetAddress3 && <div>{address?.streetAddress3}</div>}
            </SectionField>
            <SectionField label="City">{address?.municipality}</SectionField>
            <SectionField label="Province">{address?.province?.description}</SectionField>
            <SectionField label="Postal code">{address?.postal}</SectionField>
          </>
        ) : (
          <b>Property address not available</b>
        )}
      </Section>

      <Section header="Property Attributes">
        <SectionField label="MOTI region">{property?.region?.description}</SectionField>
        <SectionField label="Highways district">
          <InlineContainer>
            {property?.district?.description !== 'Cannot determine' && (
              <>{property?.district?.id}-</>
            )}
            {property?.district?.description}
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
        <SectionField label="Land parcel type">{property?.propertyType?.description}</SectionField>
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
        <SectionField label="Coordinates" labelWidth="2">{`${getPrettyLatLng(
          property?.location,
        )}`}</SectionField>
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
          <SectionField label="Highway / Road established by">
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
          <SectionField label="Adjacent Land type">
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

      <Section header="Measurements">
        <SectionField label="Area" labelWidth="2">
          <AreaContainer landArea={property?.landArea} unitCode={property?.areaUnit?.id} />
        </SectionField>

        <SectionField label="Is this a volumetric parcel?" labelWidth="auto" className="py-4">
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
        </SectionField>
        {isVolumetricParcel && (
          <SectionField label="Volume" labelWidth="2">
            <Row>
              <Col>
                <VolumeContainer
                  volumetricMeasurement={property?.volumetricMeasurement}
                  volumetricUnit={property?.volumetricUnit?.id}
                  volumetricType={property?.volumetricType?.description}
                />
              </Col>
              <Col>
                <SectionField label="Type" labelWidth="auto">
                  {property?.volumetricType?.description}
                </SectionField>
              </Col>
            </Row>
          </SectionField>
        )}
      </Section>

      <Section header="Notes">
        <p>{property?.notes}</p>
      </Section>
    </StyledSummarySection>
  );
};

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;

const StyledSubtleText = styled.p`
  color: ${props => props.theme.css.subtleColor};
  text-align: left;
`;
