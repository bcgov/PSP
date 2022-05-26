import { Text } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { Formik, FormikProps, getIn } from 'formik';
import noop from 'lodash/noop';
import Api_TypeCode from 'models/api/TypeCode';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { stringToBoolean } from 'utils/formUtils';

import { Section } from '../../Section';
import { SectionField, StyledFieldLabel } from '../../SectionField';
import { InlineContainer, LeftBorderCol, StyledReadOnlyForm } from '../../SectionStyles';
import { LandMeasurementTable } from './components/LandMeasurementTable';
import { VolumetricMeasurementTable } from './components/VolumetricMeasurementTable';
import {
  defaultPropertyInfo,
  IPropertyDetailsForm,
  readOnlyMultiSelectStyle,
  toFormValues,
} from './PropertyDetailsTabView.helpers';

export interface IPropertyDetailsTabView {
  property?: IPropertyDetailsForm;
  loading: boolean;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyDetailsTabView: React.FC<IPropertyDetailsTabView> = ({
  property,
  loading,
}) => {
  const values = property !== undefined ? property : toFormValues(defaultPropertyInfo);

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <Formik
        initialValues={values}
        onSubmit={noop}
        enableReinitialize={true}
        component={FormComponent}
      />
    </>
  );
};

const FormComponent: React.FC<FormikProps<IPropertyDetailsForm>> = ({ values }) => {
  // yes/no/unknown (true/false/undefined)
  const isProvincialHighway = getIn(values, 'isProvincialPublicHwy');
  // multi-selects
  const anomalies = getIn(values, 'anomalies') as Api_TypeCode<string>[];
  const tenureStatus = getIn(values, 'tenure') as Api_TypeCode<string>[];
  const roadType = getIn(values, 'roadType') as Api_TypeCode<string>[];
  const adjacentLand = getIn(values, 'adjacentLand') as Api_TypeCode<string>[];
  // measurement tables
  const landMeasurement = getIn(values, 'landMeasurementTable');
  const volumeMeasurement = getIn(values, 'volumetricMeasurementTable');
  // show/hide conditionals
  const isHighwayRoad = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.HighwayRoad);
  const isAdjacentLand = tenureStatus?.some(obj => obj.id === PropertyTenureTypes.AdjacentLand);
  const isIndianReserve =
    isAdjacentLand && adjacentLand?.some(obj => obj.id === PropertyAdjacentLandTypes.IndianReserve);
  const isVolumetricParcel = stringToBoolean(getIn(values, 'isVolumetricParcel'));

  return (
    <StyledReadOnlyForm>
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
        <SectionField label="Agricultural land reserve">
          <Text>{values.isALR ? 'Yes' : 'No'}</Text>
        </SectionField>
        <SectionField label="Land parcel type">
          <Text field="propertyType.description" />
        </SectionField>
        <SectionField label="Municipal zoning">
          <Text field="municipalZoning" />
        </SectionField>
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
        <SectionField label="Provincial public hwy">
          {isProvincialHighway === true && <Text>Yes</Text>}
          {isProvincialHighway === false && <Text>No</Text>}
          {isProvincialHighway === undefined && <Text>Unknown</Text>}
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
                <LandMeasurementTable data={landMeasurement} />
              </Col>
            </Row>
          </Col>
          <LeftBorderCol>
            <StyledFieldLabel>Is this a volumetric parcel?</StyledFieldLabel>
            <RadioGroup
              disabled
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
                    <VolumetricMeasurementTable data={volumeMeasurement} />
                  </Col>
                </Row>
              </>
            )}
          </LeftBorderCol>
        </Row>
      </Section>

      <Section header="Notes">
        <p>{getIn(values, 'notes')}</p>
      </Section>
    </StyledReadOnlyForm>
  );
};
