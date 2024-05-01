import { FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useRef, useState } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FileTypes } from '@/constants/fileTypes';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useResearchRepository } from '@/hooks/repositories/useResearchRepository';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, isValidString, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { PropertyForm } from '../shared/models';
import { useGetResearch } from './hooks/useGetResearch';
import { useUpdateResearchProperties } from './hooks/useUpdateResearchProperties';
import { IResearchViewProps } from './ResearchView';

export interface IResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IResearchViewProps>>;
}

export const ResearchContainer: React.FunctionComponent<IResearchContainerProps> = props => {
  const { researchFileId, onClose, View } = props;
  const {
    retrieveResearchFile: { execute: getResearchFile, loading: loadingResearchFile },
    retrieveResearchFileProperties: {
      execute: getResearchFileProperties,
      loading: loadingResearchFileProperties,
    },
  } = useGetResearch();

  const {
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: loadingLastUpdatedBy },
  } = useResearchRepository();

  const { execute: getPropertyAssociations } = usePropertyAssociations();
  const {
    getPropertyByPidWrapper: { execute: getPropertyByPid },
    getPropertyByPinWrapper: { execute: getPropertyByPin },
  } = usePimsPropertyRepository();

  const mapMachine = useMapStateMachine();
  const {
    setFile,
    file: researchFile,
    setFileLoading,
    staleFile,
    setStaleFile,
    lastUpdatedBy,
    setLastUpdatedBy,
    staleLastUpdatedBy,
    setStaleLastUpdatedBy,
  } = useContext(SideBarContext);

  const [isValid, setIsValid] = useState<boolean>(true);
  const [isShowingPropertySelector, setIsShowingPropertySelector] = useState<boolean>(false);
  const { setModalContent, setDisplayModal } = useModalContext();

  const formikRef = useRef<FormikProps<any>>(null);
  const history = useHistory();
  const match = useRouteMatch();
  const { updateResearchFileProperties } = useUpdateResearchProperties();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_ResearchFile | undefined>
  >('Failed to update Research File');

  useEffect(
    () =>
      setFileLoading(loadingResearchFile || loadingResearchFileProperties || loadingLastUpdatedBy),
    [loadingLastUpdatedBy, loadingResearchFile, loadingResearchFileProperties, setFileLoading],
  );

  const fetchResearchFile = useCallback(async () => {
    const retrieved = await getResearchFile(props.researchFileId);
    if (exists(retrieved)) {
      const researchProperties = await getResearchFileProperties(props.researchFileId);
      retrieved.fileProperties?.forEach(async fp => {
        fp.property = researchProperties?.find(ap => fp.id === ap.id)?.property ?? null;
      });
      setFile({ ...retrieved, fileType: FileTypes.Research });
    } else {
      setFile(undefined);
    }
  }, [getResearchFile, getResearchFileProperties, props.researchFileId, setFile]);

  const fetchLastUpdatedBy = useCallback(async () => {
    const retrieved = await getLastUpdatedBy(props.researchFileId);
    if (retrieved !== undefined) {
      setLastUpdatedBy(retrieved);
    } else {
      setLastUpdatedBy(null);
    }
  }, [props.researchFileId, getLastUpdatedBy, setLastUpdatedBy]);

  const push = history.push;
  const query = useQuery();
  const setIsEditing = useCallback(
    (editing: boolean) => {
      if (editing) {
        query.set('edit', 'true');
      } else {
        query.delete('edit');
      }

      push({ search: query.toString() });
    },
    [push, query],
  );

  const onSuccess = useCallback(() => {
    setStaleFile(true);
    setStaleLastUpdatedBy(true);
    mapMachine.refreshMapProperties();
    setIsEditing(false);
  }, [mapMachine, setIsEditing, setStaleFile, setStaleLastUpdatedBy]);

  useEffect(() => {
    if (researchFile === undefined || researchFileId !== researchFile?.id || staleFile) {
      fetchResearchFile();
    }
  }, [fetchResearchFile, researchFile, researchFileId, staleFile]);

  useEffect(() => {
    if (
      !exists(lastUpdatedBy) ||
      researchFileId !== lastUpdatedBy?.parentId ||
      staleLastUpdatedBy
    ) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, researchFileId, staleLastUpdatedBy]);

  const isEditing = query.get('edit') === 'true';

  const navigateToMenuRoute = (selectedIndex: number) => {
    const route = selectedIndex === 0 ? '' : `/property/${selectedIndex}`;
    history.push(`${stripTrailingSlash(match.url)}${route}`);
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

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            setDisplayModal(false);
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

  //TODO: add this if we need this check for the research file.
  const canRemove = async () => true;

  // Warn user that property is part of an existing research file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      let apiId;
      try {
        if (isValidId(propertyForm.apiId)) {
          apiId = propertyForm.apiId;
        } else if (isValidString(propertyForm.pid)) {
          const result = await getPropertyByPid(propertyForm.pid);
          apiId = result?.id;
        } else if (isValidString(propertyForm.pin)) {
          const result = await getPropertyByPin(Number(propertyForm.pin));
          apiId = result?.id;
        }
      } catch (e) {
        apiId = 0;
      }

      if (isValidId(apiId)) {
        const response = await getPropertyAssociations(apiId);
        const researchAssociations = response?.researchAssociations ?? [];
        const otherResearchFiles = researchAssociations.filter(
          a => exists(a.id) && a.id !== researchFileId,
        );
        return otherResearchFiles.length > 0;
      } else {
        // the property is not in PIMS db -> no need to confirm
        return false;
      }
    },
    [getPropertyAssociations, getPropertyByPid, getPropertyByPin, researchFileId],
  );

  const onUpdateProperties = (
    file: ApiGen_Concepts_File,
  ): Promise<ApiGen_Concepts_File | undefined> => {
    return withUserOverride((userOverrideCodes: UserOverrideCode[]) => {
      return updateResearchFileProperties(
        file as ApiGen_Concepts_ResearchFile,
        userOverrideCodes,
      ).then(response => {
        onSuccess();
        setIsShowingPropertySelector(false);
        return response;
      });
    });
  };

  if (
    loadingResearchFile ||
    (loadingResearchFileProperties && !isShowingPropertySelector) ||
    researchFile?.fileType !== FileTypes.Research ||
    researchFile?.id !== researchFileId
  ) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      researchFile={researchFile as unknown as ApiGen_Concepts_ResearchFile}
      formikRef={formikRef}
      isEditing={isEditing}
      setEditMode={setIsEditing}
      isShowingPropertySelector={isShowingPropertySelector}
      setIsShowingPropertySelector={setIsShowingPropertySelector}
      onClose={onClose}
      onSave={handleSaveClick}
      onCancel={handleCancelClick}
      onMenuChange={onMenuChange}
      onUpdateProperties={onUpdateProperties}
      confirmBeforeAdd={confirmBeforeAdd}
      canRemove={canRemove}
      onSuccess={onSuccess}
      isFormValid={isValid}
    ></View>
  );
};

export default ResearchContainer;
