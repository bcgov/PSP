import { Button } from 'components/common/buttons';
import { Input, Multiselect, Select, Text, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { YesNoSelect } from 'components/common/form/YesNoSelect';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import * as API from 'constants/API';
import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { Form, FormikProps, getIn } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { ButtonToolbar, Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { InlineContainer, LeftBorderCol } from '../../SectionStyles';
import { LandMeasurementTable } from './components/LandMeasurementTable';
import { VolumetricMeasurementTable } from './components/VolumetricMeasurementTable';
import {
  PropertyAdjacentLandFormModel,
  PropertyAnomalyFormModel,
  PropertyRoadFormModel,
  PropertyTenureFormModel,
  UpdatePropertyDetailsFormModel,
} from './models';

export interface IUpdatePropertyDetailsFormProps
  extends FormikProps<UpdatePropertyDetailsFormModel> {
  onCancel: () => void;
}

export const UpdatePropertyDetailsForm: React.FC<IUpdatePropertyDetailsFormProps> = ({
  values,
  setFieldValue,
  onCancel,
}) => {
  // Lookup codes
  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const volumetricTypeOptions = getOptionsByType(API.PROPERTY_VOLUMETRIC_TYPES);
  const anomalyOptions = getByType(API.PROPERTY_ANOMALY_TYPES).map(x =>
    PropertyAnomalyFormModel.fromLookup(x),
  );
  const tenureOptions = getByType(API.PROPERTY_TENURE_TYPES).map(x =>
    PropertyTenureFormModel.fromLookup(x),
  );
  const roadTypeOptions = getByType(API.PROPERTY_ROAD_TYPES).map(x =>
    PropertyRoadFormModel.fromLookup(x),
  );
  const adjacentLandOptions = getByType(API.PROPERTY_ADJACENT_LAND_TYPES).map(x =>
    PropertyAdjacentLandFormModel.fromLookup(x),
  );

  // multi-selects
  const tenureStatus = getIn(values, 'tenures') as PropertyTenureFormModel[];
  const adjacentLands = getIn(values, 'adjacentLands') as PropertyAdjacentLandFormModel[];
  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj.typeCode === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(
    obj => obj.typeCode === PropertyTenureTypes.AdjacentLand,
  );
  const isIndianReserve =
    isAdjacentLand &&
    adjacentLands?.some(obj => obj.typeCode === PropertyAdjacentLandTypes.IndianReserve);
  const isVolumetricParcel = stringToBoolean(getIn(values, 'isVolumetricParcel'));
  // area measurements table inputs
  const landArea = getIn(values, 'landArea') as number;
  const areaUnit = getIn(values, 'areaUnitTypeCode') as string;
  // volume measurements table inputs
  const volumetricMeasurement = getIn(values, 'volumetricMeasurement') as number;
  const volumetricUnit = getIn(values, 'volumetricUnitTypeCode') as string;

  return (
    <StyledForm>
      <Content vertical>
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
              displayValue="typeDescription"
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
              displayValue="typeDescription"
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
                displayValue="typeDescription"
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
                displayValue="typeDescription"
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
                  <LandMeasurementTable
                    area={landArea}
                    areaUnitTypeCode={areaUnit}
                    onChange={(landArea, areaUnitTypeCode) => {
                      setFieldValue('landArea', landArea);
                      setFieldValue('areaUnitTypeCode', areaUnitTypeCode);
                    }}
                  />
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
                        onChange={(volume, volumeUnitTypeCode) => {
                          setFieldValue('volumetricMeasurement', volume);
                          setFieldValue('volumetricUnitTypeCode', volumeUnitTypeCode);
                        }}
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
      </Content>

      <Footer>
        <ButtonToolbar className="cancelSave">
          <Button className="mr-5" variant="secondary" type="button" onClick={onCancel}>
            Cancel
          </Button>
          <Button className="mr-5" type="submit">
            Save
          </Button>
        </ButtonToolbar>
      </Footer>
    </StyledForm>
  );
};

const StyledForm = styled(Form)``;

const Content = styled(Scrollable)`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;

const Footer = styled(Row)`
  margin: 0;
  justify-content: end;
`;
