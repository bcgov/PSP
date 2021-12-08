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
import { FaCircle, FaRegBuilding } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { phoneFormatter } from 'utils/formUtils';

import * as Styled from '../../styles';

export interface OrganizationViewProps {
  organization: IContactOrganization;
}

interface ContactInfoField {
  info: string;
  label: string;
}

interface AddressField {
  address: IContactAddress;
  label: string;
}

const OrganizationView: React.FunctionComponent<OrganizationViewProps> = ({ organization }) => {
  const phoneTypes: Dictionary<string> = {};
  phoneTypes[ContactMethodTypes.WorkMobil] = 'Mobile';
  phoneTypes[ContactMethodTypes.WorkPhone] = 'Work';
  phoneTypes[ContactMethodTypes.PersPhone] = 'Home';
  phoneTypes[ContactMethodTypes.Fax] = 'Fax';

  const personPhoneNumbers: ContactInfoField[] = getContactInfo(organization, phoneTypes);

  const emailTypes: Dictionary<string> = {};
  emailTypes[ContactMethodTypes.WorkEmail] = 'Work';
  emailTypes[ContactMethodTypes.PerseEmail] = 'Personal';
  const personEmails: ContactInfoField[] = getContactInfo(organization, emailTypes);

  let personAddresses: AddressField[];
  if (organization.addresses === undefined) {
    personAddresses = [];
  } else {
    personAddresses = organization.addresses.reduce(
      (accumulator: AddressField[], value: IContactAddress) => {
        accumulator.push({ label: value.addressType.description, address: value });

        return accumulator;
      },
      [],
    );
  }

  return (
    <>
      <FormSection key={'contact-org-' + organization.id + '-names'} className="mb-4">
        <Styled.RowAligned>
          <Col>
            <Styled.H2>
              <FaRegBuilding size={20} className="mr-2" />
              <span data-testid="contact-organization-fullname">{organization.name}</span>
            </Styled.H2>
          </Col>
          <Col md="auto" className="ml-auto">
            <Styled.StatusIndicators className={organization.isDisabled ? 'inactive' : 'active'}>
              <FaCircle size={7} className="mr-2" />
              <span data-testid="contact-organization-status">
                {organization.isDisabled ? 'INACTIVE' : 'ACTIVE'}
              </span>
            </Styled.StatusIndicators>
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col className="ml-5">
            <div>
              <strong>Alias:</strong>
            </div>
            <div>
              <span data-testid="contact-organization-alias">{organization.alias}</span>
            </div>
            <div>
              <strong>Incorportation Number:</strong>
            </div>
            <div>
              <span data-testid="contact-organization-incorporationNumber">
                {organization.incorporationNumber}
              </span>
            </div>
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-org-' + organization.id + '-contacts'} className="mb-4">
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
      <FormSection key={'contact-org-' + organization.id + '-address'} className="mb-4">
        <Styled.H2Primary>Address</Styled.H2Primary>
        {personAddresses.map((field: AddressField, index: number) => (
          <Styled.RowAligned className="pb-3">
            <Col md="4">
              <strong>{field.label}:</strong>
            </Col>
            <Col data-testid="contact-organization-address">
              <div>{field.address.streetAddress1} </div>
              {field.address.streetAddress2 && <div>{field.address.streetAddress2} </div>}
              {field.address.streetAddress3 && <div>{field.address.streetAddress3} </div>}
              <div>{field.address.municipality + ' ' + field.address.provinceCode} </div>
              <div>{field.address.postal} </div>
              <div>{field.address.country}</div>
              {index + 1 !== personAddresses.length && <hr></hr>}
            </Col>
          </Styled.RowAligned>
        ))}
      </FormSection>
      <FormSection key={'contact-org-' + organization.id + '-individual'} className="mb-4">
        <Styled.RowAligned>
          <Col>
            <Styled.H2Primary>Individual Contacts</Styled.H2Primary>
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col>
            {organization.persons &&
              organization.persons.map((person: IContactPerson) => (
                <Link to={'/person/' + person.id} data-testid="contact-organization-person">
                  {person.fullName}
                </Link>
              ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={organization.id + '-Comments'}>
        <Styled.RowAligned>
          <Col>
            <div>
              <strong>Comments:</strong>
            </div>
            <div data-testid="contact-organization-comment">{organization.comment}</div>
          </Col>
        </Styled.RowAligned>
      </FormSection>
    </>
  );
};

function getContactInfo(
  organization: IContactOrganization,
  validTypes: Dictionary<string>,
): ContactInfoField[] {
  if (organization.contactMethods === undefined) {
    return [];
  }

  // Get only the valid types
  let filteredFields = organization.contactMethods.reduce(
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

export default OrganizationView;
