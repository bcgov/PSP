import { VolumeForm } from './VolumeForm';
import VolumeView from './VolumeView';

interface ViewOnlyProps {
  volumetricMeasurement?: number;
  volumetricUnit?: string;
  volumetricType?: string;

  isEdditable?: never;
  onChange?: never;
}

interface EditableProps {
  volumetricMeasurement?: number;
  volumetricUnit?: string;
  volumetricType?: string;

  isEdditable: boolean;
  onChange: (volume: number, volumetricUnit: string) => void;
}

type IVolumeContainerProps = EditableProps | ViewOnlyProps;

function isViewOnly(areaComponent: ViewOnlyProps | EditableProps): areaComponent is ViewOnlyProps {
  var editable = areaComponent as EditableProps;
  return editable.onChange === undefined || editable.isEdditable === false;
}

const VolumeContainer: React.FunctionComponent<IVolumeContainerProps> = props => {
  if (isViewOnly(props)) {
    return <VolumeView volume={props.volumetricMeasurement} unitCode={props.volumetricUnit} />;
  } else {
    return (
      <VolumeForm
        volume={props.volumetricMeasurement}
        volumeUnitTypeCode={props.volumetricUnit}
        onChange={props.onChange}
      />
    );
  }
};

export default VolumeContainer;
