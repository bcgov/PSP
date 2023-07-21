import { ContactMethodTypes } from '@/constants/contactMethodType';
import { UserTypes } from '@/constants/userTypes';
import { Api_Person } from '@/models/api/Person';
import { Api_Role } from '@/models/api/Role';
import Api_TypeCode from '@/models/api/TypeCode';
import { Api_User } from '@/models/api/User';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { getPreferredContactMethodValue } from '@/utils/contactMethodUtil';

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
  userTypeCode?: Api_TypeCode<string>;
  lastLogin?: string;
  appCreateTimestamp?: string;
  issueDate?: string;
  rowVersion?: NumberFieldValue;
  note?: string;
  person?: Api_Person;
  regions?: Api_TypeCode<number>[];
  roles?: (Api_Role | undefined)[];

  constructor(user: Api_User) {
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
    this.businessIdentifierValue = user.businessIdentifierValue;
    this.firstName = user?.person?.firstName ?? '';
    this.surname = user?.person?.surname ?? '';
    this.isDisabled = user.isDisabled;
    this.position = user.position ?? '';
    this.userTypeCode = {
      id: user?.userTypeCode?.id ?? UserTypes.Contractor,
      description: user?.userTypeCode?.description ?? '',
    };
    this.lastLogin = user.lastLogin;
    this.appCreateTimestamp = user.appCreateTimestamp;
    this.note = user.note;
    this.roles = user?.userRoles?.map(userRole => userRole?.role) ?? [];
    this.regions = user?.userRegions?.map(userRegion => userRegion?.region) ?? [];
    this.person = user.person;
    this.rowVersion = user.rowVersion;
  }

  public toApi(): Api_User {
    return {
      id: this.id,
      businessIdentifierValue: this.businessIdentifierValue,
      guidIdentifierValue: this.keycloakUserId,
      approvedById: this.approvedById ? this.approvedById : undefined,
      position: this.position,
      userTypeCode: this.userTypeCode,
      note: this.note,
      isDisabled: this.isDisabled,
      issueDate: this.issueDate,
      lastLogin: this.lastLogin,
      appCreateTimestamp: this.appCreateTimestamp,
      userRoles:
        this.roles?.map(role => ({
          userId: this.id,
          roleId: role?.id,
        })) ?? [],
      userRegions:
        this.regions?.map(region => ({
          userId: this.id,
          regionCode: region.id,
          region: { id: region.id },
        })) ?? [],
      person: {
        ...this.person,
        firstName: this.firstName,
        surname: this.surname,
        contactMethods: [
          { contactMethodType: { id: ContactMethodTypes.WorkEmail }, value: this.email },
        ],
      },
      rowVersion: this.rowVersion ? this.rowVersion : undefined,
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
