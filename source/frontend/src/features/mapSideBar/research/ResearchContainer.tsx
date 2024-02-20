import { FormikProps } from 'formik';
import * as React from 'react';
import { useEffect, useRef, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import { matchPath, useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FileTypes } from '@/constants/fileTypes';
import FileLayout from '@/features/mapSideBar/layout/FileLayout';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import { useResearchRepository } from '@/hooks/repositories/useResearchRepository';
import { useQuery } from '@/hooks/use-query';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, stripTrailingSlash } from '@/utils';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

import { SideBarContext } from '../context/sidebarContext';
import SidebarFooter from '../shared/SidebarFooter';
import { UpdateProperties } from '../shared/update/properties/UpdateProperties';
import ResearchHeader from './common/ResearchHeader';
import ResearchMenu from './common/ResearchMenu';
import { useGetResearch } from './hooks/useGetResearch';
import { useUpdateResearchProperties } from './hooks/useUpdateResearchProperties';
import ResearchView from './ResearchView';

export interface IResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const ResearchContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchContainerProps>
> = props => {
  const researchFileId = props.researchFileId;
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
  } = React.useContext(SideBarContext);

  const [isValid, setIsValid] = useState<boolean>(true);
  const [isShowingPropertySelector, setIsShowingPropertySelector] = useState<boolean>(false);
  const { setModalContent, setDisplayModal } = useModalContext();

  const formikRef = useRef<FormikProps<any>>(null);

  const history = useHistory();
  const match = useRouteMatch();

  const menuItems = researchFile?.fileProperties?.map(x => getFilePropertyName(x).value) || [];
  menuItems.unshift('File Summary');

  const { updateResearchFileProperties } = useUpdateResearchProperties();
  const wrapWithOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_ResearchFile | undefined>
  >('Failed to update Research File');

  useEffect(
    () =>
      setFileLoading(loadingResearchFile || loadingResearchFileProperties || loadingLastUpdatedBy),
    [loadingLastUpdatedBy, loadingResearchFile, loadingResearchFileProperties, setFileLoading],
  );

  const fetchResearchFile = React.useCallback(async () => {
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

  const fetchLastUpdatedBy = React.useCallback(async () => {
    const retrieved = await getLastUpdatedBy(props.researchFileId);
    if (retrieved !== undefined) {
      setLastUpdatedBy(retrieved);
    } else {
      setLastUpdatedBy(null);
    }
  }, [props.researchFileId, getLastUpdatedBy, setLastUpdatedBy]);

  const push = history.push;
  const query = useQuery();
  const setIsEditing = React.useCallback(
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

  const onSuccess = React.useCallback(() => {
    setStaleFile(true);
    setStaleLastUpdatedBy(true);
    mapMachine.refreshMapProperties();
    setIsEditing(false);
  }, [mapMachine, setIsEditing, setStaleFile, setStaleLastUpdatedBy]);

  React.useEffect(() => {
    if (researchFile === undefined || researchFileId !== researchFile?.id || staleFile) {
      fetchResearchFile();
    }
  }, [fetchResearchFile, researchFile, researchFileId, staleFile]);

  React.useEffect(() => {
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
  const propertiesMatch = matchPath<Record<string, string>>(
    history.location.pathname,
    `${stripTrailingSlash(match.path)}/property/:menuIndex/:tab?`,
  );
  const selectedMenuIndex = propertiesMatch !== null ? Number(propertiesMatch.params.menuIndex) : 0;

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
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
      setIsValid(formikRef.current?.isValid || false);
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

  const showPropertiesSelector = () => {
    setIsShowingPropertySelector(true);
  };

  if (
    loadingResearchFile ||
    (loadingResearchFileProperties && !isShowingPropertySelector) ||
    researchFile?.fileType !== FileTypes.Research ||
    researchFile?.id !== researchFileId
  ) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  if (isShowingPropertySelector && researchFile) {
    return (
      <UpdateProperties
        file={researchFile}
        setIsShowingPropertySelector={setIsShowingPropertySelector}
        onSuccess={onSuccess}
        updateFileProperties={(file: ApiGen_Concepts_File) =>
          wrapWithOverride((userOverrideCodes: UserOverrideCode[]) =>
            updateResearchFileProperties(
              file as ApiGen_Concepts_ResearchFile,
              userOverrideCodes,
            ).then(response => {
              onSuccess();
              setIsShowingPropertySelector(false);
              return response;
            }),
          )
        }
        canRemove={() => Promise.resolve(true)} //TODO: add this if we need this check for the research file.
        formikRef={formikRef}
      />
    );
  } else {
    return (
      <MapSideBarLayout
        title={isEditing ? 'Update Research File' : 'Research File'}
        icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
        header={
          <ResearchHeader
            researchFile={researchFile as unknown as ApiGen_Concepts_ResearchFile}
            lastUpdatedBy={lastUpdatedBy}
          />
        }
        footer={
          isEditing && (
            <SidebarFooter
              isOkDisabled={formikRef?.current?.isSubmitting}
              onSave={handleSaveClick}
              onCancel={handleCancelClick}
              displayRequiredFieldError={!isValid}
            />
          )
        }
        onClose={props.onClose}
        showCloseButton
      >
        <FileLayout
          leftComponent={
            <>
              <ResearchMenu
                items={menuItems}
                selectedIndex={selectedMenuIndex}
                onChange={onMenuChange}
                onEdit={showPropertiesSelector}
              />
            </>
          }
          bodyComponent={
            <StyledFormWrapper>
              <ResearchView
                researchFile={researchFile as unknown as ApiGen_Concepts_ResearchFile}
                onSuccess={onSuccess}
                setEditMode={setIsEditing}
                ref={formikRef}
                isEditing={isEditing}
              />
            </StyledFormWrapper>
          }
        ></FileLayout>
      </MapSideBarLayout>
    );
  }
};

export default ResearchContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
