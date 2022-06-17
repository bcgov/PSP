import { Button } from 'components/common/buttons';
import { Input, Multiselect, Select, Text, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { YesNoSelect } from 'components/common/form/YesNoSelect';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import * as API from 'constants/API';
import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { Form, FormikProps, getIn } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import React, { useEffect } from 'react';
import { ButtonToolbar, Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';
import { stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { LeftBorderCol } from '../../SectionStyles';
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
  const propertyTypeOptions = getOptionsByType(API.PROPERTY_LAND_PARCEL_TYPES);
  const pphTypeOptions = getOptionsByType(API.PPH_STATUS_TYPES);
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
  const regionOptions = getOptionsByType(API.REGION_TYPES);
  const districtOptions = getOptionsByType(API.DISTRICT_TYPES);

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

  useEffect(() => {
    if (!isVolumetricParcel) {
      setFieldValue('volumetricMeasurement', 0);
      setFieldValue('volumetricUnitTypeCode', undefined);
      setFieldValue('volumetricParcelTypeCode', undefined);
    }
  }, [isVolumetricParcel, setFieldValue]);

  const cannotDetermineInfoText =
    'This means the property is out of bounds or there was an error at the time of determining this value. If needed, edit property details and pick the appropriate  value to update it.';

  return (
    <StyledForm>
      <UnsavedChangesPrompt />
      <Content vertical>
        <Section header="Property Attributes">
          <SectionField label="MOTI region">
            <Select
              field="regionTypeCode"
              options={regionOptions}
              placeholder={values.regionTypeCode ? undefined : 'Please Select'}
            />
            {regionOptions.find(x => x.code?.toString() === values.regionTypeCode?.toString())
              ?.label === 'Cannot determine' && (
              <StyledInfoSection>{cannotDetermineInfoText}</StyledInfoSection>
            )}
          </SectionField>
          <SectionField label="Highways district">
            <Select
              field="districtTypeCode"
              options={districtOptions}
              placeholder={values.highwaysDistrict ? undefined : 'Please Select'}
            />
            {districtOptions.find(x => x.code?.toString() === values.districtTypeCode?.toString())
              ?.label === 'Cannot determine' && (
              <StyledInfoSection>{cannotDetermineInfoText}</StyledInfoSection>
            )}
          </SectionField>
          <SectionField label="Electoral district">
            <Text field="electoralDistrict.ED_NAME" />
          </SectionField>
          <SectionField label="Agricultural Land Reserve">
            <Text>{values.isALR ? 'Yes' : 'No'}</Text>
          </SectionField>
          <SectionField label="Railway belt / Dominion patent">
            <YesNoSelect field="isRwyBeltDomPatent"></YesNoSelect>
          </SectionField>
          <SectionField label="Land parcel type">
            <Select
              field="propertyTypeCode"
              options={propertyTypeOptions}
              placeholder={values.propertyTypeCode ? undefined : 'Please Select'}
            />
          </SectionField>
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
            <Select field="pphStatusTypeCode" options={pphTypeOptions} />
            {values?.pphStatusUpdateTimestamp && (
              <p className="text-right font-italic">
                PPH status last updated by{' '}
                <UserNameTooltip
                  userName={values?.pphStatusUpdateUserid}
                  userGuid={values?.pphStatusUpdateUserGuid}
                />{' '}
                on {prettyFormatDate(values?.pphStatusUpdateTimestamp)}
              </p>
            )}
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
              <Text field="firstNations.BAND_NAME" />
            </SectionField>
            <SectionField label="Reserve name">
              <Text field="firstNations.ENGLISH_NAME" />
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

const StyledInfoSection = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  padding: 0.5rem;
  margin-bottom: 1.5rem;
`;
