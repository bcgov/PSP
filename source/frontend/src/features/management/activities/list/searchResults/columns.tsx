import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import ExpandableFileProperties from '@/components/common/List/ExpandableFileProperties';
import { ColumnWithProps } from '@/components/Table';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

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
          <StyledLink
            to={`/mapview/sidebar/management/${props.row.original.managementFileId}/activities/${props.row.original.id}`}
          >
            {props.row.original.description}
          </StyledLink>
        );
      } else {
        return (
          <StyledLink
            to={`/mapview/sidebar/property/${props.row.original.activivityProperty.propertyId}/management/activity/${props.row.original.id}`}
          >
            {props.row.original.description}
          </StyledLink>
        );
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
    accessor: 'activitySubType',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
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
