import { PropertyServiceFileTypes } from 'constants/index';

export interface IPropertyServiceFile {
  id?: number;
  fileTypeId: PropertyServiceFileTypes;
  fileType?: string;
  rowVersion?: number;
}
