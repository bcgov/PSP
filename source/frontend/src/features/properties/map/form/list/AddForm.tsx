import { PlusButton } from 'components/common/buttons';
import { Select, SelectOption } from 'components/common/form';
import { Formik } from 'formik';
import { Col, Row } from 'react-bootstrap';

export interface IAddFormProps {
  onAddForm: (formTypeId: string) => void;
  templateTypes: SelectOption[];
}

export interface IAddForm {
  formTypeId: string;
}

export function AddForm(props: IAddFormProps) {
  const { onAddForm, templateTypes } = props;

  const addForm = (values: IAddForm) => {
    onAddForm(values.formTypeId);
  };

  return (
    <Formik<IAddForm>
      enableReinitialize
      initialValues={{ formTypeId: '' }}
      onSubmit={(values: IAddForm, { setSubmitting }: any) => {
        addForm(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <Row>
          <Col md={10}>
            <Select
              data-testid="add-form-type"
              field="formTypeId"
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
              disabled={!formikProps.values.formTypeId}
            ></PlusButton>
          </Col>
        </Row>
      )}
    </Formik>
  );
}
