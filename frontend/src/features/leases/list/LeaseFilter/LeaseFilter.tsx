import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import { LEASE_PROGRAM_TYPES } from 'constants/API';
import { PropertyFilterOptions } from 'features/properties/filter';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import Multiselect from 'multiselect-react-dropdown';
import React from 'react';
import { FaTimes } from 'react-icons/fa';

import { ILeaseFilter } from '../../interfaces';
import * as Styled from '../styles';

export interface ILeaseFilterProps {
  filter?: ILeaseFilter;
  setFilter: (filter: ILeaseFilter) => void;
}

interface MultiSelectOption {
  id: string;
  text: string;
}

export const defaultFilter: ILeaseFilter = {
  pidOrPin: '',
  lFileNo: '',
  searchBy: 'lFileNo',
  tenantName: '',
  programs: [],
};

const idFilterOptions = [
  {
    label: 'PID/PIN',
    value: 'pidOrPin',
  },
  {
    label: 'L-File #',
    value: 'lFileNo',
  },
];

const idFilterPlaceholders = {
  pidOrPin: 'Enter a PID or PIN',
  lFileNo: 'Enter an LIS File Number',
};

/**
 * Filter bar for leases and license.
 * @param {ILeaseFilterProps} props
 */
export const LeaseFilter: React.FunctionComponent<ILeaseFilterProps> = ({ filter, setFilter }) => {
  const onSearchSubmit = (values: ILeaseFilter, { setSubmitting }: any) => {
    let selectedPrograms: MultiSelectOption[] = multiselectRef.current?.getSelectedItems();
    let programIds = selectedPrograms.map<string>(x => x.id);
    values = { ...values, programs: programIds };
    setFilter(values);
    setSubmitting(false);
  };
  const resetFilter = () => {
    multiselectRef.current?.resetSelectedValues();
    setFilter(defaultFilter);
  };

  const multiselectRef = React.createRef<Multiselect>();

  const lookupCodes = useLookupCodeHelpers();
  const leaseProgramTypes = lookupCodes.getByType(LEASE_PROGRAM_TYPES);
  const programFilterOptions: MultiSelectOption[] = leaseProgramTypes.map<MultiSelectOption>(x => {
    return { id: x.id as string, text: x.name };
  });

  return (
    <Formik enableReinitialize initialValues={filter ?? defaultFilter} onSubmit={onSearchSubmit}>
      {({ resetForm, isSubmitting }) => (
        <Styled.FilterBox>
          <b className="pr-3 text-nowrap">Search by:</b>
          <PropertyFilterOptions options={idFilterOptions} placeholders={idFilterPlaceholders} />
          <Styled.LongInlineInput field="tenantName" label="Tenant Name" className="ml-3 mr-5" />

          <span>Program</span>
          <Multiselect
            id="properties-selector"
            ref={multiselectRef}
            options={programFilterOptions}
            displayValue="text"
            placeholder="Select Program(s)"
            customCloseIcon={<FaTimes size="18px" className="ml-3" />}
            hidePlaceholder={true}
            style={{
              chips: {
                background: '#F2F2F2',
                borderRadius: '4px',
                color: 'black',
                fontSize: '16px',
                marginRight: '1em',
              },
              multiselectContainer: {
                width: 'auto',
                color: 'black',
                flex: '3 1 auto',
                maxWidth: '32em',
              },
              searchBox: {
                background: 'white',
                border: '1px solid #606060',
              },
            }}
          />

          <SearchButton disabled={isSubmitting} className="ml-5" />
          <ResetButton
            disabled={isSubmitting}
            onClick={() => {
              resetForm();
              resetFilter();
            }}
          />
        </Styled.FilterBox>
      )}
    </Formik>
  );
};

export default LeaseFilter;
