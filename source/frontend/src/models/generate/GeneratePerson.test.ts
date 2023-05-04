import { ContactMethodTypes } from 'constants/contactMethodType';
import { getMockPerson } from 'mocks/mockContacts';

import { GeneratePerson } from './GeneratePerson';

describe('GenerateContact tests', () => {
  it('Can generate an empty contact without throwing an error', () => {
    const contact = new GeneratePerson(null);
    expect(contact.given_name).toBe('');
  });
  it('Can generate a contact email, preferring work email', () => {
    const contact = new GeneratePerson({
      ...getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: 'PERSEMAIL',
            description: 'Personal email',
            isDisabled: false,
          },
          value: 'mypersonalemail@hotmail.com',
          rowVersion: 1,
        },
        {
          id: 4,
          contactMethodType: {
            id: 'WORKEMAIL',
            description: 'work email',
            isDisabled: false,
          },
          value: 'myworkemail@biz.com',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.email).toBe(`myworkemail@biz.com`);
  });

  it('Can generate a contact email if only personal email is present', () => {
    const contact = new GeneratePerson({
      ...getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: 'PERSEMAIL',
            description: 'Personal email',
            isDisabled: false,
          },
          value: 'mypersonalemail@hotmail.com',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.email).toBe(`mypersonalemail@hotmail.com`);
  });

  it('Can generate a contact phone, preferring work phone', () => {
    const contact = new GeneratePerson({
      ...getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: ContactMethodTypes.PersonalPhone,
            description: 'Personal phone',
            isDisabled: false,
          },
          value: '22222222222',
          rowVersion: 1,
        },
        {
          id: 4,
          contactMethodType: {
            id: ContactMethodTypes.WorkPhone,
            description: 'work phone',
            isDisabled: false,
          },
          value: '11111111111',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.phone).toBe(`11111111111`);
  });

  it('Can generate a contact phone if only personal phone is present', () => {
    const contact = new GeneratePerson({
      ...getMockPerson({ id: 1, surname: 'last', firstName: 'first' }),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: ContactMethodTypes.PersonalPhone,
            description: 'Personal phone',
            isDisabled: false,
          },
          value: '1111111111',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.phone).toBe(`1111111111`);
  });

  it('Can generate organization names', () => {
    const mockPerson = getMockPerson({ id: 1, surname: 'last', firstName: 'first' });
    const contact = new GeneratePerson({
      ...mockPerson,
      personOrganizations: [
        ...(mockPerson.personOrganizations ?? []),
        ...(mockPerson.personOrganizations ?? []),
      ],
    });
    expect(contact.organizations).toBe(
      `Dairy Queen Forever! Property Management, Dairy Queen Forever! Property Management`,
    );
  });
});
