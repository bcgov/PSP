import * as React from 'react';
import { Col } from 'react-bootstrap';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { ContactInfoField } from '@/features/contacts/interfaces';
import { Dictionary } from '@/interfaces/Dictionary';
import { IContactEntity } from '@/interfaces/IContact';
import { phoneFormatter } from '@/utils/formUtils';

import * as Styled from '../../styles';
import { getContactInfo } from './utils';

interface IContactInfoSubFormProps {
  contactEntity: IContactEntity;
}

const phoneTypes: Dictionary<string> = {};
phoneTypes[ContactMethodTypes.WorkMobile] = 'Mobile';
phoneTypes[ContactMethodTypes.WorkPhone] = 'Work';
phoneTypes[ContactMethodTypes.PersonalMobile] = 'Home';
phoneTypes[ContactMethodTypes.Fax] = 'Fax';
phoneTypes[ContactMethodTypes.PersonalPhone] = 'Landline';

const emailTypes: Dictionary<string> = {};
emailTypes[ContactMethodTypes.WorkEmail] = 'Work';
emailTypes[ContactMethodTypes.PersonalEmail] = 'Personal';

const ContactInfoSubForm: React.FunctionComponent<
  React.PropsWithChildren<IContactInfoSubFormProps>
> = ({ contactEntity }) => {
  const phoneNumbers: ContactInfoField[] = getContactInfo(
    phoneTypes,
    contactEntity?.contactMethods,
  );
  const emails: ContactInfoField[] = getContactInfo(emailTypes, contactEntity?.contactMethods);

  return (
    <>
      <Styled.RowAligned className="pb-4">
        <Col md="3">
          <strong>Email:</strong>
        </Col>
        <Col md="9">
          {emails.length === 0 && <span>N.A</span>}
          {emails.map((field: ContactInfoField, index: number) => (
            <Styled.RowAligned key={'contact-email-' + index}>
              <Col data-testid="email-value">{field.info}</Col>
              <Col>
                <em>{field.label}</em>
              </Col>
            </Styled.RowAligned>
          ))}
        </Col>
      </Styled.RowAligned>
      <Styled.RowAligned>
        <Col md="3">
          <strong>Phone:</strong>
        </Col>
        <Col md="9">
          {phoneNumbers.length === 0 && <span>N.A</span>}
          {phoneNumbers.map((field: ContactInfoField, index: number) => (
            <Styled.RowAligned key={'contact-phone-' + index}>
              <Col data-testid="phone-value">{phoneFormatter(field.info)}</Col>
              <Col>
                <em>{field.label}</em>
              </Col>
            </Styled.RowAligned>
          ))}
        </Col>
      </Styled.RowAligned>
    </>
  );
};

export default ContactInfoSubForm;
