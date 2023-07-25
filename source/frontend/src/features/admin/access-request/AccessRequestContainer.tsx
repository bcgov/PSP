import * as React from 'react';
import { useEffect } from 'react';
import { Alert } from 'react-bootstrap';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { AccessRequestStatus } from '@/constants/accessStatus';
import { useAccessRequests } from '@/hooks/pims-api/useAccessRequests';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_AccessRequest } from '@/models/api/AccessRequest';

import { AccessRequestForm as AccessRequestFormComponent } from './AccessRequestForm';
import { FormAccessRequest } from './models';

export interface IAccessRequestContainerProps {
  accessRequestId?: number;
  onSave?: () => void;
  asAdmin?: boolean;
}

export const AccessRequestContainer: React.FunctionComponent<
  React.PropsWithChildren<IAccessRequestContainerProps>
> = ({ accessRequestId, onSave, asAdmin }) => {
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

  const initialValues: FormAccessRequest = new FormAccessRequest({
    ...response,
    id: response?.id,
    userId: userInfo?.id,
    accessRequestStatusTypeCode: { id: AccessRequestStatus.Received },
    note: response?.note ?? '',
    roleId: response?.roleId,
    regionCode: { id: response?.regionCode?.id },
  });
  initialValues.email = keycloak.email ?? '';

  if (!accessRequestId && !response) {
    initialValues.email = keycloak.email ?? '';
    initialValues.firstName = keycloak.firstName ?? '';
    initialValues.surname = keycloak.surname ?? '';
    initialValues.businessIdentifierValue =
      keycloak.businessIdentifierValue ?? userInfo.idir_username ?? '';
    initialValues.keycloakUserGuid = userInfo.subject;
  }
  return (
    <>
      <LoadingBackdrop parentScreen show={loading || addLoading || byIdLoading} />
      {response?.id && !asAdmin && (
        <Alert variant="success">Your access request has been submitted</Alert>
      )}
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
