import { ContactMethodTypes } from 'constants/contactMethodType';

export const getMockOrganization = () => ({
  id: 2,
  contactMethods: [
    { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: '222-333-4444' },
    { contactMethodType: { id: ContactMethodTypes.WorkMobile }, value: '555-666-7777' },
  ],
});
