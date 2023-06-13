import { Formik } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { PlusButton } from '@/components/common/buttons';
import { Select, SelectOption } from '@/components/common/form';

export interface IAddFormProps {
  onAddForm: (formTypeCode: string) => void;
  templateTypes: SelectOption[];
}

export interface IAddForm {
  formTypeCode: string;
}

export function AddForm(props: IAddFormProps) {
  const { onAddForm, templateTypes } = props;

  const addForm = (values: IAddForm) => {
    onAddForm(values.formTypeCode);
  };

  return (
    <Formik<IAddForm>
      enableReinitialize
      initialValues={{ formTypeCode: '' }}
      onSubmit={(values: IAddForm, { setSubmitting }) => {
        addForm(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <Row>
          <Col md={10}>
            <Select
              data-testid="add-form-type"
              field="formTypeCode"
              placeholder="Create new document from a template"
              options={templateTypes}
            />
          </Col>

          <Col md={2}>
            <PlusButton
              title="add form"
              toolText="Add form"
              toolId="add-form"
              onClick={() => {
                formikProps.submitForm();
              }}
              disabled={!formikProps.values.formTypeCode}
            ></PlusButton>
          </Col>
        </Row>
      )}
    </Formik>
  );
}
