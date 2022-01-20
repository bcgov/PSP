import ProtectedComponent from 'components/common/ProtectedComponent';
import { Claims } from 'constants/claims';
import queryString from 'query-string';
import * as React from 'react';
import { useLocation } from 'react-router-dom';

import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

interface IImprovementsContainerProps {}

export const ImprovementsContainer: React.FunctionComponent<IImprovementsContainerProps> = props => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  return !!edit ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer />
    </ProtectedComponent>
  ) : (
    <Improvements disabled={true} />
  );
};

export default ImprovementsContainer;
