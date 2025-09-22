import { FunctionComponent, PropsWithChildren, ReactNode, useCallback } from 'react';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

export const AddPropertiesGuide: FunctionComponent<PropsWithChildren<unknown>> = () => {
  const { toggleMapSearchControl, isShowingMapSearch } = useMapStateMachine();

  const handleOpenSearch = useCallback(() => {
    if (!isShowingMapSearch) {
      toggleMapSearchControl();
    }
  }, [isShowingMapSearch, toggleMapSearchControl]);

  const guideBodyContent = (): ReactNode => {
    return (
      <>
        <ol>
          <StyledBoldLi>
            <div>Find a Property</div>
            <StyledNormalText>
              Navigate to an area of the map OR use{' '}
              <LinkButton className="d-inline-block" onClick={handleOpenSearch}>
                Search
              </LinkButton>
            </StyledNormalText>
          </StyledBoldLi>
          <StyledBoldLi>
            <div>Select a property</div>
            <StyledNormalText>
              Click on the map and the selection will be highlighed
            </StyledNormalText>
          </StyledBoldLi>
          <StyledBoldLi>
            <div>Add it to this file</div>
            <StyledNormalText>
              Click &quot;Add selected&quot; property button when it appears below
            </StyledNormalText>
          </StyledBoldLi>
        </ol>
      </>
    );
  };

  return (
    <>
      <FormGuideContainer title="New workflow" guideBody={guideBodyContent()}></FormGuideContainer>
    </>
  );
};

export default AddPropertiesGuide;

const StyledNormalText = styled.div`
  font-weight: normal;
`;
const StyledBoldLi = styled.li`
  font-weight: bold;
`;
