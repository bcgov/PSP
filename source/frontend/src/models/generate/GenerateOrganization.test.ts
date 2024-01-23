import { ContactMethodTypes } from '@/constants/contactMethodType';
import { getMockOrganization } from '@/mocks/organization.mock';

import { Api_GenerateOrganization } from './GenerateOrganization';

describe('GenerateOrganization tests', () => {
  it('generates an empty contact without throwing an error', () => {
    const contact = new Api_GenerateOrganization(null);
    expect(contact.name).toBe('');
  });

  it('generates a contact email, preferring work email', () => {
    const contact = new Api_GenerateOrganization({
      ...getMockOrganization(),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: 'PERSEMAIL',
            description: 'Personal email',
            isDisabled: false,
            displayOrder: null,
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
            displayOrder: null,
          },
          value: 'myworkemail@biz.com',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.email).toBe(`myworkemail@biz.com`);
  });

  it('generates a contact email if only personal email is present', () => {
    const contact = new Api_GenerateOrganization({
      ...getMockOrganization(),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: 'PERSEMAIL',
            description: 'Personal email',
            isDisabled: false,
            displayOrder: null,
          },
          value: 'mypersonalemail@hotmail.com',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.email).toBe(`mypersonalemail@hotmail.com`);
  });

  it('generates a contact phone, preferring work phone', () => {
    const contact = new Api_GenerateOrganization({
      ...getMockOrganization(),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: ContactMethodTypes.PersonalPhone,
            description: 'Personal phone',
            isDisabled: false,
            displayOrder: null,
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
            displayOrder: null,
          },
          value: '11111111111',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.phone).toBe(`1 111-111-1111`);
  });

  it('generates a contact phone if only personal phone is present', () => {
    const contact = new Api_GenerateOrganization({
      ...getMockOrganization(),
      contactMethods: [
        {
          id: 3,
          contactMethodType: {
            id: ContactMethodTypes.PersonalPhone,
            description: 'Personal phone',
            isDisabled: false,
            displayOrder: null,
          },
          value: '1111111111',
          rowVersion: 1,
        },
      ],
    });
    expect(contact.phone).toBe(`1 111-111-1111`);
  });

  it('generates organization address if present', () => {
    const contact = new Api_GenerateOrganization(getMockOrganization());
    expect(contact.address?.address_single_line_string).toBe(
      `1234 mock Street, N/A, Victoria, British Columbia, V1V1V1, Canada`,
    );
  });

  it('generates organization full name', () => {
    const contact = new Api_GenerateOrganization({
      ...getMockOrganization({ name: 'ABC Corp' }),
      incorporationNumber: 'BC123456',
    });
    expect(contact.full_name_string).toBe(`ABC Corp (Inc. No. BC123456)`);
  });
});
