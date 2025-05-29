import * as yup from 'yup';

declare module 'yup' {
  interface ArraySchema<T> {
    unique(message: string, mapper?: (value: T, index?: number, list?: T[]) => T[]): ArraySchema<T>;
  }
}

yup.addMethod(yup.array, 'unique', function (message, mapper = (val: unknown) => val) {
  return this.test(
    'unique',
    message,
    (list = []) => list.length === new Set(list.map(mapper)).size,
  );
});

export const ManagementTeamYupSchema = yup.object().shape({
  team: yup
    .array()
    .of(
      yup.object().shape(
        {
          teamProfileTypeCode: yup.string().when('contact', {
            is: (contact: object) => !!contact,
            then: yup.string().required('Select a profile'),
          }),
          contact: yup
            .object()
            .nullable()
            .when('teamProfileTypeCode', {
              is: (teamProfileTypeCode: string) => !!teamProfileTypeCode,
              then: yup.object().required('Select a team member').nullable(),
            }),
        },
        [
          ['teamProfileTypeCode', 'contact'],
          ['contact', 'teamProfileTypeCode'],
        ],
      ),
    )
    .unique(
      'You have selected a team member that already has the selected role.',
      (val: any) => val.teamProfileTypeCode + val?.contact?.organizationId + val?.contact?.personId,
    ),
});
