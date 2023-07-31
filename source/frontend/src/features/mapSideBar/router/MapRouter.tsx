import queryString from 'query-string';
import { memo, useEffect, useMemo } from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router-dom';

import { SideBarType } from '@/components/common/mapFSM/machineDefinition/types';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import Claims from '@/constants/claims';
import { AddLeaseContainer } from '@/features/leases';
import { LeaseContextProvider } from '@/features/leases/context/LeaseContext';
import MotiInventoryContainer from '@/features/mapSideBar/property/MotiInventoryContainer';
import AppRoute from '@/utils/AppRoute';

import AcquisitionContainer from '../acquisition/AcquisitionContainer';
import AcquisitionView from '../acquisition/AcquisitionView';
import AddAcquisitionContainer from '../acquisition/add/AddAcquisitionContainer';
import LeaseContainer from '../lease/LeaseContainer';
import AddProjectContainer from '../project/add/AddProjectContainer';
import ProjectContainer from '../project/ProjectContainer';
import ProjectContainerView from '../project/ProjectContainerView';
import AddResearchContainer from '../research/add/AddResearchContainer';
import ResearchContainer from '../research/ResearchContainer';

interface IMapRouterProps {}

export const MapRouter: React.FunctionComponent<IMapRouterProps> = memo(props => {
  const location = useLocation();
  const history = useHistory();

  const { openSidebar, closeSidebar } = useMapStateMachine();

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

  const isProperty = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/mapview/sidebar/property/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  useEffect(() => {
    if (matched !== null) {
      let sidebarType: SideBarType = SideBarType.NOT_DEFINED;
      if (isAcquisition) {
        sidebarType = SideBarType.ACQUISITION_FILE;
      } else if (isResearch) {
        sidebarType = SideBarType.RESEARCH_FILE;
      } else if (isLease) {
        sidebarType = SideBarType.LEASE_FILE;
      } else if (isProject) {
        sidebarType = SideBarType.PROJECT;
      } else if (isProperty) {
        sidebarType = SideBarType.PROPERTY_INFORMATION;
      }

      openSidebar(sidebarType);
    } else {
      closeSidebar();
    }
  }, [
    isAcquisition,
    isResearch,
    isLease,
    isProject,
    isProperty,
    matched,
    openSidebar,
    closeSidebar,
  ]);

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
          <MotiInventoryContainer onClose={onClose} pid={match.params.pid} />
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
