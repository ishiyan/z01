import { NavigationPane } from './navigation-pane';
import { Margin } from './margin';
import { OhlcvData } from './ohlcv-data';
import { Pane } from './pane';

/** Describes an ohlcv chart layout configuration. */
export class Configuration {
  /**
   * Total width of a chart including margins.
   * Defines the width of all chart panes.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public width!: string | number;

  /** An optional minimal width in pixels including margins. */
  public widthMin?: number;

  /** An optional maximal width in pixels including margins. */
  public widthMax?: number;

  /** An optional navigation pane. */
  public navigationPane?: NavigationPane = new NavigationPane();

  /**
   * An optional height of the navigation pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   * If undefined, the navigation pane will not be created.
   */
  public heightNavigationPane?: string | number;

  /** An optional d3.timeFormat specifier for time axis annotations, e.g. '%Y-%m-%d'. */
  public timeAnnotationFormat?: string;

  /** An optional d3.timeFormat specifier for time axis ticks, e.g. '%Y-%m-%d'. */
  public timeTicksFormat?: string;

  /** An optional number of ticks in the time axis. */
  public timeTicks?: number;

  /** If left axis should be visible on the price and indicator panes. */
  public axisLeft = true;

  /** If right axis should be visible on the price and indicator panes. */
  public axisRight = false;

  /** The margins of the chart, exclusive space needed for annotation. */
  public margin: Margin = new Margin();

  public ohlcv: OhlcvData = new OhlcvData();

  /** The price pane. */
  public pricePane: Pane = new Pane();

  /** An optional array of indicator panes. */
  public indicatorPanes: Pane[] = [];

  /** If *crosshair* should be visible */
  public crosshair = false;

  /** If volume in price pane should be visible */
  public volumeInPricePane = false;

  /** If menu should be visible. */
  public menuVisible = true;

  /** If *download SVG* menu setting should be visible. */
  public downloadSvgVisible = true;
}
