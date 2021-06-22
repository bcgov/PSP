import GenericModal from 'components/common/GenericModal';
import { pidFormatter } from 'features/properties/components/forms/subforms/PidPinForm';
import queryString from 'query-string';
import { useState } from 'react';
import { useHistory } from 'react-router-dom';

/**
 * Hook that provides a modal based on the value of the duplicatePid state variable.
 * The modal allows the user to open a new tab which will zoom to the location of the duplicated pid on the map.
 */
export const useDuplicatePidModal = () => {
  const [duplicatePid, setDuplicatePid] = useState<string | undefined>();
  const history = useHistory();

  const ErrorModal = (
    <GenericModal
      display={!!duplicatePid}
      title=""
      message={
        <p>
          The parcel identifier (PID) {duplicatePid && pidFormatter(duplicatePid)} has already been
          added to the system. Duplicate PID values are not allowed. Would you like to view the
          previously entered value on the map?
        </p>
      }
      okButtonText="Yes"
      cancelButtonText="No"
      handleOk={() => {
        const search = {
          ...queryString.parse(history.location.search),
          pid: duplicatePid && pidFormatter(duplicatePid),
          sidebar: false,
        };
        window.open(`/mapview?${queryString.stringify(search)}`);
        setDuplicatePid(undefined);
      }}
      handleCancel={() => {
        setDuplicatePid(undefined);
      }}
    ></GenericModal>
  );

  return { ErrorModal, setDuplicatePid };
};
