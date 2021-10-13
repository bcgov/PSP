import { Scrollable } from 'components/common/Scrollable/Scrollable';
import { Table } from 'components/Table';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILease } from 'interfaces';
import { useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { ILeaseAndLicenseFilter } from '../interfaces';
import LeaseAndLicenseFilter from './LeaseAndLicenseFilter';

/**
 * Get an error message corresponding to what filter fields have been entered.
 * @param {ILeaseAndLicenseFilter} filter
 */
const getNoResultErrorMessage = (filter: ILeaseAndLicenseFilter) => {
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
export const LeaseAndLicenseListView = () => {
  const [filter, setFilter] = useState<ILeaseAndLicenseFilter | undefined>(undefined);
  const { getLeases } = useApiLeases();
  const search = async (searchFilter: ILeaseAndLicenseFilter) => {
    const { data } = await getLeases(searchFilter);
    if (!data.items?.length) {
      toast.warn(getNoResultErrorMessage(searchFilter));
    }
  };
  return (
    <LeasePage>
      <LeaseAndLicenseFilter filter={filter} setFilter={setFilter} search={search} />
      <Scrollable>
        <Panel>
          <h3>Leases and Licenses</h3>
          <Table<ILease> name="leasesTable" columns={[]} data={[]}></Table>
        </Panel>
      </Scrollable>
    </LeasePage>
  );
};

const LeasePage = styled.div`
  /* padding: 5.5rem 2.4rem; */
  text-align: left;
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  padding: 0;
`;

const Panel = styled.div`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export default LeaseAndLicenseListView;
