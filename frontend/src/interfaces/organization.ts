export interface IOrganization {
  parentId?: any;
  code?: string;
  id?: number;
  name?: string;
  description?: string;
  /** string value of parent not given by api but found by frontend */
  parent?: string;
}

/** for use in editing and viewing organization details */
export interface IOrganizationDetail {
  parentId?: number;
  email?: string;
  id?: number;
  name: string;
  description?: string;
  isDisabled: boolean;
  sendEmail: boolean;
  addressTo: string;
  code: string;
  rowVersion?: number;
  parent?: string;
}

/** for use in filtering organizations*/
export interface IOrganizationFilter {
  name?: string;
  description?: string;
  id?: number | '';
}

/** for use in creating an organization */
export interface IAddOrganization {
  name: string;
  code: string;
  email?: string;
  addressTo: string;
  isDisabled: boolean;
  sendEmail: boolean;
  parentId?: number;
  description?: string;
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
