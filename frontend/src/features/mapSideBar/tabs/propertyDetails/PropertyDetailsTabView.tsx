import { Input } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { Formik } from 'formik';
import { IProperty } from 'interfaces';
import noop from 'lodash/noop';
import { Api_Property } from 'models/api/Property';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { Section } from '../Section';
import { SectionField } from '../SectionField';
import { StyledReadOnlyForm, StyledScrollable } from '../SectionStyles';

interface IInventoryPropertyDetailsProps {
  details?: IProperty;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyDetailsTabView: React.FC<IInventoryPropertyDetailsProps> = ({ details }) => {
  return (
    <StyledScrollable>
      <Formik
        initialValues={details ?? defaultPropertyInfo}
        onSubmit={noop}
        enableReinitialize={true}
      >
        <StyledReadOnlyForm>
          <Section header="Property attributes">
            <SectionField label="MOTI region">TBD</SectionField>
            <SectionField label="Highways district">TBD</SectionField>
            <SectionField label="Electoral district">TBD</SectionField>
            <SectionField label="Agricultural Land Reserve">TBD</SectionField>
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

const defaultPropertyInfo: Api_Property = {
  id: 1,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
  },
  anomalies: {
    id: 'ACCESS',
    description: 'Access',
    isDisabled: false,
  },
  tenure: {
    id: 'ADJLAND',
    description: 'Adjacent Land',
    isDisabled: false,
  },
  adjacentLand: {
    id: 'PRIVATE',
    description: 'Private (Fee Simple)',
    isDisabled: false,
  },
  status: {
    id: 'MOTIADMIN',
    description: 'Under MoTI administration',
    isDisabled: false,
  },
  dataSource: {
    id: 'PAIMS',
    description: 'Property Acquisition and Inventory Management System (PAIMS)',
    isDisabled: false,
  },
  dataSourceEffectiveDate: '2021-08-31T00:00:00',
  isSensitive: false,
  isProvincialPublicHwy: false,
  address: {
    id: 204,
    streetAddress1: '456 Souris Street',
    streetAddress2: 'PO Box 250',
    streetAddress3: 'A Hoot and a holler from the A&W',
    municipality: 'North Podunk',
    province: {
      provinceStateId: 1,
      provinceStateCode: 'BC',
      description: 'British Columbia',
      rowVersion: 0,
    },
    country: {
      countryId: 1,
      countryCode: 'CA',
      description: 'Canada',
      rowVersion: 1,
    },
    postal: 'IH8 B0B',
    rowVersion: 1,
  },
  pid: '007-723-385',
  pin: 90069930,
  areaUnit: {
    id: 'HA',
    description: 'Hectare',
    isDisabled: false,
  },
  landArea: 1,
  isVolumetricParcel: false,
  volumetricMeasurement: 0,
  zoning: 'Lorem ipsum',
  notes: 'Lorem ipsum',
  rowVersion: 5,
};
