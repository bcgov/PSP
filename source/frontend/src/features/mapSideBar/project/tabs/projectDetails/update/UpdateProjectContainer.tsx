import { FormikHelpers, FormikProps } from 'formik';
import React, { useEffect, useMemo, useState } from 'react';

import * as API from '@/constants/API';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';
import { isExpiredCode, toDropDownOptions } from '@/utils/financialCodeUtils';

import { AddProjectYupSchema } from '../../../add/AddProjectFileYupSchema';
import { IAddProjectFormProps } from '../../../add/AddProjectForm';
import { ProjectForm } from '../../../models';

export interface IUpdateProjectContainerProps {
  project: ApiGen_Concepts_Project;
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

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_Project | void>
  >('Failed to Update Project File');

  const {
    getFinancialCodesByType: { execute: getFinancialCodes },
  } = useFinancialCodeRepository();

  const [businessFunctions, setBusinessFunctions] = useState<ApiGen_Concepts_FinancialCode[]>([]);
  const [costTypes, setCostTypes] = useState<ApiGen_Concepts_FinancialCode[]>([]);
  const [workActivities, setWorkActivities] = useState<ApiGen_Concepts_FinancialCode[]>([]);

  useEffect(() => {
    async function fetchBusinessFunctions() {
      const data =
        (await getFinancialCodes(ApiGen_Concepts_FinancialCodeTypes.BusinessFunction)) ?? [];
      setBusinessFunctions(data);
    }
    fetchBusinessFunctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    async function fetchCostTypes() {
      const data = (await getFinancialCodes(ApiGen_Concepts_FinancialCodeTypes.CostType)) ?? [];
      setCostTypes(data);
    }
    fetchCostTypes();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    async function fetchWorkActivities() {
      const data = (await getFinancialCodes(ApiGen_Concepts_FinancialCodeTypes.WorkActivity)) ?? [];
      setWorkActivities(data);
    }
    fetchWorkActivities();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const businessFunctionOptions = useMemo(() => {
    const options = toDropDownOptions(businessFunctions);
    if (businessFunctionCode !== null && isExpiredCode(businessFunctionCode)) {
      options.push({
        label: businessFunctionCode.description ?? '',
        value: businessFunctionCode.id ?? '',
      });
    }
    return options;
  }, [businessFunctions, businessFunctionCode]);

  const costTypeOptions = useMemo(() => {
    const options = toDropDownOptions(costTypes);
    if (costTypeCode !== null && isExpiredCode(costTypeCode)) {
      options.push({
        label: costTypeCode.description ?? '',
        value: costTypeCode.id ?? '',
      });
    }
    return options;
  }, [costTypes, costTypeCode]);

  const workActivityOptions = useMemo(() => {
    const options = toDropDownOptions(workActivities);
    if (workActivityCode !== null && isExpiredCode(workActivityCode)) {
      options.push({
        label: workActivityCode.description ?? '',
        value: workActivityCode.id ?? '',
      });
    }
    return options;
  }, [workActivities, workActivityCode]);

  const {
    updateProject: { execute: updateProject },
  } = useProjectProvider();

  const { getOptionsByType } = useLookupCodeHelpers();

  const initialValues = ProjectForm.fromApi(
    project,
    businessFunctionOptions,
    costTypeOptions,
    workActivityOptions,
  );
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const handleSubmit = async (
    values: ProjectForm,
    formikHelpers: FormikHelpers<ProjectForm>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    formikHelpers?.setSubmitting(true);
    const updatedProject = values.toApi();
    const response = await updateProject(updatedProject, userOverrideCodes);

    if (isValidId(response?.id)) {
      formikHelpers?.resetForm();
      if (typeof onSuccess === 'function') {
        onSuccess();
      }
    }

    formikHelpers?.setSubmitting(false);
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
      onSubmit={(projectForm, formikHelpers) =>
        withUserOverride((userOverrideCodes: UserOverrideCode[]) =>
          handleSubmit(projectForm, formikHelpers, userOverrideCodes),
        )
      }
    />
  );
});

export default UpdateProjectContainer;
