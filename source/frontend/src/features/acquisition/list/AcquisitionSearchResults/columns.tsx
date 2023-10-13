import { chain, uniqBy } from 'lodash';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { ColumnWithProps, renderTypeCode } from '@/components/Table';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_AcquisitionFileTeam } from '@/models/api/AcquisitionFile';
import { Api_Person } from '@/models/api/Person';
import { Api_Project } from '@/models/api/Project';
import Api_TypeCode from '@/models/api/TypeCode';
import { stringToFragment } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import AcquisitionProperties from './AcquisitionProperties';
import { AcquisitionSearchResultModel } from './models';

interface PersonRoleGroup {
  id: string;
  person: Api_Person;
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
        .map(cr => cr.alternateProject) as Api_Project[];

      return (
        <>
          {[project?.code, project?.description].filter(Boolean).join(' ')}
          <ExpandableTextList<Api_Project | undefined>
            keyFunction={p => p?.id?.toString() ?? '0'}
            renderFunction={(p, index) => (
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
    accessor: 'aquisitionTeam',
    align: 'left',
    clickable: true,
    width: 40,
    maxWidth: 40,
    Cell: (props: CellProps<AcquisitionSearchResultModel>) => {
      const acquisitionTeam = props.row.original.aquisitionTeam;
      const teamAsString: PersonRoleGroup[] = chain(acquisitionTeam)
        .groupBy((groupedTeams: Api_AcquisitionFileTeam) => groupedTeams.personId)
        .map<PersonRoleGroup>(x => {
          return {
            id: x[0].id?.toString() || '',
            person: x[0].person || {},
            roles: x
              .map(t => t.teamProfileType)
              .filter((z): z is Api_TypeCode<string> => z !== undefined)
              .flatMap(y => y.description || ''),
          };
        })
        .value();
      return (
        <ExpandableTextList<PersonRoleGroup>
          items={teamAsString ?? []}
          keyFunction={(item: PersonRoleGroup, index: number) =>
            `aquisition-team-${item.id}-person-${item.person.id ?? index}`
          }
          renderFunction={(item: PersonRoleGroup) => (
            <>{`${formatApiPersonNames(item.person)} (${item.roles.join(', ')})`}</>
          )}
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
        <AcquisitionProperties
          acquisitionProperties={props.row.original.fileProperties}
          maxDisplayCount={2}
        ></AcquisitionProperties>
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
