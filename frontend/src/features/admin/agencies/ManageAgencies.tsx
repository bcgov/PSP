import './ManageAgencies.scss';
import React, { useMemo, useCallback, useState, useEffect } from 'react';
import { Container } from 'react-bootstrap';
import { Table } from 'components/Table';
import { columnDefinitions } from '../constants';
import { IAgency, IAgencyFilter, IAgencyRecord } from 'interfaces';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';
import * as actionTypes from 'constants/actionTypes';
import { generateMultiSortCriteria } from 'utils';
import { AgencyFilterBar } from './AgencyFilterBar';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useHistory } from 'react-router-dom';
import { isEmpty } from 'lodash';
import { useLookupCodes } from 'store/slices/lookupCodes';
import { useAppSelector } from 'store/hooks';
import { useAgencies } from 'store/slices/agencies';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';

const ManageAgencies: React.FC = () => {
  const columns = useMemo(() => columnDefinitions, []);
  const [filter, setFilter] = useState<IAgencyFilter>({});
  const lookupCodes = useLookupCodeHelpers();
  const agencyLookupCodes = lookupCodes.getByType('Agency');
  const history = useHistory();

  const pagedAgencies = useAppSelector(state => {
    return state.agencies.pagedAgencies;
  });

  const pageSize = useAppSelector(state => {
    return state.agencies.rowsPerPage;
  });

  const pageIndex = useAppSelector(state => {
    return state.agencies.pageIndex;
  });

  const sort = useAppSelector(state => {
    return state.agencies.sort;
  });

  const agencies = useAppSelector(
    state => state.network[actionTypes.GET_AGENCIES] as IGenericNetworkAction,
  );

  const onRowClick = (row: IAgencyRecord) => {
    history.push(`/admin/agency/${row.id}`);
  };

  let agencyList = pagedAgencies.items.map(
    (a: IAgency): IAgencyRecord => ({
      name: a.name,
      code: a.code,
      description: a.description,
      parentId: a.parentId,
      id: a.id,
      parent: agencyLookupCodes.find(x => x.id === a.parentId)?.name,
    }),
  );

  const initialValues = useMemo(() => {
    const defaultValue: IAgencyFilter = {
      id: undefined,
    };
    const values = { ...defaultValue, ...filter };
    if (typeof values.id === 'number') {
      const agency = agencyLookupCodes.find(x => Number(x.id) === values?.id) as any;
      if (agency) {
        values.id = agency;
      }
    }
    return values;
  }, [agencyLookupCodes, filter]);

  const { fetchAgencies } = useAgencies();

  const onRequestData = useCallback(
    ({ pageIndex }) => {
      fetchAgencies(
        toFilteredApiPaginateParams<IAgencyFilter>(
          filter?.id ? 0 : pageIndex,
          pageSize,
          sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
          filter,
        ),
      );
    },
    [fetchAgencies, filter, pageSize, sort],
  );
  const { fetchLookupCodes } = useLookupCodes();

  useEffect(() => {
    fetchLookupCodes();
  }, [fetchLookupCodes]);

  return (
    <Container fluid className="ManageAgencies">
      <Container fluid className="agency-toolbar">
        <AgencyFilterBar
          value={{ ...initialValues }}
          onChange={value => {
            if ((value as any).id) {
              setFilter({ ...filter, id: Number((value as any).id) });
            } else {
              setFilter({ ...initialValues, id: undefined });
            }
          }}
          handleAdd={() => {
            history.push('/admin/agency/new');
          }}
        />
      </Container>
      <div className="table-section">
        <Table<IAgencyRecord>
          name="agenciesTable"
          columns={columns}
          pageIndex={pageIndex}
          data={agencyList}
          onRowClick={onRowClick}
          onRequestData={onRequestData}
          pageSize={pageSize}
          pageCount={Math.ceil(pagedAgencies.total / pageSize)}
          loading={!(agencies && !agencies.isFetching)}
          lockPageSize={true}
          clickableTooltip="View agency details"
        />
      </div>
    </Container>
  );
};

export default ManageAgencies;
