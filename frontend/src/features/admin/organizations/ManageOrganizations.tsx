import './ManageOrganizations.scss';

import { Table } from 'components/Table';
import * as actionTypes from 'constants/actionTypes';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IOrganization, IOrganizationFilter, IOrganizationRecord } from 'interfaces';
import { isEmpty } from 'lodash';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { useHistory } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import { useOrganizations } from 'store/slices/organizations';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { columnDefinitions } from '../constants';
import { OrganizationFilterBar } from './OrganizationFilterBar';

const ManageOrganizations: React.FC = () => {
  const columns = useMemo(() => columnDefinitions, []);
  const [filter, setFilter] = useState<IOrganizationFilter>({});
  const lookupCodes = useLookupCodeHelpers();
  const organizationLookupCodes = lookupCodes.getByType('Organization');
  const history = useHistory();

  const pagedOrganizations = useAppSelector(state => {
    return state.organizations.pagedOrganizations;
  });

  const pageSize = useAppSelector(state => {
    return state.organizations.rowsPerPage;
  });

  const pageIndex = useAppSelector(state => {
    return state.organizations.pageIndex;
  });

  const sort = useAppSelector(state => {
    return state.organizations.sort;
  });

  const organizations = useAppSelector(
    state => state.network[actionTypes.GET_ORGANIZATIONS] as IGenericNetworkAction,
  );

  const onRowClick = (row: IOrganizationRecord) => {
    history.push(`/admin/organization/${row.id}`);
  };

  let organizationList = pagedOrganizations.items.map(
    (a: IOrganization): IOrganizationRecord => ({
      name: a.name,
      parentId: a.parentId,
      id: a.id,
      parent: organizationLookupCodes.find(x => x.id === a.parentId)?.name,
    }),
  );

  const initialValues = useMemo(() => {
    const defaultValue: IOrganizationFilter = {
      id: undefined,
    };
    const values = { ...defaultValue, ...filter };
    if (typeof values.id === 'number') {
      const organization = organizationLookupCodes.find(x => Number(x.id) === values?.id) as any;
      if (organization) {
        values.id = organization;
      }
    }
    return values;
  }, [organizationLookupCodes, filter]);

  const { fetchOrganizations } = useOrganizations();

  const onRequestData = useCallback(
    ({ pageIndex }) => {
      fetchOrganizations(
        toFilteredApiPaginateParams<IOrganizationFilter>(
          filter?.id ? 0 : pageIndex,
          pageSize,
          sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
          filter,
        ),
      );
    },
    [fetchOrganizations, filter, pageSize, sort],
  );
  const { fetchLookupCodes } = useLookupCodes();

  useEffect(() => {
    fetchLookupCodes();
  }, [fetchLookupCodes]);

  return (
    <Container fluid className="ManageOrganizations">
      <Container fluid className="organization-toolbar">
        <OrganizationFilterBar
          value={{ ...initialValues }}
          onChange={value => {
            if ((value as any).id) {
              setFilter({ ...filter, id: Number((value as any).id) });
            } else {
              setFilter({ ...initialValues, id: undefined });
            }
          }}
          handleAdd={() => {
            history.push('/admin/organization/new');
          }}
        />
      </Container>
      <div className="table-section">
        <Table<IOrganizationRecord>
          name="organizationsTable"
          columns={columns}
          pageIndex={pageIndex}
          data={organizationList}
          onRowClick={onRowClick}
          onRequestData={onRequestData}
          pageSize={pageSize}
          pageCount={Math.ceil(pagedOrganizations.total / pageSize)}
          loading={!(organizations && !organizations.isFetching)}
          lockPageSize={true}
          clickableTooltip="View organization details"
        />
      </div>
    </Container>
  );
};

export default ManageOrganizations;
