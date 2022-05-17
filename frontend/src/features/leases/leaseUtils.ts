import { LeaseInitiatorTypes } from 'constants/leaseInitiatorTypes';
import { FormTenant } from 'features/leases/detail/LeasePages/tenant/Tenant';
import { IAddFormLease, IFormLeaseTerm, ILease } from 'interfaces';
import { Api_LeaseTenant } from 'models/api/LeaseTenant';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';
import { formatNames } from 'utils/personUtils';

import { IFormLease } from './../../interfaces/ILease';
import { apiLeaseTermToFormLeaseTerm } from './../../interfaces/ILeaseTerm';

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
    tenants: formLease.tenants.map<Api_LeaseTenant>(tenant => ({
      id: tenant.leaseTenantId,
      leaseId: formLease.id ?? 0,
      organizationId: !tenant.personId ? tenant.organizationId : undefined,
      personId: tenant.personId,
      primaryContactId: tenant?.primaryContactId,
      note: tenant.note,
    })),
  } as ILease;
};

export const apiLeaseToFormLease = (lease?: ILease): IFormLease | undefined => {
  if (!lease) {
    return undefined;
  }
  const formLease: IFormLease = {
    ...lease,
    amount: lease.amount ?? '',
    tfaFileNo: lease.tfaFileNo ?? '',
    terms: lease.terms.map<IFormLeaseTerm>(term => apiLeaseTermToFormLeaseTerm(term)) ?? [],
    tenants: lease.tenants.map<FormTenant>(tenant => new FormTenant(tenant)) ?? [],
  };
  return formLease;
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
