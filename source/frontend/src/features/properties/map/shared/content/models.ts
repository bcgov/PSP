import { FormikErrors } from 'formik';
import * as Yup from 'yup';
import { AssertsShape, ObjectShape, TypeOfShape } from 'yup/lib/object';

export interface IFormContent {
  EditForm: React.FunctionComponent<React.PropsWithChildren<IFormContentProps>>;
  ViewForm: React.FunctionComponent<React.PropsWithChildren<IFormContentProps>>;
  validationSchema: Yup.ObjectSchema<
    ObjectShape,
    Record<string, any>,
    TypeOfShape<ObjectShape>,
    AssertsShape<ObjectShape>
  >;
  validationFunction?: (values: any) => FormikErrors<any>;
  version: string;
  initialValues: any;
  header?: string;
}

export interface IFormContentProps {}

export enum FormTemplateTypes {
  H120 = 'H120',
  H179A = 'H179A',
  H179P = 'H179P',
  H179T = 'H179T',
}

export enum ActivityTemplateTypes {
  GENERAL = 'GENERAL',
  SITE_VISIT = 'SITEVIS',
  SURVEY = 'SURVEY',
}
