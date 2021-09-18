import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import * as Styled from 'components/common/form/styles';
import { FlexDiv } from 'components/common/styles';
import { Formik } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

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
      {formikProps => (
        <StyledFilter>
          <Styled.StackedInlineForm>
            <Styled.StackedPropertyFilterOptions
              options={options}
              placeholders={placeholders}
              label="Search for a Lease or License:"
            />
            <Styled.StackedInput field="tenantName" label="Tenant Name" />
            <Styled.StackedInput field="address" label="Civic Address" />
            <Styled.StackedInput field="municipality" label="Municipality" />
            <Styled.StackedDate field="expiryDate" label="Expiry Date" formikProps={formikProps} />
          </Styled.StackedInlineForm>
          <StyledButtonHolder>
            <SearchButton disabled={isSubmitting} onClick={() => search(formikProps.values)} />
            <ResetButton
              disabled={isSubmitting}
              onClick={() => {
                formikProps.resetForm();
                resetFilter();
              }}
            />
          </StyledButtonHolder>
        </StyledFilter>
      )}
    </Formik>
  );
};

export const defaultFilter: ILeaseAndLicenseFilter = {
  pidOrPin: '',
  lFileNo: '',
  searchBy: 'pidOrPin',
  tenantName: '',
  address: '',
  expiryDate: '',
  municipality: '',
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

const StyledFilter = styled(FlexDiv)`
  .form-control {
    background-image: none;
    padding-right: 0.75rem;
  }
`;

const StyledButtonHolder = styled(FlexDiv)`
  margin-left: 0.5rem;
  align-items: flex-end;
  column-gap: 0.5rem;
  .btn {
    min-height: 40px;
    max-height: 40px;
  }
`;

export default LeaseAndLicenseFilter;
