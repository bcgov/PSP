import { ResetButton, SearchButton } from 'components/common/buttons';
import { Form, Input, Select, SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { Formik } from 'formik';
import { useApiDocuments } from 'hooks/pims-api/useApiDocuments';
import useIsMounted from 'hooks/useIsMounted';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { defaultDocumentFilter, IDocumentFilter } from 'interfaces/IDocumentResults';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export interface IDocumentFilterFormProps {
  documentFilter?: IDocumentFilter;
  onSetFilter: (filterValues: IDocumentFilter) => void;
}

export const DocumentFilterForm = (props: IDocumentFilterFormProps) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypeOptions = getOptionsByType(API.DOCUMENT_STATUS_TYPES);
  const isMounted = useIsMounted();
  const { getDocumentTypes } = useApiDocuments();
  const [documentTypes, setDocumentTypes] = React.useState<SelectOption[]>([]);

  React.useEffect(() => {
    getDocumentTypes().then(({ data }) => {
      if (data && isMounted()) {
        const options: SelectOption[] = data.map(dt => {
          return {
            label: dt.documentType || '',
            value: dt.id ? dt.id.toString() : '',
          };
        });
        setDocumentTypes(options);
      }
    });
  }, [isMounted, getDocumentTypes]);

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
          <Row>
            <Col lg="auto">
              <label>Filter by:</label>
            </Col>
            <Col lg={3}>
              <Select
                data-testid="document-type"
                field="documentTypeId"
                placeholder="All document types"
                options={documentTypes}
              />
            </Col>
            <Col lg={3}>
              <Select
                field="status"
                data-testid="document-status"
                placeholder="All statuses"
                options={documentStatusTypeOptions}
              />
            </Col>
            <Col lg={3}>
              <Input field="filename" data-testid="document-filename" placeholder="File name" />
            </Col>
            <ColButtons xl="auto">
              <Row>
                <Col xs="auto" className="pr-0">
                  <SearchButton disabled={formikProps.isSubmitting} />
                </Col>
                <Col xs="auto">
                  <ResetButton
                    disabled={formikProps.isSubmitting}
                    onClick={() => {
                      formikProps.resetForm();
                      props.onSetFilter(defaultDocumentFilter);
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

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
`;
