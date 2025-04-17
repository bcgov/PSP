export interface IHealthCheckResponse {
  data: object;
  duration: Date;
  status: string;
  tags: string[];
}

export default IHealthCheckResponse;
