export enum ContactMethodTypes {
  Fax = 'FAX',
  PersonalEmail = 'PERSEMAIL',
  PersonalPhone = 'PERSPHONE',
  PersonalMobile = 'PERSMOBIL',
  WorkEmail = 'WORKEMAIL',
  WorkPhone = 'WORKPHONE',
  WorkMobile = 'WORKMOBIL',
}

export const validPhoneTypes: ContactMethodTypes[] = [
  ContactMethodTypes.WorkPhone,
  ContactMethodTypes.WorkMobile,
  ContactMethodTypes.PersonalPhone,
  ContactMethodTypes.PersonalMobile,
  ContactMethodTypes.Fax,
];

export const validEmailTypes: ContactMethodTypes[] = [
  ContactMethodTypes.WorkEmail,
  ContactMethodTypes.PersonalEmail,
];
