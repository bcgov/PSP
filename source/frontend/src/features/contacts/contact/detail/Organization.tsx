import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaRegBuilding } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { H2, H3 } from '@/components/common/styles';
import { IContactOrganization, IContactPerson } from '@/interfaces/IContact';

import * as Styled from '../../styles';
import { toAddressFields } from '../utils/contactUtils';
import ContactInfoSubForm from './ContactInfoSubForm';
export interface OrganizationViewProps {
  organization: IContactOrganization;
}

interface AddressField {
  label: string;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipalityAndProvince?: string;
  country?: string;
  postal?: string;
}

const OrganizationView: React.FunctionComponent<OrganizationViewProps> = ({ organization }) => {
  let organizationAddresses: AddressField[];
  if (organization.addresses === undefined) {
    organizationAddresses = [];
  } else {
    organizationAddresses = toAddressFields(organization.addresses);
  }

  return (
    <>
      <Section className="mb-4">
        <H2>
          <Row className="mb-1">
            <Col>Contact Details</Col>
            <Col md={3} className="d-flex justify-content-end">
              <Styled.StatusIndicators className={organization.isDisabled ? 'inactive' : 'active'}>
                <FaRegBuilding size={15} className="mr-2 mb-1" />
                <span data-testid="contact-organization-status">
                  {organization.isDisabled ? 'INACTIVE' : 'ACTIVE'}
                </span>
              </Styled.StatusIndicators>
            </Col>
          </Row>
        </H2>
        <SectionField
          valueTestId="contact-organization-organizationName"
          label="Organization name"
          labelWidth="3"
        >
          <FaRegBuilding size={20} className="mr-2" />
          <b>{organization.name}</b>
        </SectionField>

        <SectionField valueTestId="contact-organization-alias" label="Alias" labelWidth="3">
          {organization.alias}
        </SectionField>
        <SectionField
          valueTestId="contact-organization-incorporationNumber"
          label="Incorporation number"
          labelWidth="3"
        >
          {organization.incorporationNumber}
        </SectionField>

        <H3 className="mt-10">Preferred Contact</H3>
        <ContactInfoSubForm contactEntity={organization} />

        <H3 className="mt-10">Individual Contact(s)</H3>
        <Row>
          <Col>
            <SectionField
              label="Connected to this organization"
              labelWidth="3"
              valueTestId="contact-organization-person-list"
              tooltip="To unlink a contact from this organization, or edit a contact's information, click on the name and unlink from the individual contact page."
            >
              {organization.persons &&
                organization.persons.map((person: IContactPerson, index: number) => (
                  <Styled.ContactLink key={'organization-person-' + person.id + '-contact'}>
                    <Link
                      to={'/contact/P' + person.id}
                      key={`organization-person-${index}`}
                      className="d-block"
                      data-testid={`contact-organization-person`}
                    >
                      {person.fullName}
                    </Link>
                  </Styled.ContactLink>
                ))}
            </SectionField>
          </Col>
        </Row>
      </Section>

      <Section className="mb-4">
        <H2>Address</H2>
        {organizationAddresses.map((field: AddressField, index: number) => (
          <React.Fragment key={'contact-org-' + organization.id + '-address-' + index}>
            <H3 className="mt-10">{field.label}</H3>
            <Row className="pb-3" key={'org-address-' + index}>
              <Col md="3"></Col>
              <Col data-testid="contact-organization-address">
                {field.streetAddress1 && <div>{field.streetAddress1} </div>}
                {field.streetAddress2 && <div>{field.streetAddress2} </div>}
                {field.streetAddress3 && <div>{field.streetAddress3} </div>}
                <div>{field.municipalityAndProvince} </div>
                {field.postal && <div>{field.postal} </div>}
                {field.country && <div>{field.country}</div>}
              </Col>
            </Row>
          </React.Fragment>
        ))}
      </Section>

      <Section key={'contact-person-' + organization.id + '-comments'}>
        <H2>Comments</H2>
        <Row>
          <Col>
            <div data-testid="contact-organization-comment">{organization.comment}</div>
          </Col>
        </Row>
      </Section>
    </>
  );
};

export default OrganizationView;
