import * as yup from 'yup';

export const CompensationRequisitionYupSchema = yup.object().shape({
  agreementDate: yup.string(),
});
