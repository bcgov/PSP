import { ResetButton, SearchButton } from 'components/common/buttons';
import { Form, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { defaultFormFilter, IFormFilter } from 'interfaces/IFormResults';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export interface IFormFilterProps {
  formFilter?: IFormFilter;
  onSetFilter: (filterValues: IFormFilter) => void;
}

export const FormFilter = (props: IFormFilterProps) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const formTypeOptions = getOptionsByType(API.FORM_TYPES);

  return (
    <Formik<IFormFilter>
      enableReinitialize
      initialValues={props.formFilter ?? defaultFormFilter}
      onSubmit={(values: IFormFilter, { setSubmitting }: any) => {
        props.onSetFilter(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row>
            <Col xs="auto">
              <label>Filter by:</label>
            </Col>
            <Col xs={8}>
              <Select
                data-testid="form-type"
                field="formTypeId"
                placeholder="All form types"
                options={formTypeOptions}
              />
            </Col>
            <Col xs={1}>
              <SearchButton disabled={formikProps.isSubmitting} />
            </Col>
            <Col xs={1}>
              <ResetButton
                disabled={formikProps.isSubmitting}
                onClick={() => {
                  formikProps.resetForm();
                  props.onSetFilter(defaultFormFilter);
                }}
              />
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;
