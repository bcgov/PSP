import { LinkButton } from '@/components/common/buttons';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';

interface IGenerateItemViewProps {
  label: string;
  formType: ApiGen_CodeTypes_FormTypes;
  onGenerate: () => void;
}

const GenerateItemView: React.FunctionComponent<
  React.PropsWithChildren<IGenerateItemViewProps>
> = ({ label, formType, onGenerate }) => {
  return (
    <LinkButton key={`generate-form-entry-${formType}`} onClick={onGenerate} title="Download File">
      {label}
    </LinkButton>
  );
};

export default GenerateItemView;
