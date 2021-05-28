import { AxiosError } from 'axios';
import Claims from 'constants/claims';
import { Roles } from 'constants/roles';
import { FormikValues } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import _ from 'lodash';
import { MutableRefObject } from 'react';
import { useDispatch } from 'react-redux';
import { useAppSelector } from 'store/hooks';
import { logClear } from 'store/slices/network/networkSlice';

import { createProject, IProject, updateProject, updateWorkflowStatus } from '..';
import { ProjectActions } from '../slices/projectActions';

/** hook providing utilities for project dispose step forms. */
const useStepForm = () => {
  const dispatch = useDispatch();
  const keycloak = useKeycloakWrapper();
  const getProjectRequest = useAppSelector(
    state => state.network[ProjectActions.GET_PROJECT] as any,
  );
  const addProjectRequest = useAppSelector(
    state => state.network[ProjectActions.ADD_PROJECT] as any,
  );
  const updateProjectRequest = useAppSelector(
    state => state.network[ProjectActions.UPDATE_PROJECT] as any,
  );
  const updateWorflowStatusRequest = useAppSelector(
    state => state.network[ProjectActions.UPDATE_WORKFLOW_STATUS] as any,
  );
  const noFetchingProjectRequests =
    getProjectRequest?.isFetching !== true &&
    addProjectRequest?.isFetching !== true &&
    updateProjectRequest?.isFetching !== true &&
    updateWorflowStatusRequest?.isFetching !== true;

  //TODO: There is a known issue in formik preventing submitForm()
  // from returning promises. For the time being the higher level
  // functions will need to handle all chained promises.
  const onSubmit = (values: any, actions: any) => Promise.resolve(values);

  const onSubmitReview = (
    values: any,
    formikRef: MutableRefObject<FormikValues | undefined>,
    statusCode?: string,
    workflow?: string,
  ) => {
    const apiValues = _.cloneDeep(values);
    if (values.exemptionRequested && statusCode !== undefined) {
      return dispatch(
        updateWorkflowStatus(apiValues, statusCode, workflow ?? apiValues.workflowCode),
      );
    } else {
      return (statusCode !== undefined
        ? dispatch(updateWorkflowStatus(apiValues, statusCode, workflow ?? apiValues.workflowCode))
        : (dispatch(updateProject(apiValues)) as any)
      ).catch((error: any) => {
        const msg: string = error?.response?.data?.error ?? error.toString();
        formikRef?.current?.setStatus({ msg });
      });
    }
  };

  const canUserOverride = () => {
    return keycloak.hasRole(Roles.SRES_FINANCIAL_MANAGER); // TODO: This is a temporary way to allow for editing the historical projects and should be replaced with a better implementation.
  };

  const canUserEditForm = (projectAgencyId: number) => {
    return (
      (keycloak.hasAgency(projectAgencyId) && keycloak.hasClaim(Claims.PROJECT_EDIT)) ||
      keycloak.hasClaim(Claims.ADMIN_PROJECTS)
    );
  };

  const canUserSubmitForm = () => {
    return keycloak.hasClaim(Claims.DISPOSE_REQUEST);
  };

  const canUserApproveForm = () => {
    return keycloak.hasClaim(Claims.ADMIN_PROJECTS);
  };

  const addOrUpdateProject = (
    project: IProject,
    formikRef: MutableRefObject<FormikValues | undefined>,
  ) => {
    let promise: Promise<any>;
    if (project.projectNumber === undefined || project.projectNumber === '') {
      promise = dispatch(createProject(project)) as any;
    } else {
      promise = dispatch(updateProject(project)) as any;
    }
    return promise
      .catch((error: AxiosError) => {
        const msg: string = error?.response?.data?.error ?? error.toString();
        formikRef?.current?.setStatus({ msg });
        throw Error('axios request failed');
      })
      .finally(() => {
        dispatch(logClear(ProjectActions.UPDATE_PROJECT));
        dispatch(logClear(ProjectActions.ADD_PROJECT));
      });
  };

  const onSave = (formikRef: MutableRefObject<FormikValues | undefined>) => {
    return formikRef.current?.submitForm().then(() => {
      const values = formikRef?.current?.values;
      const errors = formikRef?.current?.errors;

      // do not go to the next step if the form has validation errors.
      if (errors === undefined || Object.keys(errors).length === 0) {
        return addOrUpdateProject(values, formikRef);
      }
    });
  };
  return {
    onSubmit,
    canUserOverride,
    canUserEditForm,
    canUserSubmitForm,
    canUserApproveForm,
    onSubmitReview,
    addOrUpdateProject,
    onSave,
    noFetchingProjectRequests,
    getProjectRequest,
  };
};

export default useStepForm;
