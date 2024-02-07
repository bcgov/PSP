import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_PropertyContact } from '@/models/api/generated/ApiGen_Concepts_PropertyContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

import { PropertyContactFormModel } from './models';

describe('Property Contact model tests', () => {
  it('PropertyContactFormModel toApi sets person properly if form is person that is associated to an org.', () => {
    const stakeholderModel = new PropertyContactFormModel();
    stakeholderModel.contact = {
      id: '1',
      personId: 1,
      person: {
        firstName: 'first',
        surname: 'last',
        middleNames: 'middle',
        isDisabled: false,
        id: 1,
        preferredName: '',
        comment: '',
        contactMethods: [],
        personAddresses: [],
        rowVersion: 0,
        personOrganizations: [
          {
            personId: 1,
            organization: {
              id: 1,
              name: 'org',
              isDisabled: false,
              alias: '',
              incorporationNumber: '',
              comment: '',
              rowVersion: 0,
              organizationPersons: [],
              organizationAddresses: [],
              contactMethods: [],
            },
            rowVersion: 0,
          },
        ],
      },
    };
    const apiModel = stakeholderModel.toApi();
    expect(apiModel.organizationId).toBeNull();
  });

  it('PropertyContactFormModel fromApi sets person properly if form is person that is associated to an org.', () => {
    const apiContact: ApiGen_Concepts_PropertyContact = {
      id: 1,
      propertyId: 1,
      organizationId: 1,
      organization: { ...getEmptyOrganization(), name: 'org' },
      personId: 2,
      person: { ...getEmptyPerson(), firstName: 'first' },
      primaryContactId: 3,
      primaryContact: { ...getEmptyPerson(), firstName: 'primary' },
      purpose: 'purpose',
      ...getEmptyBaseAudit(1),
    };
    const apiModel = PropertyContactFormModel.fromApi(apiContact);
    expect(apiModel.contact?.organization).toBeUndefined();
  });
});
