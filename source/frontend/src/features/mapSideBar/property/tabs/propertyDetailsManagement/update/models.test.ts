import { Api_PropertyContact } from '@/models/api/Property';

import { PropertyContactFormModel } from './models';

describe('Property Contact model tests', () => {
  it('PropertyContactFormModel toApi sets person properly if form is person that is associated to an org.', () => {
    const stakeholderModel = new PropertyContactFormModel();
    stakeholderModel.contact = {
      id: '1',
      personId: 1,
      person: { firstName: 'first' },
      organizationId: 1,
      organization: { name: 'org' },
    };
    const apiModel = stakeholderModel.toApi();
    expect(apiModel.organizationId).toBeNull();
  });

  it('PropertyContactFormModel fromApi sets person properly if form is person that is associated to an org.', () => {
    const apiContact: Api_PropertyContact = {
      id: 1,
      propertyId: 1,
      organizationId: 1,
      organization: { name: 'org' },
      personId: 2,
      person: { firstName: 'first' },
      primaryContactId: 3,
      primaryContact: { firstName: 'primary' },
      purpose: 'purpose',
      rowVersion: 1,
    };
    const apiModel = PropertyContactFormModel.fromApi(apiContact);
    expect(apiModel.contact?.organization).toBeUndefined();
  });
});
