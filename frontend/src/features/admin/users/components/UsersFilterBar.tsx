import { Input, Select, SelectOption } from 'components/common/form';
import FilterBar from 'components/SearchBar/FilterBar';
import { IUsersFilter } from 'interfaces';
import React from 'react';
import Col from 'react-bootstrap/Col';
import { ILookupCode } from 'store/slices/lookupCodes';

interface IProps {
  value: IUsersFilter;
  rolesLookups: ILookupCode[];
  onChange: (value: IUsersFilter) => void;
}

export const UsersFilterBar: React.FC<IProps> = ({ value, rolesLookups, onChange }) => {
  const roleOptions = rolesLookups.map(rl => ({ label: rl.name, value: rl.name } as SelectOption));

  return (
    <FilterBar<IUsersFilter> initialValues={value} onChange={onChange} searchButtonType="submit">
      <Col className="bar-item">
        <Input field="businessIdentifierValue" placeholder="IDIR/BCeID" />
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
        <Select field="role" placeholder="Role" options={roleOptions} />
      </Col>
    </FilterBar>
  );
};
