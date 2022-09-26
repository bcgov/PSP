import { PropertyTenureTypes } from 'constants/index';
import { stringToBoolean } from 'utils/formUtils';
import * as Yup from 'yup';

import { PropertyTenureFormModel } from './models';

export const UpdatePropertyDetailsYupSchema = Yup.object().shape({
  municipalZoning: Yup.string().max(100),
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
  adjacentLands: Yup.array().when('tenures', {
    is: (tenureArray: PropertyTenureFormModel[]) =>
      tenureArray?.some(obj => obj.typeCode === PropertyTenureTypes.AdjacentLand),
    then: Yup.array()
      .min(1, `Adjacent land type is required if tenure status includes 'Adjacent Land'`)
      .required(),
    otherwise: Yup.array().nullable(),
  }),
  volumetricParcelTypeCode: Yup.string().when('isVolumetricParcel', {
    is: (isVolumetricParcel: string) => stringToBoolean(isVolumetricParcel) === true,
    then: Yup.string().required('Volumetric Type is required'),
    otherwise: Yup.string().nullable(),
  }),
  notes: Yup.string().max(4000, 'Notes must be less than 4000 characters'),
});
