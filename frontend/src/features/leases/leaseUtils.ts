import { LeaseInitiatorTypes } from 'constants/leaseInitiatorTypes';
import { IAddFormLease, ILease } from 'interfaces';
import { stringToNull, stringToTypeCode } from 'utils/formUtils';

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
