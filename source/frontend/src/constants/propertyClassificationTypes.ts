/**
 * Property classifications, identify the purpose of the property.
 */
export enum PropertyClassificationTypes {
  /** The property is currently being used. */
  CoreOperational = 'COREOPER',
  /** The property is not currently being used but has a strategic purpose. */
  CoreStrategic = 'CORESTRAT',
  /** The property is available for disposal. */
  SurplusActive = 'SURPACTIVE',
  /** The property is available for disposal but is encumbered. */
  SurplusEncumbered = 'SURPENCUM',
  /** The property has been disposed. */
  Disposed = 'DISPOSED',
  /** The property has been demolished */
  Demolished = 'DEMOLISHED',
  /** The property has been subdivided */
  Subdivided = 'SUBDIVIDED',
}
