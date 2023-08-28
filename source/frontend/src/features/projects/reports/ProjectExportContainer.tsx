import fileDownload from 'js-file-download';
import * as React from 'react';
import { useCallback } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';

import { ProjectExportTypes } from './models';
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
    getAgreementsReport,
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
      onExport={async (values: Api_ExportProjectFilter) => {
        if (Object.keys(ProjectExportTypes).indexOf(ProjectExportTypes.AGREEMENT) !== 0) {
          var data = await getAgreementsReport.execute(values);
          if (data) {
            fileDownload(data, `Agreement_Export.xlsx`);
          }
        }
      }}
    />
  );
};

export default SideProjectContainer;
