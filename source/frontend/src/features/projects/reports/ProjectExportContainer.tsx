import fileDownload from 'js-file-download';
import React, { useCallback } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useModalContext } from '@/hooks/useModalContext';
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
    getCompensationReport: {
      execute: getCompensationReport,
      status: statusCompensation,
      response: dataCompensation,
    },
  } = useAcquisitionProvider();

  const { setModalContent, setDisplayModal } = useModalContext();

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
    await getCompensationReport(values);
  };

  React.useEffect(() => {
    if (statusCompensation === 204) {
      setModalContent({
        title: 'Warning',
        message: 'There is no data for the input parameters you entered.',
        okButtonText: 'Close',
        closeButton: true,
        handleOk: () => setDisplayModal(false),
      });
      setDisplayModal(true);
    } else if (dataCompensation && statusCompensation === 200) {
      fileDownload(dataCompensation, `Compensation_Requisition_Export.xlsx`);
    }
  }, [dataCompensation, statusCompensation, setDisplayModal, setModalContent]);

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
