import { useApiLeases } from 'hooks/pims-api/useApiLeases';
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
      <h3>Leases and Licenses</h3>
      <LeaseAndLicenseFilter filter={filter} setFilter={setFilter} search={search} />
    </LeasePage>
  );
};

export const LeasePage = styled.div`
  text-align: left;
  padding: 55px 24px;
`;
