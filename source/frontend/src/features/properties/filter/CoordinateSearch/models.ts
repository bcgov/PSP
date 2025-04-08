import { LatLngLiteral } from 'leaflet';

// Constants for the coordinate direction (N, S, E, W). They determine the sign of the Lat/Lng
export class DmsDirection {
  public static readonly N = '1' as const;
  public static readonly S = '-1' as const;
  public static readonly E = '1' as const;
  public static readonly W = '-1' as const;
}

// A formik model to represent coordinates in Degree, Minutes, Seconds format (DMS)
export class DmsCoordinates {
  latitude: Dms;
  longitude: Dms;

  constructor() {
    // Lat: Use 1 for North (N), -1 for South (S)
    this.latitude = new Dms(0, 0, 0, DmsDirection.N);

    // Lng: Use 1 for East (E), -1 for West (W)
    this.longitude = new Dms(0, 0, 0, DmsDirection.W);
  }

  toLatLng(): LatLngLiteral {
    return {
      lat: this.latitude.toDecimalDegrees(),
      lng: this.longitude.toDecimalDegrees(),
    };
  }
}

export class Dms {
  constructor(
    // Degrees (0 to 89, 0 to 179)
    public degrees: number = 0,
    // Minutes (0 to 59) as integers
    public minutes: number = 0,
    // Seconds (0 to 59.9999) up to 4 decimal places.
    public seconds: number = 0,
    // The coordinate direction (N, S, E, W)
    public direction: '1' | '-1' = DmsDirection.N,
  ) {}

  toDecimalDegrees(): number {
    try {
      const deg = parseFloat(this.degrees.toFixed(0));
      const min = parseFloat(this.minutes.toFixed(0));
      const sec = parseFloat(this.seconds.toFixed(4));

      // Latitude (-90 to 90) and longitude (-180 to 180).
      // Include up to 6 decimal places.
      const dd = parseInt(this.direction) * (deg + min / 60 + sec / 3600);
      return parseFloat(dd.toFixed(6));
    } catch (e) {
      return 0;
    }
  }
}
