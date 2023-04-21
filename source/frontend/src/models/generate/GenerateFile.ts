import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Person } from 'models/api/Person';

import { GenerateOwner } from './GenerateOwner';
import { GeneratePerson } from './GeneratePerson';
import { GenerateProperty } from './GenerateProperty';
export class Api_GenerateFile {
  properties: GenerateProperty[];
  property_coordinator: GeneratePerson;
  owners: GenerateOwner[];
  file_number: string;
  file_name: string;
  project_number: string;
  project_name: string;

  constructor(file: Api_AcquisitionFile, coordinatorContact: Api_Person | null | undefined) {
    this.owners = file?.acquisitionFileOwners?.map(owner => new GenerateOwner(owner)) ?? [];
    this.property_coordinator = new GeneratePerson(coordinatorContact);
    this.properties = file?.fileProperties?.map(fp => new GenerateProperty(fp?.property)) ?? [];
    this.file_name = file?.fileName ?? '';
    this.file_number = file?.fileNumber ?? '';
    this.project_name = file?.project?.description ?? '';
    this.project_number = file?.project?.code ?? '';
  }
}
