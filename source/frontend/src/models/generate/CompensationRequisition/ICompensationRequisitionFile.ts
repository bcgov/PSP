import { Api_GenerateH120Property } from '../acquisition/GenerateH120Property';
import { Api_GenerateProduct } from '../GenerateProduct';
import { Api_GenerateProject } from '../GenerateProject';

export interface ICompensationRequisitionFile {
  file_number: string;
  file_name: string;
  project: Api_GenerateProject;
  product: Api_GenerateProduct;
  properties: Api_GenerateH120Property[];
  all_owners_string: string;
}
