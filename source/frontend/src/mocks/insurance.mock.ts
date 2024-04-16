import { TypeCodeUtils } from '@/interfaces/ITypeCode';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

const mockInsuranceTypeHome: ILookupCode = {
  id: 'HOME',
  name: 'Home Insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 1,
};

export const getMockInsurance = (): ApiGen_Concepts_Insurance => ({
  id: 123459,
  leaseId: 1,
  insuranceType: TypeCodeUtils.createFromLookup<string>(mockInsuranceTypeHome),
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
  ...getEmptyBaseAudit(0),
});
