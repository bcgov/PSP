import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';

import { Api_GenerateH120Property } from '../acquisition/GenerateH120Property';
import { Api_GenerateOwner } from '../GenerateOwner';
import { Api_GenerateProduct } from '../GenerateProduct';
import { Api_GenerateProject } from '../GenerateProject';
import { ICompensationRequisitionFile } from './ICompensationRequisitionFile';

export class Api_GenerateCompReqFileLease implements ICompensationRequisitionFile {
  private readonly leaseOwners: Api_GenerateOwner[];

  readonly file_number: string;
  readonly file_name: string;
  readonly project: Api_GenerateProject;
  readonly product: Api_GenerateProduct;
  readonly properties: Api_GenerateH120Property[];
  readonly all_owners_string: string;

  constructor(
    lease: ApiGen_Concepts_Lease,
    leaseProperties: ApiGen_Concepts_PropertyLease[],
    leaseStakeholders: ApiGen_Concepts_LeaseStakeholder[],
  ) {
    this.file_number = lease.lFileNo ?? '';
    this.file_name = lease.fileNumber ?? '';

    this.project = new Api_GenerateProject(lease.project ?? null);
    this.product = new Api_GenerateProduct(lease?.product ?? null);

    this.properties = leaseProperties.map(p => {
      return new Api_GenerateH120Property(p.property, []);
    });

    this.leaseOwners = leaseStakeholders
      .filter(
        sth =>
          sth.stakeholderTypeCode.id === ApiGen_CodeTypes_LeaseStakeholderTypes.OWNER &&
          (sth.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER ||
            sth.lessorType.id === ApiGen_CodeTypes_LessorTypes.ORG),
      )
      .map(owner => {
        if (owner.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER) {
          return Api_GenerateOwner.fromApiPerson(owner.person);
        } else {
          return Api_GenerateOwner.fromApiOrganization(owner.organization);
        }
      });

    this.all_owners_string = this.leaseOwners.map(owner => owner.owner_string).join(', ');
  }
}
