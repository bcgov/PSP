import { IApiPerson } from 'hooks/pims-api/interfaces/IApiPerson';

export interface IFormPerson extends IApiPerson {}

export const defaultPerson: IFormPerson = {
  isDisabled: false,
  firstName: '',
  middleNames: '',
  surname: '',
  preferredName: '',
  comment: '',
  contactMethods: [],
};
