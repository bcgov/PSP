import { Input, Text } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { Formik, FormikProps, getIn } from 'formik';
import noop from 'lodash/noop';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Section } from '../Section';
import { SectionField } from '../SectionField';
import { InlineContainer, StyledReadOnlyForm, StyledScrollable } from '../SectionStyles';
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
          <Input disabled field="propertyType.description" />
        </SectionField>
        <SectionField label="Municipal zoning">
          <Input disabled field="municipalZoning" />
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
        <SectionField label="Band name">TBD</SectionField>
        <SectionField label="Reserve name">TBD</SectionField>
      </Section>

      <Section header="Area">
        <Row>
          <Col>
            <p style={{ fontWeight: 700 }}>Land measurement</p>
          </Col>
          <Col style={{ borderLeft: '1px solid #8c8c8c' }}>
            <p style={{ fontWeight: 700 }}>Is this a volumetric parcel?</p>
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
          </Col>
        </Row>
      </Section>

      <Section header="Notes">
        Some notes go here. We can capture information about property here.
      </Section>
    </StyledReadOnlyForm>
  );
};
