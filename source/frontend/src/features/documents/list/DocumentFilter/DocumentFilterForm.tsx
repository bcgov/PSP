import { Formik } from 'formik';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select, SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { defaultDocumentFilter, IDocumentFilter } from '@/interfaces/IDocumentResults';
import { Api_DocumentType } from '@/models/api/Document';

export interface IDocumentFilterFormProps {
  documentFilter?: IDocumentFilter;
  documentTypes: Api_DocumentType[];
  onSetFilter: (filterValues: IDocumentFilter) => void;
}

export const DocumentFilterForm = (props: IDocumentFilterFormProps) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypeOptions = getOptionsByType(API.DOCUMENT_STATUS_TYPES);

  const typeOptions: SelectOption[] = props.documentTypes.map(dt => {
    return {
      label: dt.documentTypeDescription || '',
      value: dt.id ? dt.id.toString() : '',
    };
  });

  return (
    <Formik<IDocumentFilter>
      enableReinitialize
      initialValues={props.documentFilter ?? defaultDocumentFilter}
      onSubmit={(values: IDocumentFilter, { setSubmitting }: any) => {
        props.onSetFilter(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <FilterBoxForm className="p-3">
          <Row className="no-gutters">
            <Col lg="auto">
              <label>Filter by:</label>
            </Col>
            <Col className="px-3">
              <Row>
                <Col>
                  <Select
                    data-testid="document-type"
                    field="documentTypeId"
                    placeholder="All document types"
                    options={typeOptions}
                  />
                </Col>
                <Col>
                  <Select
                    field="status"
                    data-testid="document-status"
                    placeholder="All statuses"
                    options={documentStatusTypeOptions}
                  />
                </Col>
                <Col>
                  <Input field="filename" data-testid="document-filename" placeholder="File name" />
                </Col>
              </Row>
            </Col>
            <Col lg="auto">
              <ColButtons className="no-gutters pl-2">
                <Col>
                  <SearchButton
                    onClick={() => formikProps.handleSubmit()}
                    type="button"
                    disabled={formikProps.isSubmitting}
                  />
                </Col>
                <Col>
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => {
                      formikProps.resetForm();
                      props.onSetFilter(defaultDocumentFilter);
                    }}
                  />
                </Col>
              </ColButtons>
            </Col>
          </Row>
        </FilterBoxForm>
      )}
    </Formik>
  );
};

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem 0.5rem 0rem 0rem;
`;

const ColButtons = styled(Row)`
  border-left: 0.2rem solid white;
`;
