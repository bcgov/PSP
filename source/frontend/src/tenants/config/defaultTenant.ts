import { ITenantConfig } from 'tenants/ITenantConfig';

/**
 * Default tenant configuration.
 */
export const defaultTenant: ITenantConfig = {
  id: 'DFLT',
  title: 'Default Tenant Name',
  shortName: 'PIMS',
  colour: '#003366',
  logo: {
    favicon: '',
    image: '',
    imageWithText: '',
  },
  login: {
    title: '',
    heading: '',
    body: '',
  },
  layers: [],
  propertiesUrl:
    'ogs-internal/ows?service=wfs&request=GetFeature&typeName=PIMS_PROPERTY_LOCATION_VW&outputformat=json&srsName=EPSG:4326&version=2.0.0&',
  parcelMapFullyAttributed: {
    url: 'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW/ows',
    name: 'pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW',
  },
};

export default defaultTenant;
