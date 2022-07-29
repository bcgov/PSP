import { PlusButton } from 'components/common/buttons';
import { Select, SelectOption } from 'components/common/form';
import { Formik } from 'formik';
import { Col, Row } from 'react-bootstrap';

export interface IAddActivityFormProps {
  onAddActivity: (activityTypeId: number) => void;
  templateTypes: SelectOption[];
}

export interface IAddActivityForm {
  activityTypeId: number;
}

export function AddActivityForm(props: IAddActivityFormProps) {
  const { onAddActivity, templateTypes } = props;

  const addActivity = (values: IAddActivityForm) => {
    onAddActivity(values.activityTypeId);
  };

  return (
    <Formik<IAddActivityForm>
      enableReinitialize
      initialValues={{ activityTypeId: 0 }}
      onSubmit={(values: IAddActivityForm, { setSubmitting }: any) => {
        addActivity(values);
        setSubmitting(false);
      }}
    >
      {formikProps => (
        <Row>
          <Col md={4}>
            <Select
              data-testid="add-activity-type"
              field="activityTypeId"
              placeholder="Add activity"
              options={templateTypes}
            />
          </Col>

          <Col md={4}>
            <PlusButton
              toolText="Add Activity"
              toolId="add-activity"
              onClick={() => {
                formikProps.submitForm();
              }}
              disabled={!formikProps.values.activityTypeId}
            ></PlusButton>
          </Col>
        </Row>
      )}
    </Formik>
  );
}
