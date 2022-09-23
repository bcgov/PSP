import { IInsurance } from 'interfaces';
import ITypeCode, { TypeCodeUtils } from 'interfaces/ITypeCode';
import { ILookupCode } from 'store/slices/lookupCodes';
import { NumberFieldValue } from 'typings/NumberFieldValue';

export interface IUpdateFormInsurance {
  insurances: FormInsurance[];
  visibleTypes: string[];
}

export class FormInsurance {
  public isShown!: boolean;
  public isNew!: boolean;
  public isInsuranceInPlaceRadio!: string;

  public id!: NumberFieldValue;
  public insuranceType!: ITypeCode<string>;
  public otherInsuranceType?: string;
  public coverageDescription?: string;
  public coverageLimit?: NumberFieldValue;
  public expiryDate?: string;
  private rowVersion!: number;

  private constructor() {}

  public static createEmpty(typeCode: ILookupCode): FormInsurance {
    let model = new FormInsurance();
    model.id = 0;
    model.insuranceType = TypeCodeUtils.createFromLookup(typeCode);
    model.coverageLimit = '';
    model.isInsuranceInPlaceRadio = 'no';
    model.isNew = true;
    model.isShown = false;
    return model;
  }

  public static createFromModel(baseModel: IInsurance): FormInsurance {
    let model = new FormInsurance();
    model.id = baseModel.id;
    model.insuranceType = baseModel.insuranceType;
    model.otherInsuranceType = baseModel.otherInsuranceType;
    model.coverageDescription = baseModel.coverageDescription;
    model.coverageLimit = baseModel.coverageLimit || '';
    model.expiryDate = baseModel.expiryDate;
    model.isInsuranceInPlaceRadio = baseModel.isInsuranceInPlace === true ? 'yes' : 'no';
    model.isNew = false;
    model.isShown = true;
    model.rowVersion = baseModel.rowVersion;
    return model;
  }

  public toInterfaceModel(): IInsurance {
    return {
      id: this.id as number,
      insuranceType: this.insuranceType,
      otherInsuranceType: this.otherInsuranceType,
      coverageDescription: this.coverageDescription,
      coverageLimit: this.coverageLimit === '' ? undefined : this.coverageLimit,
      expiryDate: this.expiryDate === '' ? undefined : this.expiryDate,
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
