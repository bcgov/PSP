import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Person } from 'models/api/Person';

import { Api_GenerateOwner } from './GenerateOwner';
import { Api_GeneratePerson } from './GeneratePerson';
import { Api_GenerateProduct } from './GenerateProduct';
import { Api_GenerateProject } from './GenerateProject';
import { Api_GenerateProperty } from './GenerateProperty';

export class Api_GenerateFile {
  properties: Api_GenerateProperty[];
  property_coordinator: Api_GeneratePerson;
  primary_owner?: Api_GenerateOwner;
  owners: Api_GenerateOwner[];
  person_owners: Api_GenerateOwner[];
  organization_owners: Api_GenerateOwner[];
  file_number: string;
  file_name: string;
  project_number: string;
  project_name: string;
  prov_solicitor?: Api_GeneratePerson;
  owner_solicitor?: Api_GeneratePerson;
  neg_agent?: Api_GeneratePerson;
  project?: Api_GenerateProject;
  product?: Api_GenerateProduct;

  constructor(
    file: Api_AcquisitionFile | null,
    coordinatorContact: Api_Person | null | undefined = null,
    negotiatingAgent: Api_Person | null | undefined = null,
    provincialSolicitor: Api_Person | null | undefined = null,
    ownerSolicitor: Api_Person | null | undefined = null,
  ) {
    this.owners = file?.acquisitionFileOwners?.map(owner => new Api_GenerateOwner(owner)) ?? [];
    this.property_coordinator = new Api_GeneratePerson(coordinatorContact);
    this.neg_agent = new Api_GeneratePerson(negotiatingAgent);
    this.properties = file?.fileProperties?.map(fp => new Api_GenerateProperty(fp?.property)) ?? [];
    this.file_name = file?.fileName ?? '';
    this.file_number = file?.fileNumber ?? '';
    this.project_name = file?.project?.description ?? '';
    this.project_number = file?.project?.code ?? '';
    this.primary_owner = new Api_GenerateOwner(
      file?.acquisitionFileOwners?.find(owner => owner.isPrimaryContact) ?? null,
    );
    this.prov_solicitor = new Api_GeneratePerson(provincialSolicitor);
    this.owner_solicitor = new Api_GeneratePerson(ownerSolicitor);
    this.person_owners =
      file?.acquisitionFileOwners
        ?.filter(owner => !owner.isOrganization)
        ?.map(owner => new Api_GenerateOwner(owner)) ?? [];
    this.organization_owners =
      file?.acquisitionFileOwners
        ?.filter(owner => owner.isOrganization)
        ?.map(owner => new Api_GenerateOwner(owner)) ?? [];
    this.project = new Api_GenerateProject(file?.project ?? null);
    this.product = new Api_GenerateProduct(file?.product ?? null);
  }
}
