import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import * as Styled from 'components/common/form/styles';
import { PropertyFilterOptions } from 'features/properties/filter';
import { Formik } from 'formik';
import React from 'react';

import { ILeaseFilter } from '../../interfaces';

interface ILeaseFilterProps {
  filter?: ILeaseFilter;
  setFilter: (filter: ILeaseFilter) => void;
}

/**
 * Filter bar for leases and license.
 * @param {ILeaseFilterProps} param0
 */
export const LeaseFilter: React.FunctionComponent<ILeaseFilterProps> = ({ filter, setFilter }) => {
  const resetFilter = () => {
    setFilter(defaultFilter);
  };

  return (
    <Formik
      enableReinitialize
      initialValues={filter ?? defaultFilter}
      onSubmit={(values, { setSubmitting }) => {
        // TODO: remove me
        console.log(`[Lease-Filter] Submitting...`);
        setFilter(values);
        setSubmitting(false);
      }}
    >
      {({ values, resetForm, isSubmitting }) => (
        <Styled.InlineForm>
          <b>Search for a Lease or License:</b>
          <PropertyFilterOptions options={options} placeholders={placeholders} />
          <Styled.InlineInput field="tenantName" label="Tenant Name" />
          <SearchButton disabled={isSubmitting} />
          <ResetButton
            disabled={isSubmitting}
            onClick={() => {
              resetForm();
              resetFilter();
            }}
          />
        </Styled.InlineForm>
      )}
    </Formik>
  );
};

export const defaultFilter: ILeaseFilter = {
  pidOrPin: '',
  lFileNo: '',
  searchBy: 'lFileNo',
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

export default LeaseFilter;
