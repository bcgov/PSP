export interface ISystemCheck {
  // Status of the api.
  status: string;
  // Length of time the api has been healthy.
  totalDuration: Date;
  // Dictionary of health information.
  entries: {
    PmbcExternalApi: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
    Geoserver: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
    Mayan: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
    Ltsa: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
    Geocoder: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
    Cdogs: {
      data: object;
      duration: Date;
      status: string;
      tags: string[];
    };
  };
}

export default ISystemCheck;
