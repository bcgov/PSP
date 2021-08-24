import { Input, Select, SelectOption } from 'components/common/form';
import { ParentSelect } from 'components/common/form/ParentSelect';
import FilterBar from 'components/SearchBar/FilterBar';
import { IUsersFilter } from 'interfaces';
import * as React from 'react';
import Col from 'react-bootstrap/Col';
import { ILookupCode } from 'store/slices/lookupCodes';
import { mapLookupCodeWithParentString } from 'utils';

interface IProps {
  value: IUsersFilter;
  organizationLookups: ILookupCode[];
  rolesLookups: ILookupCode[];
  onChange: (value: IUsersFilter) => void;
}

export const UsersFilterBar: React.FC<IProps> = ({
  value,
  organizationLookups,
  rolesLookups,
  onChange,
}) => {
  const organizationOptions = (organizationLookups ?? []).map(c =>
    mapLookupCodeWithParentString(c, organizationLookups),
  );
  const roleOptions = rolesLookups.map(rl => ({ label: rl.name, value: rl.name } as SelectOption));

  return (
    <FilterBar<IUsersFilter> initialValues={value} onChange={onChange}>
      <Col className="bar-item">
        <Input field="businessIdentifier" placeholder="IDIR/BCeID" />
      </Col>
      <Col className="bar-item">
        <Input field="firstName" placeholder="First name" />
      </Col>
      <Col className="bar-item">
        <Input field="surname" placeholder="Last name" />
      </Col>
      <Col className="bar-item">
        <Input field="email" placeholder="Email" />
      </Col>
      <Col className="bar-item">
        <Input field="position" placeholder="Position" />
      </Col>
      <Col className="bar-item">
        <ParentSelect
          field="organization"
          options={organizationOptions}
          filterBy={['code', 'label', 'parent']}
          placeholder="Enter an Organization"
        />
      </Col>
      <Col className="bar-item">
        <Select field="role" placeholder="Role" options={roleOptions} />
      </Col>
    </FilterBar>
  );
};
