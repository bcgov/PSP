import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';

import ExpropriationForm1 from './form1/ExpropriationForm1';

export interface IExpropriationTabContainerViewProps {
  loading: boolean;
  acquisitionFile: Api_AcquisitionFile;
}

export const ExpropriationTabContainerView: React.FunctionComponent<
  IExpropriationTabContainerViewProps
> = ({ loading, acquisitionFile }) => {
  // TODO : Load created Forms
  const acquisitionFileTypeCode = acquisitionFile.acquisitionTypeCode?.id;

  return (
    <>
      <LoadingBackdrop show={loading} />
      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section header="Form 1 - Notice of Expropriation" data-testid="form-1-section">
          <ExpropriationForm1 acquisitionFile={acquisitionFile}></ExpropriationForm1>
        </Section>
      )}

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section header="Form 5 - Certificate of Approval" data-testid="form-5-section"></Section>
      )}

      <Section header="Form 8 - Notice of Advance Payment" data-testid="form-8-section"></Section>

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section header="Form 9 - Vesting Notice" data-testid="form-9-section"></Section>
      )}
    </>
  );
};

export default ExpropriationTabContainerView;
