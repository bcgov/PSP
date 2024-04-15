import noop from 'lodash/noop';
import { createContext, useContext, useState } from 'react';

const FilterContext = createContext<{
  changed: boolean;
  setChanged: (state: boolean) => void;
}>({ changed: true, setChanged: noop });

/**
 * Map filter change state manager,
 * helps the inventory layer zoom to the results when submitting the filter form
 */
export const FilterProvider: React.FC<React.PropsWithChildren<unknown>> = ({ children }) => {
  // Default changed state to false on page load
  const [changed, setChanged] = useState(false);

  return (
    <FilterContext.Provider value={{ changed, setChanged }}>{children}</FilterContext.Provider>
  );
};

export const useFilterContext = () => useContext(FilterContext);
