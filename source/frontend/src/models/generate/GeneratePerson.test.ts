import { getMockApiAddress } from 'mocks/mockAddress';
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
