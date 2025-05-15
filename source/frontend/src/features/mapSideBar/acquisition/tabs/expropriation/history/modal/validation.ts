/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const ExpropriationEventYupSchema = Yup.object().shape({
  eventTypeCode: Yup.string().nullable().required('Event is required'),
});
