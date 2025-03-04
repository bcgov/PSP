import IHealthcheckResponse from './IHealthcheckResponse';

export interface ISystemCheck {
  // Status of the api.
  status: string;
  // Length of time the api has been healthy.
  totalDuration: Date;
  // Dictionary of health information.
  entries: {
    PmbcExternalApi: IHealthcheckResponse;
    Geoserver: IHealthcheckResponse;
    Mayan: IHealthcheckResponse;
    Ltsa: IHealthcheckResponse;
    Geocoder: IHealthcheckResponse;
    Cdogs: IHealthcheckResponse;
  };
}

export default ISystemCheck;
