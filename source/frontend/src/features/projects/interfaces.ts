export interface IProjectFilter {
  projectRegionCode: string;
  projectStatusCode: string;
  projectName: string;
  projectNumber: string;
  regions: string[];
}

export interface IProjectSearchBy {
  projectNumber: string;
  projectName: string;
}
