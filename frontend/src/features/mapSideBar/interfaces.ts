import { AddressTypes, PropertyTypes } from 'constants/index';
import { IAddress } from 'interfaces';

export const defaultPropertyValues = {
  address: {
    addressTypeId: AddressTypes.Physical,
    streetAddress1: '',
    municipality: '',
    postal: '',
    province: '',
    provinceId: 1,
    region: '',
    district: '',
  } as IAddress,
  pid: '',
  propertyTypeId: PropertyTypes.Land,
  classification: '',
  region: '',
  district: '',
  zoning: '',
  landArea: '',
  landLegalDescription: '',

  // TODO: Theses properties probably should be replaced or updated.
  titleNumber: '',
  purpose: '',
  ruralArea: '',
  ocpDesignation: '',
};
