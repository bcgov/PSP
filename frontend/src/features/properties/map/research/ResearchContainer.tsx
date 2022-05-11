import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import ResearchFileLayout from 'features/mapSideBar/layout/ResearchFileLayout';
import { FormikProps } from 'formik';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import styled from 'styled-components';
import { pidFormatter } from 'utils';

import ResearchFooter from './common/ResearchFooter';
import ResearchHeader from './common/ResearchHeader';
import ResearchMenu from './common/ResearchMenu';
import { useGetResearch } from './hooks/useGetResearch';
import ViewSelector from './ViewSelector';

export interface IResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const ResearchContainer: React.FunctionComponent<IResearchContainerProps> = props => {
  const { retrieveResearchFile } = useGetResearch();

  const [researchFile, setResearchFile] = useState<Api_ResearchFile | undefined>(undefined);

  const [selectedMenuIndex, setSelectedMenuIndex] = useState<number>(0);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [isDirty, setIsDirty] = useState<boolean>(false);

  const [formikRef, setFormikRef] = useState<React.RefObject<FormikProps<any>> | undefined>(
    undefined,
  );
  const menuItems = researchFile?.researchProperties?.map(x => getPropertyName(x)) || [];
  menuItems.unshift('RFile Summary');

  useEffect(() => {
    async function fetchResearchFile() {
      var retrieved = await retrieveResearchFile(props.researchFileId);
      setResearchFile(retrieved);
      setIsDirty(false);
    }
    fetchResearchFile();
  }, [props.researchFileId, retrieveResearchFile, isDirty]);

  if (researchFile === undefined) {
    return (
      <>
        <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>
      </>
    );
  }

  function getPropertyName(researchProperty?: Api_ResearchFileProperty): string {
    if (researchProperty === undefined) {
      return '';
    }

    if (researchProperty.propertyName !== undefined) {
      return researchProperty.propertyName;
    } else if (researchProperty.property !== undefined) {
      const property = researchProperty.property;
      if (property.pin !== undefined) {
        return property.pin.toString();
      } else if (property.pid !== undefined) {
        return pidFormatter(property.pid.toString());
      } else if (property.planNumber !== undefined) {
        return property.planNumber;
      } else if (property.location !== undefined) {
        return `${property.location.coordinate?.x} + ${property.location.coordinate?.y}`;
      }
    }
    return 'Property';
  }

  const onMenuChange = (selectedIndex: number) => {
    if (isEditing) {
      if (formikRef?.current?.dirty) {
        if (
          window.confirm('You have made changes on this form. Do you wish to leave without saving?')
        ) {
          handleCancel();
          setSelectedMenuIndex(selectedIndex);
        }
      } else {
        handleCancel();
        setSelectedMenuIndex(selectedIndex);
      }
    } else {
      setSelectedMenuIndex(selectedIndex);
    }
  };

  const handleSave = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancel = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setIsEditing(false);
  };

  const onSuccesss = () => {
    setIsDirty(true);
    setIsEditing(false);
  };

  return (
    <MapSideBarLayout
      title={isEditing ? 'Update Research File' : 'Research File'}
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
      header={<ResearchHeader researchFile={researchFile} />}
      footer={
        isEditing && (
          <ResearchFooter
            isSubmitting={formikRef?.current?.isSubmitting}
            onSave={handleSave}
            onCancel={handleCancel}
          />
        )
      }
      onClose={props.onClose}
      showCloseButton
    >
      <ResearchFileLayout
        leftComponent={
          <ResearchMenu
            items={menuItems}
            selectedIndex={selectedMenuIndex}
            onChange={onMenuChange}
          />
        }
        bodyComponent={
          <StyledFormWrapper>
            <ViewSelector
              researchFile={researchFile}
              selectedIndex={selectedMenuIndex}
              isEditMode={isEditing}
              onSuccesss={onSuccesss}
              setEditMode={setIsEditing}
              setFormikRef={setFormikRef}
            />
          </StyledFormWrapper>
        }
      ></ResearchFileLayout>
    </MapSideBarLayout>
  );
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
