/**
 * PIMS API health liveliness endpoint model.
 */
export interface IHealthLive {
  // Status of the api.
  status: string;
  // Length of time the api has been healthy.
  totalDuration: Date;
  // Dictionary of health information.
  entries: {
    liveliness: {
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

export default IHealthLive;
