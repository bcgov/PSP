import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { AccessRequestStatus } from 'constants/accessStatus';
import { useAccessRequests } from 'hooks/pims-api/useAccessRequests';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_AccessRequest } from 'models/api/AccessRequest';
import * as React from 'react';
import { useEffect } from 'react';

import { AccessRequestForm as AccessRequestFormComponent } from './AccessRequestForm';
import { AccessRequestForm } from './models';

interface IAccessRequestContainerProps {
  accessRequestId?: number;
  onSave?: () => void;
}

export const AccessRequestContainer: React.FunctionComponent<IAccessRequestContainerProps> = ({
  accessRequestId,
  onSave,
}) => {
  const {
    fetchCurrentAccessRequest: {
      loading,
      response: accessRequestResponse,
      execute: getCurrentAccessRequest,
    },
    addAccessRequest: {
      loading: addLoading,
      response: addAccessRequestResponse,
      execute: addAccessRequest,
    },
    fetchAccessRequestById: {
      loading: byIdLoading,
      response: accessRequestByIdResponse,
      execute: getAccessRequestById,
    },
  } = useAccessRequests();
  const keycloak = useKeycloakWrapper();
  const userInfo = keycloak?.obj?.userInfo;

  useEffect(() => {
    if (!accessRequestId) {
      getCurrentAccessRequest();
    } else {
      getAccessRequestById(accessRequestId);
    }
  }, [getCurrentAccessRequest, getAccessRequestById, accessRequestId]);
  const response = addAccessRequestResponse ?? accessRequestByIdResponse ?? accessRequestResponse;

  const initialValues: AccessRequestForm = new AccessRequestForm({
    ...response,
    id: response?.id,
    userId: userInfo?.id,
    accessRequestStatusTypeCode: { id: AccessRequestStatus.Received },
    note: response?.note ?? '',
    roleId: response?.roleId,
    regionCode: { id: response?.regionCode?.id },
  });

  if (!accessRequestId) {
    initialValues.email = keycloak.email ?? '';
    initialValues.firstName = keycloak.firstName ?? '';
    initialValues.surname = keycloak.surname ?? '';
    initialValues.businessIdentifierValue = keycloak.businessIdentifierValue ?? '';
    initialValues.keycloakUserGuid = userInfo.subject;
  }
  return (
    <>
      <LoadingBackdrop parentScreen show={loading || addLoading || byIdLoading} />
      <AccessRequestFormComponent
        initialValues={initialValues}
        addAccessRequest={async (accessRequest: Api_AccessRequest) => {
          const response = await addAccessRequest(accessRequest);
          onSave && onSave();
          return response;
        }}
        onCancel={onSave}
      />
    </>
  );
};

export default AccessRequestContainer;
