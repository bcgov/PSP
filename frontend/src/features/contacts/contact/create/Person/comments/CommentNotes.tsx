import { TextArea } from 'components/common/form';
import * as Styled from 'features/contacts/contact/create/styles';
import * as React from 'react';

export interface ICommentNotesProps {}

/**
 * Displays comments directly associated with this Individual Contact.
 * @param {ICommentNotesProps} param0
 */
export const CommentNotes: React.FunctionComponent<ICommentNotesProps> = () => {
  return (
    <>
      <Styled.FormLabel>Comments</Styled.FormLabel>
      <Styled.SubtleText>(Optional)</Styled.SubtleText>
      <TextArea rows={5} field="comment" />
    </>
  );
};

export default CommentNotes;
