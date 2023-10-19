import React from 'react';
import { Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import { CellProps } from 'react-table';

import { StyledRemoveIconButton } from '@/components/common/buttons/RemoveButton';
import { ColumnWithProps, Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { PropertyManagementActivityStatusTypes } from '@/constants/propertyMgmtActivityStatusTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_PropertyManagementActivity } from '@/models/api/Property';
import { stringToFragment } from '@/utils/columnUtils';
import { prettyFormatDate } from '@/utils/dateUtils';

import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListProps {
  propertyActivities: PropertyActivityRow[];
  handleDelete: (managementActivityId: number) => void;
  sort: TableSort<Api_PropertyManagementActivity>;
  setSort: (value: TableSort<Api_PropertyManagementActivity>) => void;
}

export function createTableColumns(onDelete: (managementActivityId: number) => void) {
  const columns: ColumnWithProps<PropertyActivityRow>[] = [
    {
      Header: 'Activity type',
      accessor: 'activityType',
      align: 'left',
      sortable: true,
      minWidth: 45,
      maxWidth: 45,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(cellProps.row.original?.activityType?.description || '');
      },
    },
    {
      Header: 'Activity sub-type',
      accessor: 'activitySubType',
      align: 'left',
      sortable: true,
      minWidth: 45,
      maxWidth: 45,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(cellProps.row.original?.activitySubType?.description || '');
      },
    },
    {
      Header: 'Activity status',
      accessor: 'activityStatusType',
      align: 'left',
      sortable: true,
      minWidth: 35,
      maxWidth: 35,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(cellProps.row.original?.activityStatusType?.description || '');
      },
    },
    {
      Header: 'Request added date',
      accessor: 'requestedAddedDate',
      align: 'left',
      sortable: true,
      minWidth: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(prettyFormatDate(cellProps.row.original?.requestedAddedDate));
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 15,
      maxWidth: 15,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <Row className="no-gutters">
            {hasClaim(Claims.MANAGEMENT_DELETE) &&
            cellProps.row.original?.activityStatusType?.id ===
              PropertyManagementActivityStatusTypes.NOTSTARTED ? (
              <StyledRemoveIconButton
                id={`activity-delete-${cellProps.row.id}`}
                data-testid={`activity-delete-${cellProps.row.original.id}`}
                onClick={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                title="Delete"
              >
                <FaTrash size="2rem" />
              </StyledRemoveIconButton>
            ) : null}
          </Row>
        );
      },
    },
  ];

  return columns;
}

const ManagementActivitiesList: React.FunctionComponent<IManagementActivitiesListProps> = ({
  propertyActivities,
  handleDelete,
  sort,
  setSort,
  ...rest
}) => {
  return (
    <Table<PropertyActivityRow>
      name="PropertyManagementActivitiesTable"
      manualSortBy={false}
      totalItems={propertyActivities.length}
      columns={createTableColumns(handleDelete)}
      externalSort={{ sort, setSort }}
      data={propertyActivities ?? []}
      noRowsMessage="No property management activities found"
      pageSize={10}
      manualPagination={false}
      {...rest}
    />
  );
};

export default ManagementActivitiesList;
