import { chain } from 'lodash';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId, stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DispositionSearchResultModel } from '../models';

interface MemberRoleGroup {
  id: string;
  person: ApiGen_Concepts_Person | null;
  organization: ApiGen_Concepts_Organization | null;
  roles: string[];
}

export const columns: ColumnWithProps<DispositionSearchResultModel>[] = [
  {
    Header: 'Disposition file #',
    accessor: 'fileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<DispositionSearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.DISPOSITION_VIEW)) {
        return (
          <Link to={`/mapview/sidebar/disposition/${props.row.original.id}`}>
            D-{props.row.original.fileNumber}
          </Link>
        );
      }
      return stringToFragment(props.row.original.fileNumber);
    },
  },
  {
    Header: 'Reference #',
    accessor: 'fileReference',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Disposition file name',
    accessor: 'fileName',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Disposition type',
    accessor: 'dispositionTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'MOTI Region',
    accessor: 'region',
    align: 'left',
    maxWidth: 20,
  },
  {
    Header: 'Team member',
    accessor: 'dispositionTeam',
    align: 'left',
    clickable: true,
    width: 40,
    maxWidth: 40,
    Cell: (props: CellProps<DispositionSearchResultModel>) => {
      const team = props.row.original.dispositionTeam;
      const personsInTeam = team?.filter(x => isValidId(x.personId));
      const organizationsInTeam = team?.filter(x => isValidId(x.organizationId));

      const personsAsString: MemberRoleGroup[] = chain(personsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_DispositionFileTeam) => groupedTeams.personId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() || '',
          person: x[0].person || null,
          organization: null,
          roles: x
            .map(t => t.teamProfileType)
            .filter(exists)
            .flatMap(y => y.description || ''),
        }))
        .value();

      const organizationsAsString: MemberRoleGroup[] = chain(organizationsInTeam)
        .groupBy((groupedTeams: ApiGen_Concepts_DispositionFileTeam) => groupedTeams.organizationId)
        .map<MemberRoleGroup>(x => ({
          id: x[0].id?.toString() || '',
          person: null,
          organization: x[0].organization || null,
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
              ? `disposition-team-${item.id}-person-${item.person.id ?? index}`
              : `disposition-team-${item.id}-org-${item.organization?.id ?? index}`
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
    Cell: (props: CellProps<DispositionSearchResultModel>) => {
      return (
        <ExpandableFileProperties
          fileProperties={props.row.original.fileProperties}
          maxDisplayCount={2}
        ></ExpandableFileProperties>
      );
    },
  },
  {
    Header: 'Disposition status',
    accessor: 'dispositionStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Status',
    accessor: 'dispositionFileStatusTypeCode',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
];
