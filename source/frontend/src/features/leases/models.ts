import isNumber from 'lodash/isNumber';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes } from '@/constants/index';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { exists, isValidId, isValidIsoDateTime } from '@/utils';
import {
  emptyStringToNull,
  fromTypeCode,
  stringToNull,
  stringToNumberOrNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';

import { PropertyForm } from '../mapSideBar/shared/models';
import { ChecklistItemFormModel } from '../mapSideBar/shared/tabs/checklist/update/models';
import { FormLeaseDeposit } from './detail/LeasePages/deposits/models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './detail/LeasePages/deposits/models/FormLeaseDepositReturn';
import { FormLeasePeriod } from './detail/LeasePages/payment/models';
import { FormStakeholder } from './detail/LeasePages/stakeholders/models';
import { LeasePurposeModel } from './models/LeasePurposeModel';

export class FormLeaseRenewal {
  id = 0;
  leaseId = 0;
  commencementDt: UtcIsoDateTime = '';
  expiryDt: UtcIsoDateTime = '';
  isExercised = false;
  renewalNote = '';
  rowVersion = 0;

  static fromApi(apiModel?: ApiGen_Concepts_LeaseRenewal): FormLeaseRenewal {
    const renewal = new FormLeaseRenewal();

    renewal.id = apiModel.id;
    renewal.leaseId = apiModel.leaseId;
    renewal.commencementDt = apiModel.commencementDt || '';
    renewal.expiryDt = apiModel.expiryDt || '';
    renewal.isExercised = apiModel.isExercised || false;
    renewal.renewalNote = apiModel.renewalNote || '';
    renewal.rowVersion = apiModel.rowVersion || 0;

    return renewal;
  }

  toApi(): ApiGen_Concepts_LeaseRenewal {
    return {
      id: this.id,
      leaseId: this.leaseId,
      lease: null,
      commencementDt: emptyStringToNull(this.commencementDt),
      expiryDt: emptyStringToNull(this.expiryDt),
      isExercised: this.isExercised,
      renewalNote: emptyStringToNull(this.renewalNote),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}

export class LeaseFormModel {
  id?: number;
  lFileNo = '';
  psFileNo = '';
  tfaFileNumber = '';
  expiryDate = '';
  startDate = '';
  renewals: FormLeaseRenewal[] = [];
  terminationDate = '';
  responsibilityEffectiveDate = '';
  paymentReceivableTypeCode = '';
  purposes: LeasePurposeModel[] = [];
  purposeOtherDescription: string | null = '';
  responsibilityTypeCode = '';
  initiatorTypeCode = '';
  leaseTypeCode = '';
  statusTypeCode = '';
  regionId = '';
  programTypeCode = '';
  otherLeaseTypeDescription = '';
  otherProgramTypeDescription = '';
  otherCategoryTypeDescription = '';
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
  productId: number | null;
  tenantNotes: string[] = [];
  properties: FormLeaseProperty[] = [];
  securityDeposits: FormLeaseDeposit[] = [];
  securityDepositReturns: FormLeaseDepositReturn[] = [];
  periods: FormLeasePeriod[] = [];
  stakeholders: FormStakeholder[] = [];
  fileChecklist: ChecklistItemFormModel[] = [];
  primaryArbitrationCity: string | null;
  isPublicBenefit: boolean;
  isFinancialGain: boolean;
  feeDeterminationNote: string | null = null;
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
    leaseDetail.purposes = apiModel.leasePurposes?.map(x => LeasePurposeModel.fromApi(x)) ?? [];
    const otherPurpose =
      apiModel.leasePurposes?.find(
        x => x.leasePurposeTypeCode.id === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
      ) ?? null;
    leaseDetail.purposeOtherDescription = otherPurpose ? otherPurpose.purposeOtherDescription : '';

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
    leaseDetail.rowVersion = apiModel?.rowVersion || null;
    leaseDetail.description = apiModel?.description || '';
    leaseDetail.otherProgramTypeDescription = apiModel?.otherProgramType || '';
    leaseDetail.otherLeaseTypeDescription = apiModel?.otherType || '';
    leaseDetail.project = apiModel?.project
      ? { id: apiModel?.project?.id || 0, text: apiModel?.project?.description || '' }
      : undefined;

    leaseDetail.productId = apiModel.productId;
    leaseDetail.periods = apiModel?.periods?.map(t => FormLeasePeriod.fromApi(t)) || [];
    leaseDetail.stakeholders = apiModel?.stakeholders?.map(t => new FormStakeholder(t)) || [];
    leaseDetail.renewals = apiModel?.renewals?.map(r => FormLeaseRenewal.fromApi(r)) || [];
    leaseDetail.cancellationReason = apiModel.cancellationReason || '';
    leaseDetail.terminationReason = apiModel.terminationReason || '';
    leaseDetail.primaryArbitrationCity = apiModel.primaryArbitrationCity;
    leaseDetail.isPublicBenefit = apiModel.isPublicBenefit;
    leaseDetail.isFinancialGain = apiModel.isFinancialGain;
    leaseDetail.feeDeterminationNote = apiModel.feeDeterminationNote;

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
      leasePurposes: formLease.purposes.map(x => x.toApi(formLease.id ?? 0)),
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
      fileProperties: formLease.properties
        ?.map(p => FormLeaseProperty.toApi(p))
        .filter(x => exists(x)),
      isResidential: formLease.isResidential,
      isCommercialBuilding: formLease.isCommercialBuilding,
      isOtherImprovement: formLease.isOtherImprovement,
      description: stringToNull(formLease.description),
      otherProgramType: stringToNull(formLease.otherProgramTypeDescription),
      otherType: stringToNull(formLease.otherLeaseTypeDescription),
      project: isValidId(formLease.project?.id) ? ({ id: formLease.project?.id } as any) : null,
      productId: stringToNumberOrNull(formLease.productId),
      product: null,
      consultations: null,
      stakeholders: formLease.stakeholders.map(t => FormStakeholder.toApi(t)),
      periods: formLease.periods.map(t => FormLeasePeriod.toApi(t)),
      renewals: formLease.renewals.map(r => r.toApi()),
      fileName: null,
      fileNumber: null,
      hasDigitalFile: formLease.hasDigitalLicense ?? false,
      hasPhysicalFile: formLease.hasPhysicalLicense ?? false,
      cancellationReason: stringToNull(formLease.cancellationReason),
      terminationReason: stringToNull(formLease.terminationReason),
      primaryArbitrationCity: stringToNull(formLease.primaryArbitrationCity),
      isPublicBenefit: formLease.isPublicBenefit ?? null,
      isFinancialGain: formLease.isFinancialGain ?? null,
      feeDeterminationNote: stringToNull(formLease.feeDeterminationNote),
      fileChecklistItems: formLease.fileChecklist.map(ck => ck.toApi()),
      isExpired: false,
      programName: null,
      renewalCount: formLease.renewals.length,
      totalAllowableCompensation: null,
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

  public static fromFormLeaseProperty(baseModel?: Partial<FormLeaseProperty>): FormLeaseProperty {
    const model = Object.assign(new FormLeaseProperty(), baseModel);
    return model;
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

  public static toApi(formLeaseProperty: FormLeaseProperty): ApiGen_Concepts_PropertyLease | null {
    if (!exists(formLeaseProperty?.property)) {
      return null;
    }

    const apiFileProperty = formLeaseProperty?.property?.toFilePropertyApi(
      formLeaseProperty.leaseId,
    );

    return {
      ...apiFileProperty,
      id: formLeaseProperty.id ?? 0,
      file: null,
      propertyName: formLeaseProperty.name ?? null,
      leaseArea: isNumber(formLeaseProperty.landArea) ? formLeaseProperty.landArea : 0,
      areaUnitType: isNumber(formLeaseProperty.landArea)
        ? toTypeCodeNullable(formLeaseProperty.areaUnitTypeCode) ?? null
        : null,
      ...getEmptyBaseAudit(formLeaseProperty.rowVersion),
    };
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    fileProperties: [],
    stakeholders: [],
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
    leasePurposes: [],
    programType: null,
    type: null,
    initiatorType: null,
    responsibilityType: null,
    region: null,
    otherType: null,
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
    productId: null,
    product: null,
    fileName: null,
    fileNumber: null,
    fileChecklistItems: [],
    isPublicBenefit: null,
    isFinancialGain: null,
    feeDeterminationNote: null,
    renewals: [],
    primaryArbitrationCity: null,
    ...getEmptyBaseAudit(),
    totalAllowableCompensation: null,
  });
