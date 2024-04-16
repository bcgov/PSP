import { chain, uniqBy } from 'lodash';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps, renderTypeCode } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { exists, isValidId, stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { AcquisitionSearchResultModel } from './models';

interface MemberRoleGroup {
  id: string;
  person: ApiGen_Concepts_Person | null;
  organization: ApiGen_Concepts_Organization | null;
  roles: string[];
}

export const columns: ColumnWithProps<AcquisitionSearchResultModel>[] = [
  {
    Header: 'Acquisition file #',
    accessor: 'fileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.ACQUISITION_VIEW)) {
        return (
          <Link to={`/mapview/sidebar/acquisition/${props.row.original.id}`}>
            {props.row.original.fileNumber}
          </Link>
        );
      }
      return stringToFragment(props.row.original.fileNumber);
    },
  },
  {
    Header: 'Historical file #',
    accessor: 'legacyFileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Acquisition file name',
    accessor: 'fileName',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'MOTI Region',
    accessor: 'regionCode',
    align: 'left',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) =>
      stringToFragment(props.row.original.regionCode),
  },
  {
    Header: 'Projects',
    accessor: 'project',
    align: 'left',
    clickable: true,
    width: 20,
    maxWidth: 30,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      const project = props.row.original.project;
      const altProjects = (props.row.original.compensationRequisitions ?? [])
        .filter(cr => !!cr?.alternateProject)
        .map(cr => cr.alternateProject) as ApiGen_Concepts_Project[];

      return (
        <>
          {[project?.code, project?.description].filter(Boolean).join(' ')}
          <ExpandableTextList<ApiGen_Concepts_Project | undefined>
            keyFunction={p => p?.id?.toString() ?? '0'}
            renderFunction={p => (
              <>{['Alt Project:', p?.code, p?.description].filter(Boolean).join(' ')}</>
            )}
            items={uniqBy(altProjects, project => project?.code)}
            delimiter={'; '}
            maxCollapsedLength={project === undefined ? 1 : 0}
          />
        </>
      );
    },
  },
  {
    Header: 'Team member',
    accessor: 'acquisitionTeam',
    align: 'left',
    clickable: true,
    width: 40,
    maxWidth: 40,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      const acquisitionTeam = props.row.original.acquisitionTeam;
      const personsInTeam = acquisitionTeam?.filter(x => isValidId(x.personId));
      const organizationsInTeam = acquisitionTeam?.filter(x => isValidId(x.organizationId));

      const personsAsString: MemberRoleGroup[] = chain(personsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_AcquisitionFileTeam) => groupedTeams.personId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() || '',
          person: x[0].person ?? null,
          organization: null,
          roles: x
            .map(t => t.teamProfileType)
            .filter(exists)
            .flatMap(y => y.description || ''),
        }))
        .value();

      const organizationsAsString: MemberRoleGroup[] = chain(organizationsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_AcquisitionFileTeam) => groupedTeams.organizationId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() || '',
          person: null,
          organization: x[0].organization ?? null,
          roles: x
            .map(t => t.teamProfileType)
            .filter(exists)
            .flatMap(y => y.description || ''),
        }))
        .value();

      const teamAsString = personsAsString.concat(organizationsAsString);

      return (
        <ExpandableTextList<MemberRoleGroup>
          items={teamAsString ?? []}
          keyFunction={(item: MemberRoleGroup, index: number) =>
            item.person
              ? `acquisition-team-${item.id}-person-${item.person.id ?? index}`
              : `acquisition-team-${item.id}-org-${item.organization?.id ?? index}`
          }
          renderFunction={(item: MemberRoleGroup) =>
            item.person ? (
              <>{`${formatApiPersonNames(item.person)} (${item.roles.join(', ')})`}</>
            ) : (
              <>{`${item.organization?.name} (${item.roles.join(', ')})`}</>
            )
          }
          delimiter={', '}
          maxCollapsedLength={2}
        />
      );
    },
  },
  {
    Header: 'Civic Address / PID / PIN',
    accessor: 'fileProperties',
    align: 'left',
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      return (
        <ExpandableFileProperties
          fileProperties={props.row.original.fileProperties}
          maxDisplayCount={2}
        ></ExpandableFileProperties>
      );
    },
  },
  {
    Header: 'Status',
    accessor: 'acquisitionFileStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];
