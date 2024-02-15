import { chain } from 'lodash';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_DispositionFileTeam } from '@/models/api/DispositionFile';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import Api_TypeCode from '@/models/api/TypeCode';
import { stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DispositionSearchResultModel } from '../models';

interface MemberRoleGroup {
  id: string;
  person: Api_Person | null;
  organization: Api_Organization | null;
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
    clickable: true,
    width: 10,
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
      const personsInTeam = team?.filter(x => x.personId !== undefined);
      const organizationsInTeam = team?.filter(x => x.organizationId !== undefined);

      const personsAsString: MemberRoleGroup[] = chain(personsInTeam)
        .groupBy((groupedTeams: Api_DispositionFileTeam) => groupedTeams.personId)
        .map<MemberRoleGroup>(x => {
          return {
            id: x[0].id?.toString() || '',
            person: x[0].person || {},
            organization: null,
            roles: x
              .map(t => t.teamProfileType)
              .filter((z): z is Api_TypeCode<string> => z !== undefined)
              .flatMap(y => y.description || ''),
          };
        })
        .value();

      const organizationsAsString: MemberRoleGroup[] = chain(organizationsInTeam)
        .groupBy((groupedTeams: Api_DispositionFileTeam) => groupedTeams.organizationId)
        .map<MemberRoleGroup>(x => {
          return {
            id: x[0].id?.toString() || '',
            person: null,
            organization: x[0].organization || {},
            roles: x
              .map(t => t.teamProfileType)
              .filter((z): z is Api_TypeCode<string> => z !== undefined)
              .flatMap(y => y.description || ''),
          };
        })
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
