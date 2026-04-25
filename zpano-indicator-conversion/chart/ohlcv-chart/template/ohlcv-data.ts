import { Bar } from '../../../data/entities/bar';

/** Describes an ohlcv data. */
export class OhlcvData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Bar[] = [];

  /** If data is displayed as candlesticks or as bars */
  public candlesticks = true;
}
