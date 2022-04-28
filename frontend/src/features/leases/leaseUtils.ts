import { ContactMethodTypes } from 'constants/contactMethodType';
import { LeaseInitiatorTypes } from 'constants/leaseInitiatorTypes';
import { IAddFormLease, ILease } from 'interfaces';
import { getContactMethodValue } from 'utils/contactMethodUtil';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';
import { formatNames } from 'utils/personUtils';

import { IFormLease } from './../../interfaces/ILease';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (lease?: ILease): string => {
  const allNames =
    lease?.persons
      ?.map<string>(p => formatNames([p.firstName, p.middleNames, p.surname]))
      .concat(lease?.organizations?.map(p => p.name)) ?? [];
  return allNames.join(', ');
};

export const formLeaseToApiLease = (formLease: IFormLease) => {
  return {
    ...formLease,
    renewalCount: parseInt(formLease.renewalCount.toString()) || 0,
    tfaFileNo: parseInt(formLease?.tfaFileNo?.toString() || '') || 0,
    amount: parseFloat(formLease.amount.toString()) || 0.0,
    expiryDate: stringToNull(formLease.expiryDate),
    renewalDate: stringToNull(formLease.renewalDate),
    responsibilityEffectiveDate: stringToNull(formLease.responsibilityEffectiveDate),
    tenants: formLease.tenants.map(tenant => ({
      ...tenant,
      leaseId: formLease.id,
      personId: tenant.id?.startsWith('P') ? tenant.personId : undefined,
      organizationId: tenant.id?.startsWith('O') ? tenant.organizationId : undefined,
    })),
  } as ILease;
};

export const apiLeaseToFormLease = (lease?: ILease) => {
  return !!lease
    ? ({
        ...lease,
        tenants: lease.tenants.map(tenant => {
          var addresses = tenant.person?.personAddresses ?? [];
          var firstAddress = addresses.length > 0 ? addresses[0].address : undefined;
          return {
            summary: !!tenant.person
              ? `${tenant.person?.firstName} ${
                  !!tenant.person?.middleNames ? tenant.person?.middleNames : ''
                } ${tenant.person?.surname}`
              : tenant.organization?.name,
            firstName: tenant.person?.firstName,
            surname: tenant.person?.surname,
            email:
              getContactMethodValue(tenant.person?.contactMethods, ContactMethodTypes.WorkEmail) ??
              tenant.organization?.email,
            mailingAddress:
              firstAddress?.streetAddress1 ?? tenant.organization?.address?.streetAddress1,
            municipalityName:
              firstAddress?.municipality ?? tenant.organization?.address?.municipality,
            provinceState:
              firstAddress?.province?.code ?? tenant.organization?.address?.provinceCode,
            note: tenant.note,
            personId: tenant.personId,
            organizationId: tenant.organizationId,
            leaseId: tenant.leaseId,
            rowVersion: tenant.rowVersion,
            leaseTenantId: tenant.leaseTenantId,
            id: !!tenant.personId ? `P${tenant.personId}` : `O${tenant.organizationId}`,
          };
        }),
      } as IFormLease) // TODO, Type coercion might be hiding type issues
    : undefined;
};

export const addFormLeaseToApiLease = (formLease: IAddFormLease) => {
  return {
    ...formLease,
    renewalCount: parseInt(formLease.renewalCount.toString()) || 0,
    tfaFileNo: parseInt(formLease?.tfaFileNo?.toString() || '') || 0,
    amount: parseFloat(formLease.amount.toString()) || 0.0,
    paymentReceivableType: stringToTypeCode(formLease.paymentReceivableType),
    categoryType: stringToTypeCode(formLease.categoryType),
    purposeType: stringToTypeCode(formLease.purposeType),
    responsibilityType: stringToTypeCode(formLease.responsibilityType),
    initiatorType: stringToTypeCode(formLease.initiatorType || LeaseInitiatorTypes.Hq),
    statusType: stringToTypeCode(formLease.statusType),
    type: stringToTypeCode(formLease.type),
    region: { regionCode: formLease.region },
    programType: stringToTypeCode(formLease.programType),
    expiryDate: stringToNull(formLease.expiryDate),
    psFileNo: stringToNull(formLease.psFileNo),
    renewalDate: stringToNull(formLease.renewalDate),
    responsibilityEffectiveDate: stringToNull(formLease.responsibilityEffectiveDate),
    properties: formLease.properties.map(formProperty => ({
      ...formProperty,
      pin: stringToNull(formProperty.pin),
      landArea: stringToNull(formProperty.landArea),
      areaUnitType: stringToTypeCode(formProperty.areaUnitType?.id),
    })),
  } as ILease;
};

export const apiLeaseToAddFormLease = (lease?: ILease) => {
  return !!lease
    ? ({
        ...lease,
        paymentReceivableType: lease.paymentReceivableType?.id ?? '',
        categoryType: lease.categoryType?.id ?? '',
        purposeType: lease.purposeType?.id ?? '',
        responsibilityType: lease.responsibilityType?.id ?? '',
        initiatorType: lease.initiatorType?.id ?? '',
        statusType: lease.statusType?.id ?? '',
        type: lease.type?.id ?? '',
        region: lease?.region?.regionCode ?? '',
        programType: lease.programType?.id ?? '',
      } as IAddFormLease)
    : undefined;
};
