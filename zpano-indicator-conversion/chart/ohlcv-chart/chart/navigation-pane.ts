import * as d3 from 'd3';

export class NavigationPane {
  group: any;
  priceScale!: d3.ScaleLinear<number, number>;
  timeScale: any;
  brush: any;
  area: any;
  line: any;
  timeAxis: any;
  lineSelection: any;
  timeAxisSelection: any;
  areaSelection: any;
  paneSelection: any;
}
