import { AxiosError } from 'axios';
import { FormikProps } from 'formik';
import { useCallback, useContext, useEffect, useMemo, useRef, useState } from 'react';
import { matchPath, useHistory, useLocation, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { useResearchRepository } from '@/hooks/repositories/useResearchRepository';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, sortFileProperties, stripTrailingSlash } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { PropertyForm } from '../shared/models';
import usePathGenerator from '../shared/sidebarPathGenerator';
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
  const { setModalContent, setDisplayModal } = useModalContext();

  const formikRef = useRef<FormikProps<any>>(null);
  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();
  const { updateResearchFileProperties } = useUpdateResearchProperties();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_ResearchFile | undefined>
  >('Failed to update Research File');

  const isPropertySelector = useMemo(
    () =>
      matchPath<Record<string, string>>(
        location.pathname,
        `${stripTrailingSlash(match.path)}/property/selector`,
      ),
    [location.pathname, match.path],
  );

  useEffect(
    () =>
      setFileLoading(loadingResearchFile || loadingResearchFileProperties || loadingLastUpdatedBy),
    [loadingLastUpdatedBy, loadingResearchFile, loadingResearchFileProperties, setFileLoading],
  );

  const fetchResearchFile = useCallback(async () => {
    const retrieved = await getResearchFile(props.researchFileId);
    if (exists(retrieved)) {
      const researchProperties = await getResearchFileProperties(props.researchFileId);
      retrieved.fileProperties = sortFileProperties(researchProperties) ?? null;
      setFile({ ...retrieved, fileType: ApiGen_CodeTypes_FileTypes.Research });
      setStaleFile(false);
    } else {
      setFile(undefined);
    }
  }, [getResearchFile, getResearchFileProperties, props.researchFileId, setFile, setStaleFile]);

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

  const pathGenerator = usePathGenerator();

  const onSelectFileSummary = () => {
    if (!exists(researchFile)) {
      return;
    }

    if (isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() => pathGenerator.showFile('research', researchFile.id));
        return;
      }
    }
    pathGenerator.showFile('research', researchFile.id);
  };

  const onSelectProperty = (filePropertyId: number) => {
    if (!exists(researchFile)) {
      return;
    }

    if (isEditing) {
      if (formikRef?.current?.dirty) {
        handleCancelClick(() =>
          pathGenerator.showFilePropertyId('research', researchFile.id, filePropertyId),
        );
        return;
      }
    }
    // The index needs to be offset to match the menu index
    pathGenerator.showFilePropertyId('research', researchFile.id, filePropertyId);
  };

  const onEditProperties = () => {
    if (exists(researchFile)) {
      pathGenerator.editProperties('research', researchFile.id);
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
  };

  //TODO: add this if we need this check for the research file.
  const canRemove = async () => true;

  // Warn user that property is part of an existing research file
  const confirmBeforeAdd = useCallback(
    async (propertyForm: PropertyForm): Promise<boolean> => {
      if (isValidId(propertyForm.apiId)) {
        const response = await getPropertyAssociations(propertyForm.apiId);
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
    [getPropertyAssociations, researchFileId],
  );

  const onUpdateProperties = (
    file: ApiGen_Concepts_File,
  ): Promise<ApiGen_Concepts_File | undefined> => {
    return withUserOverride(
      (userOverrideCodes: UserOverrideCode[]) => {
        return updateResearchFileProperties(
          file as ApiGen_Concepts_ResearchFile,
          userOverrideCodes,
        ).then(response => {
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
          handleOk: async () => {
            formikRef.current?.resetForm();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      },
    );
  };

  // UI components
  if (
    loadingResearchFile ||
    (loadingResearchFileProperties && !isPropertySelector) ||
    researchFile?.fileType !== ApiGen_CodeTypes_FileTypes.Research ||
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
      onClose={onClose}
      onSave={handleSaveClick}
      onCancel={handleCancelClick}
      onSelectFileSummary={onSelectFileSummary}
      onSelectProperty={onSelectProperty}
      onEditProperties={onEditProperties}
      onUpdateProperties={onUpdateProperties}
      confirmBeforeAdd={confirmBeforeAdd}
      canRemove={canRemove}
      onSuccess={onSuccess}
      isFormValid={isValid}
    ></View>
  );
};

export default ResearchContainer;
