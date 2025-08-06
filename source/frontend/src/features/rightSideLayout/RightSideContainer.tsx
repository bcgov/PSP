import { Dictionary } from '@reduxjs/toolkit/dist/entities/models';
import { useMemo } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FilterContentContainer } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentContainer';
import { FilterContentForm } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentForm';
import { LayersMenu } from '@/components/maps/leaflet/Control/LayersControl/LayersMenu';
import { SearchContainer } from '@/components/maps/leaflet/Control/Search/SearchContainer';
import { SearchView } from '@/components/maps/leaflet/Control/Search/SearchView';

import { WorklistContainer } from '../properties/worklist/WorklistContainer';
import { WorklistView } from '../properties/worklist/WorklistView';
import RightSideLayout from './RightSideLayout';

interface StateDefinition {
  title: string;
  tooltipText: string;
  testDataId: string;
  component: any;
}

interface SelectedState extends StateDefinition {
  toggleCallback: () => void;
}

const Buttonsssss: Dictionary<StateDefinition> = {
  filter: {
    title: 'Filter By:',
    tooltipText: 'Close Advanced Map Filters',
    testDataId: 'advanced-filter-sidebar',
    component: <FilterContentContainer View={FilterContentForm} />,
  },
  layers: {
    title: 'View Layer By:',
    tooltipText: 'Close Map Layers',
    testDataId: 'map-layers-sidebar',
    component: <LayersMenu />,
  },
  search: {
    title: 'Search by',
    tooltipText: 'Close search',
    testDataId: 'search-sidebar',
    component: <SearchContainer View={SearchView} />,
  },
  workingList: {
    title: 'Working list',
    tooltipText: 'Close working list',
    testDataId: 'worklist-sidebar',
    component: <WorklistContainer View={WorklistView} />,
  },
};

const RightSideContainer: React.FC<React.PropsWithChildren<unknown>> = () => {
  const {
    isShowingMapFilter,
    isShowingMapLayers,
    isShowingMapSearch,
    isShowingWorkList,
    toggleMapFilterDisplay,
    toggleMapLayerControl,
    toggleMapSearchControl,
    toggleWorkListControl,
  } = useMapStateMachine();

  const rightSideContent: SelectedState = useMemo(() => {
    if (isShowingMapFilter) {
      return { ...Buttonsssss['filter'], toggleCallback: toggleMapFilterDisplay };
    } else if (isShowingMapLayers) {
      return { ...Buttonsssss['layers'], toggleCallback: toggleMapLayerControl };
    } else if (isShowingMapSearch) {
      return { ...Buttonsssss['search'], toggleCallback: toggleMapSearchControl };
    } else if (isShowingWorkList) {
      return { ...Buttonsssss['workingList'], toggleCallback: toggleWorkListControl };
    } else {
      return null;
    }
  }, [
    isShowingMapFilter,
    isShowingMapLayers,
    isShowingMapSearch,
    isShowingWorkList,
    toggleMapFilterDisplay,
    toggleMapLayerControl,
    toggleMapSearchControl,
    toggleWorkListControl,
  ]);

  return (
    <>
      {rightSideContent !== null && (
        <RightSideLayout
          isOpen={true}
          toggle={rightSideContent.toggleCallback}
          title={rightSideContent.title}
          closeTooltipText={rightSideContent.tooltipText}
          data-testId={rightSideContent.testDataId}
        >
          {rightSideContent.component}
        </RightSideLayout>
      )}
    </>
  );
};

export default RightSideContainer;
