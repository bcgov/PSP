/**
 * Property classifications, identify the purpose of the property.
 */
export enum Classifications {
  /** The property is currently being used. */
  CoreOperational = 1,
  /** The property is not currently being used but has a strategic purpose. */
  CoreStrategic = 2,
  /** The property is available for disposal. */
  SurplusActive = 3,
  /** The property is available for disposal but is encumbered. */
  SurplusEncumbered = 4,
  /** The property has been disposed. */
  Disposed = 5,
  /** The property has been demolished */
  Demolished = 6,
  /** The property has been subdivided */
  Subdivided = 7,
}
