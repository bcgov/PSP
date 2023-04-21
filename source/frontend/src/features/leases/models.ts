import { IMapProperty } from 'components/propertySelector/models';
import { PropertyAreaUnitTypes } from 'constants/index';
import { PropertyForm } from 'features/properties/map/shared/models';
import { IAutocompletePrediction } from 'interfaces';
import { Api_Lease, Api_LeaseConsultation } from 'models/api/Lease';
import { Api_PropertyLease } from 'models/api/PropertyLease';
import { ILookupCode } from 'store/slices/lookupCodes/interfaces/ILookupCode';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { fromTypeCode, stringToNull, toTypeCode } from 'utils/formUtils';

export class LeaseFormModel {
  id?: number;
  lFileNo: string = '';
  psFileNo: string = '';
  tfaFileNumber: string = '';
  expiryDate: string = '';
  renewalDate: string = '';
  startDate: string = '';
  responsibilityEffectiveDate: string = '';
  paymentReceivableTypeCode: string = '';
  categoryTypeCode: string = '';
  purposeTypeCode: string = '';
  responsibilityTypeCode: string = '';
  initiatorTypeCode: string = '';
  leaseTypeCode: string = '';
  statusTypeCode: string = '';
  regionId: string = '';
  programTypeCode: string = '';
  otherLeaseTypeDescription: string = '';
  otherProgramTypeDescription: string = '';
  otherCategoryTypeDescription: string = '';
  otherPurposeTypeDescription: string = '';
  note: string = '';
  programName: string = '';
  motiName: string = '';
  amount: NumberFieldValue = '';
  renewalCount: NumberFieldValue = '';
  description: string = '';
  isResidential: boolean = false;
  isCommercialBuilding: boolean = false;
  isOtherImprovement: boolean = false;
  returnNotes: string = ''; // security deposit notes (free form text)
  documentationReference: string = '';
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  project?: IAutocompletePrediction;
  tenantNotes: string[] = [];
  properties: FormLeaseProperty[] = [];
  consultations: FormLeaseConsultation[] = [];
  rowVersion: number = 0;

  static fromApi(apiModel?: Api_Lease): LeaseFormModel {
    const leaseDetail = new LeaseFormModel();

    leaseDetail.id = apiModel?.id;
    leaseDetail.lFileNo = apiModel?.lFileNo || '';
    leaseDetail.psFileNo = apiModel?.psFileNo || '';
    leaseDetail.tfaFileNumber = apiModel?.tfaFileNumber || '';
    leaseDetail.expiryDate = apiModel?.expiryDate || '';
    leaseDetail.startDate = apiModel?.startDate || '';
    leaseDetail.responsibilityEffectiveDate = apiModel?.responsibilityEffectiveDate || '';
    leaseDetail.amount = parseFloat(apiModel?.amount?.toString() ?? '') || 0.0;
    leaseDetail.paymentReceivableTypeCode = fromTypeCode(apiModel?.paymentReceivableType) || '';
    leaseDetail.categoryTypeCode = fromTypeCode(apiModel?.categoryType) || '';
    leaseDetail.purposeTypeCode = fromTypeCode(apiModel?.purposeType) || '';
    leaseDetail.responsibilityTypeCode = fromTypeCode(apiModel?.responsibilityType) || '';
    leaseDetail.initiatorTypeCode = fromTypeCode(apiModel?.initiatorType) || '';
    leaseDetail.statusTypeCode = fromTypeCode(apiModel?.statusType) || '';
    leaseDetail.leaseTypeCode = fromTypeCode(apiModel?.type) || '';
    leaseDetail.regionId = fromTypeCode(apiModel?.region)?.toString() || '';
    leaseDetail.programTypeCode = fromTypeCode(apiModel?.programType) || '';
    leaseDetail.note = apiModel?.note || '';
    leaseDetail.returnNotes = apiModel?.returnNotes || '';
    leaseDetail.documentationReference = apiModel?.documentationReference || '';
    leaseDetail.motiName = apiModel?.motiName || '';
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense;
    leaseDetail.properties = apiModel?.properties?.map(p => FormLeaseProperty.fromApi(p)) ?? [];
    leaseDetail.isResidential = apiModel?.isResidential || false;
    leaseDetail.isCommercialBuilding = apiModel?.isCommercialBuilding || false;
    leaseDetail.isOtherImprovement = apiModel?.isOtherImprovement || false;
    leaseDetail.rowVersion = apiModel?.rowVersion || 0;
    leaseDetail.description = apiModel?.description || '';
    leaseDetail.otherCategoryTypeDescription = apiModel?.otherCategoryType || '';
    leaseDetail.otherProgramTypeDescription = apiModel?.otherProgramType || '';
    leaseDetail.otherPurposeTypeDescription = apiModel?.otherPurposeType || '';
    leaseDetail.otherLeaseTypeDescription = apiModel?.otherType || '';
    leaseDetail.project =
      apiModel?.project !== undefined
        ? { id: apiModel.project.id || 0, text: apiModel.project.description || '' }
        : undefined;

    const sortedConsultations = apiModel?.consultations?.sort(
      (a, b) => (a.consultationType?.displayOrder || 0) - (b.consultationType?.displayOrder || 0),
    );
    leaseDetail.consultations =
      sortedConsultations?.map(c => FormLeaseConsultation.fromApi(c)) || [];

    return leaseDetail;
  }

  public toApi(): Api_Lease {
    return {
      id: this.id,
      lFileNo: stringToNull(this.lFileNo),
      psFileNo: stringToNull(this.psFileNo),
      tfaFileNumber: stringToNull(this.tfaFileNumber),
      expiryDate: stringToNull(this.expiryDate),
      startDate: this.startDate,
      responsibilityEffectiveDate: stringToNull(this.responsibilityEffectiveDate),
      amount: parseFloat(this.amount?.toString() ?? '') || 0.0,
      paymentReceivableType: toTypeCode(this.paymentReceivableTypeCode),
      categoryType: this.categoryTypeCode ? toTypeCode(this.categoryTypeCode) : undefined,
      purposeType: toTypeCode(this.purposeTypeCode),
      responsibilityType: toTypeCode(this.responsibilityTypeCode),
      initiatorType: toTypeCode(this.initiatorTypeCode),
      statusType: toTypeCode(this.statusTypeCode),
      type: toTypeCode(this.leaseTypeCode),
      region: toTypeCode(Number(this.regionId)),
      programType: toTypeCode(this.programTypeCode),
      note: stringToNull(this.note),
      returnNotes: this.returnNotes,
      documentationReference: stringToNull(this.documentationReference),
      motiName: this.motiName,
      hasDigitalLicense: this.hasDigitalLicense,
      hasPhysicalLicense: this.hasPhysicalLicense,
      properties: this.properties?.map(p => p.toApi()) ?? [],
      isResidential: this.isResidential,
      isCommercialBuilding: this.isCommercialBuilding,
      isOtherImprovement: this.isOtherImprovement,
      description: stringToNull(this.description),
      rowVersion: this.rowVersion > 0 ? this.rowVersion : undefined,
      otherCategoryType: stringToNull(this.otherCategoryTypeDescription),
      otherProgramType: stringToNull(this.otherProgramTypeDescription),
      otherPurposeType: stringToNull(this.otherPurposeTypeDescription),
      otherType: stringToNull(this.otherLeaseTypeDescription),
      project:
        this.project?.id !== undefined && this.project?.id !== 0
          ? ({ id: this.project?.id } as any)
          : undefined,
      consultations: this.consultations.map(x => x.toApi()),
    };
  }

  public getPropertiesAsForm(): PropertyForm[] {
    return this.properties
      .map(property => {
        return property.property;
      })
      .filter((item): item is PropertyForm => !!item);
  }
}

export class FormLeaseProperty {
  id?: number;
  property?: PropertyForm;
  leaseId?: number;
  rowVersion?: number;
  name?: string;
  landArea: string;
  areaUnitTypeCode: string;

  private constructor(leaseId?: number) {
    this.leaseId = leaseId;
    this.landArea = '0';
    this.areaUnitTypeCode = PropertyAreaUnitTypes.Meter;
  }

  static fromApi(apiPropertyLease: Api_PropertyLease): FormLeaseProperty {
    const model = new FormLeaseProperty(apiPropertyLease.lease?.id);
    model.property = PropertyForm.fromApi(apiPropertyLease ?? {});
    model.id = apiPropertyLease.id;
    model.rowVersion = apiPropertyLease.rowVersion;
    model.name = apiPropertyLease.propertyName;
    model.landArea = apiPropertyLease.leaseArea?.toString() || '0';
    model.areaUnitTypeCode = apiPropertyLease.areaUnitType?.id || PropertyAreaUnitTypes.Meter;
    model.property.displayOrder = apiPropertyLease.displayOrder;
    return model;
  }

  static fromMapProperty(mapProperty: IMapProperty): FormLeaseProperty {
    const model = new FormLeaseProperty();
    model.property = PropertyForm.fromMapProperty(mapProperty);
    return model;
  }

  public toApi(): Api_PropertyLease {
    const numberLeaseArea: number | undefined = stringToNull(this.landArea);
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      property: this.property?.toApi(),
      lease: { id: this.leaseId, consultations: null },
      propertyName: this.name,
      leaseArea: numberLeaseArea,
      areaUnitType: numberLeaseArea !== undefined ? toTypeCode(this.areaUnitTypeCode) : undefined,
    };
  }
}

export class FormLeaseConsultation {
  public id: number = 0;
  public consultationType: string = '';
  public consultationTypeDescription: string = '';
  public consultationStatusType: string = '';
  public consultationStatusTypeDescription: string = '';
  public parentLeaseId: number = 0;
  public rowVersion: number | undefined = undefined;

  static fromApi(apiModel: Api_LeaseConsultation): FormLeaseConsultation {
    const model = new FormLeaseConsultation();
    model.id = apiModel.id || 0;
    model.consultationType = fromTypeCode(apiModel.consultationType) || '';
    model.consultationTypeDescription = apiModel.consultationType?.description || '';
    model.consultationStatusType = fromTypeCode(apiModel.consultationStatusType) || '';
    model.consultationStatusTypeDescription = apiModel.consultationStatusType?.description || '';
    model.parentLeaseId = apiModel.parentLeaseId || 0;
    model.rowVersion = apiModel.rowVersion || 0;
    return model;
  }

  static fromApiLookup(parentLease: number, typeModel: ILookupCode): FormLeaseConsultation {
    const model = new FormLeaseConsultation();
    model.id = 0;
    model.consultationType = typeModel.id.toString() || '';
    model.consultationTypeDescription = typeModel?.name || '';
    model.consultationStatusType = 'UNKNOWN';
    model.consultationStatusTypeDescription = 'Unknown';
    model.parentLeaseId = parentLease;
    model.rowVersion = undefined;
    return model;
  }

  public toApi(): Api_LeaseConsultation {
    return {
      id: this.id,
      consultationType: toTypeCode(this.consultationType) || null,
      consultationStatusType: toTypeCode(this.consultationStatusType) || null,
      parentLeaseId: this.parentLeaseId,
      rowVersion: this.rowVersion,
    };
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    properties: [],
    startDate: '',
    expiryDate: '',
    lFileNo: '',
    tfaFileNumber: '',
    psFileNo: '',
    motiName: '',
    amount: 0,
    isResidential: false,
    isCommercialBuilding: false,
    isOtherImprovement: false,
    returnNotes: '',
    hasDigitalLicense: undefined,
    hasPhysicalLicense: undefined,
    statusType: { id: 'DRAFT' },
    paymentReceivableType: { id: 'RCVBL' },
    consultations: null,
  });
