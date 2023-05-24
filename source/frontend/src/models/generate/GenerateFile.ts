import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Person } from 'models/api/Person';

import { GenerateOwner } from './GenerateOwner';
import { GeneratePerson } from './GeneratePerson';
import { GenerateProperty } from './GenerateProperty';
export class Api_GenerateFile {
  properties: GenerateProperty[];
  property_coordinator: GeneratePerson;
  primary_owner?: GenerateOwner;
  owners: GenerateOwner[];
  person_owners: GenerateOwner[];
  organization_owners: GenerateOwner[];
  file_number: string;
  file_name: string;
  project_number: string;
  project_name: string;
  prov_solicitor?: GeneratePerson;
  owner_solicitor?: GeneratePerson;
  neg_agent?: GeneratePerson;

  constructor(
    file: Api_AcquisitionFile | null,
    coordinatorContact: Api_Person | null | undefined = null,
    negotiatingAgent: Api_Person | null | undefined = null,
    provincialSolicitor: Api_Person | null | undefined = null,
    ownerSolicitor: Api_Person | null | undefined = null,
  ) {
    this.owners = file?.acquisitionFileOwners?.map(owner => new GenerateOwner(owner)) ?? [];
    this.property_coordinator = new GeneratePerson(coordinatorContact);
    this.neg_agent = new GeneratePerson(negotiatingAgent);
    this.properties = file?.fileProperties?.map(fp => new GenerateProperty(fp?.property)) ?? [];
    this.file_name = file?.fileName ?? '';
    this.file_number = file?.fileNumber ?? '';
    this.project_name = file?.project?.description ?? '';
    this.project_number = file?.project?.code ?? '';
    this.primary_owner = new GenerateOwner(
      file?.acquisitionFileOwners?.find(owner => owner.isPrimaryContact) ?? null,
    );
    this.prov_solicitor = new GeneratePerson(provincialSolicitor);
    this.owner_solicitor = new GeneratePerson(ownerSolicitor);
    this.person_owners =
      file?.acquisitionFileOwners
        ?.filter(owner => !owner.isOrganization)
        ?.map(owner => new GenerateOwner(owner)) ?? [];
    this.organization_owners =
      file?.acquisitionFileOwners
        ?.filter(owner => owner.isOrganization)
        ?.map(owner => new GenerateOwner(owner)) ?? [];
  }
}
