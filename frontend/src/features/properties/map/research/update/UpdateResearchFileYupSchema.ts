import * as Yup from 'yup';

export const UpdateResearchFileYupSchema = Yup.object().shape({
  name: Yup.string().required('Research File name is required'),
  properties: Yup.array().of(
    Yup.object().shape({
      pid: Yup.string().when('pin', {
        is: (pin: string) => !!pin,
        then: Yup.string().nullable(),
        otherwise: Yup.string().required('valid PID or PIN required'),
      }),
    }),
  ),
});
