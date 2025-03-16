/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { createNumberSchema } from '@/utils/YupSchema';

export const PropertyFilterValidationSchema = Yup.object().shape({
  pid: Yup.string().nullable(),
  pin: Yup.string().nullable(),
  coordinates: Yup.object().when('searchBy', {
    is: 'coordinates',
    then: Yup.object().shape({
      latitude: Yup.object().shape({
        degrees: createNumberSchema('Degrees', 0, 89, true, true),
        minutes: createNumberSchema('Minutes', 0, 59, true, true),
        seconds: createNumberSchema('Seconds', 0, 59, true, false),
      }),
      longitude: Yup.object().shape({
        degrees: createNumberSchema('Degrees', 0, 179, true, true),
        minutes: createNumberSchema('Minutes', 0, 59, true, true),
        seconds: createNumberSchema('Seconds', 0, 59, true, false),
      }),
    }),
    otherwise: Yup.object().nullable(),
  }),
});
