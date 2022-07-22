import { PlusButton } from 'components/common/buttons';
import { Select, SelectOption } from 'components/common/form';
import { Formik } from 'formik';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import useIsMounted from 'hooks/useIsMounted';
import { Api_Activity, Api_ActivityTemplate } from 'models/api/Activity';
import React, { useCallback, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

export interface IAddActivityFormProps {
  onAddActivity: (activity: Api_Activity) => void;
}

export interface IAddActivity {
  activityTypeId?: string;
}

export function AddActivityForm(props: IAddActivityFormProps) {
  const { onAddActivity } = props;
  const isMounted = useIsMounted();
  const { getActivityTemplates } = useApiResearchFile();
  const [templateTypes, setTemplateTypes] = useState<SelectOption[]>([]);

  const fetchActivityTemplates = useCallback(async () => {
    // const data = mockActivitiesResponse();
    // TODO Call actual api endpoint
    await getActivityTemplates().then(response => {
      let options = response.data.map(
        (template: Api_ActivityTemplate) =>
          ({
            value: template.id,
            label: template.activityTemplateTypeCode?.description,
            code: template.activityTemplateTypeCode?.id,
          } as SelectOption),
      );
      setTemplateTypes(options);
      if (options && isMounted()) {
        setTemplateTypes(options);
      }
    });
  }, [getActivityTemplates, isMounted]);

  React.useEffect(() => {
    fetchActivityTemplates();
  }, [fetchActivityTemplates]);

  const addActivity = (values: IAddActivity) => {
    console.log(values);
    const activity = { activityTemplateId: values.activityTypeId } as Api_Activity;
    onAddActivity(activity);
  };

  return (
    <Formik<IAddActivity>
      enableReinitialize
      initialValues={{ activityTypeId: '' }}
      onSubmit={(values: IAddActivity, { setSubmitting }: any) => {
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
