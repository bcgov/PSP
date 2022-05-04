import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import ResearchFileLayout from 'features/mapSideBar/layout/ResearchFileLayout';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useState } from 'react';
import { MdTopic } from 'react-icons/md';
import styled from 'styled-components';

import ResearchHeader from '../common/ResearchHeader';
import ResearchMenu from '../common/ResearchMenu';
import { useGetResearch } from '../hooks/useGetResearch';
import DetailResearchForm from './DetailResearchForm';

export interface IDetailResearchContainerProps {
  researchFileId: number;
  onClose: () => void;
}

export const DetailResearchContainer: React.FunctionComponent<IDetailResearchContainerProps> = props => {
  const { retrieveResearchFile } = useGetResearch();

  const [researchFile, setResearchFile] = useState<Api_ResearchFile | undefined>(undefined);

  const [selectedMenuIndex, setSelectedMenuIndex] = useState<number>(0);

  useEffect(() => {
    async function fetchResearchFile() {
      console.log(props.researchFileId);
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
            items={[
              'R-File summary',
              'NW corner at Central Park and Boulevard Road',
              'Property name 2',
              'Property name 3',
            ]}
            selectedIndex={selectedMenuIndex}
            setSelectedIndex={setSelectedMenuIndex}
          />
        }
        bodyComponent={
          <StyledFormWrapper>
            <DetailResearchForm researchFile={researchFile} />
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
