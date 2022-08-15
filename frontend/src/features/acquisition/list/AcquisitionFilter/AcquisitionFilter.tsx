import { ResetButton, SearchButton } from 'components/common/buttons';
import { Form, Input, Select } from 'components/common/form';
import { ACQUISITION_FILE_STATUS_TYPES } from 'constants/API';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import { IAcquisitionFilter } from '../interfaces';

export interface IAcquisitionFilterProps {
  filter?: IAcquisitionFilter;
  setFilter: (filter: IAcquisitionFilter) => void;
}

export const defaultAcquisitionFilter: IAcquisitionFilter = {
  acquisitionFileStatusTypeCode: 'ACTIVE',
  acquisitionFileNameOrNumber: '',
  projectNameOrNumber: '',
};

/**
 * Filter bar for acquisition files.
 * @param {IAcquisitionFilterProps} props
 */
export const AcquisitionFilter: React.FC<IAcquisitionFilterProps> = ({ filter, setFilter }) => {
  const onSearchSubmit = (
    values: IAcquisitionFilter,
    formikHelpers: FormikHelpers<IAcquisitionFilter>,
  ) => {
    values = { ...values };
    setFilter(values);
    formikHelpers.setSubmitting(false);
  };

  const resetFilter = () => {
    setFilter(defaultAcquisitionFilter);
  };

  const onResetClick = (formikProps: FormikProps<IAcquisitionFilter>) => {
    formikProps.resetForm();
    resetFilter();
  };

  const lookupCodes = useLookupCodeHelpers();

  const acquisitionStatusOptions = lookupCodes
    .getByType(ACQUISITION_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  return (
    <Formik<IAcquisitionFilter>
      enableReinitialize
      initialValues={filter ?? defaultAcquisitionFilter}
      onSubmit={onSearchSubmit}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xl="1">
              <strong>Search by:</strong>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Row>
                    <Col xl="4">
                      <Select
                        options={acquisitionStatusOptions}
                        field="acquisitionFileStatusTypeCode"
                        placeholder="Any status"
                      />
                    </Col>
                    <Col xl="8"></Col>
                  </Row>
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="12">
                  <Input
                    field="acquisitionFileNameOrNumber"
                    placeholder="Acquisition file name or number"
                  />
                </Col>
              </Row>
              <Row>
                <Col xl="12">
                  <Input
                    field="projectNameOrNumber"
                    placeholder="Ministry project name or number"
                  />
                </Col>
              </Row>
            </Col>
            <ColButtons xl="1">
              <Row>
                <Col xl="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col xl="auto">
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => onResetClick(formikProps)}
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

export default AcquisitionFilter;

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

const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
`;
