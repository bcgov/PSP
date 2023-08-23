import * as React from 'react';
import { useCallback } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';

import { IProjectExportFormProps } from './ProjectExportForm';

export interface ISideProjectContainerProps {
  View: React.FunctionComponent<React.PropsWithChildren<IProjectExportFormProps>>;
}

export const SideProjectContainer: React.FunctionComponent<ISideProjectContainerProps> = ({
  View,
}) => {
  const {
    getAllProjects: { response: projects, loading: loadingProjects, execute: loadProjects },
  } = useProjectProvider();
  const {
    getAllAcquisitionFileTeamMembers: {
      response: team,
      loading: loadingTeam,
      execute: loadAcquisitionTeam,
    },
  } = useAcquisitionProvider();

  const onExportTypeSelected = useCallback(() => {
    loadProjects();
    loadAcquisitionTeam();
  }, [loadProjects, loadAcquisitionTeam]);

  return (
    <View
      onExportTypeSelected={onExportTypeSelected}
      projects={projects ?? []}
      teamMembers={team ?? []}
      loading={loadingProjects || loadingTeam}
      onExport={(values: Api_ExportProjectFilter) => {
        //TODO: export stories will need to use this as a trigger.
        console.log(values);
      }}
    />
  );
};

export default SideProjectContainer;
