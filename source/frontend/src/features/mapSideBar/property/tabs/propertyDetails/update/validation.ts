/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { PropertyTenureTypes } from '@/constants/index';
import { exists } from '@/utils';
import { stringToBoolean } from '@/utils/formUtils';

import { PropertyTenureFormModel } from './models';

export const UpdatePropertyDetailsYupSchema = Yup.object().shape({
  municipalZoning: Yup.string().max(100, 'Zoning must be at most 100 characters'),
  tenures: Yup.array().of(
    Yup.object().shape({
      typeCode: Yup.string(),
      typeDescription: Yup.string(),
    }),
  ),
  roadTypes: Yup.array().when('tenures', {
    is: (tenureArray: PropertyTenureFormModel[]) =>
      tenureArray?.some(obj => obj.typeCode === PropertyTenureTypes.HighwayRoad),
    then: Yup.array()
      .min(1, `Highway/Road is required if tenure status includes 'Highway/Road'`)
      .required(),
    otherwise: Yup.array().nullable(),
  }),
  volumetricParcelTypeCode: Yup.string().when('isVolumetricParcel', {
    is: (isVolumetricParcel: string) => stringToBoolean(isVolumetricParcel) === true,
    then: Yup.string().required('Volumetric Type is required'),
    otherwise: Yup.string().nullable(),
  }),
  notes: Yup.string().max(4000, 'Comments must be less than 4000 characters'),
  address: Yup.object().shape({
    streetAddress1: Yup.string().max(200, 'Address (line 1) must be at most 200 characters'),
    streetAddress2: Yup.string().max(200, 'Address (line 2) must be at most 200 characters'),
    streetAddress3: Yup.string().max(200, 'Address (line 3) must be at most 200 characters'),
    municipality: Yup.string().max(200, 'City must be at most 200 characters'),
    postal: Yup.string().max(20, 'Postal must be at most 20 characters'),
  }),
  generalLocation: Yup.string().max(2000, 'General location must be less than 2000 characters'),
  historicalNumbers: Yup.array().of(
    Yup.object().shape({
      historicalNumber: Yup.string()
        .required('Historical File # is required')
        .max(500, 'Historical File # must be at most ${max} characters'),
      historicalNumberType: Yup.string().required('Historical File # type is required'),
      otherHistoricalNumberType: Yup.string().when('historicalNumberType', {
        is: (historicalType: string) => exists(historicalType) && historicalType === 'OTHER',
        then: Yup.string()
          .required('Other Description is required')
          .max(200, 'Other Description must be at most ${max} characters'),
        otherwise: Yup.string().nullable(),
      }),
    }),
  ),
});
