/**
 * API version model
 */
export interface IApiVersion {
  // The epi environment name
  environment: string;
  // The api version
  version: string;
  // The api file version (normally same as version above)
  fileVersion: string;
  // Additional information related to the version (i.e. 1.1.1.1-alpha)
  informationalVersion: string;
}

export default IApiVersion;
