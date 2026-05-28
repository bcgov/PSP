import { useContext } from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import FilePropertiesImprovementsContainer from '@/features/mapSideBar/shared/improvements/FilePropertiesImprovements/FilePropertiesImprovementsContainer';
import { FilePropertiesImprovementsView } from '@/features/mapSideBar/shared/improvements/FilePropertiesImprovements/FilePropertiesImprovementsView';

const LeasePropertiesImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps<void>>
> = ({ isEditing }) => {
  const { lease } = useContext(LeaseStateContext);
  const properties = lease?.fileProperties?.map(x => x.property) ?? [];

  return (
    !isEditing && (
      <FilePropertiesImprovementsContainer
        fileProperties={properties}
        View={FilePropertiesImprovementsView}
      ></FilePropertiesImprovementsContainer>
    )
  );
};

export default LeasePropertiesImprovementsContainer;
