import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { ILeaseAndLicenseFilter } from '../interfaces';
import LeaseAndLicenseFilter from './LeaseAndLicenseFilter';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const LeaseAndLicenseListView = () => {
  const [filter, setFilter] = useState<ILeaseAndLicenseFilter | undefined>(undefined);
  const { getLeases } = useApiLeases();
  const search = async (filter: ILeaseAndLicenseFilter) => {
    const { data } = await getLeases(filter);
    if (!data.items?.length) {
      toast.warn('There are no records for your search criteria.');
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
