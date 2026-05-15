/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { exists } from '@/utils/utils';

export const LeaseFilterYupSchema = yup.object().shape({
  lFileNo: yup
    .string()
    .nullable()
    .matches(/^\s*L?\s*-?\s*[0-9\s-]+\s*$/, 'Invalid L-File number'),
  expiryStartDate: yup.date(),
  expiryEndDate: yup
    .date()
    .when(
      'expiryStartDate',
      (expiryStartDate: any, schema: any) =>
        expiryStartDate &&
        schema.min(yup.ref('expiryStartDate'), 'Expiry "to" Date must be after "from" Date'),
    ),
  leaseTeamMemberProfileTypeCode: yup.string(),
  leaseTeamMembers: yup.array().when('leaseTeamMemberProfileTypeCode', {
    is: (leaseTeamMemberProfileTypeCode: string) => exists(leaseTeamMemberProfileTypeCode),
    then: yup.array().min(1, 'Team member is required'),
    otherwise: yup.array(),
  }),
});
