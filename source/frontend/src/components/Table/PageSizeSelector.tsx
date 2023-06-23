import React from 'react';
import Form from 'react-bootstrap/Form';
import styled from 'styled-components';

import { IMenuItemProps, Menu } from '@/components/menu/Menu';

/** align text to middle of size input */
const StyledText = styled.span`
  margin-top: 0.3rem;
`;

interface IProps {
  options: number[];
  value: number;
  onChange: (size: number) => void;
  alignTop: boolean;
}

export const TablePageSizeSelector: React.FC<React.PropsWithChildren<IProps>> = ({
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
      <div style={{ display: 'flex' }}>
        <StyledText>Show</StyledText>
        {/*the selector appears to capture the click event which prevents the page change from being registered*/}
        <Form.Control
          size="sm"
          value={`${selected}`}
          type="number"
          style={{ width: 50, marginLeft: 10, marginRight: 10, backgroundColor: 'white' }}
          disabled
        />
        <StyledText>Entries</StyledText>
      </div>
    </Menu>
  );
};
