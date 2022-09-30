import Claims from 'constants/claims';
import MotiInventoryContainer from 'features/mapSideBar/MotiInventoryContainer';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import queryString from 'query-string';
import * as React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router-dom';
import AppRoute from 'utils/AppRoute';

import AcquisitionContainer from './acquisition/AcquisitionContainer';
import AddAcquisitionContainer from './acquisition/add/AddAcquisitionContainer';
import AddResearchContainer from './research/add/AddResearchContainer';
import ResearchContainer from './research/ResearchContainer';

interface IMapRouterProps {
  showSideBar: boolean;
  setShowSideBar: (show: boolean) => void;
  onZoom?: (apiProperty?: IPropertyApiModel) => void;
}

export const MapRouter: React.FunctionComponent<IMapRouterProps> = React.memo(props => {
  const location = useLocation();
  const history = useHistory();

  let matched = matchPath(location.pathname, {
    path: '/mapview/sidebar/*',
    exact: true,
    strict: true,
  });

  React.useEffect(() => {
    if (matched !== null) {
      props.setShowSideBar(true);
    } else {
      props.setShowSideBar(false);
    }
  }, [matched, props]);

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
          <AcquisitionContainer acquisitionFileId={Number(match.params.id)} onClose={onClose} />
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
    </Switch>
  );
});

export default MapRouter;
