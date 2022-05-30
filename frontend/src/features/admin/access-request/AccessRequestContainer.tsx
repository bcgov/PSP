import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { AccessRequestStatus } from 'constants/accessStatus';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';
import { useEffect } from 'react';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';

import { AccessRequestForm as AccessRequestFormComponent } from './AccessRequestForm';
import { AccessRequestForm } from './models';

interface IAccessRequestContainerProps {}

export const AccessRequestContainer: React.FunctionComponent<IAccessRequestContainerProps> = props => {
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
  } = useAccessRequests();
  const keycloak = useKeycloakWrapper();
  const userInfo = keycloak?.obj?.userInfo;

  useEffect(() => {
    getCurrentAccessRequest();
  }, [getCurrentAccessRequest]);
  const response = addAccessRequestResponse ?? accessRequestResponse;

  const initialValues: AccessRequestForm = new AccessRequestForm({
    ...response,
    id: response?.id,
    userId: userInfo?.id,
    accessRequestStatusTypeCode: { id: AccessRequestStatus.Received },
    note: response?.note ?? '',
    roleId: response?.roleId,
    regionCode: { id: response?.regionCode?.id },
  });

  initialValues.email = keycloak.email ?? '';
  initialValues.firstName = keycloak.firstName ?? '';
  initialValues.surname = keycloak.surname ?? '';
  initialValues.businessIdentifierValue =
    response?.user?.businessIdentifierValue ?? keycloak.businessIdentifierValue ?? '';
  initialValues.keycloakUserGuid = userInfo.subject;
  return (
    <>
      <LoadingBackdrop parentScreen show={loading || addLoading} />
      <AccessRequestFormComponent
        initialValues={initialValues}
        addAccessRequest={addAccessRequest}
      />
    </>
  );
};

export default AccessRequestContainer;
