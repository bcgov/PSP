import { Formik } from 'formik';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select, SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { defaultDocumentFilter, IDocumentFilter } from '@/interfaces/IDocumentResults';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { capitalizeFirstLetter, relationshipTypeToPathName } from '@/utils';

export interface IDocumentFilterFormProps {
  documentFilter?: IDocumentFilter;
  documentTypes: ApiGen_Concepts_DocumentType[];
  relationshipTypes: ApiGen_CodeTypes_DocumentRelationType[];
  showParentFilter: boolean;
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

  const relationshipTypeOptions: SelectOption[] =
    props.relationshipTypes?.map(ctr => {
      return {
        label: capitalizeFirstLetter(relationshipTypeToPathName(ctr)) || '',
        value: ctr || '',
      };
    }) ?? [];

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
            <Col xs="1">
              <label>Filter by:</label>
            </Col>
            <Col>
              <Row className="pl-1">
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
              <Row className="no-gutters pl-2">
                <Col className="pr-1">
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
              </Row>
            </Col>
          </Row>
          {props.showParentFilter && (
            <Row>
              <Col xs="1">Parent:</Col>
              <Col xs="5">
                <Input
                  field="parentName"
                  data-testid="document-parentname"
                  placeholder="Parent name"
                />
              </Col>
              <Col xs="3">
                <Select
                  field="parentType"
                  data-testid="document-parenttype"
                  placeholder="All Relationships"
                  options={relationshipTypeOptions}
                />
              </Col>
            </Row>
          )}
        </FilterBoxForm>
      )}
    </Formik>
  );
};

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem 0.5rem 0rem 0rem;
`;
