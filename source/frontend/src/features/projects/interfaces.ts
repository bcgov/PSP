export interface IProjectFilter {
  projectStatusCode: string;
  projectName: string;
  projectNumber: string;
  teamMemberPersonId: string;
  regions: string[];
}

export interface IProjectSearchBy {
  projectNumber: string;
  projectName: string;
}
