import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import React, { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { matchPath, useHistory, useLocation, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, sortFileProperties, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import { PropertyForm } from '../shared/models';
import usePathGenerator from '../shared/sidebarPathGenerator';
import { IManagementViewProps } from './ManagementView';

export interface IManagementContainerProps {
  managementFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IManagementViewProps>>;
}

export const ManagementContainer: React.FunctionComponent<IManagementContainerProps> = props => {
  // Load state from props and side-bar context
  const { managementFileId, onClose, View } = props;
  const { setLastUpdatedBy, lastUpdatedBy, staleLastUpdatedBy, staleFile, setFile } =
    useContext(SideBarContext);
  const [isValid, setIsValid] = useState<boolean>(true);
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Management File Properties');

  const {
    getManagementFile: {
      execute: retrieveManagementFile,
      loading: loadingManagementFile,
      error,
      response: managementFile,
    },
    getManagementProperties: {
      execute: retrieveManagementFileProperties,
      loading: loadingManagementFileProperties,
      response: managementFileProperties,
    },
    updateManagementProperties,
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: loadingGetLastUpdatedBy },
  } = useManagementFileRepository();
  const { execute: getPropertyAssociations } = usePropertyAssociations();

  const { setModalContent, setDisplayModal } = useModalContext();

  const mapMachine = useMapStateMachine();

  const formikRef = useRef<FormikProps<any>>(null);
  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();
  const query = useQuery();
  const isEditing = query.get('edit') === 'true';
  const urlPathWrapper = usePathGenerator();
  const tabMatch = useRouteMatch<{ detailType: string; id: string }>(`${match.path}/:tab`);

  const setIsEditing = (value: boolean) => {
    if (value) {
      query.set('edit', value.toString());
    } else {
      query.delete('edit');
    }
    history.push({ search: query.toString() });
  };

  const isPropertySelector = useMemo(
    () =>
      matchPath<Record<string, string>>(
        location.pathname,
        `${stripTrailingSlash(match.path)}/property/selector`,
      ),
    [location.pathname, match.path],
  );

  // Retrieve management file from API and save it to local state and side-bar context
  const fetchManagementFile = useCallback(async () => {
    const retrieved = await retrieveManagementFile(managementFileId);
    if (exists(retrieved)) {
      // retrieve related entities (ie properties items) in parallel
      const fileProperties = await retrieveManagementFileProperties(managementFileId);

      retrieved.fileProperties = sortFileProperties(fileProperties) ?? null;
      setFile({ ...retrieved, fileType: ApiGen_CodeTypes_FileTypes.Management });
    } else {
      setFile(undefined);
    }
  }, [retrieveManagementFile, managementFileId, retrieveManagementFileProperties, setFile]);

  const fetchLastUpdatedBy = React.useCallback(async () => {
    const retrieved = await getLastUpdatedBy(managementFileId);
    if (retrieved !== undefined) {
      setLastUpdatedBy(retrieved);
    } else {
      setLastUpdatedBy(null);
    }
  }, [managementFileId, getLastUpdatedBy, setLastUpdatedBy]);

  useEffect(() => {
    if (
      !exists(lastUpdatedBy) ||
      managementFileId !== lastUpdatedBy.parentId ||
      staleLastUpdatedBy
    ) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, managementFileId, staleLastUpdatedBy]);

  useEffect(() => {
    if (
      (!error && managementFileId !== managementFile?.id && !loadingManagementFile) ||
      staleFile
    ) {
      fetchManagementFile();
    }
  }, [
    managementFile,
    fetchManagementFile,
    managementFileId,
    staleFile,
    loadingManagementFile,
    error,
  ]);

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const pathGenerator = usePathGenerator();

  const stripEditFromPath = () => {
    if (!tabMatch) {
      return;
    }

    urlPathWrapper.showDetails('management', managementFileId, FileTabType.FILE_DETAILS, false);
  };

  const onSelectFileSummary = () => {
    if (!exists(managementFile)) {
      return;
    }

    if (isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() => pathGenerator.showFile('management', managementFile.id));
        return;
      }
    }
    pathGenerator.showFile('management', managementFile.id);
  };

  const onSelectProperty = (filePropertyId: number) => {
    if (!exists(managementFile)) {
      return;
    }

    if (isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() =>
          pathGenerator.showFilePropertyId('management', managementFile.id, filePropertyId),
        );
        return;
      }
    }
    // The index needs to be offset to match the menu index
    pathGenerator.showFilePropertyId('management', managementFile.id, filePropertyId);
  };

  const onEditProperties = () => {
    if (exists(managementFile)) {
      pathGenerator.editProperties('management', managementFile.id);
    }
  };

  const handleSaveClick = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = (onCancelConfirm?: () => void) => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            setDisplayModal(false);
            onCancelConfirm && onCancelConfirm();
          },
          handleCancel: () => setDisplayModal(false),
        });
        setDisplayModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setIsEditing(false);
    stripEditFromPath();
  };

  const onSuccess = (refreshProperties?: boolean, refreshFile?: boolean) => {
    setIsEditing(false);
    stripEditFromPath();
    fetchLastUpdatedBy();
    if (refreshFile) {
      fetchManagementFile();
    }
    if (refreshProperties) {
      mapMachine.refreshMapProperties();
    }
  };

  const onUpdateProperties = (
    file: ApiGen_Concepts_File,
  ): Promise<ApiGen_Concepts_File | undefined> => {
    // The backend does not update the product or project so its safe to send nulls even if there might be data for those fields.
    return withUserOverride(
      (userOverrideCodes: UserOverrideCode[]) => {
        return updateManagementProperties
          .execute(
            {
              ...(file as ApiGen_Concepts_ManagementFile),
              productId: null,
              projectId: null,
              fileStatusTypeCode: null,
              project: null,
              product: null,
              managementTeam: [],
            },
            userOverrideCodes,
          )
          .then(response => {
            history.push(`${stripTrailingSlash(match.url)}`);
            onSuccess(true, true);
            return response;
          });
      },
      [],
      (axiosError: AxiosError<IApiError>) => {
        setModalContent({
          variant: 'error',
          title: 'Error',
          message: axiosError?.response?.data.error,
          okButtonText: 'Close',
          handleOk: async () => {
            formikRef.current?.resetForm();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      },
    );
  };

  const canRemove = async () => {
    return true;
  };

  // Warn user that property is part of an existing management file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const fileAssociations = response?.managementAssociations ?? [];
        const otherFiles = fileAssociations.filter(a => exists(a.id) && a.id !== managementFileId);
        return otherFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [managementFileId, getPropertyAssociations],
  );

  // UI components
  const loading =
    loadingManagementFile ||
    loadingGetLastUpdatedBy ||
    (loadingManagementFileProperties && !isPropertySelector) ||
    !managementFile;

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true}></LoadingBackdrop>
      <View
        setIsEditing={setIsEditing}
        onClose={close}
        onCancel={handleCancelClick}
        onSave={handleSaveClick}
        onSelectFileSummary={onSelectFileSummary}
        onSelectProperty={onSelectProperty}
        onEditProperties={onEditProperties}
        onUpdateProperties={onUpdateProperties}
        onSuccess={onSuccess}
        confirmBeforeAdd={confirmBeforeAdd}
        canRemove={canRemove}
        formikRef={formikRef}
        isFormValid={isValid}
        error={error}
        managementFile={
          managementFile?.id === managementFileId
            ? {
                ...managementFile,
                fileProperties: sortFileProperties(managementFileProperties) ?? null,
              }
            : undefined
        }
        isEditing={isEditing}
      ></View>
    </>
  );
};

export default ManagementContainer;
