import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import ResearchFileLayout from 'features/mapSideBar/layout/ResearchFileLayout';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import styled from 'styled-components';
import { pidFormatter } from 'utils';

import ResearchHeader from '../common/ResearchHeader';
import ResearchMenu from '../common/ResearchMenu';
import { useGetResearch } from '../hooks/useGetResearch';
import DetailViewSelector from './DetailViewSelector';

export interface IDetailResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const DetailResearchContainer: React.FunctionComponent<IDetailResearchContainerProps> = props => {
  const { retrieveResearchFile } = useGetResearch();

  const [researchFile, setResearchFile] = useState<Api_ResearchFile | undefined>(undefined);

  const [selectedMenuIndex, setSelectedMenuIndex] = useState<number>(0);

  console.log(researchFile?.researchProperties);

  const menuItems = researchFile?.researchProperties?.map(x => getPropertyName(x)) || [];
  menuItems.unshift('RFile Summary');

  useEffect(() => {
    async function fetchResearchFile() {
      var retrieved = await retrieveResearchFile(props.researchFileId);
      setResearchFile(retrieved);
    }
    fetchResearchFile();
  }, [props.researchFileId, retrieveResearchFile]);

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

  return (
    <MapSideBarLayout
      title="Research File"
      icon={<MdTopic title="User Profile" size="2.5rem" className="mr-2" />}
      header={<ResearchHeader researchFile={researchFile} />}
      onClose={props.onClose}
      showCloseButton
    >
      <ResearchFileLayout
        leftComponent={
          <ResearchMenu
            items={menuItems}
            selectedIndex={selectedMenuIndex}
            setSelectedIndex={setSelectedMenuIndex}
          />
        }
        bodyComponent={
          <StyledFormWrapper>
            <DetailViewSelector researchFile={researchFile} selectedIndex={selectedMenuIndex} />
          </StyledFormWrapper>
        }
      ></ResearchFileLayout>
    </MapSideBarLayout>
  );
};

export default DetailResearchContainer;

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
