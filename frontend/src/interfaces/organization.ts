/** for use in filtering organizations*/
export interface IOrganizationFilter {
  name?: string;
  description?: string;
  id?: number | '';
}

/** for use in organization tables */
export interface IOrganizationRecord {
  name?: string;
  id?: number;
  code?: string;
  description?: string;
  parentId?: number;
  parent?: string;
  email?: string;
  sendEmail?: boolean;
}
