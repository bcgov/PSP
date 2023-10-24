import Multiselect from 'multiselect-react-dropdown';
import React from 'react';

export interface IMultiselectTextListProps {
  /**  Values to show in the list */
  values: any[];

  /**
   * Whether 'values' is an array of objects or not (true by default).
   * Make it false to display flat array of string or number Ex. ['Test1', 'Test2']
   */
  isObject?: boolean;

  /** Property name in the object to display */
  displayValue?: string;
}

export const MultiselectTextList: React.FC<IMultiselectTextListProps> = ({
  values,
  isObject = true,
  displayValue,
}) => {
  return (
    <Multiselect
      disable
      disablePreSelectedValues
      hidePlaceholder
      placeholder=""
      isObject={isObject}
      selectedValues={values}
      displayValue={displayValue ?? 'description'}
      style={readOnlyStyle}
    />
  );
};

const readOnlyStyle = {
  multiselectContainer: {
    opacity: 1,
  },
  searchBox: {
    border: 'none',
    padding: 0,
  },
  chips: {
    opacity: 1,
    background: '#F2F2F2',
    borderRadius: '4px',
    color: 'black',
    fontSize: '16px',
    marginRight: '1em',
  },
};
