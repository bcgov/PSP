import IHealthCheckResponse from './IHealthcheckResponse';

export interface ISystemCheck {
  // Status of the api.
  status: string;
  // Length of time the api has been healthy.
  totalDuration: Date;
  // Dictionary of health information.
  entries: {
    PmbcExternalApi: IHealthCheckResponse;
    Geoserver: IHealthCheckResponse;
    Mayan: IHealthCheckResponse;
    Ltsa: IHealthCheckResponse;
    Geocoder: IHealthCheckResponse;
    Cdogs: IHealthCheckResponse;
  };
}

export default ISystemCheck;
