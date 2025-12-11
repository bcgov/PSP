import { chain } from 'lodash';
import { CellProps } from 'react-table';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { ExternalLink } from '@/components/common/ExternalLink';
import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId, stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { ManagementSearchResultModel } from '../models';

interface MemberRoleGroup {
  id: string;
  person: ApiGen_Concepts_Person | null;
  organization: ApiGen_Concepts_Organization | null;
  roles: string[];
}

export const columns: ColumnWithProps<ManagementSearchResultModel>[] = [
  {
    Header: 'Management file #',
    accessor: 'managementFileId',
    align: 'center',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ManagementSearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();
      const fileNumberString = `M-${props.row.original.id}`;

      if (hasClaim(Claims.MANAGEMENT_VIEW)) {
        return (
          <ExternalLink to={`/mapview/sidebar/management/${props.row.original.id}`}>
            {fileNumberString}
          </ExternalLink>
        );
      }

      return stringToFragment(fileNumberString);
    },
  },
  {
    Header: 'File name',
    accessor: 'fileName',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Historical File #',
    accessor: 'legacyFileNum',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Project',
    accessor: 'project',
    align: 'left',
    clickable: true,
    sortable: false,
    width: 20,
    maxWidth: 40,
    Cell: (props: CellProps<ManagementSearchResultModel>) => {
      const project = props.row.original.project;

      return <>{[project?.code, project?.description].filter(Boolean).join(' ')}</>;
    },
  },
  {
    Header: 'Purpose',
    accessor: 'managementFilePurposeTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Team member',
    accessor: 'managementTeam',
    align: 'left',
    clickable: true,
    width: 40,
    maxWidth: 40,
    Cell: (props: CellProps<ManagementSearchResultModel>) => {
      const team = props.row.original.managementTeam;
      const personsInTeam = team?.filter(x => isValidId(x.personId));
      const organizationsInTeam = team?.filter(x => isValidId(x.organizationId));

      const personsAsString: MemberRoleGroup[] = chain(personsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_ManagementFileTeam) => groupedTeams.personId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() ?? '',
          person: x[0].person ?? null,
          organization: null,
          roles: x
            .map(t => t.teamProfileType)
            .filter(exists)
            .flatMap(y => y.description ?? ''),
        }))
        .value();

      const organizationsAsString: MemberRoleGroup[] = chain(organizationsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_ManagementFileTeam) => groupedTeams.organizationId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() ?? '',
          person: null,
          organization: x[0].organization ?? null,
          roles: x
            .map(t => t.teamProfileType)
            .filter(exists)
            .flatMap(y => y.description ?? ''),
        }))
        .value();

      const teamAsString = personsAsString.concat(organizationsAsString);

      return (
        <ExpandableTextList<MemberRoleGroup>
          items={teamAsString ?? []}
          keyFunction={(item: MemberRoleGroup, index: number) =>
            item.person
              ? `management-team-${item.id}-person-${item.person.id ?? index}`
              : `management-team-${item.id}-org-${item.organization?.id ?? index}`
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
    Cell: (props: CellProps<ManagementSearchResultModel>) => {
      const properties = props.row.original.fileProperties.map(x => x.property) ?? [];

      return (
        <ExpandableFileProperties
          properties={properties}
          maxDisplayCount={2}
        ></ExpandableFileProperties>
      );
    },
  },
  {
    Header: 'Status',
    accessor: 'managementFileStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
];
