import * as React from 'react';
import { matchPath, Switch, useHistory, useLocation } from 'react-router-dom';

import { PopupTray } from '@/components/common/styles';
import { Claims, FileTypes } from '@/constants';
import AppRoute from '@/utils/AppRoute';

import FormContainer from '../../properties/map/form/detail/FormContainer';
import FormView from '../../properties/map/form/detail/FormView';

interface IActivityRouterProps {
  setShowActionBar: (show: boolean) => void;
}

export const PopupRouter: React.FunctionComponent<React.PropsWithChildren<IActivityRouterProps>> =
  React.memo(props => {
    const location = useLocation();
    const history = useHistory();

    let matched = matchPath(location.pathname, {
      path: '/mapview/sidebar/*/*/popup/*',
      exact: true,
      strict: true,
    });

    React.useEffect(() => {
      if (matched !== null) {
        props.setShowActionBar(true);
      } else {
        props.setShowActionBar(false);
      }
    }, [matched, props]);

    const onClose = () => {
      const backUrl = history.location.pathname.split('popup')[0];
      props.setShowActionBar(false);
      history.push(backUrl);
    };

    return (
      <Switch>
        <AppRoute
          path={`/mapview/sidebar/acquisition/*/popup/form/:formFileId`}
          customRender={({ match }) => (
            <PopupTray>
              <FormContainer
                onClose={onClose}
                formFileId={match.params.formFileId}
                fileType={FileTypes.Acquisition}
                View={FormView}
              />
            </PopupTray>
          )}
          claim={Claims.ACTIVITY_VIEW}
          exact
          key={'form'}
          title={'Form Template'}
        />
      </Switch>
    );
  });

export default PopupRouter;
