import { FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';

import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import * as API from '@/constants/API';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { toDropDownOptions } from '@/utils/financialCodeUtils';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddProjectForm } from '../hooks/useAddProjectFormManagement';
import { ProjectForm } from '../models';
import AddProjectForm from './AddProjectForm';

export interface IAddProjectContainerProps {
  onClose: () => void;
  onSuccess: (newProjectId: number) => void;
}

const AddProjectContainer: React.FC<React.PropsWithChildren<IAddProjectContainerProps>> = props => {
  const { onClose } = props;
  const history = useHistory();

  const {
    getFinancialCodesByType: { execute: getFinancialCodes },
  } = useFinancialCodeRepository();

  const [businessFunctions, setBusinessFunctions] = useState<ApiGen_Concepts_FinancialCode[]>([]);
  const [costTypes, setCostTypes] = useState<ApiGen_Concepts_FinancialCode[]>([]);
  const [workActivities, setWorkActivities] = useState<ApiGen_Concepts_FinancialCode[]>([]);
  const [isValid, setIsValid] = useState<boolean>(true);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_Project | void>
  >('Failed to Add Project File');

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

  const businessFunctionOptions = useMemo(
    () => toDropDownOptions(businessFunctions),
    [businessFunctions],
  );

  const costTypeOptions = useMemo(() => toDropDownOptions(costTypes), [costTypes]);

  const workActivityOptions = useMemo(() => toDropDownOptions(workActivities), [workActivities]);

  const { getOptionsByType } = useLookupCodeHelpers();
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const formikRef = useRef<FormikProps<ProjectForm>>(null);

  const close = useCallback(() => onClose(), [onClose]);

  const handleSave = async () => {
    const result = await (formikRef.current?.submitForm() ?? Promise.resolve());
    setIsValid(formikRef.current?.isValid ?? false);
    return result;
  };

  const onSuccess = async (proj: ApiGen_Concepts_Project) => {
    formikRef.current?.resetForm();
    props.onSuccess(proj.id);
  };

  const helper = useAddProjectForm({ onSuccess });

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Project"
      icon={<FaBriefcase size={26} />}
      onClose={close}
      footer={
        <SidebarFooter onSave={handleSave} onCancel={close} displayRequiredFieldError={!isValid} />
      }
    >
      <AddProjectForm
        ref={formikRef}
        initialValues={helper.initialValues}
        projectStatusOptions={projectStatusTypeCodes}
        businessFunctionOptions={businessFunctionOptions ?? []}
        costTypeOptions={costTypeOptions ?? []}
        workActivityOptions={workActivityOptions ?? []}
        onSubmit={(projectForm, formikHelpers) =>
          withUserOverride((userOverrideCodes: UserOverrideCode[]) =>
            helper.handleSubmit(projectForm, formikHelpers, userOverrideCodes),
          )
        }
        validationSchema={helper.validationSchema}
        isCreating
      />
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddProjectContainer;
