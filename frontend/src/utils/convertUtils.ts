import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { VolumeUnitTypes } from 'constants/volumeUnitTypes';
import convert, { Area, Volume } from 'convert';

export function convertArea(value: number, from: AreaUnitTypes, to: AreaUnitTypes): number {
  const _from = getAreaUnit(from);
  const _to = getAreaUnit(to);
  return convert(value, _from).to(_to);
}

export function convertVolume(value: number, from: VolumeUnitTypes, to: VolumeUnitTypes): number {
  const _from = getVolumeUnit(from);
  const _to = getVolumeUnit(to);
  return convert(value, _from).to(_to);
}

export function getAreaUnit(unit: AreaUnitTypes): Area {
  switch (unit) {
    case AreaUnitTypes.Acres:
      return 'ac';
    case AreaUnitTypes.Hectares:
      return 'ha';
    case AreaUnitTypes.SquareFeet:
      return 'sq ft';
    case AreaUnitTypes.SquareMeters:
    default:
      return 'm2';
  }
}

export function getVolumeUnit(unit: VolumeUnitTypes): Volume {
  switch (unit) {
    case VolumeUnitTypes.CubicFeet:
      return 'ft3';
    case VolumeUnitTypes.CubicMeters:
    default:
      return 'm3';
  }
}
