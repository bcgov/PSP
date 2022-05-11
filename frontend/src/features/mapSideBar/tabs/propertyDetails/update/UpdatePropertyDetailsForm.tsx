import { Button } from 'components/common/buttons';
import { Input, Multiselect, Select, Text, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { YesNoSelect } from 'components/common/form/YesNoSelect';
import * as API from 'constants/API';
import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { Form, FormikProps, getIn } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import Api_TypeCode from 'models/api/TypeCode';
import React from 'react';
import { ButtonToolbar, Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';
import { stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { InlineContainer, LeftBorderCol } from '../../SectionStyles';
import { LandMeasurementTable } from './components/LandMeasurementTable';
import { VolumetricMeasurementTable } from './components/VolumetricMeasurementTable';
import { UpdatePropertyDetailsFormModel } from './models';

export const UpdatePropertyDetailsForm: React.FC<FormikProps<UpdatePropertyDetailsFormModel>> = ({
  values,
}) => {
  const location = useHistory();
  // Lookup codes
  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const anomalyOptions = getByType(API.PROPERTY_ANOMALY_TYPES);
  const tenureOptions = getByType(API.PROPERTY_TENURE_TYPES);
  const roadTypeOptions = getByType(API.PROPERTY_ROAD_TYPES);
  const adjacentLandOptions = getByType(API.PROPERTY_ADJACENT_LAND_TYPES);
  const volumetricTypeOptions = getOptionsByType(API.PROPERTY_VOLUMETRIC_TYPES);
  // multi-selects
  const tenureStatus = getIn(values, 'tenures') as Api_TypeCode<string>[];
  const adjacentLands = getIn(values, 'adjacentLands') as Api_TypeCode<string>[];
  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.AdjacentLand);
  const isIndianReserve =
    isAdjacentLand &&
    adjacentLands?.some(obj => obj.id === PropertyAdjacentLandTypes.IndianReserve);
  const isVolumetricParcel = stringToBoolean(getIn(values, 'isVolumetricParcel'));
  // area measurements table inputs
  const landArea = getIn(values, 'landArea') as number;
  const areaUnit = getIn(values, 'areaUnitTypeCode') as string;
  // volume measurements table inputs
  const volumetricMeasurement = getIn(values, 'volumetricMeasurement') as number;
  const volumetricUnit = getIn(values, 'volumetricUnitTypeCode') as string;

  const onCancel = () => {
    location.goBack();
  };

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
            placeholder=""
            hidePlaceholder
            options={anomalyOptions}
          />
        </SectionField>
      </Section>

      <Section header="Tenure Status">
        <SectionField label="Tenure status">
          <Multiselect
            field="tenures"
            displayValue="name"
            placeholder=""
            hidePlaceholder
            options={tenureOptions}
          />
        </SectionField>
        <SectionField label="Provincial Public Hwy">
          <YesNoSelect field="isProvincialPublicHwy"></YesNoSelect>
        </SectionField>
        {isHighwayRoad && (
          <SectionField label="Highway / Road">
            <Multiselect
              field="roadTypes"
              displayValue="name"
              placeholder=""
              hidePlaceholder
              options={roadTypeOptions}
            />
          </SectionField>
        )}
        {isAdjacentLand && (
          <SectionField label="Adjacent land">
            <Multiselect
              field="adjacentLands"
              displayValue="name"
              placeholder=""
              hidePlaceholder
              options={adjacentLandOptions}
            />
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
              <Col className="col-10">
                <LandMeasurementTable area={landArea} areaUnitTypeCode={areaUnit} />
              </Col>
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
                  <Select
                    field="volumetricParcelTypeCode"
                    options={volumetricTypeOptions}
                    placeholder={values.volumetricParcelTypeCode ? undefined : 'Please Select'}
                  />
                </SectionField>

                <Row>
                  <Col className="col-10">
                    <VolumetricMeasurementTable
                      volume={volumetricMeasurement}
                      volumeUnitTypeCode={volumetricUnit}
                    />
                  </Col>
                </Row>
              </>
            )}
          </LeftBorderCol>
        </Row>
      </Section>

      <Section header="Notes">
        <TextArea field="notes" rows={4} />
      </Section>

      <Row className="m-0 justify-content-md-end">
        <ButtonToolbar className="cancelSave">
          <Button className="mr-5" variant="secondary" type="button" onClick={onCancel}>
            Cancel
          </Button>
          <Button className="mr-5" type="submit">
            Save
          </Button>
        </ButtonToolbar>
      </Row>
    </StyledForm>
  );
};

const StyledForm = styled(Form)``;
