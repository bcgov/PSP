import isNumber from 'lodash/isNumber';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes } from '@/constants/index';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes/interfaces/ILookupCode';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { exists, isValidId, isValidIsoDateTime } from '@/utils';
import {
  emptyStringtoNullable,
  fromTypeCode,
  stringToNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';

import { PropertyForm } from '../mapSideBar/shared/models';
import { ChecklistItemFormModel } from '../mapSideBar/shared/tabs/checklist/update/models';
import { FormLeaseDeposit } from './detail/LeasePages/deposits/models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './detail/LeasePages/deposits/models/FormLeaseDepositReturn';
import { FormLeasePeriod } from './detail/LeasePages/payment/models';
import { FormTenant } from './detail/LeasePages/tenant/models';

export class LeaseFormModel {
  id?: number;
  lFileNo = '';
  psFileNo = '';
  tfaFileNumber = '';
  expiryDate = '';
  renewalDate = '';
  startDate = '';
  terminationDate = '';
  responsibilityEffectiveDate = '';
  paymentReceivableTypeCode = '';
  categoryTypeCode = '';
  purposeTypeCode = '';
  responsibilityTypeCode = '';
  initiatorTypeCode = '';
  leaseTypeCode = '';
  statusTypeCode = '';
  regionId = '';
  programTypeCode = '';
  otherLeaseTypeDescription = '';
  otherProgramTypeDescription = '';
  otherCategoryTypeDescription = '';
  otherPurposeTypeDescription = '';
  note = '';
  programName = '';
  motiName = '';
  amount: NumberFieldValue = '';
  renewalCount: NumberFieldValue = '';
  description = '';
  isResidential = false;
  isCommercialBuilding = false;
  isOtherImprovement = false;
  returnNotes = ''; // security deposit notes (free form text)
  documentationReference = '';
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  cancellationReason: string | null = null;
  terminationReason: string | null = null;
  project?: IAutocompletePrediction;
  tenantNotes: string[] = [];
  properties: FormLeaseProperty[] = [];
  consultations: FormLeaseConsultation[] = [];
  securityDeposits: FormLeaseDeposit[] = [];
  securityDepositReturns: FormLeaseDepositReturn[] = [];
  periods: FormLeasePeriod[] = [];
  tenants: FormTenant[] = [];
  fileChecklist: ChecklistItemFormModel[] = [];
  primaryArbitrationCity: string | null;
  IsPublicBenefit: boolean;
  IsFinancialGain: boolean;
  FeeDeterminationNote: string | null = null;
  rowVersion = 0;

  static fromApi(apiModel?: ApiGen_Concepts_Lease): LeaseFormModel {
    const leaseDetail = new LeaseFormModel();

    leaseDetail.id = apiModel?.id ?? undefined;
    leaseDetail.lFileNo = apiModel?.lFileNo || '';
    leaseDetail.psFileNo = apiModel?.psFileNo || '';
    leaseDetail.tfaFileNumber = apiModel?.tfaFileNumber || '';
    leaseDetail.expiryDate = isValidIsoDateTime(apiModel?.expiryDate) ? apiModel.expiryDate : '';
    leaseDetail.startDate = isValidIsoDateTime(apiModel?.startDate) ? apiModel.startDate : '';
    leaseDetail.terminationDate = isValidIsoDateTime(apiModel?.terminationDate)
      ? apiModel.terminationDate
      : '';
    leaseDetail.responsibilityEffectiveDate = apiModel?.responsibilityEffectiveDate || '';
    leaseDetail.amount = parseFloat(apiModel?.amount?.toString() ?? '') || 0.0;
    leaseDetail.paymentReceivableTypeCode = fromTypeCode(apiModel?.paymentReceivableType) || '';
    leaseDetail.categoryTypeCode = fromTypeCode(apiModel?.categoryType) || '';
    leaseDetail.purposeTypeCode = fromTypeCode(apiModel?.purposeType) || '';
    leaseDetail.responsibilityTypeCode = fromTypeCode(apiModel?.responsibilityType) || '';
    leaseDetail.initiatorTypeCode = fromTypeCode(apiModel?.initiatorType) || '';
    leaseDetail.statusTypeCode = fromTypeCode(apiModel?.fileStatusTypeCode) || '';
    leaseDetail.leaseTypeCode = fromTypeCode(apiModel?.type) || '';
    leaseDetail.regionId = fromTypeCode(apiModel?.region)?.toString() || '';
    leaseDetail.programTypeCode = fromTypeCode(apiModel?.programType) || '';
    leaseDetail.note = apiModel?.note || '';
    leaseDetail.returnNotes = apiModel?.returnNotes || '';
    leaseDetail.documentationReference = apiModel?.documentationReference || '';
    leaseDetail.motiName = apiModel?.motiName || '';
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense ?? undefined;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense ?? undefined;
    leaseDetail.properties = apiModel?.fileProperties?.map(p => FormLeaseProperty.fromApi(p)) ?? [];
    leaseDetail.isResidential = apiModel?.isResidential || false;
    leaseDetail.isCommercialBuilding = apiModel?.isCommercialBuilding || false;
    leaseDetail.isOtherImprovement = apiModel?.isOtherImprovement || false;
    leaseDetail.rowVersion = apiModel?.rowVersion || 0;
    leaseDetail.description = apiModel?.description || '';
    leaseDetail.otherCategoryTypeDescription = apiModel?.otherCategoryType || '';
    leaseDetail.otherProgramTypeDescription = apiModel?.otherProgramType || '';
    leaseDetail.otherPurposeTypeDescription = apiModel?.otherPurposeType || '';
    leaseDetail.otherLeaseTypeDescription = apiModel?.otherType || '';
    leaseDetail.project = apiModel?.project
      ? { id: apiModel?.project?.id || 0, text: apiModel?.project?.description || '' }
      : undefined;

    const sortedConsultations = apiModel?.consultations?.sort(
      (a, b) => (a.consultationType?.displayOrder || 0) - (b.consultationType?.displayOrder || 0),
    );
    leaseDetail.consultations =
      sortedConsultations?.map(c => FormLeaseConsultation.fromApi(c)) || [];
    leaseDetail.periods = apiModel?.periods?.map(t => FormLeasePeriod.fromApi(t)) || [];
    leaseDetail.tenants = apiModel?.tenants?.map(t => new FormTenant(t)) || [];
    leaseDetail.cancellationReason = apiModel.cancellationReason || '';
    leaseDetail.terminationReason = apiModel.terminationReason || '';
    leaseDetail.primaryArbitrationCity = apiModel.primaryArbitrationCity;
    leaseDetail.IsPublicBenefit = apiModel.isPublicBenefit;
    leaseDetail.IsFinancialGain = apiModel.isFinancialGain;
    leaseDetail.FeeDeterminationNote = apiModel.feeDeterminationNote;

    return leaseDetail;
  }

  public static toApi(formLease: LeaseFormModel): ApiGen_Concepts_Lease {
    return {
      id: formLease.id ?? 0,
      lFileNo: stringToNull(formLease.lFileNo),
      psFileNo: stringToNull(formLease.psFileNo),
      tfaFileNumber: stringToNull(formLease.tfaFileNumber),
      expiryDate: isValidIsoDateTime(formLease.expiryDate) ? formLease.expiryDate : null,
      startDate: isValidIsoDateTime(formLease.startDate) ? formLease.startDate : null,
      terminationDate: isValidIsoDateTime(formLease.terminationDate)
        ? formLease.terminationDate
        : null,
      responsibilityEffectiveDate: isValidIsoDateTime(formLease.responsibilityEffectiveDate)
        ? formLease.responsibilityEffectiveDate
        : null,
      amount: parseFloat(formLease.amount?.toString() ?? '') || 0.0,
      paymentReceivableType: toTypeCodeNullable(formLease.paymentReceivableTypeCode) ?? null,
      categoryType: formLease.categoryTypeCode
        ? toTypeCodeNullable(formLease.categoryTypeCode) ?? null
        : null,
      purposeType: toTypeCodeNullable(formLease.purposeTypeCode) ?? null,
      responsibilityType: toTypeCodeNullable(formLease.responsibilityTypeCode) ?? null,
      initiatorType: toTypeCodeNullable(formLease.initiatorTypeCode) ?? null,
      fileStatusTypeCode: toTypeCodeNullable(formLease.statusTypeCode) ?? null,
      type: toTypeCodeNullable(formLease.leaseTypeCode) ?? null,
      region: toTypeCodeNullable(Number(formLease.regionId)) ?? null,
      programType: toTypeCodeNullable(formLease.programTypeCode) ?? null,
      note: stringToNull(formLease.note),
      returnNotes: formLease.returnNotes,
      documentationReference: stringToNull(formLease.documentationReference),
      motiName: formLease.motiName,
      hasDigitalLicense: formLease.hasDigitalLicense ?? null,
      hasPhysicalLicense: formLease.hasPhysicalLicense ?? null,
      fileProperties: formLease.properties?.map(p => FormLeaseProperty.toApi(p)),
      isResidential: formLease.isResidential,
      isCommercialBuilding: formLease.isCommercialBuilding,
      isOtherImprovement: formLease.isOtherImprovement,
      description: stringToNull(formLease.description),
      otherCategoryType: stringToNull(formLease.otherCategoryTypeDescription),
      otherProgramType: stringToNull(formLease.otherProgramTypeDescription),
      otherPurposeType: stringToNull(formLease.otherPurposeTypeDescription),
      otherType: stringToNull(formLease.otherLeaseTypeDescription),
      project: isValidId(formLease.project?.id) ? ({ id: formLease.project?.id } as any) : null,
      consultations: formLease.consultations.map(x => x.toApi()),
      tenants: formLease.tenants.map(t => FormTenant.toApi(t)),
      periods: formLease.periods.map(t => FormLeasePeriod.toApi(t)),
      fileName: null,
      fileNumber: null,
      hasDigitalFile: formLease.hasDigitalLicense ?? false,
      hasPhysicalFile: formLease.hasPhysicalLicense ?? false,
      cancellationReason: stringToNull(formLease.cancellationReason),
      terminationReason: stringToNull(formLease.terminationReason),
      primaryArbitrationCity: stringToNull(formLease.primaryArbitrationCity),
      isPublicBenefit: formLease.IsPublicBenefit ?? null,
      isFinancialGain: formLease.IsFinancialGain ?? null,
      feeDeterminationNote: stringToNull(formLease.FeeDeterminationNote),
      fileChecklistItems: formLease.fileChecklist.map(ck => ck.toApi()),
      isExpired: false,
      programName: null,
      renewalCount: 0,
      ...getEmptyBaseAudit(formLease.rowVersion),
    };
  }

  public static getPropertiesAsForm(leaseForm: LeaseFormModel): PropertyForm[] {
    return leaseForm.properties
      .map(property => {
        return property.property;
      })
      .filter(exists);
  }
}

export class FormLeaseProperty {
  id?: number;
  property?: PropertyForm;
  leaseId: number | null;
  rowVersion?: number;
  name?: string;
  landArea: number;
  areaUnitTypeCode: string;

  private constructor(leaseId?: number | null) {
    this.leaseId = leaseId ?? null;
    this.landArea = 0;
    this.areaUnitTypeCode = AreaUnitTypes.SquareMeters;
  }

  public static fromApi(apiPropertyLease: ApiGen_Concepts_PropertyLease): FormLeaseProperty {
    const model = new FormLeaseProperty(apiPropertyLease.file?.id);
    model.property = PropertyForm.fromApi(apiPropertyLease);
    model.id = apiPropertyLease.id;
    model.rowVersion = apiPropertyLease.rowVersion ?? undefined;
    model.name = apiPropertyLease.propertyName ?? undefined;
    model.landArea = apiPropertyLease.leaseArea ?? 0;
    model.areaUnitTypeCode = apiPropertyLease.areaUnitType?.id || AreaUnitTypes.SquareMeters;
    return model;
  }

  public static fromMapProperty(mapProperty: IMapProperty): FormLeaseProperty {
    const model = new FormLeaseProperty();
    model.property = PropertyForm.fromMapProperty(mapProperty);
    return model;
  }

  public static fromFeatureDataset(mapProperty: LocationFeatureDataset): FormLeaseProperty {
    const model = new FormLeaseProperty();
    model.property = PropertyForm.fromFeatureDataset(mapProperty);
    return model;
  }

  public static toApi(formLeaseProperty: FormLeaseProperty): ApiGen_Concepts_PropertyLease {
    return {
      id: formLeaseProperty.id ?? 0,
      fileId: formLeaseProperty.leaseId ?? 0,
      file: null,
      property: formLeaseProperty.property?.toApi() ?? null,
      propertyId: formLeaseProperty.property?.id ?? 0,
      propertyName: formLeaseProperty.name ?? null,
      leaseArea: isNumber(formLeaseProperty.landArea) ? formLeaseProperty.landArea : 0,
      areaUnitType: isNumber(formLeaseProperty.landArea)
        ? toTypeCodeNullable(formLeaseProperty.areaUnitTypeCode) ?? null
        : null,
      displayOrder: null,
      location: null, // TODO: Add proper file location values when DB schema gets added
      ...getEmptyBaseAudit(formLeaseProperty.rowVersion),
    };
  }
}

export class FormLeaseConsultation {
  public id = 0;
  public consultationType = '';
  public consultationTypeDescription = '';
  public consultationStatusType = '';
  public consultationStatusTypeDescription = '';
  public consultationTypeOtherDescription = '';
  public parentLeaseId = 0;
  public rowVersion: number | undefined = undefined;

  static fromApi(apiModel: ApiGen_Concepts_ConsultationLease): FormLeaseConsultation {
    const model = new FormLeaseConsultation();
    model.id = apiModel.id || 0;
    model.consultationType = fromTypeCode(apiModel.consultationType) || '';
    model.consultationTypeDescription = apiModel.consultationType?.description || '';
    model.consultationStatusType = fromTypeCode(apiModel.consultationStatusType) || '';
    model.consultationStatusTypeDescription = apiModel.consultationStatusType?.description || '';
    model.consultationTypeOtherDescription = apiModel.otherDescription || '';
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

  public toApi(): ApiGen_Concepts_ConsultationLease {
    return {
      id: this.id,
      consultationType: toTypeCodeNullable(this.consultationType) || null,
      consultationStatusType: toTypeCodeNullable(this.consultationStatusType) || null,
      parentLeaseId: this.parentLeaseId,
      otherDescription: emptyStringtoNullable(this.consultationTypeOtherDescription),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    fileProperties: [],
    tenants: [],
    startDate: EpochIsoDateTime,
    expiryDate: EpochIsoDateTime,
    terminationDate: null,
    lFileNo: '',
    tfaFileNumber: '',
    psFileNo: '',
    motiName: '',
    amount: 0,
    isResidential: false,
    isCommercialBuilding: false,
    isOtherImprovement: false,
    returnNotes: '',
    hasDigitalLicense: null,
    hasPhysicalLicense: null,
    fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    paymentReceivableType: toTypeCodeNullable(ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    categoryType: null,
    purposeType: null,
    programType: null,
    type: null,
    initiatorType: null,
    responsibilityType: null,
    region: null,
    otherType: null,
    otherCategoryType: null,
    otherPurposeType: null,
    otherProgramType: null,
    consultations: [],
    periods: [],
    id: 0,
    programName: null,
    documentationReference: null,
    note: null,
    description: null,
    renewalCount: 0,
    responsibilityEffectiveDate: null,
    hasPhysicalFile: false,
    hasDigitalFile: false,
    cancellationReason: null,
    terminationReason: null,
    isExpired: false,
    project: null,
    primaryArbitrationCity: null,
    ...getEmptyBaseAudit(),
    fileName: null,
    fileNumber: null,
    fileChecklistItems: [],
    isPublicBenefit: null,
    isFinancialGain: null,
    feeDeterminationNote: null,
  });
