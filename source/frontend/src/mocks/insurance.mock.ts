import { TypeCodeUtils } from '@/interfaces';
import { Api_Insurance } from '@/models/api/Insurance';
import { ILookupCode } from '@/store/slices/lookupCodes';

const mockInsuranceTypeHome: ILookupCode = {
  id: 'HOME',
  name: 'Home Insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 1,
};

export const getMockInsurance = (): Api_Insurance => ({
  id: 123459,
  leaseId: 1,
  insuranceType: TypeCodeUtils.createFromLookup(mockInsuranceTypeHome),
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
  rowVersion: 0,
});
