import {
  MapState,
  MapStateActionTypes,
  MapStateContext,
} from 'components/maps/providers/MapStateContext';
import { useMapStateMachine } from 'components/maps/providers/MapStateMachineContext';
import Claims from 'constants/claims';
import { AddLeaseContainer } from 'features/leases';
import { LeaseContextProvider } from 'features/leases/context/LeaseContext';
import MotiInventoryContainer from 'features/mapSideBar/MotiInventoryContainer';
import { Api_Property } from 'models/api/Property';
import queryString from 'query-string';
import { memo, useContext, useEffect, useMemo } from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';

import AcquisitionContainer from './acquisition/AcquisitionContainer';
import AcquisitionView from './acquisition/AcquisitionView';
import AddAcquisitionContainer from './acquisition/add/AddAcquisitionContainer';
import LeaseContainer from './lease/LeaseContainer';
import AddProjectContainer from './project/add/AddProjectContainer';
import ProjectContainer from './project/ProjectContainer';
import ProjectContainerView from './project/ProjectContainerView';
import AddResearchContainer from './research/add/AddResearchContainer';
import ResearchContainer from './research/ResearchContainer';

interface IMapRouterProps {
  showSideBar: boolean;
  setShowSideBar: (show: boolean) => void;
  onZoom?: (apiProperty?: Api_Property) => void;
}

export const MapRouter: React.FunctionComponent<IMapRouterProps> = memo(props => {
  const location = useLocation();
  const history = useHistory();
  const { setState } = useContext(MapStateContext);

  // TODO: PSP-5606 WIP
  const { service } = useMapStateMachine();

  const matched = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isResearch = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/research/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isAcquisition = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/acquisition/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isLease = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/lease/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isProject = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/project/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const setShowSideBar = props.setShowSideBar;

  useEffect(() => {
    if (matched !== null) {
      service.send('OPEN_SIDEBAR');

      if (isAcquisition) {
        setState({ type: MapStateActionTypes.MAP_STATE, mapState: MapState.ACQUISITION_FILE });
      } else if (isResearch) {
        setState({ type: MapStateActionTypes.MAP_STATE, mapState: MapState.RESEARCH_FILE });
      } else if (isLease) {
        setState({ type: MapStateActionTypes.MAP_STATE, mapState: MapState.LEASE_FILE });
      } else if (isProject) {
        setState({ type: MapStateActionTypes.MAP_STATE, mapState: MapState.MAP });
      }
      setShowSideBar(true);
    } else {
      service.send('CLOSE_SIDEBAR');

      setShowSideBar(false);
      setState({ type: MapStateActionTypes.MAP_STATE, mapState: MapState.MAP });
    }
  }, [isAcquisition, isResearch, isLease, isProject, matched, setShowSideBar, setState, service]);

  const onClose = () => {
    history.push('/mapview');
  };

  const pidQueryString = queryString.parse(location.search).pid?.toString() ?? '';
  return (
    <Switch>
      <AppRoute
        path={`/mapview/sidebar/research/new`}
        customRender={() => <AddResearchContainer onClose={onClose} />}
        claim={Claims.RESEARCH_ADD}
        exact
        key={'NewResearch'}
        title={'Create Research File'}
      />
      <AppRoute
        path={`/mapview/sidebar/research/:researchId`}
        customRender={({ match }) => (
          <ResearchContainer researchFileId={Number(match.params.researchId)} onClose={onClose} />
        )}
        claim={Claims.RESEARCH_VIEW}
        key={'Research'}
        title={'Research File'}
      />
      <AppRoute
        path={`/mapview/sidebar/acquisition/new`}
        customRender={() => <AddAcquisitionContainer onClose={onClose} />}
        claim={Claims.ACQUISITION_ADD}
        key={'NewAcquisition'}
        title={'Create Acquisition File'}
      />
      <AppRoute
        path={`/mapview/sidebar/acquisition/:id`}
        customRender={({ match }) => (
          <AcquisitionContainer
            acquisitionFileId={Number(match.params.id)}
            onClose={onClose}
            View={AcquisitionView}
          />
        )}
        claim={Claims.ACQUISITION_VIEW}
        key={'Acquisition'}
        title={'Acquisition File'}
      />
      <AppRoute
        path={`/mapview/sidebar/property/:propertyId`}
        customRender={({ match }) => (
          <MotiInventoryContainer
            onClose={onClose}
            id={Number(match.params.propertyId)}
            pid={pidQueryString}
            onZoom={props.onZoom}
          />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'Property'}
        title={'Property Information'}
      />
      <AppRoute
        path={`/mapview/sidebar/non-inventory-property/:pid`}
        customRender={({ match }) => (
          <MotiInventoryContainer onClose={onClose} pid={match.params.pid} onZoom={props.onZoom} />
        )}
        claim={Claims.PROPERTY_VIEW}
        exact
        key={'PropertyNonInventory'}
        title={'Property Information - Non Inventory'}
      />
      <AppRoute
        path={`/mapview/sidebar/lease/new`}
        customRender={() => <AddLeaseContainer onClose={onClose} />}
        claim={Claims.LEASE_ADD}
        exact
        key={'NewLease'}
        title={'Create Lease'}
      />
      <AppRoute
        path={`/mapview/sidebar/project/new`}
        customRender={() => <AddProjectContainer onClose={onClose} />}
        claim={Claims.PROJECT_ADD}
        exact
        key={'NewProject'}
        title={'Create Project'}
      />
      <AppRoute
        path={`/mapview/sidebar/project/:id`}
        customRender={({ match }) => (
          <ProjectContainer
            projectId={Number(match.params.id)}
            onClose={onClose}
            View={ProjectContainerView}
          />
        )}
        claim={Claims.PROJECT_VIEW}
        exact
        key={'Project'}
        title={'Project'}
      />
      <AppRoute
        path={`/mapview/sidebar/lease/:id`}
        customRender={({ match }) => (
          <LeaseContextProvider>
            <LeaseContainer leaseId={Number(match.params.id)} onClose={onClose} />
          </LeaseContextProvider>
        )}
        claim={Claims.LEASE_VIEW}
        key={'LeaseLicense'}
        title={'Lease / License File'}
      />
    </Switch>
  );
});

export default MapRouter;
