import { TextArea } from 'components/common/form';
import { Stack } from 'components/common/Stack/Stack';
import * as Styled from 'features/contacts/contact/create/styles';
import React from 'react';

export interface ICommentNotesProps {}

/**
 * Displays comments directly associated with this Contact Person.
 * @param {ICommentNotesProps} param0
 */
export const CommentNotes: React.FunctionComponent<ICommentNotesProps> = () => {
  return (
    <>
      <Stack $direction="row" gap={1.6}>
        <Styled.FormLabel>Comments</Styled.FormLabel>
        <Styled.SubtleText>(Optional)</Styled.SubtleText>
      </Stack>
      <TextArea rows={5} field="comment" />
    </>
  );
};

export default CommentNotes;
