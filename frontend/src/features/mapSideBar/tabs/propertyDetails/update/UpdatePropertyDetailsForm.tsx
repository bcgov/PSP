import { Input, Multiselect, Text, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import * as API from 'constants/API';
import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { Form, FormikProps, getIn } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import Api_TypeCode from 'models/api/TypeCode';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { InlineContainer, LeftBorderCol } from '../../SectionStyles';
import { UpdatePropertyDetailsFormModel } from './models';

export const UpdatePropertyDetailsForm: React.FC<FormikProps<UpdatePropertyDetailsFormModel>> = ({
  values,
}) => {
  // Lookup codes
  const { getByType } = useLookupCodeHelpers();
  const anomalyOptions = getByType(API.PROPERTY_ANOMALY_TYPES);

  // multi-selects
  const tenureStatus = getIn(values, 'tenure') as Api_TypeCode<string>[];
  const adjacentLand = getIn(values, 'adjacentLand') as Api_TypeCode<string>[];
  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.AdjacentLand);
  const isIndianReserve =
    isAdjacentLand && adjacentLand?.some(obj => obj.id === PropertyAdjacentLandTypes.IndianReserve);
  const isVolumetricParcel = stringToBoolean(getIn(values, 'isVolumetricParcel'));

  return (
    <StyledForm>
      <Section header="Property attributes">
        <SectionField label="MOTI region">
          <Text field="motiRegion.REGION_NAME" />
        </SectionField>
        <SectionField label="Highways district">
          <InlineContainer>
            <Text field="highwaysDistrict.DISTRICT_NUMBER" />
            {'-'}
            <Text field="highwaysDistrict.DISTRICT_NAME" />
          </InlineContainer>
        </SectionField>
        <SectionField label="Electoral district">
          <Text field="electoralDistrict.ED_NAME" />
        </SectionField>
        <SectionField label="Agricultural Land Reserve">
          <Text>{values.isALR ? 'Yes' : 'No'}</Text>
        </SectionField>
        <SectionField label="Land parcel type">{/* TODO */}</SectionField>
        <SectionField label="Municipal zoning">
          <Input field="municipalZoning" />
        </SectionField>
        <SectionField label="Anomalies">
          <Multiselect
            field="anomalies"
            displayValue="name"
            options={anomalyOptions}
            hidePlaceholder
            placeholder=""
          />
        </SectionField>
      </Section>

      <Section header="Tenure Status">
        <SectionField label="Tenure status">
          {/* <Multiselect
            disable
            disablePreSelectedValues
            hidePlaceholder
            placeholder=""
            selectedValues={tenureStatus}
            displayValue="description"
            style={readOnlyMultiSelectStyle}
          /> */}
        </SectionField>
        <SectionField label="Provincial Public Hwy">
          {/* TODO: YES / NO / UNKNOWN component */}
        </SectionField>
        {isHighwayRoad && (
          <SectionField label="Highway / Road">
            {/* <Multiselect
              disable
              disablePreSelectedValues
              hidePlaceholder
              placeholder=""
              selectedValues={roadType}
              displayValue="description"
              style={readOnlyMultiSelectStyle}
            /> */}
          </SectionField>
        )}
        {isAdjacentLand && (
          <SectionField label="Adjacent land">
            {/* <Multiselect
              disable
              disablePreSelectedValues
              hidePlaceholder
              placeholder=""
              selectedValues={adjacentLand}
              displayValue="description"
              style={readOnlyMultiSelectStyle}
            /> */}
          </SectionField>
        )}
      </Section>

      {isIndianReserve && (
        <Section header="First Nations Information">
          <SectionField label="Band name">
            <Text field="firstNations.bandName" />
          </SectionField>
          <SectionField label="Reserve name">
            <Text field="firstNations.reserveName" />
          </SectionField>
        </Section>
      )}

      <Section header="Area">
        <Row>
          <Col>
            <Row>
              <Col className="col-10">{/* <LandMeasurementTable data={landMeasurement} /> */}</Col>
            </Row>
          </Col>
          <LeftBorderCol>
            <StyledFieldLabel>Is this a volumetric parcel?</StyledFieldLabel>
            <RadioGroup
              flexDirection="row"
              field="isVolumetricParcel"
              radioGroupClassName="pb-3"
              radioValues={[
                {
                  radioLabel: 'Yes',
                  radioValue: 'true',
                },
                {
                  radioLabel: 'No',
                  radioValue: 'false',
                },
              ]}
            />

            {isVolumetricParcel && (
              <>
                <SectionField label="Type">
                  <Text field="volumetricType.description" />
                </SectionField>

                <Row>
                  <Col className="col-10">
                    {/* <VolumetricMeasurementTable data={volumeMeasurement} /> */}
                  </Col>
                </Row>
              </>
            )}
          </LeftBorderCol>
        </Row>
      </Section>

      <Section header="Notes">
        <TextArea field="notes" />
      </Section>
    </StyledForm>
  );
};

const StyledForm = styled(Form)``;
