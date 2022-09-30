import { FormikErrors } from 'formik';
import * as Yup from 'yup';
import { AssertsShape, ObjectShape, TypeOfShape } from 'yup/lib/object';

import { ActivityModel } from './../models';

export interface IActivityFormContent {
  EditForm: React.FunctionComponent<IActivityFormContentProps>;
  ViewForm: React.FunctionComponent<IActivityFormContentProps>;
  validationSchema: Yup.ObjectSchema<
    ObjectShape,
    Record<string, any>,
    TypeOfShape<ObjectShape>,
    AssertsShape<ObjectShape>
  >;
  validationFunction?: (values: ActivityModel) => FormikErrors<ActivityModel>;
  version: string;
  initialValues: any;
  header?: string;
}

export interface IActivityFormContentProps {}

export enum ActivityTemplateTypes {
  GENERAL = 'GENERAL',
  SITE_VISIT = 'SITEVIS',
  SURVEY = 'SURVEY',
}
