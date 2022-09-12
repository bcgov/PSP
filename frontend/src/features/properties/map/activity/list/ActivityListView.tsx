import { SelectOption } from 'components/common/form/Select';
import { TableSort } from 'components/Table/TableSort';
import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { getDeleteModalProps, useModalContext } from 'hooks/useModalContext';
import { defaultActivityFilter, IActivityFilter } from 'interfaces/IActivityResults';
import { orderBy } from 'lodash';
import { Api_Activity, Api_ActivityTemplate, Api_FileActivity } from 'models/api/Activity';
import React, { useCallback, useContext } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import { SideBarContext } from '../../context/sidebarContext';
import { useActivityRepository } from '../hooks/useActivityRepository';
import { ActivityFilterForm } from './ActivityFilter/ActivityFilterForm';
import { ActivityResults } from './ActivityResults/ActivityResults';
import { AddActivityForm } from './ActivityResults/AddActivityForm';
import * as Styled from './styles';

export interface IActivityListViewProps {
  fileId: number;
  defaultFilters?: IActivityFilter;
  fileType: FileTypes;
}

/**
 * Page that displays Activity information.
 */
export const ActivityListView: React.FunctionComponent<IActivityListViewProps> = ({
  fileId,
  defaultFilters,
  fileType,
}: IActivityListViewProps) => {
  const history = useHistory();
  const match = useRouteMatch();
  const activityMatch = useRouteMatch<{ activityId?: string }>(`${match.url}/activity/:activityId`);
  const {
    getActivityTemplates: { response: templateResponse },
    getFileActivities: {
      execute: getFileActivities,
      response: activityResults,
      loading: fileActivitiesLoading,
    },
    addFileActivity: { execute: addFileActivity, loading: saveFileActivityLoading },
    deleteActivity: { execute: deleteActivity, loading: deleteActivityLoading },
  } = useActivityRepository();
  const [sort, setSort] = React.useState<TableSort<Api_Activity>>({});
  const [filters, setFilters] = React.useState<IActivityFilter>(
    defaultFilters ?? defaultActivityFilter,
  );
  const { staleFile, setStaleFile } = useContext(SideBarContext);
  const { setModalProps, setDisplayModal } = useModalContext();
  const { hasClaim } = useKeycloakWrapper();

  const fetchData = useCallback(async () => {
    await getFileActivities(fileType, fileId);
  }, [getFileActivities, fileId, fileType]);

  React.useEffect(() => {
    if (activityResults === undefined || staleFile) {
      fetchData();
      setStaleFile(false);
    }
  }, [fetchData, staleFile, setStaleFile, activityResults]);

  const templateTypes =
    templateResponse?.map(
      (template: Api_ActivityTemplate) =>
        ({
          value: template.id,
          label: template.activityTemplateTypeCode?.description,
          code: template.activityTemplateTypeCode?.id,
        } as SelectOption),
    ) ?? [];

  const sortedFilteredActivities = React.useMemo(() => {
    if (!!activityResults && activityResults?.length > 0) {
      let activityItems = [...activityResults];

      if (filters) {
        activityItems = activityItems.filter(activity => {
          return (
            (!filters.activityTypeId ||
              activity.activityTemplate.activityTemplateTypeCode?.id === filters.activityTypeId) &&
            (!filters.status || activity.activityStatusTypeCode?.id === filters.status)
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = sort[sortFields[0] as keyof Api_Activity];
          return orderBy(
            activityItems,
            sortFields[0] === 'activityTemplate'
              ? 'activityTemplate.activityTemplateTypeCode.description'
              : sortFields[0],
            keyName,
          );
        }
      }
      return activityItems;
    }
    return [];
  }, [activityResults, sort, filters]);

  const saveActivity = (activityTypeId: number) => {
    const fileActivity: Api_FileActivity = {
      fileId: fileId,
      activity: {
        activityTemplateId: activityTypeId,
        description: '',
        activityTemplate: {},
        activityDataJson: '',
      },
    };
    addFileActivity(fileType, fileActivity).then(async () => {
      await getFileActivities(fileType, fileId);
    });
  };

  const onDeleteActivity = async (activity: Api_Activity) => {
    setModalProps({
      ...deleteModalProps,
      handleOk: async () => {
        activity?.id !== undefined &&
          (await deleteActivity(activity?.id).then(async () => {
            if (
              !!activityMatch?.params?.activityId &&
              +activityMatch?.params?.activityId === activity?.id
            ) {
              history.replace(match.url);
            }
            await getFileActivities(fileType, fileId);
          }));
        setDisplayModal(false);
      },
    });
  };

  return (
    <>
      <Styled.ListPage>
        <Styled.Scrollable vertical={true}>
          <Styled.PageHeader>Activities</Styled.PageHeader>
          {hasClaim(Claims.ACTIVITY_ADD) && (
            <AddActivityForm
              onAddActivity={(activityTypeId: number) => {
                saveActivity(activityTypeId);
              }}
              templateTypes={templateTypes}
            ></AddActivityForm>
          )}
          <ActivityFilterForm onSetFilter={setFilters} activityFilter={filters} />
          <ActivityResults
            results={sortedFilteredActivities}
            loading={fileActivitiesLoading || saveFileActivityLoading || deleteActivityLoading}
            sort={sort}
            setSort={setSort}
            onShowActivity={(activity: Api_Activity) => {
              history.push(`${match.url}/activity/${activity?.id}`);
            }}
            onDelete={onDeleteActivity}
          />
        </Styled.Scrollable>
      </Styled.ListPage>
    </>
  );
};

const deleteModalProps = {
  ...getDeleteModalProps(),
  display: true,
  title: 'Delete Activity',
  closeButton: true,
  message: (
    <>
      <b>You have chosen to delete this activity.</b>
      <br />
      <br />
      <p>
        Deleting this activity will also permanently delete any data that has been associated to the
        activity.
      </p>
      <p>
        Additionally, any documents specific to this activity (ie: not referenced elsewhere in the
        system) will also be permanently deleted from the document store.
      </p>
      <br />
      <b>Do you wish to continue to remove this activity?</b>
    </>
  ),
};

export default ActivityListView;
