import { FormSection } from 'components/common/form/styles';
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
  label: string;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipalityAndProvince?: string;
  country?: string;
  postal?: string;
}

const PersonView: React.FunctionComponent<PersonViewProps> = ({ person }) => {
  const phoneTypes: Dictionary<string> = {};
  phoneTypes[ContactMethodTypes.WorkMobile] = 'Mobile';
  phoneTypes[ContactMethodTypes.WorkPhone] = 'Work';
  phoneTypes[ContactMethodTypes.PersonalMobile] = 'Home';
  phoneTypes[ContactMethodTypes.Fax] = 'Fax';
  phoneTypes[ContactMethodTypes.PersonalPhone] = 'Landline';

  const personPhoneNumbers: ContactInfoField[] = getContactInfo(person, phoneTypes);

  const emailTypes: Dictionary<string> = {};
  emailTypes[ContactMethodTypes.WorkEmail] = 'Work';
  emailTypes[ContactMethodTypes.PersonalEmail] = 'Personal';
  const personEmails: ContactInfoField[] = getContactInfo(person, emailTypes);

  let personAddresses: AddressField[];
  if (person.addresses === undefined) {
    personAddresses = [];
  } else {
    personAddresses = person.addresses.reduce(
      (accumulator: AddressField[], value: IContactAddress) => {
        accumulator.push({
          label: value.addressType.description || '',
          streetAddress1: value.streetAddress1,
          streetAddress2: value.streetAddress2,
          streetAddress3: value.streetAddress3,
          municipalityAndProvince:
            (value.municipality !== undefined ? value.municipality + ' ' : '') +
            value.province.provinceStateCode,
          country: value.country?.description,
          postal: value.postal,
        });

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
              person.organizations.map((organization: IContactOrganization, index: number) => (
                <>
                  <Link
                    to={'/contact/O' + organization.id}
                    data-testid="contact-person-organization"
                    key={'person-org-' + index}
                  >
                    {organization.name}
                  </Link>
                  <br />
                </>
              ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-contacts'} className="mb-4">
        <Styled.H2Primary>Contact info</Styled.H2Primary>
        <Styled.RowAligned className="pb-4">
          <Col md="2">
            <strong>Email:</strong>
          </Col>
          <Col md="10">
            {personEmails.length === 0 && <span>N.A</span>}
            {personEmails.map((field: ContactInfoField, index: number) => (
              <Styled.RowAligned key={'person-email-' + index}>
                <Col data-testid="email-value">{field.info}</Col>
                <Col>
                  <em>{field.label}</em>
                </Col>
              </Styled.RowAligned>
            ))}
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col md="2">
            <strong>Phone:</strong>
          </Col>
          <Col md="10">
            {personPhoneNumbers.length === 0 && <span>N.A</span>}
            {personPhoneNumbers.map((field: ContactInfoField, index: number) => (
              <Styled.RowAligned key={'person-phone-' + index}>
                <Col data-testid="phone-value">{phoneFormatter(field.info)}</Col>
                <Col>
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
          <Styled.RowAligned className="pb-3" key={'person-address-' + index}>
            <Col>
              <div className="pb-2">
                <strong>{field.label}</strong>
              </div>
              <span data-testid="contact-person-address">
                {field.streetAddress1 && <div>{field.streetAddress1} </div>}
                {field.streetAddress2 && <div>{field.streetAddress2} </div>}
                {field.streetAddress3 && <div>{field.streetAddress3} </div>}
                <div>{field.municipalityAndProvince} </div>
                {field.postal && <div>{field.postal} </div>}
                {field.country && <div>{field.country}</div>}
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
