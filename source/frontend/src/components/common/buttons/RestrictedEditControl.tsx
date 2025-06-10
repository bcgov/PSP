import React, { CSSProperties } from 'react';

import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';

import TooltipIcon from '../TooltipIcon';
import EditButton from './EditButton';

type RestrictedEditControlProps = {
  /**
   * Whether the current user has permission to edit this section.
   */
  canEdit: boolean;

  /**
   * Whether the section is in a non-editable state due to business logic.
   * If true, a tooltip is shown instead of the Edit button.
   */
  isInNonEditableState: boolean;

  /**
   * Optional custom message shown in the tooltip when the section is in a restricted state.
   */
  editRestrictionMessage?: string;

  /**
   * Callback triggered when the Edit button is clicked.
   */
  onEdit: () => void;

  // additional props to pass to Edit button
  title?: string;
  icon?: React.ReactNode;
  'data-testId'?: string;
  style?: CSSProperties | null;
  toolTipId?: string;
};

/**
 * `RestrictedEditControl` renders an Edit button or a tooltip based on access control logic.
 *
 * - Renders nothing if the user cannot edit (`canEdit` is false).
 * - Shows a tooltip if the section is non-editable due to business rules (`isInNonEditableState` is true).
 * - Shows an Edit button and triggers `onEdit` when clicked, if editing is allowed.
 *
 * This component is designed for admin panels or similar environments where conditional edit access is common.
 */
export const RestrictedEditControl: React.FC<RestrictedEditControlProps> = ({
  canEdit,
  isInNonEditableState,
  editRestrictionMessage = cannotEditMessage,
  title,
  icon,
  style,
  'data-testId': dataTestId,
  toolTipId = 'summary-cannot-edit-tooltip',
  onEdit,
}) => {
  if (!canEdit) {
    return null;
  }

  // If the section is in a non-editable state due to business logic, show a tooltip instead of the Edit button.
  if (isInNonEditableState) {
    return <TooltipIcon toolTipId={toolTipId} toolTip={editRestrictionMessage} />;
  }

  return (
    <EditButton title={title} icon={icon} style={style} data-testId={dataTestId} onClick={onEdit} />
  );
};

export default RestrictedEditControl;
