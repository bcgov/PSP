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
  GENERATE_LETTER = 'LETTER',
  H0074 = 'H0074',
  EXPROP_FORM_1 = 'FORM1',
  EXPROP_FORM_5 = 'FORM5',
  EXPROP_FORM_8 = 'FORM8',
  EXPROP_FORM_9 = 'FORM9',
}
