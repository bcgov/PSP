import { FormikProps, getIn } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { RemoveButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import OverflowTip from '@/components/common/OverflowTip';
import AreaContainer from '@/components/measurements/AreaContainer';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { FormLeaseProperty, LeaseFormModel } from '@/features/leases/models';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { withNameSpace } from '@/utils/formUtils';
import { getPropertyName, NameSourceType } from '@/utils/mapPropertyUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: PropertyForm;
  formikProps: FormikProps<LeaseFormModel>;
  showSeparator?: boolean;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  property,
  formikProps,
  showSeparator = false,
}) => {
  const propertyName = getPropertyName(property.toMapProperty());
  let propertyIdentifier = '';
  switch (propertyName.label) {
    case NameSourceType.PID:
    case NameSourceType.PIN:
    case NameSourceType.PLAN:
    case NameSourceType.ADDRESS:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.LOCATION:
      propertyIdentifier = `${propertyName.value}`;
      break;
    default:
      propertyIdentifier = '';
      break;
  }

  const currentLeaseProperty: FormLeaseProperty = getIn(
    formikProps.values,
    withNameSpace(nameSpace),
  );

  return (
    <>
      <Row className="align-items-center my-3 no-gutters">
        <Col md={3} className="mb-0 d-flex align-items-center">
          <DraftCircleNumber text={(index + 1).toString()} />
          <OverflowTip fullText={propertyIdentifier} className="pl-3" />
        </Col>
        <Col md={7}>
          <InlineInput
            className="mb-0 w-100 pr-3"
            label=""
            field={withNameSpace(nameSpace, 'name')}
            displayErrorTooltips={true}
          />
        </Col>
        <Col md={2}>
          <RemoveButton onRemove={onRemove} />
        </Col>
      </Row>
      <Row className="align-items-center mb-3 no-gutters">
        <Col md={{ span: 9, offset: 3 }}>
          <AreaContainer
            isEditable
            field={withNameSpace(nameSpace, 'landArea')}
            landArea={currentLeaseProperty.landArea}
            unitCode={currentLeaseProperty.areaUnitTypeCode}
            onChange={(landArea, areaUnitTypeCode) => {
              formikProps.setFieldValue(withNameSpace(nameSpace, 'landArea'), landArea);
              formikProps.setFieldValue(
                withNameSpace(nameSpace, 'areaUnitTypeCode'),
                areaUnitTypeCode,
              );
            }}
          />
        </Col>
      </Row>
      {showSeparator && <hr className="my-3"></hr>}
    </>
  );
};

export default SelectedPropertyRow;
