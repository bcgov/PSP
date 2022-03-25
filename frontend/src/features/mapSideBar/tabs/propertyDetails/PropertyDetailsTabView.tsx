import { Text } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { Table } from 'components/Table';
import { Formik, FormikProps, getIn } from 'formik';
import noop from 'lodash/noop';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Section } from '../Section';
import { SectionField, StyledFieldLabel } from '../SectionField';
import {
  InlineContainer,
  LeftBorderCol,
  StyledReadOnlyForm,
  StyledScrollable,
  TableCaption,
} from '../SectionStyles';
import { LandMeasurementTable } from './components/LandMeasurementTable';
import { VolumetricMeasurementTable } from './components/VolumetricMeasurementTable';
import {
  defaultPropertyInfo,
  IPropertyDetailsForm,
  readOnlyMultiSelectStyle,
  toFormValues,
} from './PropertyDetailsTabView.helpers';

interface IPropertyDetailsTabView {
  details?: IPropertyDetailsForm;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyDetailsTabView: React.FC<IPropertyDetailsTabView> = ({ details }) => {
  const values = details ?? toFormValues(defaultPropertyInfo);

  return (
    <StyledScrollable>
      <Formik
        initialValues={values}
        onSubmit={noop}
        enableReinitialize={true}
        component={FormComponent}
      />
    </StyledScrollable>
  );
};

const FormComponent: React.FC<FormikProps<IPropertyDetailsForm>> = ({ values }) => {
  // multi-selects
  const anomalies = getIn(values, 'anomalies');
  const tenureStatus = getIn(values, 'tenure');
  const roadType = getIn(values, 'roadType');
  const adjacentLand = getIn(values, 'adjacentLand');
  // measurement tables
  const landMeasurement = getIn(values, 'landMeasurementTable');
  const volumeMeasurement = getIn(values, 'volumetricMeasurementTable');

  const isProvincialHighway = getIn(values, 'isProvincialPublicHwy');

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
        <SectionField label="Agricultural Land Reserve">
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
            selectedValues={tenureStatus}
            displayValue="description"
            style={readOnlyMultiSelectStyle}
          />
        </SectionField>
        <SectionField label="Provincial Public Hwy">
          {isProvincialHighway === true && <Text>Yes</Text>}
          {isProvincialHighway === false && <Text>No</Text>}
          {isProvincialHighway === undefined && <Text>Unknown</Text>}
        </SectionField>
        <SectionField label="Highway / Road">
          <Multiselect
            disable
            disablePreSelectedValues
            hidePlaceholder
            selectedValues={roadType}
            displayValue="description"
            style={readOnlyMultiSelectStyle}
          />
        </SectionField>
        <SectionField label="Adjacent land">
          <Multiselect
            disable
            disablePreSelectedValues
            hidePlaceholder
            selectedValues={adjacentLand}
            displayValue="description"
            style={readOnlyMultiSelectStyle}
          />
        </SectionField>
      </Section>

      <Section header="First Nations Information">
        <SectionField label="Band name">
          <Text field="firstNations.bandName" />
        </SectionField>
        <SectionField label="Reserve name">
          <Text field="firstNations.reserveName" />
        </SectionField>
      </Section>

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
            <SectionField label="Type">
              <Text field="volumetricType.description" />
            </SectionField>

            <Row>
              <Col className="col-10">
                <VolumetricMeasurementTable data={volumeMeasurement} />
              </Col>
            </Row>
          </LeftBorderCol>
        </Row>
      </Section>

      <Section header="Notes">
        Some notes go here. We can capture information about property here.
      </Section>
    </StyledReadOnlyForm>
  );
};
