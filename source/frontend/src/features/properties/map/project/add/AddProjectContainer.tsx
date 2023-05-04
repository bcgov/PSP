import { SelectOption } from 'components/common/form';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import * as API from 'constants/API';
import { FinancialCodeTypes } from 'constants/index';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import { useFinancialCodeRepository } from 'hooks/repositories/useFinancialCodeRepository';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import orderBy from 'lodash/orderBy';
import { Api_Project } from 'models/api/Project';
import { useCallback, useEffect, useRef, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';

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
  const { search } = useMapSearch();

  const {
    getFinancialCodesByType: { execute: getFinancialCodes },
  } = useFinancialCodeRepository();

  const [businessFunctionOptions, setBusinessFunctionOptions] = useState<SelectOption[]>([]);
  const [costTypeOptions, setCostTypeOptions] = useState<SelectOption[]>([]);
  const [workActivityOptions, setWorkActivityOptions] = useState<SelectOption[]>([]);

  useEffect(() => {
    async function fetchData() {
      if (businessFunctionOptions === undefined) {
        let records = (await getFinancialCodes(FinancialCodeTypes.BusinessFunction)) ?? [];
        records = orderBy(records, ['displayOrder'], ['asc']);
        const options = records.map<SelectOption>(c => {
          return {
            label: c.description!,
            value: c.id!,
          };
        });
        setBusinessFunctionOptions(options);
      }

      if (costTypeOptions === undefined) {
        let records = (await getFinancialCodes(FinancialCodeTypes.CostType)) ?? [];
        records = orderBy(records, ['displayOrder'], ['asc']);
        const options = records.map<SelectOption>(c => {
          return {
            label: c.description!,
            value: c.id!,
          };
        });
        setCostTypeOptions(options);
      }

      if (workActivityOptions === undefined) {
        let records = (await getFinancialCodes(FinancialCodeTypes.WorkActivity)) ?? [];
        records = orderBy(records, ['displayOrder'], ['asc']);
        const options = records.map<SelectOption>(c => {
          return {
            label: c.description!,
            value: c.id!,
          };
        });
        setWorkActivityOptions(options);
      }
    }

    fetchData();
  }, [businessFunctionOptions, costTypeOptions, getFinancialCodes, workActivityOptions]);

  const { getOptionsByType } = useLookupCodeHelpers();
  const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

  const formikRef = useRef<FormikProps<ProjectForm>>(null);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const handleSave = () => {
    formikRef.current?.submitForm();
  };

  const onSuccess = async (proj: Api_Project) => {
    formikRef.current?.resetForm();
    await search();
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
        businessFunctionOptions={businessFunctionOptions}
        costTypeOptions={costTypeOptions}
        workActivityOptions={workActivityOptions}
        onSubmit={helper.handleSubmit}
        validationSchema={helper.validationSchema}
        isCreating
      />
    </MapSideBarLayout>
  );
};

export default AddProjectContainer;
