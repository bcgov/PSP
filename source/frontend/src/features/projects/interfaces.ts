export interface IProjectFilter {
  projectStatusCode: string;
  projectName: string;
  projectNumber: string;
  projectCreatedBy?: string | null;
  teamMemberPersonId: string;
  regions: string[];
}

export interface IProjectSearchBy {
  projectNumber: string;
  projectName: string;
}
