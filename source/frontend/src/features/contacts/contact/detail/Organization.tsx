import { Col } from 'react-bootstrap';
import { FaCircle, FaRegBuilding } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { FormSection } from '@/components/common/form/styles';
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
              <strong>Incorporation Number:</strong>
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
        <ContactInfoSubForm contactEntity={organization} />
      </FormSection>
      <FormSection key={'contact-org-' + organization.id + '-address'} className="mb-4">
        <Styled.H2Primary>Address</Styled.H2Primary>
        {organizationAddresses.map((field: AddressField, index: number) => (
          <Styled.RowAligned className="pb-3" key={'org-address-' + index}>
            <Col md="4">
              <strong>{field.label}:</strong>
            </Col>
            <Col data-testid="contact-organization-address">
              {field.streetAddress1 && <div>{field.streetAddress1} </div>}
              {field.streetAddress2 && <div>{field.streetAddress2} </div>}
              {field.streetAddress3 && <div>{field.streetAddress3} </div>}
              <div>{field.municipalityAndProvince} </div>
              {field.postal && <div>{field.postal} </div>}
              {field.country && <div>{field.country}</div>}
              {index + 1 !== organizationAddresses.length && <hr></hr>}
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
              organization.persons.map((person: IContactPerson, index: number) => (
                <span key={`organization-person-${index}`}>
                  <Link to={'/contact/P' + person.id} data-testid="contact-organization-person">
                    {person.fullName}
                  </Link>
                  <br />
                </span>
              ))}
          </Col>
        </Styled.RowAligned>
      </FormSection>
      <FormSection key={'contact-person-' + organization.id + '-comments'}>
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

export default OrganizationView;
