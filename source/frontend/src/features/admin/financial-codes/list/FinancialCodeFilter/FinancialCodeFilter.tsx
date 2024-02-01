import { Formik, FormikHelpers } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select } from '@/components/common/form';
import ActiveFilterCheck from '@/components/common/form/ActiveFilterCheck';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

import { formatAsSelectOptions } from '../../financialCodeUtils';

export interface IFinancialCodeFilter {
  financialCodeType?: ApiGen_Concepts_FinancialCodeTypes;
  codeValueOrDescription: string;
  showExpiredCodes: boolean;
}

export interface IFinancialCodeFilterProps {
  filter?: IFinancialCodeFilter;
  setFilter: (filter: IFinancialCodeFilter) => void;
}

export const defaultFinancialCodeFilter: IFinancialCodeFilter = {
  financialCodeType: undefined,
  codeValueOrDescription: '',
  showExpiredCodes: false,
};

/**
 * Filter bar for financial codes.
 * @param {IFinancialCodeFilterProps} props
 */
export const FinancialCodeFilter: React.FC<IFinancialCodeFilterProps> = ({ filter, setFilter }) => {
  // map typescript enum to SELECT options
  const financialCodeTypeOptions = formatAsSelectOptions();

  return (
    <Formik<IFinancialCodeFilter>
      enableReinitialize
      initialValues={filter ?? defaultFinancialCodeFilter}
      onSubmit={(
        values: IFinancialCodeFilter,
        formikHelpers: FormikHelpers<IFinancialCodeFilter>,
      ) => {
        setFilter(values);
        formikHelpers.setSubmitting(false);
      }}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xs="auto">
              <strong>Search by:</strong>
            </Col>
            <Col xs="3">
              <Select
                options={financialCodeTypeOptions}
                field="financialCodeType"
                placeholder="All financial codes"
              />
            </Col>
            <Col xs="3">
              <Input field="codeValueOrDescription" placeholder="Code value or description" />
            </Col>
            <Col xs="auto">
              <ActiveFilterCheck<IFinancialCodeFilter>
                fieldName="showExpiredCodes"
                setFilter={setFilter}
              />
              <span>Show expired codes</span>
            </Col>
            <ColButtons xs="2">
              <Row>
                <Col xs="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col xs="auto">
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => {
                      formikProps.resetForm();
                      setFilter(defaultFinancialCodeFilter);
                    }}
                  />
                </Col>
              </Row>
            </ColButtons>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

export default FinancialCodeFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
`;
