/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AddProjectYupSchema = Yup.object().shape({
  projectNumber: Yup.string().max(20, 'Project number must be at most ${max} characters'),
  projectName: Yup.string()
    .required('Name is required.')
    .max(200, 'Project name must be at most ${max} characters'),
  projectStatusType: Yup.string().required(),
  region: Yup.string().required('Region is required.'),
  summary: Yup.string().max(2000, 'Project summary must be at most ${max} characters'),
});
