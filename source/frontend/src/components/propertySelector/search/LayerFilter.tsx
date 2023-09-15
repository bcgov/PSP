import './LayerFilter.scss';

import { Formik, FormikProps } from 'formik';
import React, { useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { IResearchFilterProps } from '@/features/research/list/ResearchFilter/ResearchFilter';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';

import { ILayerSearchCriteria } from '../models';

export const defaultLayerFilter: ILayerSearchCriteria = {
  pid: '',
  pin: '',
  planNumber: '',
  legalDescription: '',
  searchBy: 'pid',
};

export interface ILayerFilterProps {
  setFilter: (searchCriteria: ILayerSearchCriteria) => void;
  filter?: ILayerSearchCriteria;
  addressResults?: IGeocoderResponse[];
  onAddressChange: (searchText: string) => void;
  onAddressSelect: (selectedItem: IGeocoderResponse) => void;
  loading: boolean;
}

/**
 * Filter bar for research files.
 * @param {IResearchFilterProps} props
 */
export const LayerFilter: React.FunctionComponent<React.PropsWithChildren<ILayerFilterProps>> = ({
  setFilter,
  filter,
  addressResults,
  onAddressChange,
  onAddressSelect,
  loading,
}) => {
  const formikRef = useRef<FormikProps<ILayerSearchCriteria>>(null);

  const onSearchSubmit = (values: ILayerSearchCriteria) => {
    setFilter(values);
  };

  const resetFilter = () => {
    setFilter(defaultLayerFilter);
  };

  const onSuggestionSelected = (val: IGeocoderResponse) => {
    onAddressSelect(val);
  };

  const renderAddressSuggestions = () => {
    if (!addressResults || addressResults.length === 0) {
      return null;
    }
    return (
      <div className="suggestionList">
        {addressResults?.map((geoResponse: IGeocoderResponse, index: number) => (
          <option
            key={index}
            onClick={() => {
              formikRef.current?.setFieldValue('address', geoResponse.fullAddress);
              onSuggestionSelected(geoResponse);
            }}
          >
            {geoResponse.fullAddress}
          </option>
        ))}
      </div>
    );
  };

  const internalFilter = filter ?? { ...defaultLayerFilter };
  const isSearchByAddress = internalFilter?.searchBy === 'address';
  const isSearchByLegalDescription = internalFilter?.searchBy === 'legalDescription';

  return (
    <Formik
      enableReinitialize
      initialValues={internalFilter}
      onSubmit={onSearchSubmit}
      innerRef={formikRef}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl={isSearchByLegalDescription ? 10 : 6}>
              <SelectInput<ILayerSearchCriteria, IResearchFilterProps>
                field="searchBy"
                defaultKey="pid"
                as={isSearchByLegalDescription ? 'textarea' : 'input'}
                helpText={
                  isSearchByLegalDescription
                    ? 'Searching by Legal Description may result in a slower search.'
                    : ''
                }
                onSelectItemChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                  setFilter({ ...internalFilter, searchBy: e.target.value });
                }}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                  if (isSearchByAddress && onAddressChange) {
                    onAddressChange(e.target.value);
                  }
                }}
                selectOptions={[
                  {
                    key: 'pid',
                    placeholder: `Enter a PID`,
                    label: 'PID',
                  },
                  {
                    key: 'pin',
                    placeholder: `Enter a PIN`,
                    label: 'PIN',
                  },
                  {
                    key: 'address',
                    placeholder: `Enter a Address`,
                    label: 'Address',
                  },
                  {
                    key: 'planNumber',
                    placeholder: `Enter a Plan #`,
                    label: 'Plan #',
                  },
                  {
                    key: 'legalDescription',
                    placeholder: '',
                    label: 'Legal Description',
                  },
                ]}
                autoSetting="off"
              />
              {isSearchByAddress && renderAddressSuggestions()}
            </Col>
            <Col xl={2} className="pr-0">
              <Row>
                <Col className="pr-0">
                  <SearchButton disabled={loading} onClick={formikProps.submitForm} type="button" />
                </Col>
                <Col className="pl-0">
                  <ResetButton
                    disabled={loading}
                    onClick={() => {
                      formikProps.resetForm();
                      resetFilter();
                    }}
                  />
                </Col>
              </Row>
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

export default LayerFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .idir-input-group {
    .input-group-prepend select {
      width: 16rem;
    }
    input {
      width: 18rem;
      max-width: 100%;
    }
  }
`;
