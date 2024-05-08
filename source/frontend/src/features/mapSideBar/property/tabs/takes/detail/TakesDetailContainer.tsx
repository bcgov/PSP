import { AxiosError } from 'axios';
import orderBy from 'lodash/orderBy';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { stripTrailingSlash } from '@/utils/utils';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { ITakesDetailViewProps } from './TakesDetailView';

interface ITakesDetailContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  View: React.FunctionComponent<React.PropsWithChildren<ITakesDetailViewProps>>;
}

export interface IAreaWarning {
  show: boolean;
  onConfirm?: () => void;
}

const TakesDetailContainer: React.FunctionComponent<ITakesDetailContainerProps> = ({
  fileProperty,
  View,
}) => {
  const history = useHistory();
  const { url } = useRouteMatch();
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
    deleteTakeByAcquisitionPropertyId: { loading: deleteTakesLoading, execute: executeTakeDelete },
  } = useTakesRepository();

  const refresh = useCallback(() => {
    fileId && propertyId && executeTakesByFileProperty(fileId, propertyId);
    propertyId && executeTakesCount(propertyId);
  }, [executeTakesByFileProperty, executeTakesCount, fileId, propertyId]);

  useEffect(() => {
    refresh();
  }, [executeTakesByFileProperty, executeTakesCount, fileId, propertyId, refresh]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<any | void>
  >('Failed to add delete take');

  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <View
      onEdit={(takeId: number) => history.push(`${stripTrailingSlash(url)}/${takeId}?edit=true`)}
      takes={orderBy(takes, t => t.id, 'desc') ?? []}
      loading={takesByFileLoading || takesCountLoading || deleteTakesLoading}
      allTakesCount={takesCount ?? 0}
      fileProperty={fileProperty}
      onDelete={async takeId => {
        return await withUserOverride(
          (userOverrideCodes: UserOverrideCode[]) =>
            executeTakeDelete(fileProperty.id, takeId, userOverrideCodes).then(response => {
              refresh();
              return response;
            }),
          [],
          (axiosError: AxiosError<IApiError>) => {
            setModalContent({
              variant: 'error',
              title: 'Error',
              message: axiosError?.response?.data.error,
              okButtonText: 'Close',
            });
            setDisplayModal(true);
          },
        );
      }}
      onAdd={() => history.push(`${stripTrailingSlash(url)}?edit=true`)}
    />
  );
};

export default TakesDetailContainer;
