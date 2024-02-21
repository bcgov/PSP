import { FormikProps } from 'formik';
import noop from 'lodash/noop';
import React, { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { matchPath, useHistory, useLocation, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useQuery } from '@/hooks/use-query';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { exists, isValidId, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { IDispositionViewProps } from './DispositionView';

export interface IDispositionContainerProps {
  dispositionFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IDispositionViewProps>>;
}

export const DispositionContainer: React.FunctionComponent<IDispositionContainerProps> = props => {
  // Load state from props and side-bar context
  const { dispositionFileId, onClose, View } = props;
  const { setLastUpdatedBy, lastUpdatedBy, staleLastUpdatedBy, staleFile } =
    useContext(SideBarContext);
  const [isValid, setIsValid] = useState<boolean>(true);

  const {
    getDispositionFile: {
      execute: retrieveDispositionFile,
      loading: loadingDispositionFile,
      error,
      response: dispositionFile,
    },
    getDispositionProperties: {
      execute: retrieveDispositionFileProperties,
      loading: loadingDispositionFileProperties,
      response: dispositionFileProperties,
    },
    getDispositionChecklist: {
      execute: retrieveDispositionFileChecklist,
      loading: loadingDispositionFileChecklist,
      response: dispositionFileChecklist,
    },
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: loadingGetLastUpdatedBy },
  } = useDispositionProvider();

  const { setModalContent, setDisplayModal } = useModalContext();
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

  // Retrieve disposition file from API and save it to local state and side-bar context
  const fetchDispositionFile = useCallback(async () => {
    const retrieved = await retrieveDispositionFile(dispositionFileId);
    if (!exists(retrieved)) {
      return;
    }

    // retrieve related entities (ie properties items) in parallel
    const dispositionPropertiesTask = retrieveDispositionFileProperties(dispositionFileId);
    const dispositionChecklistTask = retrieveDispositionFileChecklist(dispositionFileId);
    await Promise.all([dispositionPropertiesTask, dispositionChecklistTask]);
  }, [
    dispositionFileId,
    retrieveDispositionFileProperties,
    retrieveDispositionFile,
    retrieveDispositionFileChecklist,
  ]);

  const fetchLastUpdatedBy = React.useCallback(async () => {
    const retrieved = await getLastUpdatedBy(dispositionFileId);
    if (retrieved !== undefined) {
      setLastUpdatedBy(retrieved);
    } else {
      setLastUpdatedBy(null);
    }
  }, [dispositionFileId, getLastUpdatedBy, setLastUpdatedBy]);

  React.useEffect(() => {
    if (
      !exists(lastUpdatedBy) ||
      dispositionFileId !== lastUpdatedBy.parentId ||
      staleLastUpdatedBy
    ) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, dispositionFileId, staleLastUpdatedBy]);

  useEffect(() => {
    if (
      !isValidId(dispositionFileId) ||
      (dispositionFileId !== dispositionFile?.id && !loadingDispositionFile) ||
      staleFile
    ) {
      fetchDispositionFile();
    }
  }, [dispositionFile, fetchDispositionFile, dispositionFileId, staleFile, loadingDispositionFile]);

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

  const onSuccess = (refreshProperties?: boolean) => {
    fetchDispositionFile();
    setIsEditing(false);
    fetchLastUpdatedBy();
    if (refreshProperties) {
      mapMachine.refreshMapProperties();
    }
  };

  const canRemove = async () => {
    return true;
  };

  // UI components
  const loading =
    loadingDispositionFile ||
    loadingGetLastUpdatedBy ||
    (loadingDispositionFileProperties && !isPropertySelector) ||
    loadingDispositionFileChecklist ||
    !dispositionFile;

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true}></LoadingBackdrop>
      <View
        setIsEditing={setIsEditing}
        onClose={close}
        onCancel={handleCancelClick}
        onSave={handleSaveClick}
        onMenuChange={onMenuChange}
        onShowPropertySelector={onShowPropertySelector}
        onUpdateProperties={noop as any}
        onSuccess={onSuccess}
        canRemove={canRemove}
        formikRef={formikRef}
        isFormValid={isValid}
        error={error}
        dispositionFile={
          !loading && exists(dispositionFile)
            ? {
                ...dispositionFile,
                fileProperties: dispositionFileProperties ?? null,
                fileChecklistItems: dispositionFileChecklist ?? [],
              }
            : undefined
        }
        isEditing={isEditing}
      ></View>
    </>
  );
};

export default DispositionContainer;
