import { DisplayError } from '@/components/common/form';

import { AreaForm } from './AreaForm';
import AreaView from './AreaView';

interface ViewOnlyProps {
  landArea?: number;
  unitCode?: string;

  isEditable?: never;
  onChange?: never;
}

interface EditableProps {
  landArea?: number;
  unitCode?: string;
  field?: string;

  isEditable: boolean;
  onChange: (landArea: number, areaUnitTypeCode: string) => void;
}

type IAreaContainerProps = EditableProps | ViewOnlyProps;

function isViewOnly(areaComponent: ViewOnlyProps | EditableProps): areaComponent is ViewOnlyProps {
  return areaComponent.onChange === undefined || areaComponent.isEditable === false;
}

const AreaContainer: React.FunctionComponent<IAreaContainerProps> = props => {
  if (isViewOnly(props)) {
    return <AreaView landArea={props.landArea} unitCode={props.unitCode} />;
  } else {
    return (
      <>
        <AreaForm
          area={props.landArea}
          areaUnitTypeCode={props.unitCode}
          onChange={props.onChange}
        />
        <DisplayError field={props.field} />
      </>
    );
  }
};

export default AreaContainer;
