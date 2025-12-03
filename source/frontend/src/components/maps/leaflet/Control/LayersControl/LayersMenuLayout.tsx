import { LayerMenuEntry } from './types';

export const initialEnabledLayers: Set<string> = new Set(['parcelLayer', 'bctfa_property']);

export const layersMenuTree: LayerMenuEntry = {
  key: 'root',
  label: '',
  nodes: [
    {
      key: 'pims_layers',
      label: 'PIMS',
      nodes: [
        {
          layerDefinitionId: 'pims_research_files',
          key: 'research',
          label: 'Research',
          color: '#3388ff',
        },
        {
          layerDefinitionId: 'pims_acquisition_files',
          key: 'acquisition',
          label: 'Acquisition',
          color: '#42814A',
        },
        {
          layerDefinitionId: 'pims_management_files',
          key: 'management',
          label: 'Management',
          color: '#FC802D',
        },
        {
          layerDefinitionId: 'pims_disposed_files',
          key: 'disposition',
          label: 'Disposition',
          color: '#555555',
        },
        {
          key: 'LeaseLayers',
          label: 'Lease',
          nodes: [
            {
              layerDefinitionId: 'pims_lease_receivable',
              key: 'lease_receivable',
              label: 'Lease (Receivable)',
              color: '#fee391',
            },
            {
              layerDefinitionId: 'pims_lease_payable',
              key: 'lease_payable',
              label: 'Lease (Payable)',
              color: '#fe9929',
            },
          ],
        },
        {
          key: 'InterestLayers',
          label: 'Interests',
          nodes: [
            {
              layerDefinitionId: 'pims_license_to_construct_take',
              key: 'license_to_construct_take',
              label: 'Licence to Construct Take',
              color: '#fa9fb5',
            },
            {
              layerDefinitionId: 'pims_land_act_take',
              key: 'land_act_take',
              label: 'Land Act Take',
              color: '#f768a1',
            },
            {
              layerDefinitionId: 'pims_srw_take',
              key: 'srw_take',
              label: 'SRW Take',
              color: '#dd3497',
            },
            {
              layerDefinitionId: 'pims_surplus_take',
              key: 'surplus_take',
              label: 'Surplus Take',
              color: '#ae017e',
            },
            {
              layerDefinitionId: 'pims_inventory_take',
              key: 'inventory_take',
              label: 'Inventory Take',
              color: '#7a0177',
            },
          ],
        },
      ],
    },
    {
      key: 'external_layers',
      label: 'External',
      nodes: [
        {
          layerDefinitionId: 'pmbc_parcel_pid',
          key: 'pmbc_parcel_pid',
          label: 'Parcels PID',
        },
        {
          key: 'administrative_group',
          label: 'Administrative Boundaries',
          nodes: [
            {
              layerDefinitionId: 'currentEconomicRegions',
              key: 'currentEconomicRegions',
              label: 'Current Census Economic Regions',
              color: '#bbdefb',
            },
            {
              // only works on the vpn
              layerDefinitionId: 'moti',
              key: 'moti',
              label: 'MOTI Regions (multiple colors)',
            },
            {
              // only works on the vpn
              layerDefinitionId: 'motiHighwayDistricts',
              key: 'motiHighwayDistricts',
              label: 'MOTI Highway Districts (multiple colors)',
            },
            {
              layerDefinitionId: 'municipalityLayer',
              key: 'municipalities',
              label: 'Municipalities',
              color: '#b39ddb',
            },
            {
              layerDefinitionId: 'regionalDistricts',
              key: 'regionalDistricts ',
              label: 'Regional Districts (multiple colors)',
            },
          ],
        },
        {
          // The actual layers are populated via tenant.json configuration for each live environment
          key: 'legalHighwayResearch',
          label: 'Legal Highway Research',
          nodes: [
            {
              layerDefinitionId: 'gazettedHighway',
              key: 'gazettedHighway',
              label: 'Gazetted Highway',
              color: '#FF0000',
            },
            {
              layerDefinitionId: 'closedHighway',
              key: 'closedHighway',
              label: 'Closed Highway',
              color: '#00FF04',
            },
            {
              layerDefinitionId: 'parentParcelAcquisition',
              key: 'parentParcelAcquisition',
              label: 'Parent Parcel Acquisition',
              color: '#FFB237',
            },
            {
              layerDefinitionId: 'section107Plan',
              key: 'section107Plan',
              label: 'S. 107 Plan',
              color: '#1818FF',
            },
            {
              layerDefinitionId: 'motiPlan',
              key: 'motiPlan',
              label: 'MoTI Plan',
              color: '#101010',
            },
            {
              layerDefinitionId: 'motiPlanFootprint',
              key: 'motiPlanFootprint',
              label: 'MoTI Plan Footprint',
              color: '#FF88DF',
            },
            {
              layerDefinitionId: 'plans',
              key: 'plans',
              label: 'Plans',
            },
          ],
        },
        {
          key: 'firstNations',
          label: 'First Nations',
          nodes: [
            {
              layerDefinitionId: 'firstNationsReserves',
              key: 'firstNationsReserves',
              label: 'First Nations Reserves',
              color: '#DCBDAD',
            },
            {
              layerDefinitionId: 'firstNationTreatyAreas',
              key: 'firstNationTreatyAreas',
              label: 'First Nation Treaty Areas',
              color: '#367428',
            },
            {
              layerDefinitionId: 'firstNationTreatyLands',
              key: 'firstNationTreatyLands',
              label: 'First Nations Treaty Lands (multiple colors)',
            },
            {
              layerDefinitionId: 'firstNationTreatyRelatedLands',
              key: 'firstNationTreatyRelatedLands',
              label: 'First Nations Treaty Related Lands (multiple colors)',
            },
            {
              layerDefinitionId: 'firstNationTreatySideAgreement',
              key: 'firstNationTreatySideAgreement',
              label: 'First Nation Treaty Side Agreements',
              color: '#DEA39F',
            },
          ],
        },
        {
          key: 'landOwnership',
          label: 'Land Ownership',
          nodes: [
            {
              layerDefinitionId: 'crownLeases',
              key: 'crownLeases',
              label: 'Crown Leases',
              color: '#36708D',
            },
            {
              layerDefinitionId: 'crownInventory',
              key: 'crownInventory',
              label: 'Crown Inventory',
              color: '#D2A9FE',
            },
            {
              layerDefinitionId: 'crownInclusions',
              key: 'crownInclusions',
              label: 'Crown Inclusions (multiple colors)',
            },
            {
              layerDefinitionId: 'crownLandLicenses',
              key: 'crownLandLicenses',
              label: 'Crown Land Licenses',
              color: '#2C519B',
            },
            {
              layerDefinitionId: 'crownTenures',
              key: 'crownTenures',
              label: 'Crown Tenures',
              color: '#FC802D',
            },
            {
              layerDefinitionId: 'crownSurveyParcels',
              key: 'crownSurveyParcels',
              label: 'Crown Surveyed Parcels',
              color: '#777777',
            },
            {
              layerDefinitionId: 'parcelLayer',
              key: 'parcelBoundaries',
              label: 'Parcel Boundaries',
              color: '#E9AD34',
            },
            {
              layerDefinitionId: 'srwInterestParcels',
              key: 'srwInterestParcels',
              label: 'Interest Parcels - SRW',
              color: '#E6270A',
            },
            {
              layerDefinitionId: 'bctfa_property',
              key: 'PMBC_BCTFA_PARCEL_POLYGON_FABRIC_KEY',
              label: 'BCTFA Ownership',
              color: '#42814A',
            },
            {
              layerDefinitionId: 'pmbc_parcel_by_class',
              key: 'pmbc_parcel_by_class',
              label: 'Parcels By Class (multiple colors)',
            },
            {
              layerDefinitionId: 'pmbc_parcel_by_owner',
              key: 'pmbc_parcel_by_owner',
              label: 'Parcels By Owner Type (multiple colors)',
            },
          ],
        },
        {
          key: 'zoning',
          label: 'Zoning',
          nodes: [
            {
              layerDefinitionId: 'agriculturalLandReserve',
              key: 'agriculturalLandReserve',
              label: 'Agricultural Land Reserve',
              color: '#BBD99E',
            },
          ],
        },
        {
          key: 'electoral',
          label: 'Electoral',
          nodes: [
            {
              layerDefinitionId: 'currentElectoralDistricts',
              key: 'currentElectoralDistricts',
              label: 'Current Provincial Electoral Districts of British Columbia',
              color: '#da2223',
            },
          ],
        },
        {
          key: 'federal_bc_parks',
          label: 'Federal/BC Parks',
          nodes: [
            {
              layerDefinitionId: 'federalParks',
              key: 'federalParks',
              label: 'Federal Parks',
              color: '#AEF51F',
            },
            {
              layerDefinitionId: 'bcParks',
              key: 'bcParks',
              label: 'BC Parks',
              color: '#AAD589',
            },
          ],
        },
      ],
    },
  ],
};
