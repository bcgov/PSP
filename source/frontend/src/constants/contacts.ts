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

/** Restricts a team-member contact picker to PIMS users only when the selected team profile requires it. */
export const getTeamContactTypeRestriction = (
  teamProfileTypeCode: string | undefined,
  pimsUserOnlyProfileCodes: string[],
): RestrictContactType | undefined =>
  pimsUserOnlyProfileCodes.includes(teamProfileTypeCode ?? '')
    ? RestrictContactType.ONLY_PIMSUSERS
    : undefined;
