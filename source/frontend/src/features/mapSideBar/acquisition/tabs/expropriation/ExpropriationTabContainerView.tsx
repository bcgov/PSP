import { FormikHelpers, FormikProps } from 'formik';
import { useRef } from 'react';
import { FaFileContract, FaPlus } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants';
import { EnumAcquisitionFileType } from '@/constants/acquisitionFileType';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';

import { cannotEditMessage } from '../../common/constants';
import { useGenerateExpropriationForm1 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm1';
import { useGenerateExpropriationForm5 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm5';
import { useGenerateExpropriationForm8 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm8';
import { useGenerateExpropriationForm9 } from '../../common/GenerateForm/hooks/useGenerateExpropriationForm9';
import ExpropriationForm1 from './form1/ExpropriationForm1';
import ExpropriationForm5 from './form5/ExpropriationForm5';
import ExpropriationForm8Details from './form8/details/ExpropriationForm8Details';
import ExpropriationForm9 from './form9/ExpropriationForm9';
import ExpropriationEventHistoryContainer from './history/ExpropriationEventHistoryContainer';
import ExpropriationEventHistoryView from './history/ExpropriationEventHistoryView';
import ExpropriationEventModal from './history/modal/ExpropriationEventModal';
import {
  ExpropriationForm1Model,
  ExpropriationForm5Model,
  ExpropriationForm9Model,
} from './models';

export interface IExpropriationTabContainerViewProps {
  loading: boolean;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  form8s: ApiGen_Concepts_ExpropriationPayment[];
  isFileFinalStatus: boolean;
  onForm8Deleted: (form8Id: number) => void;
}

export const ExpropriationTabContainerView: React.FunctionComponent<
  IExpropriationTabContainerViewProps
> = ({ loading, acquisitionFile, form8s, onForm8Deleted, isFileFinalStatus }) => {
  const history = useHistory();
  const match = useRouteMatch();

  const acquisitionFileTypeCode = acquisitionFile.acquisitionTypeCode?.id;
  const formikRefForm1 = useRef<FormikProps<ExpropriationForm1Model>>(null);
  const formikRefForm5 = useRef<FormikProps<ExpropriationForm5Model>>(null);
  const formikRefForm9 = useRef<FormikProps<ExpropriationForm9Model>>(null);

  const onGenerateForm1 = useGenerateExpropriationForm1();
  const onGenerateForm5 = useGenerateExpropriationForm5();
  const onGenerateForm8 = useGenerateExpropriationForm8();
  const onGenerateForm9 = useGenerateExpropriationForm9();

  const handleGenerateForm1 = async (
    values: ExpropriationForm1Model,
    formikHelpers: FormikHelpers<ExpropriationForm1Model>,
  ) => {
    try {
      if (acquisitionFile.id) {
        await onGenerateForm1(acquisitionFile.id, values);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const onGenerateForm1Click = () => {
    formikRefForm1.current?.setSubmitting(true);
    formikRefForm1.current?.submitForm();
  };

  const handleGenerateForm5 = async (
    values: ExpropriationForm5Model,
    formikHelpers: FormikHelpers<ExpropriationForm5Model>,
  ) => {
    try {
      if (acquisitionFile.id) {
        await onGenerateForm5(acquisitionFile.id, values);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const onGenerateForm5Click = () => {
    formikRefForm5.current?.setSubmitting(true);
    formikRefForm5.current?.submitForm();
  };

  const handleGenerateForm9 = async (
    values: ExpropriationForm9Model,
    formikHelpers: FormikHelpers<ExpropriationForm9Model>,
  ) => {
    try {
      if (acquisitionFile.id) {
        await onGenerateForm9(acquisitionFile.id, values);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const onGenerateForm9Click = () => {
    formikRefForm9.current?.setSubmitting(true);
    formikRefForm9.current?.submitForm();
  };

  return (
    <>
      <LoadingBackdrop show={loading} />

      <ExpropriationEventHistoryContainer
        acquisitionFileId={acquisitionFile.id}
        View={ExpropriationEventHistoryView}
        ModalView={ExpropriationEventModal}
      />

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded
          header={
            <SectionListHeader
              title="Form 1 - Notice of Expropriation"
              addButtonText="Generate Form 1"
              addButtonIcon={<FaFileContract size={'2rem'} />}
              onGenerate={onGenerateForm1Click}
            />
          }
          data-testid="form-1-section"
        >
          <ExpropriationForm1
            acquisitionFile={acquisitionFile}
            formikRef={formikRefForm1}
            onGenerate={(
              values: ExpropriationForm1Model,
              formikHelpers: FormikHelpers<ExpropriationForm1Model>,
            ) => {
              handleGenerateForm1(values, formikHelpers);
            }}
          ></ExpropriationForm1>
        </Section>
      )}

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header={
            <SectionListHeader
              title="Form 5 - Certificate of Approval of Expropriation"
              addButtonText="Generate Form 5"
              addButtonIcon={<FaFileContract size={'2rem'} />}
              onGenerate={onGenerateForm5Click}
            />
          }
          data-testid="form-5-section"
        >
          <ExpropriationForm5
            acquisitionFile={acquisitionFile}
            formikRef={formikRefForm5}
            onGenerate={(
              values: ExpropriationForm5Model,
              formikHelpers: FormikHelpers<ExpropriationForm5Model>,
            ) => {
              handleGenerateForm5(values, formikHelpers);
            }}
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
            addButtonIcon={<FaPlus size={'2rem'} />}
            onAdd={() => {
              history.push(`${match.url}/add`);
            }}
            cannotAddComponent={
              <TooltipIcon
                toolTipId={`deposit-notes-cannot-edit-tooltip`}
                toolTip={cannotEditMessage}
              />
            }
            isAddEnabled={!isFileFinalStatus}
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
            onDelete={() => form?.id && onForm8Deleted(form.id)}
            isFileFinalStatus={isFileFinalStatus}
          ></ExpropriationForm8Details>
        ))}
      </Section>

      {acquisitionFileTypeCode === EnumAcquisitionFileType.SECTN6 && (
        <Section
          isCollapsable
          initiallyExpanded={false}
          header={
            <SectionListHeader
              title="Form 9 - Vesting Notice"
              addButtonText="Generate Form 9"
              addButtonIcon={<FaFileContract size={'2rem'} />}
              onGenerate={onGenerateForm9Click}
            />
          }
          data-testid="form-9-section"
        >
          <ExpropriationForm9
            acquisitionFile={acquisitionFile}
            formikRef={formikRefForm9}
            onGenerate={(
              values: ExpropriationForm9Model,
              formikHelpers: FormikHelpers<ExpropriationForm9Model>,
            ) => {
              handleGenerateForm9(values, formikHelpers);
            }}
          ></ExpropriationForm9>
        </Section>
      )}
    </>
  );
};

export default ExpropriationTabContainerView;
