import { Check } from 'components/common/form';
import { IContactFilter } from 'features/contacts/interfaces';
import { useFormikContext } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import * as React from 'react';

interface IActiveFilterCheckProps {
  setFilter: (filter: IContactFilter) => void;
}

/**
 * Trigger the ContactFilter to search automatically when this checkbox is toggled.
 * Broken out into a separate component for readability.
 * @param {IActiveFilterCheckProps} param0
 */
const ActiveFilterCheck: React.FunctionComponent<IActiveFilterCheckProps> = ({
  setFilter,
}: IActiveFilterCheckProps) => {
  const { values } = useFormikContext<IContactFilter>();
  const { activeContactsOnly } = values;
  useDeepCompareEffect(() => {
    setFilter(values);
  }, [activeContactsOnly, setFilter]);
  return <Check field="activeContactsOnly" className="d-inline-block m-0" />;
};

export default ActiveFilterCheck;
