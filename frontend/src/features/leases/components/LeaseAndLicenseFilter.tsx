import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import * as Styled from 'components/common/form/styles';
import { PropertyFilterOptions } from 'features/properties/filter';
import { Formik } from 'formik';
import * as React from 'react';

import { ILeaseAndLicenseFilter } from '../interfaces';

interface ILeaseAndLicenseFilterProps {
  isSubmitting?: boolean;
  filter?: ILeaseAndLicenseFilter;
  setFilter: (filter: ILeaseAndLicenseFilter) => void;
  search: (filter: ILeaseAndLicenseFilter) => void;
}

/**
 * Filter bar for leases and license.
 * @param {ILeaseAndLicenseFilterProps} param0
 */
const LeaseAndLicenseFilter: React.FunctionComponent<ILeaseAndLicenseFilterProps> = ({
  isSubmitting,
  filter,
  setFilter,
  search,
}) => {
  const resetFilter = () => {
    setFilter(defaultFilter);
  };

  return (
    <Formik
      enableReinitialize
      initialValues={filter ?? defaultFilter}
      onSubmit={values => {
        setFilter(values);
      }}
    >
      {({ values }) => (
        <Styled.InlineForm>
          <b>Search for a Lease or License:</b>
          <PropertyFilterOptions options={options} placeholders={placeholders} />
          <Styled.InlineInput field="tenantName" label="Tenant Name" />
          <SearchButton disabled={isSubmitting} onClick={() => search(values)} />
          <ResetButton disabled={isSubmitting} onClick={resetFilter} />
        </Styled.InlineForm>
      )}
    </Formik>
  );
};

const defaultFilter: ILeaseAndLicenseFilter = {
  pidOrPin: '',
  lFileNo: '',
  searchBy: 'pidOrPin',
  tenantName: '',
};

const options = [
  {
    label: 'PID/PIN',
    value: 'pidOrPin',
  },
  {
    label: 'L-File #',
    value: 'lFileNo',
  },
];

const placeholders = {
  pid: 'Enter a PID or PIN',
  lFileNumber: 'Enter an LIS File Number',
};

export default LeaseAndLicenseFilter;
