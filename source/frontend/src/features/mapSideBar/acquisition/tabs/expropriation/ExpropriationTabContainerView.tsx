import { useHistory, useRouteMatch } from 'react-router-dom';
import { toast } from 'react-toastify';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { Claims } from '@/constants';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';

import { useGenerateExpropriationForm1 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm1';
import { useGenerateExpropriationForm5 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm5';
import { useGenerateExpropriationForm8 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm8';
import { useGenerateExpropriationForm9 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm9';
import ExpropriationForm1 from './form1/ExpropriationForm1';
import ExpropriationForm5 from './form5/ExpropriationForm5';
import ExpropriationForm8Details from './form8/details/ExpropriationForm8Details';
import ExpropriationForm9 from './form9/ExpropriationForm9';

export interface IExpropriationTabContainerViewProps {
  loading: boolean;
  acquisitionFile: Api_AcquisitionFile;
  form8s: Api_ExpropriationPayment[];
  onForm8Deleted: (form8Id: number) => void;
}

export const ExpropriationTabContainerView: React.FunctionComponent<
  IExpropriationTabContainerViewProps
> = ({ loading, acquisitionFile, form8s, onForm8Deleted }) => {
  const history = useHistory();
  const match = useRouteMatch();

  const acquisitionFileTypeCode = acquisitionFile.acquisitionTypeCode?.id;

  const onGenerateForm1 = useGenerateExpropriationForm1();
  const onGenerateForm5 = useGenerateExpropriationForm5();
  const onGenerateForm8 = useGenerateExpropriationForm8();
  const onGenerateForm9 = useGenerateExpropriationForm9();

  const onError = (error: Error) => {
    if (error) {
      toast.error(error?.message, { autoClose: 7000 });
    }
  };

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
          <ExpropriationForm1
            acquisitionFile={acquisitionFile}
            onGenerate={onGenerateForm1}
            onError={onError}
          ></ExpropriationForm1>
        </Section>
      )}

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header="Form 5 - Certificate of Approval of Expropriation"
          data-testid="form-5-section"
        >
          <ExpropriationForm5
            acquisitionFile={acquisitionFile}
            onGenerate={onGenerateForm5}
            onError={onError}
          ></ExpropriationForm5>
        </Section>
      )}

      <Section
        isCollapsable
        initiallyExpanded={true}
        data-testid="form-8-section"
        header={
          <SectionListHeader
            claims={[Claims.ACQUISITION_EDIT]}
            title="Form 8 - Notice of Advance Payment"
            addButtonText="Add Form 8"
            addButtonIcon={'add'}
            onAdd={() => {
              history.push(`${match.url}/add`);
            }}
          />
        }
      >
        {form8s.map((form, index) => (
          <ExpropriationForm8Details
            key={index}
            form8={form}
            form8Index={index}
            acquisitionFileNumber={acquisitionFile.fileNumber ?? ''}
            onGenerate={onGenerateForm8}
            onDelete={() => onForm8Deleted(form.id!)}
          ></ExpropriationForm8Details>
        ))}
      </Section>

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header="Form 9 - Vesting Notice"
          data-testid="form-9-section"
        >
          <ExpropriationForm9
            acquisitionFile={acquisitionFile}
            onGenerate={onGenerateForm9}
            onError={onError}
          ></ExpropriationForm9>
        </Section>
      )}
    </>
  );
};

export default ExpropriationTabContainerView;
