import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import ProjectIcon from '@/assets/images/projects-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import * as API from '@/constants/API';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { toDropDownOptions } from '@/utils/financialCodeUtils';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddProjectForm } from '../hooks/useAddProjectFormManagement';
import { ProjectForm } from '../models';
import { IAddProjectFormProps } from './AddProjectForm';

export interface IAddProjectContainerProps {
  onClose: () => void;
  onSuccess: (newProjectId: number) => void;
  View: React.ForwardRefExoticComponent<
    IAddProjectFormProps & React.RefAttributes<FormikProps<ProjectForm>>
  >;
}

const AddProjectContainer: React.FC<React.PropsWithChildren<IAddProjectContainerProps>> = props => {
  const { onClose, onSuccess, View } = props;
  const history = useHistory();
  const { setModalContent, setDisplayModal } = useModalContext();

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

  const handleSuccess = async (proj: ApiGen_Concepts_Project) => {
    formikRef.current?.resetForm();
    onSuccess(proj.id);
  };

  const helper = useAddProjectForm({ onSuccess: handleSuccess });

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Project"
      icon={<ProjectIcon title="Project Icon" fill="currentColor" />}
      onClose={close}
      footer={
        <SidebarFooter onSave={handleSave} onCancel={close} displayRequiredFieldError={!isValid} />
      }
    >
      <View
        ref={formikRef}
        initialValues={helper.initialValues}
        projectStatusOptions={projectStatusTypeCodes}
        businessFunctionOptions={businessFunctionOptions ?? []}
        costTypeOptions={costTypeOptions ?? []}
        workActivityOptions={workActivityOptions ?? []}
        onSubmit={(projectForm, formikHelpers) =>
          withUserOverride(
            (userOverrideCodes: UserOverrideCode[]) =>
              helper.handleSubmit(projectForm, formikHelpers, userOverrideCodes),
            [],
            (axiosError: AxiosError<IApiError>) => {
              setModalContent({
                variant: 'error',
                title: 'Error',
                message: axiosError?.response?.data.error,
                okButtonText: 'Close',
                handleOk: async () => {
                  setDisplayModal(false);
                },
              });
              setDisplayModal(true);
            },
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
