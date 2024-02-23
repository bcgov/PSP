import { TypeCodeUtils } from '@/interfaces/ITypeCode';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { numberFieldToRequiredNumber } from '@/utils/formUtils';

export interface IUpdateFormInsurance {
  insurances: FormInsurance[];
  visibleTypes: string[];
}

export class FormInsurance {
  public isShown!: boolean;
  public isNew!: boolean;
  public isInsuranceInPlaceRadio!: string;

  public id: number | null = null;
  public leaseId: NumberFieldValue = '';
  public insuranceType: ApiGen_Base_CodeType<string> | null = null;
  public otherInsuranceType?: string;
  public coverageDescription?: string;
  public coverageLimit?: NumberFieldValue;
  public expiryDate?: string;
  private rowVersion!: number;

  public static createEmpty(typeCode: ILookupCode, leaseId: number): FormInsurance {
    const model = new FormInsurance();
    model.leaseId = leaseId;
    model.insuranceType = TypeCodeUtils.createFromLookup(typeCode);
    model.coverageLimit = '';
    model.isInsuranceInPlaceRadio = 'no';
    model.isNew = true;
    model.isShown = false;
    return model;
  }

  public static createFromModel(baseModel: ApiGen_Concepts_Insurance): FormInsurance {
    const model = new FormInsurance();
    model.id = baseModel.id;
    model.leaseId = baseModel.leaseId;
    model.insuranceType = baseModel.insuranceType;
    model.otherInsuranceType = baseModel.otherInsuranceType ?? undefined;
    model.coverageDescription = baseModel.coverageDescription ?? undefined;
    model.coverageLimit = baseModel.coverageLimit || '';
    model.expiryDate = isValidIsoDateTime(baseModel.expiryDate) ? baseModel.expiryDate : undefined;
    model.isInsuranceInPlaceRadio = baseModel.isInsuranceInPlace === true ? 'yes' : 'no';
    model.isNew = false;
    model.isShown = true;
    model.rowVersion = baseModel.rowVersion ?? 0;
    return model;
  }

  public toInterfaceModel(): ApiGen_Concepts_Insurance {
    return {
      id: this.id ?? null,
      leaseId: numberFieldToRequiredNumber(this.leaseId),
      insuranceType: this.insuranceType,
      otherInsuranceType: this.otherInsuranceType ?? null,
      coverageDescription: this.coverageDescription ?? null,
      coverageLimit: this.coverageLimit === '' ? null : this.coverageLimit ?? null,
      expiryDate: !isValidIsoDateTime(this.expiryDate) ? null : this.expiryDate ?? null,
      isInsuranceInPlace: this.isInsuranceInPlaceRadio === 'yes' ? true : false,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  public isEqual(other: FormInsurance): boolean {
    return (
      this.isInsuranceInPlaceRadio === other.isInsuranceInPlaceRadio &&
      this.id === other.id &&
      this.insuranceType?.id === other.insuranceType?.id &&
      this.otherInsuranceType === other.otherInsuranceType &&
      this.coverageDescription === other.coverageDescription &&
      this.coverageLimit === other.coverageLimit &&
      this.expiryDate === other.expiryDate
    );
  }
}
