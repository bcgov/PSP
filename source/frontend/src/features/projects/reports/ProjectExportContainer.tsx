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
    getCompensationReport,
  } = useAcquisitionProvider();

  const onExportTypeSelected = useCallback(() => {
    loadProjects();
    loadAcquisitionTeam();
  }, [loadProjects, loadAcquisitionTeam]);

  const generateAgreementReport = async (values: Api_ExportProjectFilter) => {
    var data = await getAgreementsReport.execute(values);
    if (data) {
      fileDownload(data, `Agreement_Export.xlsx`);
    }
  };

  const generateCompensationReport = async (values: Api_ExportProjectFilter) => {
    var data = await getCompensationReport.execute(values);
    if (data) {
      fileDownload(data, `Compensation_Requisition_Export.xlsx`);
    }
  };

  return (
    <View
      onExportTypeSelected={onExportTypeSelected}
      projects={projects ?? []}
      teamMembers={team ?? []}
      loading={loadingProjects || loadingTeam}
      onExport={async (values: Api_ExportProjectFilter) => {
        if (values.type && Object.keys(ProjectExportTypes).includes(values.type)) {
          if (ProjectExportTypes[values.type] === ProjectExportTypes.AGREEMENT) {
            await generateAgreementReport(values);
          } else if (ProjectExportTypes[values.type] === ProjectExportTypes.COMPENSATION) {
            await generateCompensationReport(values);
          }
        }
      }}
    />
  );
};

export default SideProjectContainer;
