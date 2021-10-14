import { Center } from 'components/common/Center/Center';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import { Table } from 'components/Table';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { ILeaseFilter } from '../interfaces';
import LeaseFilter from './components/LeaseFilter';

/**
 * Get an error message corresponding to what filter fields have been entered.
 * @param {ILeaseFilter} filter
 */
const getNoResultErrorMessage = (filter: ILeaseFilter) => {
  let message = 'Unable to find any records';
  if (filter.lFileNo) {
    message = 'There is no record for this L-File #';
  } else if (filter.pidOrPin) {
    message = 'There is no record for this PID/ PIN';
  } else if (filter.tenantName) {
    message = 'There is no record for this Tenant Name';
  }
  return message;
};

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const LeaseListView = () => {
  const [filter, setFilter] = useState<ILeaseFilter | undefined>(undefined);
  const { getLeases } = useApiLeases();

  useEffect(() => {
    const search = async (searchFilter: ILeaseFilter) => {
      const { data } = await getLeases(searchFilter);
      if (!data.items?.length) {
        toast.warn(getNoResultErrorMessage(searchFilter));
      }
    };

    if (filter) {
      search(filter);
    }
  }, [filter, getLeases]);

  return (
    <StyledListPage>
      <Center>
        <LeaseFilter filter={filter} setFilter={setFilter} />
      </Center>
      <StyledScrollable>
        <h3>Leases &amp; Licenses</h3>
        <Table<ILease> name="leasesTable" columns={[]} data={[]}></Table>
      </StyledScrollable>
    </StyledListPage>
  );
};

const StyledListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

const StyledScrollable = styled(Scrollable)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export default LeaseListView;
