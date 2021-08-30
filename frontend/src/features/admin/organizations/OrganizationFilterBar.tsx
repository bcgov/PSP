import { ParentSelect } from 'components/common/form/ParentSelect';
import { Label } from 'components/common/Label';
import FilterBar from 'components/SearchBar/FilterBar';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IOrganizationFilter } from 'interfaces';
import * as React from 'react';
import { mapLookupCodeWithParentString } from 'utils';

interface IProps {
  value: IOrganizationFilter;
  onChange: (value: IOrganizationFilter) => void;
  handleAdd: (value: any) => void;
}

export const OrganizationFilterBar: React.FC<IProps> = ({ value, onChange, handleAdd }) => {
  const lookupCodes = useLookupCodeHelpers();
  const organizationOptions = lookupCodes.getByType('Organization');
  const organizationWithParent = (organizationOptions ?? []).map(c =>
    mapLookupCodeWithParentString(c, organizationOptions),
  );
  return (
    <FilterBar<IOrganizationFilter>
      initialValues={value}
      onChange={onChange}
      searchClassName="bg-primary"
      plusButton={true}
      filterBarHeading="Organizations"
      handleAdd={handleAdd}
      toolTipAddId="organization-filter-add"
      toolTipAddText="Add a new Organization"
      customReset={() => {
        onChange?.({ id: '' });
      }}
      customResetField="id"
    >
      <Label>Search organization by name: </Label>
      <ParentSelect
        field="id"
        options={organizationWithParent}
        placeholder="Enter an Organization"
        filterBy={['parent', 'code', 'name']}
      />
    </FilterBar>
  );
};
