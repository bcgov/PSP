export enum ContactMethodTypes {
  Fax = 'FAX',
  PersonalEmail = 'PERSEMAIL',
  PersonalPhone = 'PERSPHONE',
  PersonalMobile = 'PERSMOBIL',
  WorkEmail = 'WORKEMAIL',
  WorkPhone = 'WORKPHONE',
  WorkMobile = 'WORKMOBIL',
}

export const phoneContactMethods: string[] = [
  ContactMethodTypes.WorkPhone,
  ContactMethodTypes.WorkMobile,
  ContactMethodTypes.PersonalPhone,
  ContactMethodTypes.PersonalMobile,
  ContactMethodTypes.Fax,
];

export const emailContactMethods: string[] = [
  ContactMethodTypes.WorkEmail,
  ContactMethodTypes.PersonalEmail,
];
