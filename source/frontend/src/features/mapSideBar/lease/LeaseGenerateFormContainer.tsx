import { useCallback } from 'react';

import { FormDocumentType } from '@/constants/formDocumentTypes';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

import { FormDocumentEntry } from '../acquisition/common/GenerateForm/formDocumentEntry';
import { IGenerateFormViewProps } from '../acquisition/common/GenerateForm/GenerateFormView';
import { useGenerateLicenceOfOccupation } from '../acquisition/common/GenerateForm/hooks/useGenerateLicenceOfOccupation';

export interface ILeaseGenerateContainerProps {
  lease: ApiGen_Concepts_Lease;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const LeaseGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<ILeaseGenerateContainerProps>
> = ({ lease, View }) => {
  const generateLicenceOfOccupation = useGenerateLicenceOfOccupation();

  const onGenerate = useCallback(
    (lease: ApiGen_Concepts_Lease) => {
      generateLicenceOfOccupation(lease);
    },
    [generateLicenceOfOccupation],
  );

  const onGenerateClick = (formType: FormDocumentType) => {
    switch (formType) {
      case FormDocumentType.H1005A:
        onGenerate(lease);
        break;
      case FormDocumentType.H1005:
        onGenerate(lease);
        break;
      default:
        console.error('Form Document type not recognized');
    }
  };

  const formEntries: FormDocumentEntry[] = [];

  if (lease?.type.id === ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA) {
    formEntries.push({ formType: FormDocumentType.H1005A, text: 'Generate H1005(a)' });
  }
  if (lease?.type.id === ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY) {
    formEntries.push({ formType: FormDocumentType.H1005, text: 'Generate H1005 ' });
  }

  return (
    <View
      formEntries={formEntries}
      onGenerateClick={onGenerateClick}
      isLoading={false}
      letterRecipientsInitialValues={[]}
      openGenerateLetterModal={null}
      onGenerateLetterCancel={null}
      onGenerateLetterOk={null}
    />
  );
};

export default LeaseGenerateContainer;
