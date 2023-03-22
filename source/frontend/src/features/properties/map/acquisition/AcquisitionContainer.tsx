import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { FileTypes } from 'constants/index';
import { InventoryTabNames } from 'features/mapSideBar/tabs/InventoryTabs';
import { FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useContext, useEffect, useReducer, useRef } from 'react';

import { SideBarContext } from '../context/sidebarContext';
import { IAcquisitionViewProps } from './AcquisitionView';
import { EditFormNames } from './EditFormNames';
import { useAcquisitionProvider } from './hooks/useAcquisitionProvider';

export interface IAcquisitionContainerProps {
  acquisitionFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAcquisitionViewProps>>;
}

// Interface for our internal state
export interface AcquisitionContainerState {
  isEditing: boolean;
  activeEditForm?: EditFormNames;
  selectedMenuIndex: number;
  showConfirmModal: boolean;
  acquisitionFile: Api_AcquisitionFile | undefined;
  defaultPropertyTab: InventoryTabNames;
}

const initialState: AcquisitionContainerState = {
  isEditing: false,
  activeEditForm: undefined,
  selectedMenuIndex: 0,
  showConfirmModal: false,
  acquisitionFile: undefined,
  defaultPropertyTab: InventoryTabNames.property,
};

export const AcquisitionContainer: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionContainerProps>
> = props => {
  // Load state from props and side-bar context
  const { acquisitionFileId, onClose, View } = props;
  const { setFile, setFileLoading } = useContext(SideBarContext);
  const { search } = useMapSearch();
  const {
    getAcquisitionFile: { execute: retrieveAcquisitionFile, loading: loadingAcquisitionFile },
    updateAcquisitionProperties,
    getAcquisitionProperties: {
      execute: retrieveAcquisitionFileProperties,
      loading: loadingAcquisitionFileProperties,
    },
  } = useAcquisitionProvider();

  const formikRef = useRef<FormikProps<any>>(null);

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
    var acquisitionProperties = await retrieveAcquisitionFileProperties(acquisitionFileId);
    if (retrieved) {
      retrieved.fileProperties = acquisitionProperties;
    }

    setContainerState({ acquisitionFile: retrieved });
    setFile({ ...retrieved, fileType: FileTypes.Acquisition });
  }, [acquisitionFileId, retrieveAcquisitionFileProperties, retrieveAcquisitionFile, setFile]);

  useEffect(() => {
    if (acquisitionFile === undefined || acquisitionFileId !== acquisitionFile.id) {
      fetchAcquisitionFile();
    }
  }, [acquisitionFile, fetchAcquisitionFile, acquisitionFileId]);

  useEffect(
    () => setFileLoading(loadingAcquisitionFile || loadingAcquisitionFileProperties),
    [loadingAcquisitionFile, setFileLoading, loadingAcquisitionFileProperties],
  );

  const close = useCallback(() => onClose && onClose(), [onClose]);

  const onMenuChange = (selectedIndex: number) => {
    if (containerState.isEditing) {
      if (formikRef?.current?.dirty) {
        if (
          window.confirm('You have made changes on this form. Do you wish to leave without saving?')
        ) {
          handleCancelClick();
          setContainerState({ selectedMenuIndex: selectedIndex });
        }
      } else {
        handleCancelClick();
        setContainerState({ selectedMenuIndex: selectedIndex });
      }
    } else {
      setContainerState({ selectedMenuIndex: selectedIndex });
    }
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
    setContainerState({
      showConfirmModal: false,
      isEditing: false,
      activeEditForm: undefined,
    });
  };

  const onSuccess = () => {
    fetchAcquisitionFile();
    search();
    setContainerState({ activeEditForm: undefined, isEditing: false });
  };

  const canRemove = async (propertyId: number) => {
    const fileProperties = await retrieveAcquisitionFileProperties(acquisitionFileId);
    const fp = fileProperties?.find(fp => fp.property?.id === propertyId);
    return (
      fp?.activityInstanceProperties?.length === undefined ||
      fp?.activityInstanceProperties?.length === 0
    );
  };

  // UI components

  if (
    loadingAcquisitionFile ||
    (loadingAcquisitionFileProperties &&
      containerState.activeEditForm !== EditFormNames.propertySelector)
  ) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      containerState={containerState}
      setContainerState={setContainerState}
      onClose={close}
      onCancel={handleCancelClick}
      onSave={handleSaveClick}
      onMenuChange={onMenuChange}
      onCancelConfirm={handleCancelConfirm}
      onUpdateProperties={updateAcquisitionProperties.execute}
      onSuccess={onSuccess}
      canRemove={canRemove}
      formikRef={formikRef}
    ></View>
  );
};

export default AcquisitionContainer;
