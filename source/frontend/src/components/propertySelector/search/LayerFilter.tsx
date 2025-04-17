import './LayerFilter.scss';

import { Formik, FormikProps } from 'formik';
import React, { useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form } from '@/components/common/form';
import { SelectInput } from '@/components/common/List/SelectInput';
import { CoordinateSearchForm } from '@/features/properties/filter/CoordinateSearch/CoordinateSearchForm';
import { DmsCoordinates } from '@/features/properties/filter/CoordinateSearch/models';
import { PropertyFilterValidationSchema } from '@/features/properties/filter/validation';
import { IResearchFilterProps } from '@/features/research/list/ResearchFilter/ResearchFilter';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';

import { ILayerSearchCriteria } from '../models';

export const defaultLayerFilter: ILayerSearchCriteria = {
  pid: '',
  pin: '',
  planNumber: '',
  legalDescription: '',
  address: '',
  coordinates: null,
  searchBy: 'pid',
};

export interface ILayerFilterProps {
  onSearch: (searchCriteria: ILayerSearchCriteria) => void;
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
  onSearch,
  filter,
  addressResults,
  onAddressChange,
  onAddressSelect,
  loading,
}) => {
  const formikRef = useRef<FormikProps<ILayerSearchCriteria>>(null);

  const onSearchSubmit = (values: ILayerSearchCriteria) => {
    onSearch(values);
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

  const getSpacing = (searchBy: string) => {
    let searchColSpacing = 6;
    if (searchBy === 'legalDescription') {
      searchColSpacing = 10;
    } else if (searchBy === 'coordinates') {
      searchColSpacing = 3;
    }
    return searchColSpacing;
  };

  return (
    <Formik
      enableReinitialize
      initialValues={internalFilter}
      onSubmit={onSearchSubmit}
      innerRef={formikRef}
      validationSchema={PropertyFilterValidationSchema}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row noGutters>
            <Col xs={getSpacing(formikProps.values.searchBy)}>
              <SelectInput<ILayerSearchCriteria, IResearchFilterProps>
                field="searchBy"
                defaultKey="pid"
                as={formikProps.values.searchBy === 'legalDescription' ? 'textarea' : 'input'}
                helpText={
                  formikProps.values.searchBy === 'legalDescription'
                    ? 'Searching by Legal Description may result in a slower search.'
                    : ''
                }
                onSelectItemChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                  if (e.target.value === 'coordinates') {
                    formikProps.setFieldValue('coordinates', new DmsCoordinates());
                  }
                }}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                  if (formikProps.values.searchBy === 'address' && onAddressChange) {
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
                  {
                    label: 'Lat/Long',
                    key: 'coordinates',
                    placeholder: `Enter a Latitude and Longitude`,
                    hideInput: true,
                  },
                ]}
                autoSetting="off"
              />
              {formikProps.values.searchBy === 'address' && renderAddressSuggestions()}
            </Col>
            {formikProps.values.searchBy === 'coordinates' && (
              <Col>
                <CoordinateSearchForm
                  field="coordinates"
                  innerClassName="flex-column"
                ></CoordinateSearchForm>
              </Col>
            )}
            <Col xs={2} className="pr-0">
              <Row>
                <Col className="pr-0">
                  <SearchButton disabled={loading} onClick={formikProps.submitForm} type="button" />
                </Col>
                <Col className="pl-0">
                  <ResetButton
                    disabled={loading}
                    onClick={() => {
                      formikProps.resetForm();
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
`;
