import React from 'react';
import { Dropdown } from 'react-bootstrap';
import { FaEllipsisH } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';

export interface IMoreOptionsDropdownProps {
  /** Whether the "Clear All" action is enabled. When false, the option is grayed out and unclickable. */
  canClearAll?: boolean;

  /** Callback invoked when the "Clear All" option is clicked. */
  onClearAll: () => void;
}

/**
 * A dropdown menu component with options to operate on the property worklist
 */
const MoreOptionsDropdown: React.FC<IMoreOptionsDropdownProps> = ({
  onClearAll,
  canClearAll = true,
}) => {
  return (
    <Dropdown alignRight>
      <Dropdown.Toggle
        variant="light"
        id="dropdown-ellipsis"
        className="border-0 p-0 d-flex align-items-center"
      >
        <FaEllipsisH />
      </Dropdown.Toggle>

      <Dropdown.Menu>
        <Dropdown.Item disabled={!canClearAll} onClick={canClearAll ? onClearAll : undefined}>
          <MdClose className="mr-2" />
          Clear list
        </Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default MoreOptionsDropdown;
