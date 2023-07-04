/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const LetterRecipientsFormYupSchema = Yup.object().shape({
  recipients: Yup.array().min(1, 'At least one recipient is required'),
});
