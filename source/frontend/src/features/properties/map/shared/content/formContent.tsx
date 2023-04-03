import { Input } from 'components/common/form';
import { useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';
import * as Yup from 'yup';

import { ActivityModel } from '../../activity/detail/models';
import { ActivityTemplateTypes, FormTemplateTypes, IFormContent } from './models';
export const formContent = new Map<ActivityTemplateTypes | FormTemplateTypes, IFormContent>([
  [
    ActivityTemplateTypes.SURVEY,
    {
      header: 'Survey',
      initialValues: { test: '' },
      EditForm: () => (
        <>
          <b>TEST SURVEY FORM</b>
          <Row>
            <Col>
              <Input field={withNameSpace('activityData', 'test')}></Input>
            </Col>
          </Row>
        </>
      ),
      ViewForm: () => {
        const { values } = useFormikContext<ActivityModel>();
        return (
          <>
            <b>TEST SURVEY FORM</b>
            <Row>
              <Col>{values.activityData.test}</Col>
            </Row>
          </>
        );
      },
      validationSchema: Yup.object().shape({
        activityData: Yup.object().shape({ test: Yup.string().required('test is required') }),
      }),
      version: '1.0',
    },
  ],
  [
    ActivityTemplateTypes.SITE_VISIT,
    {
      header: 'Site Visit',
      initialValues: {},
      EditForm: () => <>Site Visit</>,
      ViewForm: () => <>Site Visit</>,
      validationSchema: Yup.object().shape({}),
      version: '1.0',
    },
  ],
]);
