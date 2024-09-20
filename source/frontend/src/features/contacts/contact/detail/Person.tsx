import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { H2, H3 } from '@/components/common/styles';
import { AddressField } from '@/features/contacts/interfaces';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { formatApiPersonNames } from '@/utils/personUtils';

import * as Styled from '../../styles';
import { toAddressFields } from '../utils/contactUtils';
import ContactInfoSubForm from './ContactInfoSubForm';

export interface PersonFormViewProps {
  person: ApiGen_Concepts_Person;
}

const PersonFormView: React.FunctionComponent<React.PropsWithChildren<PersonFormViewProps>> = ({
  person,
}) => {
  let personAddresses: AddressField[];
  if (person.personAddresses === undefined) {
    personAddresses = [];
  } else {
    personAddresses = toAddressFields(person.personAddresses);
  }

  return (
    <>
      <Section key={'contact-person-' + person.id + '-names'} className="mb-4">
        <H2>
          <Row className="mb-1">
            <Col>Contact Details</Col>
            <Col md={3} className="d-flex justify-content-end">
              <Styled.StatusIndicators className={person.isDisabled ? 'inactive' : 'active'}>
                <FaRegUser size={15} className="mr-2 mb-1" />
                <span data-testid="contact-person-status">
                  {person.isDisabled ? 'INACTIVE' : 'ACTIVE'}
                </span>
              </Styled.StatusIndicators>
            </Col>
          </Row>
        </H2>

        <SectionField label="Individual name" labelWidth="4" valueTestId="contact-person-fullname">
          <FaRegUser size={20} className="mr-2" />
          <b>{formatApiPersonNames(person)}</b>
        </SectionField>

        <SectionField label="Preferred name" labelWidth="4" valueTestId="contact-person-preferred">
          <p>{person.preferredName}</p>
        </SectionField>

        <SectionField
          label="Linked organization"
          labelWidth="4"
          valueTestId="contact-person-organization"
        >
          {person.personOrganizations &&
            person.personOrganizations.map(
              (personOrganization: ApiGen_Concepts_PersonOrganization, index: number) => (
                <Styled.ContactLink key={'contact-person-' + person.id + '-organization'}>
                  <FaRegBuilding size={20} className="mr-2" />
                  <Link
                    to={'/contact/O' + personOrganization.organizationId}
                    key={'person-org-' + index}
                  >
                    {personOrganization?.organization?.name}
                  </Link>
                </Styled.ContactLink>
              ),
            )}
        </SectionField>

        <H3 className="mt-10">Contact Info</H3>
        <ContactInfoSubForm contactEntity={person} />
      </Section>

      <Section>
        <H2>Address</H2>
        {personAddresses.map((field: AddressField, index: number) => (
          <React.Fragment key={'contact-person-' + person.id + '-address-' + index}>
            <H3 className="mt-10">{field.label}</H3>
            <Row className="pt-4" key={'person-address-' + index}>
              <Col md="3"></Col>
              <Col data-testid="contact-person-address">
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

      <Section key={'contact-person-' + person.id + '-comments'}>
        <H2>Comments</H2>
        <Row>
          <Col>
            <div data-testid="contact-person-comment">{person.comment}</div>
          </Col>
        </Row>
      </Section>
    </>
  );
};

export default PersonFormView;
