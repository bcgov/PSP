import { TextArea } from '@/components/common/form';
import * as Styled from '@/features/leases/detail/styles';
import { withNameSpace } from '@/utils/formUtils';

export interface IDetailNotesProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Displays all notes directly associated with this lease.
 * @param {IDetailNotesProps} param0
 */
export const DetailNotes: React.FunctionComponent<React.PropsWithChildren<IDetailNotesProps>> = ({
  nameSpace,
  disabled,
}) => {
  return (
    <>
      <Styled.FormDescriptionLabel>Lease Notes</Styled.FormDescriptionLabel>
      <TextArea disabled={disabled} field={withNameSpace(nameSpace, 'note')} />
    </>
  );
};

export default DetailNotes;
