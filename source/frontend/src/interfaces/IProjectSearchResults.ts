import ITypeCode from './ITypeCode';

export interface IProjectSearchResult {
  id: number;
  projectNumber: string;
  projectName: string;
  regionType?: ITypeCode<string>;
  statusType?: ITypeCode<string>;
  lastUpdatedBy: string;
  lastUpdatedDate: Date;
}
