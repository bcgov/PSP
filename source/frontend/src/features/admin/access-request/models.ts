import { ContactMethodTypes } from 'constants/contactMethodType';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { getPreferredContactMethodValue } from 'utils/contactMethodUtil';
import { stringToNull, toTypeCode } from 'utils/formUtils';

import { Api_AccessRequest } from './../../../models/api/AccessRequest';

export enum UserTypeCodesEnum {
  CONTRACT = 'CONTRACT',
  MINSTAFF = 'MINSTAFF',
}

export class FormAccessRequest {
  public id: NumberFieldValue;
  public userId: NumberFieldValue;
  public note: string;
  public roleName: string;
  public roleId: NumberFieldValue;
  public accessRequestStatusTypeCodeId: string;
  public accessRequestStatusTypeCodeName: string;
  public regionCodeId: NumberFieldValue;
  public regionName: string;
  public appCreateTimestamp?: string;
  public email: string;
  public firstName: string;
  public surname: string;
  public middleNames: string;
  public position: string;
  public userTypeCode: string;
  public businessIdentifierValue: string;
  public keycloakUserGuid: string;
  public rowVersion?: number;

  constructor(accessRequest: Api_AccessRequest) {
    this.id = accessRequest.id ?? '';
    this.userId = accessRequest.userId;
    this.roleId = accessRequest.roleId ?? '';
    this.roleName = accessRequest?.role?.name ?? '';
    this.note = accessRequest.note ?? '';
    this.accessRequestStatusTypeCodeId = accessRequest.accessRequestStatusTypeCode?.id ?? '';
    this.accessRequestStatusTypeCodeName = accessRequest.accessRequestStatusTypeCode?.id ?? '';
    this.regionCodeId = accessRequest.regionCode?.id ?? '';
    this.regionName = accessRequest.regionCode?.description ?? '';
    this.email =
      (accessRequest?.user?.person?.contactMethods &&
        getPreferredContactMethodValue(
          accessRequest?.user?.person?.contactMethods,
          ContactMethodTypes.WorkEmail,
          ContactMethodTypes.PersonalEmail,
        )) ??
      '';
    this.firstName = accessRequest?.user?.person?.firstName ?? '';
    this.surname = accessRequest?.user?.person?.surname ?? '';
    this.middleNames = accessRequest?.user?.person?.middleNames ?? '';
    this.businessIdentifierValue = accessRequest.user?.businessIdentifierValue ?? '';
    this.keycloakUserGuid = accessRequest?.user?.guidIdentifierValue ?? '';
    this.position = accessRequest?.user?.position ?? '';
    this.userTypeCode = accessRequest?.user?.userTypeCode ?? UserTypeCodesEnum.CONTRACT;
    this.rowVersion = accessRequest.rowVersion;
  }

  public toApi(): Api_AccessRequest {
    return {
      id: stringToNull(this.id),
      userId: stringToNull(this.userId),
      roleId: stringToNull(this.roleId),
      note: this.note,
      accessRequestStatusTypeCode: toTypeCode(this.accessRequestStatusTypeCodeId),
      regionCode: this.regionCodeId ? toTypeCode<number>(this.regionCodeId) : undefined,
      user: {
        guidIdentifierValue: this.keycloakUserGuid,
        position: this.position,
        userTypeCode: this.userTypeCode,
        userRoles: [],
        userRegions: [],
      },
      rowVersion: this.rowVersion,
    };
  }
}

export const userTypeCodeValues = [
  {
    radioValue: UserTypeCodesEnum.MINSTAFF,
    radioLabel: 'Ministry staff',
  },
  {
    radioValue: UserTypeCodesEnum.CONTRACT,
    radioLabel: 'Contractor',
  },
];
