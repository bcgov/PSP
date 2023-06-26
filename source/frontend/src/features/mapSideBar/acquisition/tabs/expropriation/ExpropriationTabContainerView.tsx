import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';

export interface IExpropriationTabcontainerViewProps {
  loading: boolean;
  acquisitionFileTypeCode: string;
}

export const ExpropriationTabcontainerView: React.FunctionComponent<
  IExpropriationTabcontainerViewProps
> = ({ loading, acquisitionFileTypeCode }) => {
  // TODO : Load created Forms

  return (
    <>
      <LoadingBackdrop show={loading} />
      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section header="Form 1 - Notice of Expropiation" data-testid="form-1-section"></Section>
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

export default ExpropriationTabcontainerView;
