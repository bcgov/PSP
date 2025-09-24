import { useCallback, useMemo, useState } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { exists, isValidId } from '@/utils';

import GenerateItemView from '../../acquisition/common/GenerateForm/GenerateItemView';
import NoticeSelectorModal from './NoticeSelectorModal';

export interface IGenerateLetterContainerProps {
  acquisitionFileId?: number;
  onGenerate: (
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => void;
}

const NoticeGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateLetterContainerProps>
> = ({ acquisitionFileId, onGenerate }) => {
  const [isOpen, openModal, closeModal] = useModalManagement();

  const [acquisitionTeam, setAcquisitionTeam] = useState<ApiGen_Concepts_AcquisitionFileTeam[]>([]);
  const [acquisitionOwners, setAcquisitionOwners] = useState<
    ApiGen_Concepts_AcquisitionFileOwner[]
  >([]);

  const {
    getAcquisitionFile: { execute: getAcquisitionFile, loading: loadingProperties },
    getAcquisitionOwners: {
      execute: retrieveAcquisitionFileOwners,
      loading: loadingAcquisitionFileOwners,
    },
  } = useAcquisitionProvider();

  const featchAcquisitionTeam = useCallback(async () => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file === undefined) {
      return;
    }

    const fileOwnersFetchCall = retrieveAcquisitionFileOwners(acquisitionFileId);

    const team = file.acquisitionTeam;
    setAcquisitionTeam(team);

    // Add owners and interest holders
    const [fileOwners] = await Promise.all([fileOwnersFetchCall]);

    if (exists(fileOwners)) {
      setAcquisitionOwners(fileOwners);
    }
  }, [acquisitionFileId, getAcquisitionFile, retrieveAcquisitionFileOwners]);

  const handleSelectedNoticeEntries = (
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => {
    //onGenerate(selectedProperties);
    closeModal();
    onGenerate(selectedOwners, responsibleTeamMember, signingTeamMember);
  };

  const handleGenerateClick = useCallback(() => {
    if (isValidId(acquisitionFileId)) {
      featchAcquisitionTeam();
    }
    openModal();
  }, [acquisitionFileId, featchAcquisitionTeam, openModal]);

  const handleCancelClick = () => {
    closeModal();
  };

  const isLoading = useMemo(
    () => loadingAcquisitionFileOwners || loadingProperties,
    [loadingAcquisitionFileOwners, loadingProperties],
  );

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <GenerateItemView
        label="Generate Notice of Entry"
        formType={ApiGen_CodeTypes_FormTypes.FORM1}
        onGenerate={handleGenerateClick}
      />
      <NoticeSelectorModal
        isOpened={isOpen}
        teamMembers={acquisitionTeam}
        fileOwners={acquisitionOwners}
        onSelectOk={handleSelectedNoticeEntries}
        onCancelClick={handleCancelClick}
      />
    </>
  );
};

export default NoticeGenerateContainer;
