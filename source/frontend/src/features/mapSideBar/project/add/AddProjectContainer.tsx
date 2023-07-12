import { FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';

import * as API from '@/constants/API';
import { FinancialCodeTypes } from '@/constants/index';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { Api_Project } from '@/models/api/Project';
import { toDropDownOptions } from '@/utils/financialCodeUtils';

import SidebarFooter from '../../shared/SidebarFooter';
import { useAddProjectForm } from '../hooks/useAddProjectFormManagement';
import { ProjectForm } from '../models';
import AddProjectForm from './AddProjectForm';

export interface IAddProjectContainerProps {
  onClose?: () => void;
}

const AddProjectContainer: React.FC<React.PropsWithChildren<IAddProjectContainerProps>> = props => {
  const { onClose } = props;
  const history = useHistory();

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

  const businessFunctionOptions = useMemo(
    () => toDropDownOptions(businessFunctions),
    [businessFunctions],
  );

  const costTypeOptions = useMemo(() => toDropDownOptions(costTypes), [costTypes]);

  const workActivityOptions = useMemo(() => toDropDownOptions(workActivities), [workActivities]);

  const { getOptionsByType } = useLookupCodeHelpers();
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const formikRef = useRef<FormikProps<ProjectForm>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSave = () => {
    formikRef.current?.submitForm();
  };

  const onSuccess = async (proj: Api_Project) => {
    formikRef.current?.resetForm();
    history.replace(`/mapview/sidebar/project/${proj.id}`);
  };

  const helper = useAddProjectForm({ onSuccess });

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create Project"
      icon={<FaBriefcase className="mr-2 mb-2" size={32} />}
      onClose={close}
      footer={<SidebarFooter onSave={handleSave} onCancel={close} />}
    >
      <AddProjectForm
        ref={formikRef}
        initialValues={helper.initialValues}
        projectStatusOptions={projectStatusTypeCodes}
        businessFunctionOptions={businessFunctionOptions ?? []}
        costTypeOptions={costTypeOptions ?? []}
        workActivityOptions={workActivityOptions ?? []}
        onSubmit={helper.handleSubmit}
        validationSchema={helper.validationSchema}
        isCreating
      />
    </MapSideBarLayout>
  );
};

export default AddProjectContainer;
