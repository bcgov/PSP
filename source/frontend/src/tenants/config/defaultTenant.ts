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
  bcAssessment: {
    url: 'https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca',
    names: {
      LEGAL_DESCRIPTION: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_LEGAL_DESCRIPTS_SV',
      ADDRESS: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_ADDRESSES_SV',
      FOLIO_DESCRIPTION: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_DESCRIPTIONS_SV',
      CHARGE: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_LAND_CHARS_SV',
      VALUE: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_GNRL_PROP_VALUES_SV',
      SALE: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_SALES_SV',
    },
  },
  idlePromptTimeout: 15,
  idleTimeout: 15,
};

export default defaultTenant;
