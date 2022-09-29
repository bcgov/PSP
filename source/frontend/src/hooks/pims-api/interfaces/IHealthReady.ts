export interface IHealthReady {
  // Status of the api.
  status: string;
  // Length of time the api has been healthy.
  totalDuration: Date;
  // Dictionary of health information.
  entries: {
    sqlserver: {
      data: {};
      // Length of time this service has been healthy.
      duration: Date;
      // Status of the liveness service.
      status: string;
      // Related tags for this service.
      tags: string[];
    };
  };
}

export default IHealthReady;
