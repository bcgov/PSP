import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';

export default interface ComposedProperty {
  pid?: string;
  ltsaDataRequestedOn?: Date;
  ltsaData?: LtsaOrders;
  ltsaLoading: boolean;
  apiProperty?: IPropertyApiModel;
  apiPropertyLoading: boolean;
  propertyAssociations?: Api_PropertyAssociations;
  propertyAssociationsLoading: boolean;
}
