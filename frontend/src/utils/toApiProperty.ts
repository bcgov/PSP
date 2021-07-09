import { EvaluationKeys } from 'constants/evaluationKeys';
import { FiscalKeys } from 'constants/fiscalKeys';
import { PropertyTypes } from 'constants/propertyTypes';
import { IApiProperty, IEvaluation, IFiscal, IParentParcel } from 'interfaces';
import { formatDate, getCurrentFiscalYear } from 'utils';

export const toApiProperty = (
  property: IProperty,
  useCurrentFiscal: boolean = false,
): IApiProperty => {
  const apiProperty: IApiProperty = {
    id: property.id,
    propertyTypeId: property.propertyTypeId,
    parcelId: isParcelOrSubdivision(property) ? property.id : undefined,
    buildingId: property.propertyTypeId === PropertyTypes.Building ? property.id : undefined,
    pid: property.pid,
    pin: Number(property.pin),
    projectNumbers: property.projectNumbers ?? [],
    latitude: property.latitude,
    longitude: property.longitude,
    classificationId: property.classificationId,
    name: property.name,
    description: property.description,
    address: {
      id: property.addressId,
      line1: property.address,
      administrativeArea: property.administrativeArea,
      postal: property.postal,
      provinceId: property.province,
    },
    zoning: property.zoning ?? '',
    zoningPotential: property.zoningPotential ?? '',
    agency: property.agency,
    subAgency: property.subAgency,
    agencyId: property.agencyId,
    isSensitive: property.isSensitive,
    landArea: property.landArea,
    landLegalDescription: property.landLegalDescription,
    buildings: [], //parcel buildings should not be relevant to this api.
    evaluations: getApiEvaluations(property),
    fiscals: getApiFiscals(property, useCurrentFiscal),
    rowVersion: property.rowVersion,
  };
  return apiProperty;
};

const isParcelOrSubdivision = (property: IProperty) =>
  [PropertyTypes.Parcel, PropertyTypes.Subdivision].includes(property?.propertyTypeId);

const getApiEvaluations = (property: IProperty): IEvaluation[] => {
  const evaluations: IEvaluation[] = [];
  if (isParcelOrSubdivision(property)) {
    if (property.assessedLand !== '' && property.assessedLand !== undefined) {
      evaluations.push({
        parcelId: property.id,
        value: property.assessedLand,
        date: property.assessedLandDate ?? formatDate(new Date()),
        rowVersion: property.assessedLandRowVersion,
        key: EvaluationKeys.Assessed,
        firm: property.assessedLandFirm ?? '',
      });
    }
    if (property.assessedBuilding !== '' && property.assessedBuilding !== undefined) {
      evaluations.push({
        parcelId: property.id,
        value: property.assessedBuilding,
        date: property.assessedBuildingDate ?? formatDate(new Date()),
        rowVersion: property.assessedBuildingRowVersion,
        key: EvaluationKeys.Improvements,
        firm: property.assessedBuildingFirm ?? '',
      });
    }
  } else {
    if (property.assessedBuilding !== '' && property.assessedBuilding !== undefined) {
      evaluations.push({
        buildingId: property.id,
        value: property.assessedBuilding,
        date: property.assessedBuildingDate ?? formatDate(new Date()),
        rowVersion: property.assessedBuildingRowVersion,
        key: EvaluationKeys.Assessed,
        firm: property.assessedBuildingFirm ?? '',
      });
    }
  }

  return evaluations;
};

/** create api fiscal objects based on flat app fiscal structure */
const getApiFiscals = (property: IProperty, useCurrentFiscal: boolean): IFiscal[] => {
  const fiscals: IFiscal[] = [];
  if (property.netBook !== '' && property.netBook !== undefined) {
    fiscals.push({
      parcelId: isParcelOrSubdivision(property) ? property.id : undefined,
      buildingId: property.propertyTypeId === PropertyTypes.Building ? property.id : undefined,
      value: property.netBook,
      fiscalYear: !useCurrentFiscal
        ? property.netBookFiscalYear ?? getCurrentFiscalYear()
        : getCurrentFiscalYear(),
      rowVersion: property.netBookRowVersion,
      key: FiscalKeys.NetBook,
    });
  }
  if (property.market !== '' && property.market !== undefined) {
    fiscals.push({
      parcelId: isParcelOrSubdivision(property) ? property.id : undefined,
      buildingId: property.propertyTypeId === PropertyTypes.Building ? property.id : undefined,
      value: property.market,
      fiscalYear: !useCurrentFiscal
        ? property.marketFiscalYear ?? getCurrentFiscalYear()
        : getCurrentFiscalYear(),
      rowVersion: property.marketRowVersion,
      key: FiscalKeys.Market,
    });
  }

  return fiscals;
};

export interface IProperty {
  id: number;
  projectPropertyId?: number;
  propertyTypeId: number;
  propertyType: string;
  pid: string;
  pin?: number;
  classificationId: number;
  classification: string;
  name: string;
  description: string;
  projectNumbers?: string[];
  latitude: number;
  longitude: number;
  isSensitive: boolean;
  agencyId: number;
  agency: string;
  agencyCode: string;
  subAgency?: string;
  subAgencyCode?: string;

  addressId: number;
  address: string;
  administrativeArea: string;
  province: string;
  postal: string;

  // Financial Values
  market: number | '';
  marketFiscalYear?: number;
  marketRowVersion?: number;
  netBook: number | '';
  netBookFiscalYear?: number;
  netBookRowVersion?: number;

  assessedLand?: number | '';
  assessedLandDate?: Date | string;
  assessedLandFirm?: string;
  assessedLandRowVersion?: number;
  assessedBuilding?: number | '';
  assessedBuildingDate?: Date | string;
  assessedBuildingFirm?: string;
  assessedBuildingRowVersion?: number;

  // Parcel Properties
  landArea: number;
  landLegalDescription: string;
  zoning?: string;
  zoningPotential?: string;
  parcels?: IParentParcel[];

  // Building Properties
  parcelId?: number;
  constructionTypeId?: number;
  constructionType?: string;
  predominateUseId?: number;
  predominateUse?: string;
  occupantTypeId?: number;
  occupantType?: string;
  floorCount?: number;
  tenancy?: string;
  occupantName?: string;
  leaseExpiry?: Date | string;
  transferLeaseOnSale?: boolean;
  rentableArea?: number;
  rowVersion?: number;
}
