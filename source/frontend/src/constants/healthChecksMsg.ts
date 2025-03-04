type TSystemCheckMessages = {
  [dict_key: string]: string;
};

const SystemCheckMessages: TSystemCheckMessages = {
  PimsApi:
    'The PIMS server is currently unavailable, PIMS will not be useable until this is resolved',
  PmbcExternalApi:
    'The BC Data Warehouse is experiencing service degradation, this will limit PIMS map functionality until resolved.',
  Geoserver:
    'The MOTT Geoserver is experiencing service degradation, PIMS map layer functionality will be limited until resolved.',
  Mayan:
    'The PIMS Document server is experiencing service degradation, you will be unable to view, download or upload documents until resolved.',
  Ltsa: 'The LTSA title service is experiencing service degradation, the LTSA tab within a property will not be viewable until resolved.',
  Geocoder:
    'The BC Geocoder is experiencing service degradation, address search will be unavailable until resolved.',
  Cdogs:
    'The DevExchange Document Generation Service is experiencing service degradation, you will be unable to generate form documents (ie. H120, H1005) until resolved.',
};

export default SystemCheckMessages;
