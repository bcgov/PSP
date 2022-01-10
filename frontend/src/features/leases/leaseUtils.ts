import { LeaseInitiatorTypes } from 'constants/leaseInitiatorTypes';
import { IAddFormLease, ILease } from 'interfaces';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';

import { IFormLease } from './../../interfaces/ILease';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (lease?: ILease) => {
  const allNames = (lease?.persons?.map(p => p.fullName) ?? []).concat(
    lease?.organizations?.map(p => p.name) ?? [],
  );
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
    })),
  } as ILease;
};

export const apiLeaseToFormLease = (lease?: ILease) => {
  return !!lease
    ? ({
        ...lease,
        tenants: lease.tenants.map(tenant => ({
          summary: !!tenant.person
            ? `${tenant.person?.firstName} ${tenant.person?.middleNames} ${tenant.person?.surname}`
            : tenant.organization?.name,
          firstName: tenant.person?.firstName,
          surname: tenant.person?.surname,
          email: tenant.person?.email ?? tenant.organization?.email,
          mailingAddress:
            tenant.person?.address?.streetAddress1 ?? tenant.organization?.address?.streetAddress1,
          municipalityName:
            tenant.person?.address?.municipality ?? tenant.organization?.address?.municipality,
          provinceState:
            tenant.person?.address?.provinceCode ?? tenant.organization?.address?.provinceCode,
          note: tenant.note,
          personId: tenant.personId,
          organizationId: tenant.organizationId,
          leaseId: tenant.leaseId,
          rowVersion: tenant.rowVersion,
          leaseTenantId: tenant.leaseTenantId,
          id: !!tenant.personId ? `P${tenant.personId}` : `O${tenant.organizationId}`,
        })),
      } as IFormLease)
    : undefined;
};

export const addFormLeaseToApiLease = (formLease: IAddFormLease) => {
  return {
    ...formLease,
    renewalCount: parseInt(formLease.renewalCount.toString()) || 0,
    tfaFileNo: parseInt(formLease?.tfaFileNo?.toString() || '') || 0,
    amount: parseFloat(formLease.amount.toString()) || 0.0,
    paymentFrequencyType: stringToTypeCode(formLease.paymentFrequencyType),
    paymentReceivableType: stringToTypeCode(formLease.paymentReceivableType),
    categoryType: stringToTypeCode(formLease.categoryType),
    purposeType: stringToTypeCode(formLease.purposeType),
    responsibilityType: stringToTypeCode(formLease.responsibilityType),
    initiatorType: stringToTypeCode(formLease.initiatorType || LeaseInitiatorTypes.Hq),
    statusType: stringToTypeCode(formLease.statusType),
    type: stringToTypeCode(formLease.type),
    programType: stringToTypeCode(formLease.programType),
    expiryDate: stringToNull(formLease.expiryDate),
    psFileNo: stringToNull(formLease.psFileNo),
    renewalDate: stringToNull(formLease.renewalDate),
    responsibilityEffectiveDate: stringToNull(formLease.responsibilityEffectiveDate),
    properties: formLease.properties.map(formProperty => ({
      ...formProperty,
      pin: stringToNull(formProperty.pin),
      areaUnit: stringToNull(formProperty.areaUnit),
      areaUnitType: stringToTypeCode(formProperty.areaUnitType),
    })),
  } as ILease;
};

export const apiLeaseToAddFormLease = (lease?: ILease) => {
  return !!lease
    ? ({
        ...lease,
        paymentFrequencyType: lease.paymentFrequencyType?.id ?? '',
        paymentReceivableType: lease.paymentReceivableType?.id ?? '',
        categoryType: lease.categoryType?.id ?? '',
        purposeType: lease.purposeType?.id ?? '',
        responsibilityType: lease.responsibilityType?.id ?? '',
        initiatorType: lease.initiatorType?.id ?? '',
        statusType: lease.statusType?.id ?? '',
        type: lease.type?.id ?? '',
        programType: lease.programType?.id ?? '',
      } as IAddFormLease)
    : undefined;
};
