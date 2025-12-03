import { Formik, FormikHelpers, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResetButton } from '@/components/common/buttons/ResetButton';
import { SearchButton } from '@/components/common/buttons/SearchButton';
import { Form, Input, Select } from '@/components/common/form';
import { SelectOption } from '@/components/common/form/Select';
import { SelectInput } from '@/components/common/List/SelectInput';
import { ColButtons } from '@/components/common/styles';
import * as API from '@/constants/API';
import { DocumentSearchFilterModel } from '@/features/documents/models/DocumentFilterModel';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_DocumentSearchFilter } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchFilter';

export interface IDocumentFilterProps {
  documentTypeOptions: SelectOption[];
  filter: ApiGen_Concepts_DocumentSearchFilter | null;
  setFilter: (filter: ApiGen_Concepts_DocumentSearchFilter) => void;
}

export const DocumentSearchFilter: React.FC<React.PropsWithChildren<IDocumentFilterProps>> = ({
  documentTypeOptions,
  filter,
  setFilter,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const documentStatusTypeOptions = getOptionsByType(API.DOCUMENT_STATUS_TYPES);

  const onSearchSubmit = async (
    values: DocumentSearchFilterModel,
    formikHelpers: FormikHelpers<DocumentSearchFilterModel>,
  ) => {
    setFilter(values.toApi());
    formikHelpers.setSubmitting(false);
  };

  const onResetClick = (formikProps: FormikProps<DocumentSearchFilterModel>) => {
    setFilter(new DocumentSearchFilterModel().toApi());
    formikProps.resetForm();
  };

  return (
    <Formik<DocumentSearchFilterModel>
      enableReinitialize
      initialValues={
        filter ? DocumentSearchFilterModel.fromApi(filter) : new DocumentSearchFilterModel()
      }
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
                  <Input field="documentName" placeholder="Document name" />
                </Col>
              </Row>
              <Row>
                <Col xl="12">
                  <SelectInput<
                    {
                      pin: string;
                      pid: string;
                      plan: string;
                    },
                    DocumentSearchFilterModel
                  >
                    field="searchBy"
                    defaultKey="pid"
                    selectOptions={[
                      { label: 'PID', key: 'pid', placeholder: 'Enter a PID' },
                      {
                        label: 'PIN',
                        key: 'pin',
                        placeholder: 'Enter a PIN',
                      },
                      { label: 'Plan #', key: 'plan', placeholder: 'Enter a Plan number' },
                    ]}
                    className="idir-input-group"
                  />
                </Col>
              </Row>
            </Col>
            <Col xl="5">
              <Row>
                <Col xl="7">
                  <Select
                    options={documentTypeOptions ?? []}
                    field="documentTypTypeCode"
                    placeholder="Select document type..."
                  />
                </Col>
                <Col xl="5">
                  <Select
                    field="documentStatusTypeCode"
                    data-testid="document-status"
                    placeholder="All statuses"
                    options={documentStatusTypeOptions ?? []}
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
                    onClick={() => {
                      formikProps.resetForm();
                      onResetClick(formikProps);
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

export default DocumentSearchFilter;

const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .idir-input-group {
    .input-group-prepend select {
      width: 16rem;
    }
    input {
      max-width: 100%;
    }
  }
`;
