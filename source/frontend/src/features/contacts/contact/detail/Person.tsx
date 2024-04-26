import { Col } from 'react-bootstrap';
import { FaCircle, FaRegBuilding, FaRegUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { FormSection } from '@/components/common/form/styles';
import { AddressField } from '@/features/contacts/interfaces';
import { IContactOrganization, IContactPerson } from '@/interfaces/IContact';

import * as Styled from '../../styles';
import { toAddressFields } from '../utils/contactUtils';
import ContactInfoSubForm from './ContactInfoSubForm';

export interface PersonViewProps {
  person: IContactPerson;
}

const PersonView: React.FunctionComponent<React.PropsWithChildren<PersonViewProps>> = ({
  person,
}) => {
  let personAddresses: AddressField[];
  if (person.addresses === undefined) {
    personAddresses = [];
  } else {
    personAddresses = toAddressFields(person.addresses);
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
                <span key={'person-org-' + index}>
                  <Link
                    to={'/contact/O' + organization.id}
                    data-testid="contact-person-organization"
                  >
                    {organization.name}
                  </Link>
                  <br />
                </span>
              ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + person.id + '-contacts'} className="mb-4">
        <ContactInfoSubForm contactEntity={person} />
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

export default PersonView;
