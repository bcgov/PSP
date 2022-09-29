import { TextArea } from 'components/common/form';
import { FlexBox } from 'components/common/styles';
import * as Styled from 'features/contacts/contact/create/styles';
import React from 'react';

export interface ICommentNotesProps {}

/**
 * Displays comments directly associated with this Contact.
 * @param {ICommentNotesProps} param0
 */
export const CommentNotes: React.FunctionComponent<ICommentNotesProps> = () => {
  return (
    <>
      <FlexBox gap="1.6rem">
        <Styled.FormLabel>Comments</Styled.FormLabel>
        <Styled.SubtleText>(Optional)</Styled.SubtleText>
      </FlexBox>
      <TextArea rows={5} field="comment" />
    </>
  );
};

export default CommentNotes;
