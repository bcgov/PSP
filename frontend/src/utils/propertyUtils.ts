import { EvaluationKeys, FiscalKeys, PropertyTypeNames, PropertyTypes } from 'constants/index';
import { IApiProperty, IFlatProperty } from 'features/properties/list';
import { IEvaluation, IFiscal } from 'interfaces';
import {
  formatDate,
  getCurrentFiscal,
  getCurrentFiscalYear,
  getCurrentYearEvaluation,
} from 'utils';

const isParcelOrSubdivision = (property: IFlatProperty) =>
  [PropertyTypes.Parcel, PropertyTypes.Subdivision].includes(property?.propertyTypeId);

export const toFlatProperty = (apiProperty: IApiProperty): IFlatProperty => {
  const assessedLand = isParcelOrSubdivision(apiProperty as any)
    ? getCurrentYearEvaluation(apiProperty.evaluations, EvaluationKeys.Assessed)
    : undefined;
  const assessedBuilding = getCurrentYearEvaluation(
    apiProperty.evaluations,
    isParcelOrSubdivision(apiProperty as any)
      ? EvaluationKeys.Improvements
      : EvaluationKeys.Assessed,
  );
  const netBook = getCurrentFiscal(apiProperty.fiscals, FiscalKeys.NetBook);
  const market = getCurrentFiscal(apiProperty.fiscals, FiscalKeys.Market);
  return {
    id: apiProperty.id,
    projectNumbers: apiProperty.projectNumbers,
    parcelId: apiProperty.parcelId ?? apiProperty.id,
    pid: apiProperty.pid ?? '',
    name: apiProperty.name,
    description: apiProperty.description,
    landLegalDescription: apiProperty.landLegalDescription,
    zoning: apiProperty.zoning,
    zoningPotential: apiProperty.zoningPotential,
    isSensitive: apiProperty.isSensitive,
    latitude: apiProperty.latitude,
    longitude: apiProperty.longitude,
    organizationId: apiProperty.organizationId,
    organization: apiProperty.organization ?? '',
    organizationCode: apiProperty.organization ?? '',
    subOrganization: apiProperty.subOrganization,
    classification: apiProperty.classification ?? '',
    classificationId: apiProperty.classificationId,
    addressId: apiProperty.address?.id as number,
    address: `${apiProperty.address?.line1 ?? ''} , ${apiProperty.address?.administrativeArea ??
      ''}`,
    administrativeArea: apiProperty.address?.administrativeArea ?? '',
    province: apiProperty.address?.province ?? '',
    postal: apiProperty.address?.postal ?? '',
    assessedLand: (assessedLand?.value as number) ?? '',
    assessedLandDate: assessedLand?.date,
    assessedLandFirm: assessedLand?.firm,
    assessedLandRowVersion: assessedLand?.rowVersion,
    assessedBuilding: (assessedBuilding?.value as number) ?? '',
    assessedBuildingDate: assessedBuilding?.date,
    assessedBuildingFirm: assessedBuilding?.firm,
    assessedBuildingRowVersion: assessedBuilding?.rowVersion,
    netBook: (netBook?.value as number) ?? '',
    netBookFiscalYear: netBook?.fiscalYear as number,
    netBookRowVersion: netBook?.rowVersion,
    market: (market?.value as number) ?? '',
    marketFiscalYear: market?.fiscalYear as number,
    marketRowVersion: market?.rowVersion,
    propertyTypeId: isParcelOrSubdivision(apiProperty as any) ? 0 : 1,
    propertyType: isParcelOrSubdivision(apiProperty as any)
      ? PropertyTypeNames.Land
      : PropertyTypeNames.Building,
    landArea: apiProperty.landArea,
    parcels: apiProperty.parcels ?? [],
  };
};

export const toApiProperty = (
  property: IFlatProperty,
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
    organization: property.organization,
    subOrganization: property.subOrganization,
    organizationId: property.organizationId,
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

/** create api evaluation objects based on flat app evaluation structure */
const getApiEvaluations = (property: IFlatProperty): IEvaluation[] => {
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
const getApiFiscals = (property: IFlatProperty, useCurrentFiscal: boolean): IFiscal[] => {
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

/**
 * The pidFormatter is used to format the specified PID value
 * @param {string} pid This is the target PID to be formatted
 */
export const pidFormatter = (pid: string) => {
  pid = pid.padStart(9, '0');
  const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
  const format = pid.match(regex);
  if (format !== null && format.length === 4) {
    pid = `${format[1]}-${format[2]}-${format[3]}`;
  }
  return pid;
};
