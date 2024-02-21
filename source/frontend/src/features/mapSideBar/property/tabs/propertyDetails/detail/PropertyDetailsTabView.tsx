import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';

import { EditButton } from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  InlineContainer,
  StyledEditWrapper,
  StyledSubtleText,
  StyledSummarySection,
} from '@/components/common/Section/SectionStyles';
import AreaContainer from '@/components/measurements/AreaContainer';
import VolumeContainer from '@/components/measurements/VolumeContainer';
import * as API from '@/constants/API';
import { Claims, PropertyTenureTypes } from '@/constants/index';
import { useQuery } from '@/hooks/use-query';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { exists } from '@/utils';
import { booleanToYesNoUnknownString, stringToBoolean } from '@/utils/formUtils';
import { getPrettyLatLng } from '@/utils/mapPropertyUtils';

import { IPropertyDetailsForm, readOnlyMultiSelectStyle } from './PropertyDetailsTabView.helpers';

export interface IPropertyDetailsTabView {
  property?: IPropertyDetailsForm;
  loading: boolean;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyDetailsTabView: React.FunctionComponent<IPropertyDetailsTabView> = ({
  property,
  loading,
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
  const address = property?.address;
  const query = useQuery();
  const history = useHistory();

  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj?.id === PropertyTenureTypes.HighwayRoad);
  const isIndianReserve = tenureStatus?.some(obj => obj?.id === PropertyTenureTypes.IndianReserve);

  const isVolumetricParcel = stringToBoolean(property?.isVolumetricParcel || '');

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper className="mr-3 my-1">
        {hasClaim(Claims.PROPERTY_EDIT) && (
          <EditButton
            title="Edit property details"
            onClick={() => {
              query.set('edit', 'true');
              history.push({ search: query.toString() });
            }}
          />
        )}
      </StyledEditWrapper>
      <Section header="Property Address">
        {exists(address) ? (
          <>
            <StyledSubtleText>
              This is the address stored in PIMS application for this property and will be used
              wherever this property&apos;s address is needed.
            </StyledSubtleText>
            <SectionField label="Address">
              {address?.streetAddress1 && <div>{address?.streetAddress1}</div>}
              {address?.streetAddress2 && <div>{address?.streetAddress2}</div>}
              {address?.streetAddress3 && <div>{address?.streetAddress3}</div>}
            </SectionField>
            <SectionField label="City">{address?.municipality}</SectionField>
            <SectionField label="Province">{address?.province?.description}</SectionField>
            <SectionField label="Postal code">{address?.postal}</SectionField>
            <SectionField label="General location">{property?.generalLocation}</SectionField>
          </>
        ) : (
          <b>Property address not available.</b>
        )}
      </Section>

      <Section header="Property Attributes">
        <SectionField label="Legal Description">{property?.landLegalDescription}</SectionField>
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
          {property?.electoralDistrict?.properties.ED_NAME}
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
          <SectionField label="Highway / Road Details">
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
      </Section>

      {isIndianReserve && (
        <Section header="First Nations Information">
          <SectionField label="Band name">{property?.firstNations?.bandName}</SectionField>
          <SectionField label="Reserve name">{property?.firstNations?.reserveName}</SectionField>
        </Section>
      )}

      <Section header="Measurements">
        <SectionField label="Area" labelWidth="2">
          <AreaContainer
            landArea={property?.landArea ?? undefined}
            unitCode={property?.areaUnit?.id ?? undefined}
          />
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
                  volumetricMeasurement={property?.volumetricMeasurement ?? undefined}
                  volumetricUnit={property?.volumetricUnit?.id ?? undefined}
                  volumetricType={property?.volumetricType?.description ?? undefined}
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
