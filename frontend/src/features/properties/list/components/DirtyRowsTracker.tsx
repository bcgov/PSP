import { getIn, useFormikContext } from 'formik';
import isEmpty from 'lodash/isEmpty';
import { useEffect } from 'react';

interface IChangedRow {
  rowId: number;
  assessedLand?: boolean;
  assessedBuilding?: boolean;
  netBook?: boolean;
  market?: boolean;
}

/**
 *  Component to track edited rows in the formik table
 * @param param0 {setDirtyRows: event listener for changed rows}
 */
export const DirtyRowsTracker: React.FC<{ setDirtyRows: (changes: IChangedRow[]) => void }> = ({
  setDirtyRows,
}) => {
  const { touched, isSubmitting } = useFormikContext();

  useEffect(() => {
    if (!!touched && !isEmpty(touched) && !isSubmitting) {
      const changed = Object.keys(getIn(touched, 'properties')).map(key => ({
        rowId: Number(key),
        ...getIn(touched, 'properties')[key],
      }));
      setDirtyRows(changed);
    }
  }, [touched, setDirtyRows, isSubmitting]);

  return null;
};
