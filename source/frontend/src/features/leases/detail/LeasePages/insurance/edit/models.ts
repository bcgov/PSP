import { TypeCodeUtils } from '@/interfaces';
import ITypeCode from '@/interfaces/ITypeCode';
import { Api_Insurance } from '@/models/api/Insurance';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
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
  public insuranceType!: ITypeCode<string>;
  public otherInsuranceType?: string;
  public coverageDescription?: string;
  public coverageLimit?: NumberFieldValue;
  public expiryDate?: string;
  private rowVersion!: number;

  private constructor() {}

  public static createEmpty(typeCode: ILookupCode, leaseId: number): FormInsurance {
    let model = new FormInsurance();
    model.leaseId = leaseId;
    model.insuranceType = TypeCodeUtils.createFromLookup(typeCode);
    model.coverageLimit = '';
    model.isInsuranceInPlaceRadio = 'no';
    model.isNew = true;
    model.isShown = false;
    return model;
  }

  public static createFromModel(baseModel: Api_Insurance): FormInsurance {
    let model = new FormInsurance();
    model.id = baseModel.id;
    model.leaseId = baseModel.leaseId;
    model.insuranceType = baseModel.insuranceType;
    model.otherInsuranceType = baseModel.otherInsuranceType ?? undefined;
    model.coverageDescription = baseModel.coverageDescription ?? undefined;
    model.coverageLimit = baseModel.coverageLimit || '';
    model.expiryDate = baseModel.expiryDate ?? undefined;
    model.isInsuranceInPlaceRadio = baseModel.isInsuranceInPlace === true ? 'yes' : 'no';
    model.isNew = false;
    model.isShown = true;
    model.rowVersion = baseModel.rowVersion ?? 0;
    return model;
  }

  public toInterfaceModel(): Api_Insurance {
    return {
      id: this.id ?? null,
      leaseId: numberFieldToRequiredNumber(this.leaseId),
      insuranceType: this.insuranceType,
      otherInsuranceType: this.otherInsuranceType ?? null,
      coverageDescription: this.coverageDescription ?? null,
      coverageLimit: this.coverageLimit === '' ? null : this.coverageLimit ?? null,
      expiryDate: this.expiryDate === '' ? null : this.expiryDate ?? null,
      isInsuranceInPlace: this.isInsuranceInPlaceRadio === 'yes' ? true : false,
      rowVersion: this.rowVersion,
    };
  }

  public isEqual(other: FormInsurance): boolean {
    return (
      this.isInsuranceInPlaceRadio === other.isInsuranceInPlaceRadio &&
      this.id === other.id &&
      this.insuranceType.id === other.insuranceType.id &&
      this.otherInsuranceType === other.otherInsuranceType &&
      this.coverageDescription === other.coverageDescription &&
      this.coverageLimit === other.coverageLimit &&
      this.expiryDate === other.expiryDate
    );
  }
}
