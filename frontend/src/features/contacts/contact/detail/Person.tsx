import { FormSection } from 'components/common/form/styles';
import { AddressTypes } from 'constants/addressTypes';
import { ContactMethodTypes } from 'constants/contactMethodType';
import { Dictionary } from 'interfaces/Dictionary';
import {
  IContactAddress,
  IContactMethod,
  IContactOrganization,
  IContactPerson,
} from 'interfaces/IContact';
import * as React from 'react';
import { Col } from 'react-bootstrap';
import { FaCircle, FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { phoneFormatter } from 'utils/formUtils';

import * as Styled from '../../styles';

export interface PersonViewProps {
  person: IContactPerson;
}

interface ContactInfoField {
  info: string;
  label: string;
}

interface AddressField {
  address: IContactAddress;
  label: string;
}

const PersonView: React.FunctionComponent<PersonViewProps> = ({ person }) => {
  const phoneTypes: Dictionary<string> = {};
  phoneTypes[ContactMethodTypes.WorkMobil] = 'Mobile';
  phoneTypes[ContactMethodTypes.WorkPhone] = 'Home';
  phoneTypes[ContactMethodTypes.PersPhone] = 'Work';
  phoneTypes[ContactMethodTypes.Fax] = 'Fax';

  const personPhoneNumbers: ContactInfoField[] = getContactInfo(person, phoneTypes);

  const emailTypes: Dictionary<string> = {};
  emailTypes[ContactMethodTypes.WorkEmail] = 'Work';
  emailTypes[ContactMethodTypes.PerseEmail] = 'Personal';
  const personEmails: ContactInfoField[] = getContactInfo(person, emailTypes);

  let personAddresses: AddressField[];
  if (person.addresses === undefined) {
    personAddresses = [];
  } else {
    personAddresses = person.addresses.reduce(
      (accumulator: AddressField[], value: IContactAddress) => {
        accumulator.push({ label: value.addressType.description, address: value });

        return accumulator;
      },
      [],
    );
  }

  return (
    <>
      <FormSection key={'contact-person-' + person.id + '-names'} className="mb-4">
        <Styled.RowAligned>
          <Col>
            <Styled.H2 data-testid="contact-person-fullname">
              <FaRegUser size={20} className="mr-2" />
              {person.fullName}
            </Styled.H2>
          </Col>
          <Col md="auto" className="ml-auto">
            <Styled.StatusIndicators className={person.isDisabled ? 'inactive' : 'active'}>
              <FaCircle size={7} className="mr-2" />
              <span data-testid="contact-person-status">
                {person.isDisabled ? 'INACTIVE' : 'ACTIVE'}
              </span>
            </Styled.StatusIndicators>
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col md="auto">
            <strong>Preferred name:</strong>
          </Col>
          <Col md="auto">
            <span data-testid="contact-person-preferred">{person.preferredName}</span>
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-organization'} className="mb-4">
        <Styled.RowAligned>
          <Col md="auto">
            <FaRegBuilding className="mr-2" />
            <strong>Organization(s):</strong>
          </Col>
          <Col md="auto">
            {person.organizations &&
              person.organizations.map((organization: IContactOrganization) => (
                <Link
                  to={'/organization/' + organization.id}
                  data-testid="contact-person-organization"
                >
                  {organization.name}
                </Link>
              ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-contacts'} className="mb-4">
        <Styled.H2Primary>Contact info</Styled.H2Primary>
        <Styled.RowAligned className="pb-4">
          <Col md={2}>
            <strong>Email:</strong>
          </Col>
          <Col>
            {personEmails.length === 0 && <span>N.A</span>}
            {personEmails.map((field: ContactInfoField, index: number) => (
              <Styled.RowAligned>
                <Col md="4" data-testid="email-value">
                  {field.info}
                </Col>
                <Col md="auto">
                  <em>{field.label}</em>
                </Col>
              </Styled.RowAligned>
            ))}
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col md={2}>
            <strong>Phone:</strong>
          </Col>
          <Col>
            {personPhoneNumbers.length === 0 && <span>N.A</span>}
            {personPhoneNumbers.map((field: ContactInfoField, index: number) => (
              <Styled.RowAligned>
                <Col md="4" data-testid="phone-value">
                  {phoneFormatter(field.info)}
                </Col>
                <Col md="auto">
                  <em>{field.label}</em>
                </Col>
              </Styled.RowAligned>
            ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-address'} className="mb-4">
        <Styled.H2Primary>Address</Styled.H2Primary>
        {personAddresses.map((field: AddressField, index: number) => (
          <Styled.RowAligned className="pb-3">
            <Col>
              <div className="pb-2">
                <strong>{field.label}</strong>
                {field.address.addressType.id === AddressTypes.Mailing && (
                  <em className="ml-4">from organization</em>
                )}
              </div>
              <span data-testid="contact-person-address">
                <div>{field.address.streetAddress1} </div>
                {field.address.streetAddress2 && <div>{field.address.streetAddress2} </div>}
                {field.address.streetAddress3 && <div>{field.address.streetAddress3} </div>}
                <div>{field.address.municipality + ' ' + field.address.provinceCode} </div>
                <div>{field.address.postal} </div>
                <div>{field.address.country}</div>
                {index + 1 !== personAddresses.length && <hr></hr>}
              </span>
            </Col>
          </Styled.RowAligned>
        ))}
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-comments'}>
        <Styled.RowAligned>
          <Col>
            <div>
              <strong>Comments:</strong>
            </div>
            <span data-testid="contact-person-comment">{person.comment}</span>
          </Col>
        </Styled.RowAligned>
      </FormSection>
    </>
  );
};

function getContactInfo(
  person: IContactPerson,
  validTypes: Dictionary<string>,
): ContactInfoField[] {
  if (person.contactMethods === undefined) {
    return [];
  }
  // Get only the valid types
  let filteredFields = person.contactMethods.reduce(
    (accumulator: ContactInfoField[], method: IContactMethod) => {
      if (Object.keys(validTypes).includes(method.contactMethodType.id)) {
        accumulator.push({
          info: method.value,
          label: validTypes[method.contactMethodType.id],
        });
      }
      return accumulator;
    },
    [],
  );

  // Sort according to the dictionary order
  return filteredFields.sort((a, b) => {
    return Object.values(validTypes).indexOf(a.label) - Object.values(validTypes).indexOf(b.label);
  });
}

export default PersonView;
