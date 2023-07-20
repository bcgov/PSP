import { IMapProperty } from '@/components/propertySelector/models';
import { PropertyAreaUnitTypes } from '@/constants/index';
import { IAutocompletePrediction } from '@/interfaces';
import { Api_Lease, Api_LeaseConsultation } from '@/models/api/Lease';
import { Api_Project } from '@/models/api/Project';
import { Api_PropertyLease } from '@/models/api/PropertyLease';
import { ILookupCode } from '@/store/slices/lookupCodes/interfaces/ILookupCode';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import {
  emptyStringtoNullable,
  fromTypeCode,
  stringToNull,
  stringToUndefined,
  toTypeCode,
} from '@/utils/formUtils';

import { PropertyForm } from '../mapSideBar/shared/models';
import { FormLeaseDeposit } from './detail/LeasePages/deposits/models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './detail/LeasePages/deposits/models/FormLeaseDepositReturn';
import { FormInsurance } from './detail/LeasePages/insurance/edit/models';
import { FormLeaseTerm } from './detail/LeasePages/payment/models';
import { FormTenant } from './detail/LeasePages/tenant/models';

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
  apiProject: Api_Project | null = null;
  tenantNotes: string[] = [];
  properties: FormLeaseProperty[] = [];
  consultations: FormLeaseConsultation[] = [];
  securityDeposits: FormLeaseDeposit[] = [];
  securityDepositReturns: FormLeaseDepositReturn[] = [];
  terms: FormLeaseTerm[] = [];
  tenants: FormTenant[] = [];
  insurances: FormInsurance[] = [];
  rowVersion: number = 0;

  static fromApi(apiModel?: Api_Lease): LeaseFormModel {
    const leaseDetail = new LeaseFormModel();

    leaseDetail.id = apiModel?.id ?? undefined;
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
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense ?? undefined;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense ?? undefined;
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
    leaseDetail.project = !!apiModel?.project
      ? { id: apiModel?.project?.id || 0, text: apiModel?.project?.description || '' }
      : undefined;
    leaseDetail.apiProject = apiModel?.project || null;

    const sortedConsultations = apiModel?.consultations?.sort(
      (a, b) => (a.consultationType?.displayOrder || 0) - (b.consultationType?.displayOrder || 0),
    );
    leaseDetail.consultations =
      sortedConsultations?.map(c => FormLeaseConsultation.fromApi(c)) || [];
    leaseDetail.securityDeposits =
      apiModel?.securityDeposits?.map(d => FormLeaseDeposit.fromApi(d)) || [];
    leaseDetail.terms = apiModel?.terms?.map(t => FormLeaseTerm.fromApi(t)) || [];
    leaseDetail.tenants = apiModel?.tenants?.map(t => new FormTenant(t)) || [];
    leaseDetail.insurances = apiModel?.insurances?.map(i => FormInsurance.createFromModel(i)) || [];

    return leaseDetail;
  }

  public static toApi(formLease: LeaseFormModel): Api_Lease {
    return {
      id: formLease.id,
      lFileNo: stringToNull(formLease.lFileNo),
      psFileNo: stringToNull(formLease.psFileNo),
      tfaFileNumber: stringToNull(formLease.tfaFileNumber),
      expiryDate: stringToNull(formLease.expiryDate),
      startDate: formLease.startDate,
      responsibilityEffectiveDate: stringToNull(formLease.responsibilityEffectiveDate),
      amount: parseFloat(formLease.amount?.toString() ?? '') || 0.0,
      paymentReceivableType: toTypeCode(formLease.paymentReceivableTypeCode) ?? null,
      categoryType: formLease.categoryTypeCode
        ? toTypeCode(formLease.categoryTypeCode) ?? null
        : null,
      purposeType: toTypeCode(formLease.purposeTypeCode) ?? null,
      responsibilityType: toTypeCode(formLease.responsibilityTypeCode) ?? null,
      initiatorType: toTypeCode(formLease.initiatorTypeCode) ?? null,
      statusType: toTypeCode(formLease.statusTypeCode) ?? null,
      type: toTypeCode(formLease.leaseTypeCode) ?? null,
      region: toTypeCode(Number(formLease.regionId)) ?? null,
      programType: toTypeCode(formLease.programTypeCode) ?? null,
      note: stringToNull(formLease.note),
      returnNotes: formLease.returnNotes,
      documentationReference: stringToNull(formLease.documentationReference),
      motiName: formLease.motiName,
      hasDigitalLicense: formLease.hasDigitalLicense,
      hasPhysicalLicense: formLease.hasPhysicalLicense,
      properties: formLease.properties?.map(p => FormLeaseProperty.toApi(p)) ?? [],
      isResidential: formLease.isResidential,
      isCommercialBuilding: formLease.isCommercialBuilding,
      isOtherImprovement: formLease.isOtherImprovement,
      description: stringToNull(formLease.description),
      rowVersion: formLease.rowVersion > 0 ? formLease.rowVersion : undefined,
      otherCategoryType: stringToNull(formLease.otherCategoryTypeDescription),
      otherProgramType: stringToNull(formLease.otherProgramTypeDescription),
      otherPurposeType: stringToNull(formLease.otherPurposeTypeDescription),
      otherType: stringToNull(formLease.otherLeaseTypeDescription),
      project:
        formLease.project?.id !== undefined && formLease.project?.id !== 0
          ? ({ id: formLease.project?.id } as any)
          : undefined,
      consultations: formLease.consultations.map(x => x.toApi()),
      tenants: formLease.tenants.map(t => FormTenant.toApi(t)),
      terms: formLease.terms.map(t => FormLeaseTerm.toApi(t)),
      insurances: formLease.insurances.map(i => i.toInterfaceModel()),
    };
  }

  public static getPropertiesAsForm(leaseForm: LeaseFormModel): PropertyForm[] {
    return leaseForm.properties
      .map(property => {
        return property.property;
      })
      .filter((item): item is PropertyForm => !!item);
  }
}

export class FormLeaseProperty {
  id?: number;
  property?: PropertyForm;
  leaseId: number | null;
  rowVersion?: number;
  name?: string;
  landArea: string;
  areaUnitTypeCode: string;

  private constructor(leaseId?: number | null) {
    this.leaseId = leaseId ?? null;
    this.landArea = '0';
    this.areaUnitTypeCode = PropertyAreaUnitTypes.Meter;
  }

  public static fromApi(apiPropertyLease: Api_PropertyLease): FormLeaseProperty {
    const model = new FormLeaseProperty(apiPropertyLease.lease?.id);
    model.property = PropertyForm.fromApi(apiPropertyLease);
    model.id = apiPropertyLease.id;
    model.rowVersion = apiPropertyLease.rowVersion;
    model.name = apiPropertyLease.propertyName ?? undefined;
    model.landArea = apiPropertyLease.leaseArea?.toString() || '0';
    model.areaUnitTypeCode = apiPropertyLease.areaUnitType?.id || PropertyAreaUnitTypes.Meter;
    return model;
  }

  public static fromMapProperty(mapProperty: IMapProperty): FormLeaseProperty {
    const model = new FormLeaseProperty();
    model.property = PropertyForm.fromMapProperty(mapProperty);
    return model;
  }

  public static toApi(formLeaseProperty: FormLeaseProperty): Api_PropertyLease {
    const numberLeaseArea: number | undefined = stringToUndefined(formLeaseProperty.landArea);
    return {
      id: formLeaseProperty.id,
      leaseId: formLeaseProperty.leaseId,
      lease: null,
      rowVersion: formLeaseProperty.rowVersion,
      property: formLeaseProperty.property?.toApi(),
      propertyName: formLeaseProperty.name ?? undefined,
      leaseArea: numberLeaseArea ?? null,
      areaUnitType:
        numberLeaseArea !== undefined
          ? toTypeCode(formLeaseProperty.areaUnitTypeCode) ?? null
          : null,
    };
  }
}

export class FormLeaseConsultation {
  public id: number = 0;
  public consultationType: string = '';
  public consultationTypeDescription: string = '';
  public consultationStatusType: string = '';
  public consultationStatusTypeDescription: string = '';
  public consultationTypeOtherDescription: string = '';
  public parentLeaseId: number = 0;
  public rowVersion: number | undefined = undefined;

  static fromApi(apiModel: Api_LeaseConsultation): FormLeaseConsultation {
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

  public toApi(): Api_LeaseConsultation {
    return {
      id: this.id,
      consultationType: toTypeCode(this.consultationType) || null,
      consultationStatusType: toTypeCode(this.consultationStatusType) || null,
      parentLeaseId: this.parentLeaseId,
      otherDescription: emptyStringtoNullable(this.consultationTypeOtherDescription),
      rowVersion: this.rowVersion,
    };
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    properties: [],
    tenants: [],
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
    terms: [],
    insurances: [],
  });
