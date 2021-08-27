import { AddressTypes } from 'constants/index';
import { IAddress } from 'interfaces';

export interface IProperty {
  address: IAddress;
  titleNumber: string;
  legalDescription: string;
  Region: string;
  classification: string;
  purpose: string;
  regionalDistrict: string;
  ruralArea: string;
  electoralDistrict: string;
  municipality: string;
  zoning: string;
  ocpDesignation: string;
  pid: string;
  latitude?: number;
  longitude?: number;
}

export const defaultPropertyValues: IProperty = {
  address: {
    addressTypeId: AddressTypes.Physical,
    streetAddress1: '',
    municipality: '',
    postal: '',
    province: '',
    provinceId: 1,
  },
  titleNumber: '',
  legalDescription: '',
  Region: '',
  classification: '',
  purpose: '',
  regionalDistrict: '',
  ruralArea: '',
  electoralDistrict: '',
  municipality: '',
  zoning: '',
  ocpDesignation: '',
  pid: '',
};
