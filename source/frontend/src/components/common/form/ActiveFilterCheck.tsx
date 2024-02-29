import { getIn, useFormikContext } from 'formik';

import { Check } from '@/components/common/form';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

interface IActiveFilterCheckProps<T> {
  setFilter: (filter: T) => void;
  fieldName: string;
}

/**
 * Generic checkbox to search automatically when this checkbox is toggled.
 * Broken out into a separate component for readability.
 * @param {IActiveFilterCheckProps} param0
 */
function ActiveFilterCheck<T>({ setFilter, fieldName }: IActiveFilterCheckProps<T>) {
  const { values } = useFormikContext<T>();
  const active = getIn(values, fieldName);
  useDeepCompareEffect(() => {
    setFilter(values);
  }, [active, setFilter]);
  return <Check field={fieldName} className="d-inline-block m-0" />;
}

export default ActiveFilterCheck;
