import * as Yup from 'yup';

export const UpdateAcquisitionTeamYupSchema = Yup.object().shape({
  fundingTypeOtherDescription: Yup.string().when('fundingTypeCode', {
    is: (fundingTypeCode: string) => fundingTypeCode && fundingTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  team: Yup.array().of(
    Yup.object().shape(
      {
        contactTypeCode: Yup.string().when('contact', {
          is: (contact: object) => !!contact,
          then: Yup.string().required('Select a profile'),
        }),
        contact: Yup.object().when('contactTypeCode', {
          is: (contactTypeCode: string) => !!contactTypeCode,
          then: Yup.object().required('Select a team member'),
        }),
      },
      [
        ['contactTypeCode', 'contact'],
        ['contact', 'contactTypeCode'],
      ],
    ),
  ),
});
