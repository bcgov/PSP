import { FeatureCollection, Geometry } from 'geojson';
import moment from 'moment';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { exists, firstOrNull } from '@/utils';

import { ApiGen_Concepts_AcquisitionFileOwner } from '../api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '../api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_Project } from '../api/generated/ApiGen_Concepts_Project';
import { PMBC_FullyAttributed_Feature_Properties } from '../layers/parcelMapBC';
import { Api_GenerateOwner } from './GenerateOwner';
import { Api_GeneratePerson } from './GeneratePerson';
import { Api_GenerateProject } from './GenerateProject';
import { Api_GenerateProperty } from './GenerateProperty';
export class Api_GenerateNotice {
  date_generated: string;

  owners: Api_GenerateOwner[];
  responsible_member: Api_GeneratePerson;
  signing_member: Api_GeneratePerson;
  project: Api_GenerateProject;
  properties: Api_GenerateProperty[];

  constructor(
    project: ApiGen_Concepts_Project | null | undefined,
    owners: ApiGen_Concepts_AcquisitionFileOwner[] | null | undefined,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null | undefined,
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null | undefined,
    properties: ComposedProperty[],
  ) {
    this.date_generated = moment().format('DD/M/YYYY');
    this.project = new Api_GenerateProject(project ?? null);
    this.owners = owners?.map(owner => new Api_GenerateOwner(owner)) ?? [];
    this.signing_member = new Api_GeneratePerson(
      signingTeamMember?.person,
      signingTeamMember?.teamProfileType.description,
    );
    this.responsible_member = new Api_GeneratePerson(
      responsibleTeamMember?.person,
      responsibleTeamMember?.teamProfileType.description,
    );
    this.properties = properties.filter(exists).map(p => {
      const parcelMapFeatures =
        (
          p.parcelMapFeatureCollection as FeatureCollection<
            Geometry,
            PMBC_FullyAttributed_Feature_Properties
          >
        )?.features ?? [];

      return new Api_GenerateProperty(p.pimsProperty, firstOrNull(parcelMapFeatures)?.properties);
    });
  }
}
