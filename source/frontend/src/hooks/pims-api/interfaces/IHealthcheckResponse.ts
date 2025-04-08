export interface IHealthcheckResponse {
  data: object;
  duration: Date;
  status: string;
  tags: string[];
}

export default IHealthcheckResponse;
