import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { ExternalLink } from '@/components/common/ExternalLink';
import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementActivitySubType } from '@/models/api/generated/ApiGen_Concepts_ManagementActivitySubType';
import { stringToFragment } from '@/utils';

import { ManagementActivitySearchResultModel } from '../../models/ManagementActivitySearchResultModel';

export const columns: ColumnWithProps<ManagementActivitySearchResultModel>[] = [
  {
    Header: 'Description',
    accessor: 'description',
    align: 'center',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
    Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();

      if (hasClaim(Claims.MANAGEMENT_VIEW) && props.row.original.managementFileId) {
        return (
          <ExternalLink
            to={`/mapview/sidebar/management/${props.row.original.managementFileId}/activities/${props.row.original.id}`}
          >
            {props.row.original.description}
          </ExternalLink>
        );
      } else if (props.row.original.activivityProperty?.propertyId) {
        return (
          <StyledLink
            to={`/mapview/sidebar/property/${props.row.original.activivityProperty.propertyId}/management/activity/${props.row.original.id}`}
            target="_blank"
            rel="noopener noreferrer"
            style={{ display: 'flex', alignItems: 'center', gap: '8px' }}
          >
            {props.row.original.description}
            <FaExternalLinkAlt />
          </StyledLink>
        );
      } else {
        return stringToFragment(props.row.original.description);
      }
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
    Header: 'Civic Address / PID / PIN',
    accessor: 'properties',
    align: 'left',
    maxWidth: 40,
    Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
      return (
        <ExpandableFileProperties
          properties={props.row.original.properties}
          maxDisplayCount={2}
        ></ExpandableFileProperties>
      );
    },
  },
  {
    Header: 'Type',
    accessor: 'activityType',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Sub-type',
    accessor: 'activitySubTypes',
    align: 'left',
    clickable: true,
    sortable: false,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ManagementActivitySearchResultModel>) => {
      return (
        <GroupWrapper>
          <ExpandableTextList<ApiGen_Concepts_ManagementActivitySubType>
            items={props.row.original.activitySubTypes}
            keyFunction={p => `activity-subtype-${p.id}`}
            renderFunction={p => <>{p.managementActivitySubtypeCode.description}</>}
            delimiter={'; '}
            maxCollapsedLength={3}
            maxExpandedLength={10}
            className="d-flex flex-wrap"
          />
        </GroupWrapper>
      );
    },
  },
  {
    Header: 'Status',
    accessor: 'activityStatus',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
];

const StyledLink = styled(Link)`
  text-overflow: ellipsis;
  overflow: hidden;
  display: -webkit-box;
  line-clamp: 1;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
`;

export const GroupWrapper = styled.div`
  display: flex;
  gap: 0.5rem;
  align-items: baseline;
  flex-wrap: wrap;
`;
