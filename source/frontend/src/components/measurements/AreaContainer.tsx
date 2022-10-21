import { AreaForm } from './AreaForm';
import AreaView from './AreaView';

interface ViewOnlyProps {
  landArea?: number;
  unitCode?: string;

  isEdditable?: never;
  onChange?: never;
}

interface EditableProps {
  landArea?: number;
  unitCode?: string;

  isEdditable: boolean;
  onChange: (landArea: number, areaUnitTypeCode: string) => void;
}

type IAreaContainerProps = EditableProps | ViewOnlyProps;

function isViewOnly(areaComponent: ViewOnlyProps | EditableProps): areaComponent is ViewOnlyProps {
  var editable = areaComponent as EditableProps;
  return editable.onChange === undefined || editable.isEdditable === false;
}

const AreaContainer: React.FunctionComponent<IAreaContainerProps> = props => {
  if (isViewOnly(props)) {
    return <AreaView landArea={props.landArea} unitCode={props.unitCode} />;
  } else {
    return (
      <AreaForm area={props.landArea} areaUnitTypeCode={props.unitCode} onChange={props.onChange} />
    );
  }
};

export default AreaContainer;
