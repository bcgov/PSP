import { ContactMethodTypes } from '@/constants/contactMethodType';
import { UserTypes } from '@/constants/userTypes';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_Role } from '@/models/api/generated/ApiGen_Concepts_Role';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { ApiGen_Concepts_UserRole } from '@/models/api/generated/ApiGen_Concepts_UserRole';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { getPreferredContactMethodValue } from '@/utils/contactMethodUtil';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidIsoDateTime } from '@/utils/utils';

export class FormUser {
  id?: number;
  approvedById?: NumberFieldValue;
  keycloakUserId?: string;
  email?: string;
  businessIdentifierValue?: string;
  firstName?: string;
  surname?: string;
  isDisabled?: boolean;
  position?: string;
  userTypeCode?: ApiGen_Base_CodeType<string>;
  lastLogin?: string;
  appCreateTimestamp?: UtcIsoDateTime;
  issueDate?: string;
  rowVersion?: number;
  note?: string;
  person?: ApiGen_Concepts_Person;
  regions?: ApiGen_Base_CodeType<number>[];
  roles?: ApiGen_Concepts_Role[];

  constructor(user: ApiGen_Concepts_User) {
    this.id = user.id;
    this.keycloakUserId = user.guidIdentifierValue;
    this.email =
      (user?.person?.contactMethods &&
        getPreferredContactMethodValue(
          user?.person?.contactMethods,
          ContactMethodTypes.WorkEmail,
          ContactMethodTypes.PersonalEmail,
        )) ??
      '';
    this.businessIdentifierValue = user.businessIdentifierValue ?? undefined;
    this.firstName = user?.person?.firstName ?? '';
    this.surname = user?.person?.surname ?? '';
    this.isDisabled = user.isDisabled;
    this.position = user.position ?? '';
    this.userTypeCode = {
      id: user?.userTypeCode?.id ?? UserTypes.Contractor,
      description: user?.userTypeCode?.description ?? '',
      displayOrder: null,
      isDisabled: false,
    };
    this.lastLogin = user.lastLogin ?? undefined;
    this.appCreateTimestamp = user.appCreateTimestamp;
    this.note = user.note ?? undefined;
    this.roles = user?.userRoles?.map(userRole => userRole?.role).filter(exists) ?? [];
    this.regions = user?.userRegions?.map(userRegion => userRegion?.region).filter(exists) ?? [];
    this.person = user.person ?? undefined;
    this.rowVersion = user.rowVersion ?? undefined;
  }

  public toApi(): ApiGen_Concepts_User {
    return {
      id: this.id ?? 0,
      businessIdentifierValue: this.businessIdentifierValue ?? null,
      guidIdentifierValue: this.keycloakUserId ?? '',
      approvedById: this.approvedById ? this.approvedById : 0,
      position: this.position ?? null,
      userTypeCode: this.userTypeCode ?? null,
      note: this.note ?? null,
      isDisabled: this.isDisabled ?? false,
      issueDate: isValidIsoDateTime(this.issueDate) ? this.issueDate : null,
      lastLogin: isValidIsoDateTime(this.lastLogin) ? this.lastLogin : null,
      userRoles:
        this.roles?.map<ApiGen_Concepts_UserRole>(role => ({
          userId: this.id ?? 0,
          roleId: role?.id ?? 0,
          id: 0,
          role: null,
          user: null,
          ...getEmptyBaseAudit(),
        })) ?? [],
      userRegions:
        this.regions?.map<ApiGen_Concepts_RegionUser>(region => ({
          userId: this.id ?? 0,
          regionCode: region.id ?? 0,
          region: toTypeCodeNullable(region.id),
          id: 0,
          user: null,
          ...getEmptyBaseAudit(),
        })) ?? [],
      person: {
        ...this.person,
        firstName: this.firstName ?? '',
        surname: this.surname ?? '',
        contactMethods: [
          {
            contactMethodType: toTypeCodeNullable(ContactMethodTypes.WorkEmail),
            value: this.email ?? null,
            id: 0,
            rowVersion: null,
          },
        ],
        comment: null,
        id: 0,
        isDisabled: false,
        middleNames: null,
        personAddresses: null,
        personOrganizations: null,
        preferredName: null,
        rowVersion: null,
      },
      ...getEmptyBaseAudit(this.rowVersion),
      appCreateTimestamp: this.appCreateTimestamp ?? EpochIsoDateTime,
    };
  }
}

export const userTypeCodeValues = [
  {
    radioValue: UserTypes.MinistryStaff,
    radioLabel: 'Ministry staff',
  },
  {
    radioValue: UserTypes.Contractor,
    radioLabel: 'Contractor',
  },
];
