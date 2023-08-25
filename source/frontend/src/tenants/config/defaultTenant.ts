import { ITenantConfig2 } from '@/hooks/pims-api/interfaces/ITenantConfig';

const MUNICIPALITY_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_MUNICIPALITIES_SP/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_MUNICIPALITIES_SP';
const PARCELS_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW/wfs?service=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW';
const ELECTORAL_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.EBC_ELECTORAL_DISTS_BS10_SVW/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.EBC_ELECTORAL_DISTS_BS10_SVW';
const REGIONAL_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_REGIONAL_DISTRICTS_SP/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_REGIONAL_DISTRICTS_SP';

const MOTI_REGION_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY';
const HWY_DISTRICT_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY';
const ALR_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_POLYS/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_POLYS';
const INDIAN_RESERVES_LAYER_URL =
  'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.ADM_INDIAN_RESERVES_BANDS_SP/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.ADM_INDIAN_RESERVES_BANDS_SP';
const PIMS_BOUNDARY_LAYER_URL =
  '/ogs-internal/ows?service=wfs&request=GetFeature&typeName=PIMS_PROPERTY_BOUNDARY_VW&outputformat=json&version=2.0.0';

/**
 * Default tenant configuration.
 */
export const defaultTenant: ITenantConfig2 = {
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
  electoralLayerUrl: ELECTORAL_LAYER_URL,
  municipalLayerUrl: MUNICIPALITY_LAYER_URL,
  parcelsLayerUrl: PARCELS_LAYER_URL,
  regionalLayerUrl: REGIONAL_LAYER_URL,
  motiRegionLayerUrl: MOTI_REGION_LAYER_URL,
  hwyDistrictLayerUrl: HWY_DISTRICT_LAYER_URL,
  alrLayerUrl: ALR_LAYER_URL,
  reservesLayerUrl: INDIAN_RESERVES_LAYER_URL,
  boundaryLayerUrl: PIMS_BOUNDARY_LAYER_URL,
  bcAssessment: {
    url: 'https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca',
    names: {
      LEGAL_DESCRIPTION: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_LEGAL_DESCRIPTS_SV',
      ADDRESSES: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_ADDRESSES_SV',
      FOLIO_DESCRIPTION: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_DESCRIPTIONS_SV',
      CHARGES: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_LAND_CHARS_SV',
      VALUES: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_GNRL_PROP_VALUES_SV',
      SALES: 'geo.bca:WHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_SALES_SV',
    },
  },
  idlePromptTimeout: 15,
  idleTimeout: 15,
};

export default defaultTenant;
