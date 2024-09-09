import React from 'react';
import Form from 'react-bootstrap/Form';
import styled from 'styled-components';

import { IMenuItemProps, Menu } from '@/components/menu/Menu';

export interface ITablePageSizeSelectorProps {
  options: number[];
  value: number;
  onChange: (size: number) => void;
  alignTop: boolean;
}

export const TablePageSizeSelector: React.FC<ITablePageSizeSelectorProps> = ({
  options,
  value,
  onChange,
  alignTop,
}) => {
  const [selected, setSelected] = React.useState(value);

  const handleValueChange = (newSelection: number) => {
    if (newSelection !== selected) {
      setSelected(newSelection);
      onChange(newSelection);
    }
  };

  const pageSizeOptions: IMenuItemProps[] = options.map(option => ({
    label: option,
    value: option,
    onClick: () => handleValueChange(option),
  }));
  return (
    <Menu options={pageSizeOptions} alignTop={alignTop}>
      <div className="d-flex">
        <StyledText>Show</StyledText>
        <StyledInput defaultValue={`${selected}`} type="number" />
        <StyledText>Entries</StyledText>
      </div>
    </Menu>
  );
};

/** align text to middle of size input */
const StyledText = styled.span`
  margin-top: 0.3rem;
`;

const StyledInput = styled(Form.Control)`
  min-width: 5rem;
  max-width: 5rem;
  margin-left: 1rem;
  margin-right: 1rem;
  &:disabled {
    background: white;
  }
  text-align: center;
  padding: 0;
`;
