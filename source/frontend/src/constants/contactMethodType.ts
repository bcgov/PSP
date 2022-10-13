export enum ContactMethodTypes {
  Fax = 'FAX',
  PersonalEmail = 'PERSEMAIL',
  PersonalPhone = 'PERSPHONE',
  PersonalMobile = 'PERSMOBIL',
  WorkEmail = 'WORKEMAIL',
  WorkPhone = 'WORKPHONE',
  WorkMobile = 'WORKMOBIL',
}

export const PhoneContactMethods: string[] = [
  ContactMethodTypes.WorkPhone,
  ContactMethodTypes.WorkMobile,
  ContactMethodTypes.PersonalPhone,
  ContactMethodTypes.PersonalMobile,
  ContactMethodTypes.Fax,
];

export const EmailContactMethods: string[] = [
  ContactMethodTypes.WorkEmail,
  ContactMethodTypes.PersonalEmail,
];
