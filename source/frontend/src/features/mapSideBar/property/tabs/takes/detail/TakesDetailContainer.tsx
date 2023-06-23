import orderBy from 'lodash/orderBy';
import moment from 'moment';
import * as React from 'react';

import { Api_PropertyFile } from '@/models/api/PropertyFile';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { ITakesDetailViewProps } from './TakesDetailView';

interface ITakesDetailContainerProps {
  fileProperty: Api_PropertyFile;
  onEdit: (edit: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<ITakesDetailViewProps>>;
}

export interface IAreaWarning {
  show: boolean;
  onConfirm?: () => void;
}

const TakesDetailContainer: React.FunctionComponent<ITakesDetailContainerProps> = ({
  fileProperty,
  onEdit,
  View,
}) => {
  const fileId = fileProperty.fileId;
  const propertyId = fileProperty.property?.id;

  const {
    getTakesByPropertyId: {
      loading: takesByFileLoading,
      response: takes,
      execute: executeTakesByFileProperty,
    },
    getTakesCountByPropertyId: {
      loading: takesCountLoading,
      response: takesCount,
      execute: executeTakesCount,
    },
  } = useTakesRepository();

  React.useEffect(() => {
    fileId && executeTakesByFileProperty(fileId, propertyId!);
    propertyId && executeTakesCount(propertyId);
  }, [executeTakesByFileProperty, executeTakesCount, fileId, propertyId]);

  return (
    <View
      onEdit={onEdit}
      takes={orderBy(takes, t => moment(t.appCreateTimestamp), 'desc') ?? []}
      loading={takesByFileLoading && takesCountLoading}
      allTakesCount={takesCount ?? 0}
      fileProperty={fileProperty}
    />
  );
};

export default TakesDetailContainer;
