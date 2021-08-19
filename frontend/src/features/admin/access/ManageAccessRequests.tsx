import './ManageAccessRequests.scss';

import { Table } from 'components/Table';
import { AccessRequestStatus } from 'constants/accessStatus';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { IAccessRequest } from 'interfaces';
import * as React from 'react';
import Container from 'react-bootstrap/Container';
import { useDispatch } from 'react-redux';
import { useAppSelector } from 'store/hooks';
import {
  filterAccessRequestsAdmin,
  IAccessRequestsFilterData,
  updateAccessRequestPageIndex,
  useAccessRequests,
} from 'store/slices/accessRequests';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import { useTenant } from 'tenants';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { AccessRequestDetails } from './components/Details';
import { AccessRequestFilter } from './components/Filter';
import { columnDefinitions } from './constants/constants';
import { IAccessRequestModel } from './interfaces';

const ManageAccessRequests = () => {
  const dispatch = useDispatch();
  const tenant = useTenant();
  const [selectedRequest, setSelectedRequest] = React.useState<IAccessRequestModel | undefined>(
    undefined,
  );
  const columns = React.useMemo(() => columnDefinitions, []);
  const updateRequestAccessAdmin = useAppSelector(
    state => state.network[actionTypes.UPDATE_REQUEST_ACCESS_ADMIN] as IGenericNetworkAction,
  );

  const pagedAccessRequests = useAppSelector(state => state.accessRequests.pagedAccessRequests);
  const pageSize = useAppSelector(state => state.accessRequests?.pageSize);
  const pageIndex = useAppSelector(state => state.accessRequests.pageIndex);
  const filter = useAppSelector(state => state.accessRequests.filter);

  const { fetchAccessRequests } = useAccessRequests();

  React.useEffect(() => {
    if (!updateRequestAccessAdmin?.isFetching) {
      const paginateParams: API.IPaginateAccessRequests = toFilteredApiPaginateParams<
        IAccessRequestsFilterData
      >(pageIndex, pageSize, '', filter);
      paginateParams.status = AccessRequestStatus.OnHold;
      fetchAccessRequests(paginateParams);
    }
  }, [updateRequestAccessAdmin, pageSize, filter, pageIndex, fetchAccessRequests]);

  const requests = (pagedAccessRequests.items as IAccessRequest[]).map(
    ar =>
      ({
        id: ar.id as number,
        userId: ar.user.id as number,
        username: ar.user.username as string,
        firstName: ar.user.firstName as string,
        lastName: ar.user.lastName as string,
        email: ar.user.email as string,
        status: ar.status as string,
        note: ar.note as string,
        position: ar.user.position,
        organization:
          ar.organizations && ar.organizations.length !== 0 ? ar.organizations[0].name : '',
        role: ar.roles && ar.roles.length !== 0 ? ar.roles[0].name : '',
      } as IAccessRequestModel),
  );

  const showDetails = (req: IAccessRequestModel) => {
    setSelectedRequest(req);
  };

  return (
    <div className="manage-access-requests">
      <div className="ScrollContainer">
        <Container fluid className="TableToolbar">
          <span className="title mr-auto">{tenant.shortName} Guests (Pending Approval)</span>
        </Container>
        <div className="search-bar">
          <AccessRequestFilter
            initialValues={filter}
            applyFilter={filter => dispatch(filterAccessRequestsAdmin(filter))}
          />
        </div>
        {!!selectedRequest && (
          <AccessRequestDetails
            request={selectedRequest}
            onClose={() => setSelectedRequest(undefined)}
          />
        )}
        <Table<IAccessRequestModel>
          name="accessRequestsTable"
          columns={columns}
          data={requests}
          defaultCanSort={true}
          pageCount={Math.ceil(pagedAccessRequests.total / pageSize)}
          onRequestData={req => dispatch(updateAccessRequestPageIndex(req.pageIndex))}
          onRowClick={showDetails}
          clickableTooltip="Click user IDIR/BCeID to view User Information. Click row to open Access Request details."
        />
      </div>
    </div>
  );
};

export default ManageAccessRequests;
