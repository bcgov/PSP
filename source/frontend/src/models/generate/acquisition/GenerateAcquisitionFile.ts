import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';

import { Api_GenerateOrganization } from '../GenerateOrganization';
import { Api_GenerateOwner } from '../GenerateOwner';
import { Api_GeneratePerson } from '../GeneratePerson';
import { Api_GenerateProduct } from '../GenerateProduct';
import { Api_GenerateProject } from '../GenerateProject';
import { Api_GenerateH120InterestHolder } from './GenerateH120InterestHolder';
import { Api_GenerateH120Property } from './GenerateH120Property';
import { Api_GenerateInterestHolder } from './GenerateInterestHolder';

export interface IApiGenerateAcquisitionFileInput {
  file: ApiGen_Concepts_AcquisitionFile | null;
  coordinatorContact?: ApiGen_Concepts_AcquisitionFileTeam | null;
  negotiatingAgent?: ApiGen_Concepts_AcquisitionFileTeam | null;
  provincialSolicitor?: ApiGen_Concepts_AcquisitionFileTeam | null;
  ownerSolicitor?: ApiGen_Concepts_InterestHolder | null;
  interestHolders?: ApiGen_Concepts_InterestHolder[];
}

export class Api_GenerateAcquisitionFile {
  properties: Api_GenerateH120Property[];
  property_coordinator?: Api_GeneratePerson | Api_GenerateOrganization;
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
  prov_solicitor?: Api_GeneratePerson | Api_GenerateOrganization;
  prov_solicitor_attn?: Api_GeneratePerson;
  owner_solicitor?: Api_GenerateInterestHolder;
  neg_agent?: Api_GeneratePerson | Api_GenerateOrganization;
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
    this.property_coordinator = this.getTeam(coordinatorContact);
    this.neg_agent = this.getTeam(negotiatingAgent, true);
    const allInterestHoldersPropertes = interestHolders.flatMap(
      ih => ih?.interestHolderProperties ?? [],
    );

    this.properties =
      file?.fileProperties?.map(fp => {
        const matchingInterestHolderProperties =
          allInterestHoldersPropertes.filter(
            ihp =>
              ihp.acquisitionFilePropertyId === fp?.id &&
              ihp.propertyInterestTypes?.some(pit => pit.id !== 'NIP'),
          ) ?? [];

        const interestHoldersForAcquisitionFile = matchingInterestHolderProperties.flatMap(
          (mihp: ApiGen_Concepts_InterestHolderProperty) =>
            mihp.propertyInterestTypes?.map(
              pit =>
                new Api_GenerateH120InterestHolder(
                  interestHolders.find(ih => ih.interestHolderId === mihp.interestHolderId) ?? null,
                  mihp,
                  pit,
                ),
            ) || [],
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
    this.prov_solicitor = this.getTeam(provincialSolicitor);
    this.prov_solicitor_attn = new Api_GeneratePerson(provincialSolicitor?.primaryContact);
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

  getTeam = (team: ApiGen_Concepts_AcquisitionFileTeam | null, overrideOrgAddress = false) => {
    if (!team) return undefined;

    if (team.person) {
      return new Api_GeneratePerson(team.person);
    }

    const org = new Api_GenerateOrganization(team.organization);
    const primary = new Api_GeneratePerson(team.primaryContact);
    //replace organization contact info with primary contact info but leave address, name.
    org.phone = primary?.phone;
    org.email = primary?.email;
    if (overrideOrgAddress) {
      org.address = primary?.address;
    }
    return org;
  };
}
