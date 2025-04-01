import find from 'lodash/find';
import noop from 'lodash/noop';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Table } from '@/components/Table';
import { AreaUnitTypes } from '@/constants';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getApiPropertyName, prettyFormatUTCDate, prettyFormatUTCTime } from '@/utils';

import { IOperationFileAssociationsContainerProps } from './OperationFileAssocationsContainer';
import getPropertyOperationColumns from './propertyOperationColumns';

export interface IOperationViewProps {
  operationType: ApiGen_CodeTypes_PropertyOperationTypes;
  operationTimeStamp: UtcIsoDateTime;
  sourceProperties: ApiGen_Concepts_Property[];
  destinationProperties: ApiGen_Concepts_Property[];
  ExpandedRowComponent: React.FunctionComponent<IOperationFileAssociationsContainerProps>;
}

export interface PropertyOperationResult {
  id: number;
  isSource: boolean;
  identifier: string;
  plan: string;
  status: string;
  area: number;
  areaUnitCode: string;
}

export const OperationView: React.FunctionComponent<IOperationViewProps> = ({
  operationType,
  operationTimeStamp,
  sourceProperties,
  destinationProperties,
  ExpandedRowComponent,
}) => {
  const toOperationResult = (property: ApiGen_Concepts_Property, isSource: boolean) => {
    const propertyName = getApiPropertyName(property);
    return {
      id: property.id,
      isSource: isSource,
      identifier: `${propertyName.label}: ${propertyName.value}`,
      plan: property.planNumber?.toString() ?? '',
      status: property.isRetired ? 'Retired' : 'Active',
      area: property.landArea ?? 0,
      areaUnitCode: property.areaUnit?.id ?? AreaUnitTypes.SquareMeters,
    };
  };

  let operationData = sourceProperties.map<PropertyOperationResult>(o => {
    return toOperationResult(o, true);
  });

  operationData = operationData.concat(
    destinationProperties.map<PropertyOperationResult>(o => {
      return toOperationResult(o, false);
    }),
  );

  /** This is the operation association subtable displayed for each term row. */
  const renderOperationAssociations = useDeepCompareMemo(
    () => (row: PropertyOperationResult) => {
      const matchingOperation = operationData.find(t => t.id === row.id);
      return <ExpandedRowComponent operation={matchingOperation}></ExpandedRowComponent>;
    },
    [ExpandedRowComponent],
  );

  return (
    <Section
      header={
        <SectionField label="Created on" labelWidth={{ xs: 'auto' }} className="">
          {prettyFormatUTCDate(operationTimeStamp)} at {prettyFormatUTCTime(operationTimeStamp)}
        </SectionField>
      }
      isStyledHeader
      initiallyExpanded
      isCollapsable
      noPadding
    >
      <Table<PropertyOperationResult>
        name="propertyOperationTable"
        columns={getPropertyOperationColumns(
          ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE === operationType,
        )}
        data={operationData}
        hideToolbar
        manualPagination
        disableSelection={true}
        canRowExpand={() => true}
        detailsPanel={{
          render: renderOperationAssociations,
          onExpand: noop,
          checkExpanded: (row: PropertyOperationResult, state: PropertyOperationResult[]) =>
            !!find(state, operation => operation.id === row.id),
          getRowId: (row: PropertyOperationResult) => row.id,
          icons: { open: <MdArrowDropDown size={24} />, closed: <MdArrowRight size={24} /> },
        }}
        noRowsMessage={`This property is not part of a ${
          operationType === ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE
            ? 'consolidation'
            : 'subdivision'
        }`}
      ></Table>
    </Section>
  );
};
