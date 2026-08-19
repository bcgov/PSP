export enum RestrictContactType {
  ONLY_INDIVIDUALS = 'persons',
  ONLY_ORGANIZATIONS = 'organizations',
  ONLY_PIMSUSERS = 'pimsusers',
}

export const allContactTypes: RestrictContactType[] = [
  RestrictContactType.ONLY_PIMSUSERS,
  RestrictContactType.ONLY_INDIVIDUALS,
  RestrictContactType.ONLY_ORGANIZATIONS,
];
