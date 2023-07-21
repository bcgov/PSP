import * as React from 'react';

import { TextArea } from '@/components/common/form';
import { withNameSpace } from '@/utils/formUtils';

import { LeaseH5 } from '../../styles';

export interface ITenantNotesProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Displays all notes directly associated with this lease tenant.
 * @param {ITenantNotesProps} param0
 */
export const TenantNotes: React.FunctionComponent<React.PropsWithChildren<ITenantNotesProps>> = ({
  nameSpace,
  disabled,
}) => {
  return (
    <>
      <LeaseH5>Notes</LeaseH5>
      <TextArea
        innerClassName="notes"
        disabled={disabled}
        field={withNameSpace(nameSpace, 'note')}
      />
    </>
  );
};

export default TenantNotes;
