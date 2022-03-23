import { Input } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Section } from '../Section';
import { SectionField } from '../SectionField';
import { StyledReadOnlyForm, StyledScrollable, StyledText } from '../SectionStyles';
import {
  defaultPropertyInfo,
  IPropertyDetailsForm,
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
      <Formik initialValues={values} onSubmit={noop} enableReinitialize={true}>
        <StyledReadOnlyForm>
          <Section header="Property attributes">
            <SectionField label="MOTI region">
              <Input disabled field="motiRegion" />
            </SectionField>
            <SectionField label="Highways district">
              <Input disabled field="highwaysDistrict" />
            </SectionField>
            <SectionField label="Electoral district">
              <Input disabled field="electoralDistrict" />
            </SectionField>
            <SectionField label="Agricultural Land Reserve">
              <StyledText>{values.isALR ? 'Yes' : 'No'}</StyledText>
            </SectionField>
            <SectionField label="Land parcel type">
              <Input disabled field="propertyType.description" />
            </SectionField>
            <SectionField label="Municipal zoning">
              <Input disabled field="zoning" />
            </SectionField>
            <SectionField label="Anomalies">
              <Input disabled field="anomalies.description" />
            </SectionField>
          </Section>

          <Section header="Tenure Status">
            <SectionField label="Tenure status">
              <Input disabled field="tenure.description" />
            </SectionField>
            <SectionField label="Provincial Public Hwy">TBD</SectionField>
            <SectionField label="Highway / Road">
              <Input disabled field="roadType.description" />
            </SectionField>
            <SectionField label="Adjacent land">Indian Reserve (IR)</SectionField>
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
      </Formik>
    </StyledScrollable>
  );
};
