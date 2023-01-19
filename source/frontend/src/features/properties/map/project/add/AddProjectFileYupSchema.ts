/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AddProjectYupSchema = Yup.object().shape({
  projectNumber: Yup.string().max(100, 'Project number must be at most ${max} characters'),
  projectName: Yup.string().max(200, 'Project name must be at most ${max} characters'),
  projectStatusType: Yup.string(),
  region: Yup.string().required(),
  summary: Yup.string().max(2000, 'Project  must be at most ${max} characters'),
});
