import axios from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React, { useEffect, useMemo, useState } from 'react';
import { toast } from 'react-toastify';

import { FinancialCodeTypes } from '@/constants';
import * as API from '@/constants/API';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { Api_Project } from '@/models/api/Project';
import { isExpiredCode, toDropDownOptions } from '@/utils/financialCodeUtils';

import { AddProjectYupSchema } from '../../../add/AddProjectFileYupSchema';
import { IAddProjectFormProps } from '../../../add/AddProjectForm';
import { ProjectForm } from '../../../models';

export interface IUpdateProjectContainerProps {
  project: Api_Project;
  View: React.ForwardRefExoticComponent<
    IAddProjectFormProps & React.RefAttributes<FormikProps<ProjectForm>>
  >;
  onSuccess: () => void;
}

const UpdateProjectContainer = React.forwardRef<
  FormikProps<ProjectForm>,
  IUpdateProjectContainerProps
>((props, formikRef) => {
  const { project, View, onSuccess } = props;
  const { costTypeCode, businessFunctionCode, workActivityCode } = project;

  const {
    getFinancialCodesByType: { execute: getFinancialCodes },
  } = useFinancialCodeRepository();

  const [businessFunctions, setBusinessFunctions] = useState<Api_FinancialCode[]>([]);
  const [costTypes, setCostTypes] = useState<Api_FinancialCode[]>([]);
  const [workActivities, setWorkActivities] = useState<Api_FinancialCode[]>([]);

  useEffect(() => {
    async function fetchBusinessFunctions() {
      const data = (await getFinancialCodes(FinancialCodeTypes.BusinessFunction)) ?? [];
      setBusinessFunctions(data);
    }
    fetchBusinessFunctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    async function fetchCostTypes() {
      const data = (await getFinancialCodes(FinancialCodeTypes.CostType)) ?? [];
      setCostTypes(data);
    }
    fetchCostTypes();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    async function fetchWorkActivities() {
      const data = (await getFinancialCodes(FinancialCodeTypes.WorkActivity)) ?? [];
      setWorkActivities(data);
    }
    fetchWorkActivities();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const businessFunctionOptions = useMemo(() => {
    const options = toDropDownOptions(businessFunctions);
    if (businessFunctionCode !== null && isExpiredCode(businessFunctionCode)) {
      options.push({
        label: businessFunctionCode.description!,
        value: businessFunctionCode.id!,
      });
    }
    return options;
  }, [businessFunctions, businessFunctionCode]);

  const costTypeOptions = useMemo(() => {
    const options = toDropDownOptions(costTypes);
    if (costTypeCode !== null && isExpiredCode(costTypeCode)) {
      options.push({
        label: costTypeCode.description!,
        value: costTypeCode.id!,
      });
    }
    return options;
  }, [costTypes, costTypeCode]);

  const workActivityOptions = useMemo(() => {
    const options = toDropDownOptions(workActivities);
    if (workActivityCode !== null && isExpiredCode(workActivityCode)) {
      options.push({
        label: workActivityCode.description!,
        value: workActivityCode.id!,
      });
    }
    return options;
  }, [workActivities, workActivityCode]);

  const {
    updateProject: { execute: updateProject },
  } = useProjectProvider();

  const { getOptionsByType } = useLookupCodeHelpers();

  const initialValues = ProjectForm.fromApi(project);
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const handleSubmit = async (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
    try {
      formikHelpers?.setSubmitting(true);
      const updatedProject = values.toApi();
      const response = await updateProject(updatedProject);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    } catch (e) {
      if (axios.isAxiosError(e) && e.response?.status === 409) {
        toast.error(e.response.data as any);
      } else {
        toast.error('Failed to update project.');
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <View
      ref={formikRef}
      validationSchema={AddProjectYupSchema}
      projectStatusOptions={projectStatusTypeCodes}
      businessFunctionOptions={businessFunctionOptions ?? []}
      costTypeOptions={costTypeOptions ?? []}
      workActivityOptions={workActivityOptions ?? []}
      initialValues={initialValues}
      onSubmit={handleSubmit}
    />
  );
});

export default UpdateProjectContainer;
