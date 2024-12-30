import { AxiosError } from 'axios';
import { FormikProps } from 'formik/dist/types';
import React, {
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useReducer,
  useRef,
  useState,
} from 'react';
import { matchPath, useHistory, useLocation, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import { PropertyForm } from '../shared/models';
import { IAcquisitionViewProps } from './AcquisitionView';

export interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAcquisitionViewProps>>;
}

// Interface for our internal state
export interface AcquisitionContainerState {
  isEditing: boolean;
  selectedMenuIndex: number;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
}

const initialState: AcquisitionContainerState = {
  isEditing: false,
  selectedMenuIndex: 0,
  defaultFileTab: FileTabType.FILE_DETAILS,
  defaultPropertyTab: InventoryTabNames.property,
};

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  // Load state from props and side-bar context
  const { acquisitionFileId, onClose, View } = props;
  const {
    setFile,
    setFileLoading,
    staleFile,
    setStaleFile,
    file,
    setLastUpdatedBy,
    lastUpdatedBy,
    staleLastUpdatedBy,
  } = useContext(SideBarContext);
  const [isValid, setIsValid] = useState<boolean>(true);
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Acquisition File');

  const {
    getAcquisitionFile: {
      execute: retrieveAcquisitionFile,
      loading: loadingAcquisitionFile,
      error,
    },
    updateAcquisitionProperties,
    getAcquisitionProperties: {
      execute: retrieveAcquisitionFileProperties,
      loading: loadingAcquisitionFileProperties,
    },
    getAcquisitionFileChecklist: { execute: retrieveAcquisitionFileChecklist },
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: loadingGetLastUpdatedBy },
  } = useAcquisitionProvider();
  const {
    getProject: { execute: getProjectFunction },
  } = useProjectProvider();

  const { setModalContent, setDisplayModal } = useModalContext();
  const { execute: getPropertyAssociations } = usePropertyAssociations();

  const mapMachine = useMapStateMachine();

  const formikRef = useRef<FormikProps<any>>(null);
  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();
  const query = useQuery();
  const isEditing = query.get('edit') === 'true';

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

  /**
   See here that we are using `newState: Partial<AcquisitionContainerState>` in our reducer
   so we can provide only the properties that are updated on our state
   */
  const [containerState, setContainerState] = useReducer(
    (prevState: AcquisitionContainerState, newState: Partial<AcquisitionContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );
  const acquisitionFile = file;

  // Retrieve acquisition file from API and save it to local state and side-bar context
  const fetchAcquisitionFile = useCallback(async () => {
    const retrieved = await retrieveAcquisitionFile(acquisitionFileId);
    if (exists(retrieved)) {
      if (isValidId(retrieved.projectId)) {
        retrieved.project = await getProjectFunction(retrieved.projectId);
      }
      // retrieve related entities (ie properties, checklist items) in parallel
      const acquisitionPropertiesTask = retrieveAcquisitionFileProperties(acquisitionFileId);
      const acquisitionChecklistTask = retrieveAcquisitionFileChecklist(acquisitionFileId);
      const acquisitionProperties = await acquisitionPropertiesTask;
      const acquisitionChecklist = await acquisitionChecklistTask;

      retrieved.fileProperties = acquisitionProperties ?? null;
      retrieved.fileChecklistItems = acquisitionChecklist ?? [];
      setFile({ ...retrieved, fileType: ApiGen_CodeTypes_FileTypes.Acquisition });
      setStaleFile(false);
    } else {
      setFile(undefined);
    }
  }, [
    acquisitionFileId,
    retrieveAcquisitionFileProperties,
    retrieveAcquisitionFile,
    retrieveAcquisitionFileChecklist,
    setFile,
    setStaleFile,
    getProjectFunction,
  ]);

  const fetchLastUpdatedBy = React.useCallback(async () => {
    const retrieved = await getLastUpdatedBy(acquisitionFileId);
    if (retrieved !== undefined) {
      setLastUpdatedBy(retrieved);
    } else {
      setLastUpdatedBy(null);
    }
  }, [acquisitionFileId, getLastUpdatedBy, setLastUpdatedBy]);

  useEffect(() => {
    if (
      !exists(lastUpdatedBy) ||
      acquisitionFileId !== lastUpdatedBy?.parentId ||
      staleLastUpdatedBy
    ) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, acquisitionFileId, staleLastUpdatedBy]);

  useEffect(() => {
    if (!exists(acquisitionFile) || acquisitionFileId !== acquisitionFile.id || staleFile) {
      fetchAcquisitionFile();
    }
  }, [acquisitionFile, fetchAcquisitionFile, acquisitionFileId, staleFile]);

  useEffect(
    () =>
      setFileLoading(
        loadingAcquisitionFile || loadingAcquisitionFileProperties || loadingGetLastUpdatedBy,
      ),
    [
      loadingAcquisitionFile,
      setFileLoading,
      loadingAcquisitionFileProperties,
      loadingGetLastUpdatedBy,
    ],
  );

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const navigateToMenuRoute = (selectedIndex: number) => {
    const route = selectedIndex === 0 ? '' : `/property/${selectedIndex}`;
    history.push(`${stripTrailingSlash(match.url)}${route}`);
  };

  const onMenuChange = (selectedIndex: number) => {
    if (isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() => navigateToMenuRoute(selectedIndex));
        return;
      }
    }
    navigateToMenuRoute(selectedIndex);
  };

  const onShowPropertySelector = () => {
    history.push(`${stripTrailingSlash(match.url)}/property/selector`);
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
  };

  const onSuccess = () => {
    fetchAcquisitionFile();
    fetchLastUpdatedBy();
    mapMachine.refreshMapProperties();
    setIsEditing(false);
  };

  const canRemove = async () => {
    return true;
  };

  // Warn user that property is part of an existing acquisition file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
        const acquisitionAssociations = response?.acquisitionAssociations ?? [];
        const otherAcqFiles = acquisitionAssociations.filter(
          a => exists(a.id) && a.id !== acquisitionFileId,
        );
        return otherAcqFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations, acquisitionFileId],
  );

  const onUpdateProperties = (
    file: ApiGen_Concepts_File,
  ): Promise<ApiGen_Concepts_File | undefined> => {
    // The backend does not update the product or project so its safe to send nulls even if there might be data for those fields.
    return withUserOverride(
      (userOverrideCodes: UserOverrideCode[]) => {
        return updateAcquisitionProperties
          .execute(
            {
              ...(file as ApiGen_Concepts_AcquisitionFile),
              productId: null,
              projectId: null,
              fileChecklistItems: [],
            },
            userOverrideCodes,
          )
          .then(response => {
            onSuccess();
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
        });
        setDisplayModal(true);
      },
    );
  };

  // UI components
  if (
    loadingAcquisitionFile ||
    (loadingAcquisitionFileProperties && !isPropertySelector) ||
    file?.fileType !== ApiGen_CodeTypes_FileTypes.Acquisition ||
    file?.id !== acquisitionFileId
  ) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      isEditing={isEditing}
      setIsEditing={setIsEditing}
      containerState={containerState}
      setContainerState={setContainerState}
      onClose={close}
      onCancel={handleCancelClick}
      onSave={handleSaveClick}
      onMenuChange={onMenuChange}
      onShowPropertySelector={onShowPropertySelector}
      onCancelConfirm={handleCancelConfirm}
      onUpdateProperties={onUpdateProperties}
      onSuccess={onSuccess}
      confirmBeforeAdd={confirmBeforeAdd}
      canRemove={canRemove}
      formikRef={formikRef}
      isFormValid={isValid}
      error={error}
    ></View>
  );
};

export default AcquisitionContainer;
