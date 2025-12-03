import orderBy from 'lodash/orderBy';
import React from 'react';
import { FaExternalLinkAlt, FaInfoCircle } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import { matchPath, useLocation } from 'react-router-dom';
import { CellProps, Column } from 'react-table';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import ViewButton from '@/components/common/buttons/ViewButton';
import ExpandableTextList from '@/components/common/ExpandableTextList';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { ColumnWithProps, Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { PropertyManagementActivityStatusTypes } from '@/constants/propertyMgmtActivityStatusTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivitySubType } from '@/models/api/generated/ApiGen_Concepts_ManagementActivitySubType';
import { stringToFragment } from '@/utils/columnUtils';
import { prettyFormatDate } from '@/utils/dateUtils';
import { exists } from '@/utils/utils';

import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListProps {
  loading: boolean;
  propertyActivities: PropertyActivityRow[];
  columns: Column<PropertyActivityRow>[];
  sort: TableSort<ApiGen_Concepts_ManagementActivity>;
  setSort: (value: TableSort<ApiGen_Concepts_ManagementActivity>) => void;
  dataTestId?: string;
}

export function createActivityTableColumns() {
  const columns: ColumnWithProps<PropertyActivityRow>[] = [
    {
      Header: 'Activity type',
      accessor: 'activityType',
      align: 'left',
      sortable: true,
      minWidth: 35,
      maxWidth: 35,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(cellProps.row.original?.activityType?.description || '');
      },
    },
    {
      Header: 'Activity sub-type',
      accessor: 'activitySubTypes',
      align: 'left',
      sortable: true,
      minWidth: 35,
      maxWidth: 35,
      Cell: (props: CellProps<PropertyActivityRow>) => {
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
      Header: 'Commencement',
      accessor: 'requestedAddedDate',
      align: 'left',
      sortable: true,
      minWidth: 50,
      maxWidth: 50,
      Cell: (cellProps: CellProps<PropertyActivityRow>) => {
        return stringToFragment(prettyFormatDate(cellProps.row.original?.requestedAddedDate));
      },
    },
  ];

  return columns;
}

export function activityActionColumn(
  canEdit: boolean,
  onView: (activityId: number) => void,
  onDelete: (activityId: number) => void,
) {
  return {
    Header: 'Actions',
    align: 'center',
    sortable: false,
    width: 20,
    maxWidth: 20,
    Cell: (cellProps: CellProps<PropertyActivityRow>) => {
      const { hasClaim } = useKeycloakWrapper();
      const activityRow = cellProps.row.original;
      const renderDelete = () => {
        if (hasClaim(Claims.MANAGEMENT_DELETE) && canEdit && exists(onDelete)) {
          if (
            activityRow?.activityStatusType?.id === PropertyManagementActivityStatusTypes.NOTSTARTED
          ) {
            return (
              <RemoveIconButton
                id={`activity-delete-${cellProps.row.id}`}
                data-testId={`activity-delete-${activityRow.id}`}
                onRemove={() => activityRow.id && onDelete(activityRow.id)}
                title="Delete"
              />
            );
          } else {
            return (
              <TooltipWrapper
                tooltipId={`activity-delete-tooltip-${activityRow.id}`}
                tooltip="Only activity that is not started can be deleted"
              >
                <FaInfoCircle className="tooltip-icon h-24" size="2rem" />
              </TooltipWrapper>
            );
          }
        } else {
          return null;
        }
      };
      return (
        <StyledDiv>
          {hasClaim(Claims.MANAGEMENT_VIEW) && (
            <ViewButton
              data-testId={`activity-view-${activityRow.id}`}
              onClick={() => activityRow?.id && onView(activityRow.activityId)}
              id={`activity-view-${cellProps.row.id}`}
              title="property-activity view details"
            />
          )}
          {renderDelete()}
        </StyledDiv>
      );
    },
  };
}

export function activityNavigationColumn(
  getNavigationUrlTitle: (row: PropertyActivityRow) => { url: string; title: string },
) {
  return {
    Header: 'Navigation',
    align: 'center',
    sortable: false,
    width: 35,
    maxWidth: 35,
    Cell: (cellProps: CellProps<PropertyActivityRow>) => {
      const activityRow = cellProps.row.original;
      if (!exists(getNavigationUrlTitle)) {
        return null;
      }
      const urlAndTitle = getNavigationUrlTitle(activityRow);
      return (
        <StyledLink target="_blank" rel="noopener noreferrer" to={urlAndTitle.url}>
          <span>{urlAndTitle.title}</span>
          <FaExternalLinkAlt className="ml-2" size="1rem" />
        </StyledLink>
      );
    },
  };
}

const ManagementActivitiesList: React.FunctionComponent<IManagementActivitiesListProps> = ({
  loading,
  propertyActivities,
  columns,
  sort,
  setSort,
  dataTestId,
  ...rest
}) => {
  const location = useLocation();

  const isActiveRow = (entityId: number): boolean => {
    const matched = matchPath(location.pathname, {
      path: [`/mapview/sidebar/management/*/activities/${entityId}`],
      exact: true,
      strict: true,
    });

    return matched?.isExact ?? false;
  };

  return (
    <Table<PropertyActivityRow>
      loading={loading}
      name={dataTestId}
      manualSortBy={true}
      totalItems={propertyActivities.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={sortedActivities(propertyActivities, sort) ?? []}
      noRowsMessage="No property management activities found"
      pageSize={10}
      manualPagination={false}
      isRowActive={isActiveRow}
      {...rest}
    />
  );
};

const sortedActivities = (
  propertyActivities: PropertyActivityRow[],
  sort: TableSort<ApiGen_Concepts_ManagementActivity>,
) => {
  if (propertyActivities?.length > 0) {
    let items: PropertyActivityRow[] = [...propertyActivities];

    if (sort) {
      const sortFields = Object.keys(sort);
      if (sortFields?.length > 0) {
        const keyName = (sort as any)[sortFields[0]];
        return orderBy(items, mapSortField(sortFields[0]), keyName);
      } else {
        items = orderBy(items, ['activityType'], ['asc']);
      }
    }
    return items;
  }
  return [];
};

export const mapSortField = (sortField: string) => {
  if (sortField === 'activityType') {
    return 'activityType.description';
  } else if (sortField === 'activitySubType') {
    return 'activitySubType.description';
  } else if (sortField === 'activityStatusType') {
    return 'activityStatusType.description';
  }

  return sortField;
};

export default ManagementActivitiesList;

const StyledDiv = styled(InlineFlexDiv)`
  justify-content: center;
  > * {
    margin: 0.5rem;
    padding: 0 !important;
  }
`;

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
  svg {
    min-width: 2rem;
  }
`;

export const GroupWrapper = styled.div`
  display: flex;
  gap: 0.5rem;
  align-items: baseline;
  flex-wrap: wrap;
`;
