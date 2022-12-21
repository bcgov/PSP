import { FormTenant } from 'features/leases/detail/LeasePages/tenant/Tenant';
import { IFormLeaseTerm, ILease } from 'interfaces';
import { Api_LeaseTenant } from 'models/api/LeaseTenant';
import {
  nullableBooleanToString,
  stringToNull,
  toTypeCode,
  yesNoUnknownToBoolean,
} from 'utils/formUtils';
import { formatNames } from 'utils/personUtils';

import { IFormLease } from './../../interfaces/ILease';
import {
  apiLeaseTermToFormLeaseTerm,
  formLeaseTermToApiLeaseTerm,
  ILeaseTerm,
} from './../../interfaces/ILeaseTerm';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (lease?: ILease): string[] => {
  const allNames =
    lease?.persons
      ?.map<string>(p => formatNames([p.firstName, p.middleNames, p.surname]))
      .concat(lease?.organizations?.map(p => p.name)) ?? [];
  return allNames;
};

export const formLeaseToApiLease = (formLease: IFormLease): ILease => {
  return {
    ...formLease,
    renewalCount: parseInt(formLease.renewalCount.toString()) || 0,
    tfaFileNumber: stringToNull(formLease?.tfaFileNumber?.toString() || '') || 0,
    amount: parseFloat(formLease.amount.toString()) || 0.0,
    expiryDate: stringToNull(formLease.expiryDate),
    renewalDate: stringToNull(formLease.renewalDate),
    responsibilityEffectiveDate: stringToNull(formLease.responsibilityEffectiveDate),
    terms: formLease.terms.map<ILeaseTerm>(term => formLeaseTermToApiLeaseTerm(term)),
    hasDigitalLicense: yesNoUnknownToBoolean(formLease.hasDigitalLicense),
    hasPhysicalLicense: yesNoUnknownToBoolean(formLease.hasPhysicalLicense),
    tenants: formLease.tenants.map<Api_LeaseTenant>(tenant => ({
      id: tenant.leaseTenantId,
      leaseId: formLease.id ?? 0,
      organizationId: !tenant.personId ? tenant.organizationId : undefined,
      personId: tenant.personId,
      primaryContactId: tenant?.primaryContactId,
      note: tenant.note,
      tenantTypeCode: toTypeCode(tenant.tenantType),
    })),
  };
};

export const apiLeaseToFormLease = (lease?: ILease): IFormLease | undefined => {
  if (!lease) {
    return undefined;
  }
  const formLease: IFormLease = {
    ...lease,
    amount: lease.amount ?? '',
    tfaFileNumber: lease.tfaFileNumber ?? '',
    hasDigitalLicense: nullableBooleanToString(lease.hasDigitalLicense),
    hasPhysicalLicense: nullableBooleanToString(lease.hasPhysicalLicense),
    terms: lease.terms.map<IFormLeaseTerm>(term => apiLeaseTermToFormLeaseTerm(term)) ?? [],
    tenants: lease.tenants.map<FormTenant>(tenant => new FormTenant(tenant)) ?? [],
  };
  return formLease;
};
