import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import { Api_Person } from '@/models/api/Person';

import { Api_GenerateOwner } from '../GenerateOwner';
import { Api_GeneratePerson } from '../GeneratePerson';
import { Api_GenerateProduct } from '../GenerateProduct';
import { Api_GenerateProject } from '../GenerateProject';
import { Api_GenerateH120InterestHolder } from './GenerateH120InterestHolder';
import { Api_GenerateH120Property } from './GenerateH120Property';
import { Api_GenerateInterestHolder } from './GenerateInterestHolder';

export interface IApiGenerateAcquisitionFileInput {
  file: Api_AcquisitionFile | null;
  coordinatorContact?: Api_Person | null;
  negotiatingAgent?: Api_Person | null;
  provincialSolicitor?: Api_Person | null;
  ownerSolicitor?: Api_InterestHolder | null;
  interestHolders?: Api_InterestHolder[];
}

export class Api_GenerateAcquisitionFile {
  properties: Api_GenerateH120Property[];
  property_coordinator: Api_GeneratePerson;
  primary_owner?: Api_GenerateOwner;
  owners: Api_GenerateOwner[];
  person_owners: Api_GenerateOwner[];
  organization_owners: Api_GenerateOwner[];
  all_owners_string: string;
  all_owners_string_and: string;
  file_number: string;
  file_name: string;
  project_number: string;
  project_name: string;
  prov_solicitor?: Api_GeneratePerson;
  owner_solicitor?: Api_GenerateInterestHolder;
  neg_agent?: Api_GeneratePerson;
  project?: Api_GenerateProject;
  product?: Api_GenerateProduct;

  constructor({
    file,
    coordinatorContact = null,
    negotiatingAgent = null,
    provincialSolicitor = null,
    ownerSolicitor = null,
    interestHolders = [],
  }: IApiGenerateAcquisitionFileInput) {
    this.owners = file?.acquisitionFileOwners?.map(owner => new Api_GenerateOwner(owner)) ?? [];
    this.property_coordinator = new Api_GeneratePerson(coordinatorContact);
    this.neg_agent = new Api_GeneratePerson(negotiatingAgent);
    const allInterestHoldersPropertes = interestHolders.flatMap(
      ih => ih?.interestHolderProperties ?? [],
    );

    this.properties =
      file?.fileProperties?.map(fp => {
        const matchingInterestHolderProperties =
          allInterestHoldersPropertes.filter(
            ihp =>
              ihp.acquisitionFilePropertyId === fp?.id &&
              ihp.propertyInterestTypes.some(pit => pit.id !== 'NIP'),
          ) ?? [];

        const interestHoldersForAcquisitionFile = matchingInterestHolderProperties.flatMap(
          (mihp: Api_InterestHolderProperty) =>
            mihp.propertyInterestTypes.map(
              pit =>
                new Api_GenerateH120InterestHolder(
                  interestHolders.find(ih => ih.interestHolderId === mihp.interestHolderId) ?? null,
                  mihp,
                  pit,
                ),
            ),
        );

        return new Api_GenerateH120Property(fp?.property, interestHoldersForAcquisitionFile);
      }) ?? [];

    this.file_name = file?.fileName ?? '';
    this.file_number = file?.fileNumber ?? '';
    this.project_name = file?.project?.description ?? '';
    this.project_number = file?.project?.code ?? '';
    this.primary_owner = new Api_GenerateOwner(
      file?.acquisitionFileOwners?.find(owner => owner.isPrimaryContact) ?? null,
    );
    this.prov_solicitor = new Api_GeneratePerson(provincialSolicitor);
    this.owner_solicitor = new Api_GenerateInterestHolder(ownerSolicitor);
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
    this.all_owners_string = this.owners.map(owner => owner.owner_string).join(', ');
    this.all_owners_string_and = this.owners.map(owner => owner.owner_string).join(' And ');
  }
}
