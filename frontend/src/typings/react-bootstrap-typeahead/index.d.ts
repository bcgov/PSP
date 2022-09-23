import 'react-bootstrap-typeahead';

declare module 'react-bootstrap-typeahead' {
  // This interface is missing from default typing file
  // See https://github.com/ericgio/react-bootstrap-typeahead/blob/master/src/types.ts
  export interface TypeaheadManagerChildProps {
    activeIndex: number;
    getInputProps: (props: HTMLProps<HTMLInputElement>) => InputProps;
    hideMenu: () => void;
    isMenuShown: boolean;
    labelKey: LabelKey;
    onClear: () => void;
    onHide: () => void;
    onRemove: OptionHandler;
    results: Option[];
    selected: Option[];
    text: string;
    toggleMenu: () => void;
  }
}
