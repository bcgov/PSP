import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';

import ExpropriationForm1 from './form1/ExpropriationForm1';
import ExpropriationForm5 from './form5/ExpropriationForm5';
import ExpropriationForm9 from './form9/ExpropriationForm9';

export interface IExpropriationTabContainerViewProps {
  loading: boolean;
  acquisitionFile: Api_AcquisitionFile;
}

export const ExpropriationTabContainerView: React.FunctionComponent<
  IExpropriationTabContainerViewProps
> = ({ loading, acquisitionFile }) => {
  // TODO: Load Form 8 into this container
  const acquisitionFileTypeCode = acquisitionFile.acquisitionTypeCode?.id;

  return (
    <>
      <LoadingBackdrop show={loading} />
      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded
          header="Form 1 - Notice of Expropriation"
          data-testid="form-1-section"
        >
          <ExpropriationForm1 acquisitionFile={acquisitionFile}></ExpropriationForm1>
        </Section>
      )}

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header="Form 5 - Certificate of Approval"
          data-testid="form-5-section"
        >
          <ExpropriationForm5 acquisitionFile={acquisitionFile}></ExpropriationForm5>
        </Section>
      )}

      <Section
        isCollapsable
        initiallyExpanded={false}
        header="Form 8 - Notice of Advance Payment"
        data-testid="form-8-section"
      ></Section>

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header="Form 9 - Vesting Notice"
          data-testid="form-9-section"
        >
          <ExpropriationForm9 acquisitionFile={acquisitionFile}></ExpropriationForm9>
        </Section>
      )}
    </>
  );
};

export default ExpropriationTabContainerView;
