import { FormikProps } from 'formik';
import React, { useCallback, useContext, useEffect, useReducer, useRef } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapSearch } from '@/components/maps/hooks/useMapSearch';
import { FileTypes } from '@/constants/index';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_File } from '@/models/api/File';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { SideBarContext } from '../context/sidebarContext';
import { FileTabType } from '../shared/detail/FileTabs';
import { IAcquisitionViewProps } from './AcquisitionView';
import { EditFormType } from './EditFormNames';

export interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAcquisitionViewProps>>;
}

// Interface for our internal state
export interface AcquisitionContainerState {
  isEditing: boolean;
  activeEditForm?: EditFormType;
  selectedMenuIndex: number;
  showConfirmModal: boolean;
  acquisitionFile: Api_AcquisitionFile | undefined;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
}

const initialState: AcquisitionContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  selectedMenuIndex: 0,
  showConfirmModal: false,
  acquisitionFile: undefined,
  defaultFileTab: FileTabType.FILE_DETAILS,
  defaultPropertyTab: InventoryTabNames.property,
};

export const AcquisitionContainer: React.FunctionComponent<IAcquisitionContainerProps> = props => {
  // Load state from props and side-bar context
  const { acquisitionFileId, onClose, View } = props;
  const { setFile, setFileLoading, staleFile, setStaleFile } = useContext(SideBarContext);
  const { searchMany } = useMapSearch();
  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to update Acquisition File');
  const {
    getAcquisitionFile: { execute: retrieveAcquisitionFile, loading: loadingAcquisitionFile },
    updateAcquisitionProperties,
    getAcquisitionProperties: {
      execute: retrieveAcquisitionFileProperties,
      loading: loadingAcquisitionFileProperties,
    },
    getAcquisitionFileChecklist: { execute: retrieveAcquisitionFileChecklist },
  } = useAcquisitionProvider();

  const formikRef = useRef<FormikProps<any>>(null);

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
  const acquisitionFile = containerState.acquisitionFile;

  // Retrieve acquisition file from API and save it to local state and side-bar context
  const fetchAcquisitionFile = useCallback(async () => {
    var retrieved = await retrieveAcquisitionFile(acquisitionFileId);
    // retrieve related entities (ie properties, checklist items) in parallel
    const acquisitionPropertiesTask = retrieveAcquisitionFileProperties(acquisitionFileId);
    const acquisitionChecklistTask = retrieveAcquisitionFileChecklist(acquisitionFileId);
    const acquisitionProperties = await acquisitionPropertiesTask;
    const acquisitionChecklist = await acquisitionChecklistTask;

    if (retrieved) {
      retrieved.fileProperties = acquisitionProperties;
      retrieved.acquisitionFileChecklist = acquisitionChecklist;
    }

    setContainerState({ acquisitionFile: retrieved });
    setFile({ ...retrieved, fileType: FileTypes.Acquisition });
    setStaleFile(false);
  }, [
    acquisitionFileId,
    retrieveAcquisitionFileProperties,
    retrieveAcquisitionFile,
    retrieveAcquisitionFileChecklist,
    setFile,
    setStaleFile,
  ]);

  useEffect(() => {
    if (acquisitionFile === undefined || acquisitionFileId !== acquisitionFile.id || staleFile) {
      fetchAcquisitionFile();
    }
  }, [acquisitionFile, fetchAcquisitionFile, acquisitionFileId, staleFile]);

  useEffect(
    () => setFileLoading(loadingAcquisitionFile || loadingAcquisitionFileProperties),
    [loadingAcquisitionFile, setFileLoading, loadingAcquisitionFileProperties],
  );

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const navigateToMenuRoute = (selectedIndex: number) => {
    const route = selectedIndex === 0 ? '' : `/property/${selectedIndex}`;
    history.push(`${match.url}${route}`);
  };

  const onMenuChange = (selectedIndex: number) => {
    if (isEditing) {
      if (formikRef?.current?.dirty) {
        if (
          window.confirm('You have made changes on this form. Do you wish to leave without saving?')
        ) {
          handleCancelClick();
          navigateToMenuRoute(selectedIndex);
        }
      } else {
        handleCancelClick();
        navigateToMenuRoute(selectedIndex);
      }
    } else {
      navigateToMenuRoute(selectedIndex);
    }
  };

  const onShowPropertySelector = () => {
    history.push(`${match.url}/property/selector`);
  };

  const handleSaveClick = () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setContainerState({ showConfirmModal: true });
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
    setContainerState({ showConfirmModal: false });
    setIsEditing(false);
  };

  const onSuccess = () => {
    fetchAcquisitionFile();
    searchMany();
    setIsEditing(false);
  };

  const canRemove = async (propertyId: number) => {
    const fileProperties = await retrieveAcquisitionFileProperties(acquisitionFileId);
    const fp = fileProperties?.find(fp => fp.property?.id === propertyId);
    return (
      fp?.activityInstanceProperties?.length === undefined ||
      fp?.activityInstanceProperties?.length === 0
    );
  };

  const onUpdateProperties = (file: Api_File): Promise<Api_File | undefined> => {
    // The backend does not update the product or project so its safe to send nulls even if there might be data for those fields.
    return withUserOverride((userOverrideCodes: UserOverrideCode[]) => {
      return updateAcquisitionProperties
        .execute({ ...file, productId: null, projectId: null }, userOverrideCodes)
        .then(response => {
          onSuccess();
          return response;
        });
    });
  };

  // UI components
  if (
    loadingAcquisitionFile ||
    (loadingAcquisitionFileProperties &&
      containerState.activeEditForm !== EditFormType.PROPERTY_SELECTOR)
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
      canRemove={canRemove}
      formikRef={formikRef}
    ></View>
  );
};

export default AcquisitionContainer;
