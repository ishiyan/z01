import { Component, HostListener, ChangeDetectionStrategy, PLATFORM_ID, inject, input, effect, afterNextRender, DOCUMENT } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MatIconRegistry, MatIcon } from '@angular/material/icon';
import { MatMiniFabButton } from '@angular/material/button';
import { MatButtonToggleGroup, MatButtonToggle } from '@angular/material/button-toggle';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { MatExpansionPanel, MatExpansionPanelHeader, MatExpansionPanelTitle } from '@angular/material/expansion';
import * as d3 from 'd3';

import { primitives } from '../d3-primitives';
import { Ohlcv } from '../../data/entities/ohlcv';
import { Scalar } from '../../data/entities/scalar';
/* import { Band } from '../entities/band';
import { Heatmap } from '../entities/heatmap'; */
import * as Template from './template/template';
import * as Chart from './chart/chart';
import { Downloader } from '../downloader';

/** *Ohlcv* view type: *candlesticks*. */
const ohlcvViewCandlesticks = 0;
/** *Ohlcv* view type: *bars*. */
const ohlcvViewBars = 1;
/** The width of a vertical axis in pixels including annotation. */
const verticalAxisWidth = 50;
/** The minimal navigation selection width in pixels which causes the chart to re-draw. */
const minSelection = 10;
/** The height of a time axis in pixels. */
const timeAxisHeight = 22;
/** The horizontal distance between y axis and legend item in pixels. */
const whitespaceBetweenAxisAndLegend = 8;
/** The horizontal distance between legend items in pixels. */
const whitespaceBetweenLegendItems = 5;
/** The width of a legend heatmap item image in pixels. */
const legendHeatmapImageWidth = 50;
/** The width of a legend heatmap item image in pixels. */
const legendHeatmapImageHeight = 6;
/** The width of a legend area item image in pixels. */
const legendAreaImageWidth = 20;
/** The width of a legend area item image in pixels. */
const legendAreaImageHeight = 6;
/** The width of a legend line item image in pixels. */
const legendLineImageWidth = 20;
/** The width of a legend line item image in pixels. */
const legendLineImageHeight = 6;
/** Default number of pixels between time ticks on horizontal time axis. */
const defaultWhitespaceBetweenTimeTicks = 100;
/** Default number of pixels between value ticks on vertical value axis. */
const defaultWhitespaceBetweenValueTicks = 20;
/** The minimal date. */
const minDate = new Date(-8640000000000000);
/** The maximal date. */
const maxDate = new Date(8640000000000000);
/** The text to place before the SVG line when exporting chart as SVG. */
const textBeforeSvg = `<html><meta charset="utf-8"><style>
  text { fill: black; font-family: Arial, Helvetica, sans-serif; }
  path.candle { stroke: black; }
  path.candle.up { fill: white; }
  path.candle.down { fill: black; }
  path.ohlc.up { fill: none; stroke: black; }
  path.ohlc.down { fill: none; stroke: black; }
  path.volume { fill: lightgrey; }
  path.area { fill: lightgrey; }
  path.line { stroke: black; }
  rect.selection { fill: darkgrey; }
</style><body>
`;
/** The text to place after the SVG line when exporting chart as SVG. */
const textAfterSvg = `
</body></html>
`;
/** If *brushing* (zooming) is real-time. */
const smoothBrushing = false;

@Component({
  selector: 'mb-ohlcv-chart',
  templateUrl: './ohlcv-chart.component.html',
  styleUrls: ['./ohlcv-chart.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    FormsModule,
    MatIcon,
    MatMiniFabButton,
    MatButtonToggle,
    MatButtonToggleGroup,
    MatSlideToggle,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle
  ]
})
export class OhlcvChartComponent {
  private readonly document = inject(DOCUMENT);
  private readonly platformId = inject(PLATFORM_ID);
  private random = Math.random().toString(36).substring(2);
  protected svgContainerId = 'ohlcv-chart-svg-' + this.random;
  protected widthContainerId = 'ohlcv-chart-width-' + this.random;
  private config!: Template.Configuration;
  private currentSelection: any = null;
  private renderVolume: boolean;
  private renderCrosshair: boolean;
  private ohlcvView: number;
  public readonly ohlcvViewCandlesticks = ohlcvViewCandlesticks;
  public readonly ohlcvViewBars = ohlcvViewBars;

  private static valueToPixels(value: number | string, reference: number): number {
    if (typeof value === 'number') {
      return +value;
    }

    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    const numeric = +value.match(/\d+/);
    if (value.endsWith('%')) {
      return numeric / 100 * reference;
    }

    return numeric;
  }

  private static layoutHorizontal(cfg: Template.Configuration, referenceWidth: number): Chart.HorizontalLayout {
    let totalWidth: number = OhlcvChartComponent.valueToPixels(cfg.width, referenceWidth);
    if (cfg.widthMin && cfg.widthMin > totalWidth) {
      totalWidth = cfg.widthMin;
    }
    if (cfg.widthMax && cfg.widthMax < totalWidth) {
      totalWidth = cfg.widthMax;
    }

    const chartLeft: number = cfg.margin.left;
    const chartWidth: number = totalWidth - chartLeft - cfg.margin.right;

    const contentLeft: number = cfg.axisLeft ? verticalAxisWidth : 0;
    const contentWidth: number = chartWidth - contentLeft - (cfg.axisRight ? verticalAxisWidth : 0);

    return { width: totalWidth, chart: { left: chartLeft, width: chartWidth }, content: { left: contentLeft, width: contentWidth } };
  }

  private static textBoundingClientRect(t: any, remove = false): any {
    const node = t.node();
    let rect: any;
    if (node && node != null) {
      rect = node.getBoundingClientRect();
      if (remove) {
        t.remove();
      }
    } else {
      rect = { width: 100, height: 12 };
    }
    return rect;
  }

  private static layoutVertical(svg: any, cfg: Template.Configuration,
                                lh: Chart.HorizontalLayout): Chart.VerticalLayout {
    const t = svg.append('text').text(`w`);
    const lineHeight = OhlcvChartComponent.textBoundingClientRect(t, true).height;
    const heightPricePaneLegend: number = OhlcvChartComponent.appendLegend(svg, cfg.margin.top, lineHeight, lh.content.left,
      lh.content.width, cfg.pricePane, cfg.ohlcv.name);
    const l = new Chart.VerticalLayout();
    l.pricePane.top = cfg.margin.top + heightPricePaneLegend;
    let q = OhlcvChartComponent.valueToPixels(cfg.pricePane.height, lh.content.width);
    if (cfg.pricePane.heightMin && cfg.pricePane.heightMin > q) {
      q = cfg.pricePane.heightMin;
    }
    if (cfg.pricePane.heightMax && cfg.pricePane.heightMax < q) {
      q = cfg.pricePane.heightMax;
    }
    l.pricePane.height = q;

    let top = l.pricePane.top + l.pricePane.height;
    if (cfg.indicatorPanes && cfg.indicatorPanes.length) {
      for (const pane of cfg.indicatorPanes) {
        const block = new Chart.VerticalLayoutBlock();
        const legendHeight: number = OhlcvChartComponent.appendLegend(svg, top, lineHeight, lh.content.left,
          lh.content.width, pane, undefined);
        block.top = top + legendHeight;
        q = OhlcvChartComponent.valueToPixels(pane.height, lh.content.width);
        if (pane.heightMin && pane.heightMin > q) {
          q = pane.heightMin;
        }
        if (pane.heightMax && pane.heightMax < q) {
          q = pane.heightMax;
        }
        block.height = q;
        l.indicatorPanes.push(block);
        top = block.top + block.height;
      }
    }

    l.timeAxis.top = top;
    l.timeAxis.height = timeAxisHeight;
    top += l.timeAxis.height;

    l.navigationPane.top = top;
    if (cfg.navigationPane) {
      const nav = cfg.navigationPane;
      q = OhlcvChartComponent.valueToPixels(nav.height, lh.content.width);
      if (nav.heightMin && nav.heightMin > q) {
        q = nav.heightMin;
      }
      if (nav.heightMax && nav.heightMax < q) {
        q = nav.heightMax;
      }
      if (nav.hasTimeAxis) {
        if (!nav.timeTicks || nav.timeTicks > 0) {
          q += timeAxisHeight;
        }
      }
      l.navigationPane.height = q;
      top += q;
    }

    l.height = top + cfg.margin.bottom;
    return l;
  }

  private static appendLegend(g: any, top: number, lineHeight: number, left: number, width: number,
                              pane: Template.Pane, instrument = ''): number {
    g = g.append('g').attr('class', 'legend');
    top += lineHeight / 2;
    left += whitespaceBetweenAxisAndLegend;
    let l = left;
    let height = 0;

    if (instrument && instrument.length > 0) {
      const t = OhlcvChartComponent.appendText(g, l, top, ` ${instrument} `);
      const r = OhlcvChartComponent.textBoundingClientRect(t);
      l += r.width + whitespaceBetweenAxisAndLegend;
      height = r.height;
    }

    if (pane.heatmap) {
      const q = g.append('image').attr('x', l).attr('y', top - legendHeatmapImageHeight)
        .attr('width', legendHeatmapImageWidth).attr('height', legendHeatmapImageHeight).attr('preserveAspectRatio', 'none')
        .attr('xlink:href',
          OhlcvChartComponent.ramp(OhlcvChartComponent.convertGradient(pane.heatmap.gradient), pane.heatmap.invertGradient,
            legendHeatmapImageWidth, legendHeatmapImageHeight).toDataURL());
      const d1 = legendHeatmapImageWidth + whitespaceBetweenLegendItems;
      l += d1;
      const t = OhlcvChartComponent.appendText(g, l, top, pane.heatmap.name);
      const r = OhlcvChartComponent.textBoundingClientRect(t);
      if (height === 0) {
        height = r.height;
      }
      const w = r.width;
      if (l + w > width) {
        top += r.height;
        height += r.height;
        q.attr('x', left).attr('y', top - legendHeatmapImageHeight);
        l = left + d1;
        t.attr('x', l).attr('y', top);
      }
      l += w + whitespaceBetweenAxisAndLegend;
    }

    for (const band of pane.bands) {
      const q = g.append('rect').attr('x', l).attr('y', top - legendAreaImageHeight)
        .attr('width', legendAreaImageWidth).attr('height', legendAreaImageHeight).attr('stroke-width', 0)
        .attr('fill', band.color);
      const d1 = legendAreaImageWidth + whitespaceBetweenLegendItems;
      l += d1;
      const t = OhlcvChartComponent.appendText(g, l, top, band.name);
      const r = OhlcvChartComponent.textBoundingClientRect(t);
      if (height === 0) {
        height = r.height;
      }
      const w = r.width;
      if (l + w > width) {
        top += r.height;
        height += r.height;
        q.attr('x', left).attr('y', top - legendAreaImageHeight);
        l = left + d1;
        t.attr('x', l).attr('y', top);
      }
      l += w + whitespaceBetweenAxisAndLegend;
    }

    for (const lineArea of pane.lineAreas) {
      const q = g.append('rect').attr('x', l).attr('y', top - legendAreaImageHeight)
        .attr('width', legendAreaImageWidth).attr('height', legendAreaImageHeight).attr('stroke-width', 0)
        .attr('fill', lineArea.color);
      const d1 = legendAreaImageWidth + whitespaceBetweenLegendItems;
      l += d1;

      const t = OhlcvChartComponent.appendText(g, l, top, lineArea.name);
      const r = OhlcvChartComponent.textBoundingClientRect(t);
      if (height === 0) {
        height = r.height;
      }
      const w = r.width;
      if (l + w > width) {
        top += r.height;
        height += r.height;
        q.attr('x', left).attr('y', top - legendAreaImageHeight);
        l = left + d1;
        t.attr('x', l).attr('y', top);
      }
      l += w + whitespaceBetweenAxisAndLegend;
    }

    for (const line of pane.lines) {
      const hei = (legendLineImageHeight - line.width) / 2;
      const q = g.append('line').attr('x1', l).attr('y1', top - hei)
        .attr('x2', l + legendLineImageWidth).attr('y2', top - hei).attr('stroke-width', line.width)
        .attr('stroke', line.color).attr('stroke-dasharray', line.dash).attr('fill', 'none');
      const d1 = legendLineImageWidth + whitespaceBetweenLegendItems;
      l += d1;

      const t = OhlcvChartComponent.appendText(g, l, top, line.name);
      const r = OhlcvChartComponent.textBoundingClientRect(t);
      if (height === 0) {
        height = r.height;
      }
      const w = r.width;
      if (l + w > width) {
        top += r.height;
        height += r.height;
        q.attr('x1', left).attr('y1', top - hei).attr('x2', left + legendLineImageWidth).attr('y2', top - hei);
        l = left + d1;
        t.attr('x', l).attr('y', top);
      }
      l += w + whitespaceBetweenAxisAndLegend;
    }
    return height;
  }

  private static firstTime(cfg: Template.Configuration): Date {
    let time = cfg.ohlcv.data[0].time;
    for (const item of cfg.pricePane.bands) {
      const data = item.data;
      const t = data[0].time;
      if (time > t) {
        time = t;
      }
    }
    for (const item of cfg.pricePane.lines) {
      const data = item.data;
      const t = data[0].time;
      if (time > t) {
        time = t;
      }
    }
    for (const pane of cfg.indicatorPanes) {
      for (const item of pane.bands) {
        const data = item.data;
        const t = data[0].time;
        if (time > t) {
          time = t;
        }
      }
      for (const item of pane.lines) {
        const data = item.data;
        const t = data[0].time;
        if (time > t) {
          time = t;
        }
      }
    }
    return time;
  }

  private static lastTime(cfg: Template.Configuration): Date {
    let time = cfg.ohlcv.data[cfg.ohlcv.data.length - 1].time;
    for (const item of cfg.pricePane.bands) {
      const data = item.data;
      const t = data[data.length - 1].time;
      if (time < t) {
        time = t;
      }
    }
    for (const item of cfg.pricePane.lines) {
      const data = item.data;
      const t = data[data.length - 1].time;
      if (time < t) {
        time = t;
      }
    }
    for (const pane of cfg.indicatorPanes) {
      for (const item of pane.bands) {
        const data = item.data;
        const t = data[data.length - 1].time;
        if (time < t) {
          time = t;
        }
      }
      for (const item of pane.lines) {
        const data = item.data;
        const t = data[data.length - 1].time;
        if (time < t) {
          time = t;
        }
      }
    }
    // return time;
    return new Date(time.getFullYear(), time.getMonth(), time.getDay() + 10);
  }

  private static appendText(group: any, left: number, top: number, text: string): any {
    return group.append('text')
      .attr('font-size', '10px')
      .attr('font-family', 'sans-serif')
      .attr('x', left)
      .attr('y', top)
      .text(text);
  }

  private static estimateNumberOfVerticalTicks(height: number): number {
    return height / defaultWhitespaceBetweenValueTicks;
  }

  private static createPricePane(cfg: Template.Configuration, lh: Chart.HorizontalLayout,
                                 lv: Chart.VerticalLayout, timeScale: any, timeAnnotationBottom: any, svg: any,
                                 isCandlestick: boolean, isVolume: boolean, isCrossHair: boolean, id: string): Chart.PricePane {
    const cf = cfg.pricePane;
    const pane = new Chart.PricePane();
    pane.yPrice = d3.scaleLinear().range([lv.pricePane.height, 0]);
    const factor = cf.valueMarginPercentageFactor;
    pane.yMarginFactorTop = 1 + factor;
    pane.yMarginFactorBottom = 1 - factor;
    pane.priceShape = (isCandlestick ? primitives.plot.candlestick() as any : primitives.plot.ohlc() as any)
      .xScale(timeScale).yScale(pane.yPrice);
    pane.priceAccessor = pane.priceShape.accessor();

    const clip = 'price-clip-' + id;
    const clipUrl = `url(#${clip})`;
    pane.group = svg.append('g').attr('class', 'price-pane').attr('transform', `translate(${lh.content.left}, ${lv.pricePane.top})`);
    pane.group.append('clipPath').attr('id', clip).append('rect').attr('x', 0).attr('y', pane.yPrice(1))
      .attr('width', lh.content.width).attr('height', pane.yPrice(0) as number - pane.yPrice(1) as number);

    for (let i = 0; i < cf.bands.length; ++i) {
      const band = cf.bands[i];
      const indicatorBand = new Chart.IndicatorBand();
      indicatorBand.path = pane.group.append('g').attr('class', `band-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('fill', band.color);
      indicatorBand.area = d3.area()
        .curve(OhlcvChartComponent.convertInterpolation(band.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.lower) && !isNaN(w.upper); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y0(d => { const w: any = d; return pane.yPrice(w.lower) as number; })
        .y1(d => { const w: any = d; return pane.yPrice(w.upper) as number; });
      indicatorBand.data = band.data;
      pane.indicatorBands.push(indicatorBand);
    }

    for (let i = 0; i < cf.lineAreas.length; ++i) {
      const lineArea = cf.lineAreas[i];
      const value = lineArea.value;
      const indicatorLineArea = new Chart.IndicatorLineArea();
      indicatorLineArea.path = pane.group.append('g').attr('class', `linearea-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('fill', lineArea.color);
      indicatorLineArea.area = d3.area()
        .curve(OhlcvChartComponent.convertInterpolation(lineArea.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.value); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y0(d => { const w: any = d; return pane.yPrice(w.value) as number; })
        .y1(() => pane.yPrice(value) as number);
      indicatorLineArea.data = lineArea.data;
      indicatorLineArea.value = lineArea.value;
      pane.indicatorLineAreas.push(indicatorLineArea);
    }

    for (let i = 0; i < cf.horizontals.length; ++i) {
      const horizontal = cf.horizontals[i];
      const indicatorHorizontal = new Chart.IndicatorHorizontal();
      indicatorHorizontal.path = pane.group.append('g').attr('class', `horizontal-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('stroke', horizontal.color)
        .attr('stroke-width', horizontal.width)
        .attr('stroke-dasharray', horizontal.dash)
        .attr('stroke-linejoin', 'round')
        .attr('stroke-linecap', 'round')
        .attr('fill', 'none');
      const val = horizontal.value;
      indicatorHorizontal.value = val;
      indicatorHorizontal.data =
        [new Scalar({ time: minDate, value: val }), new Scalar({ time: maxDate, value: val })];
      indicatorHorizontal.line = d3.line()
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y(() => pane.yPrice(val) as number);
      pane.indicatorHorizontals.push(indicatorHorizontal);
    }

    for (let i = 0; i < cf.lines.length; ++i) {
      const line = cf.lines[i];
      const indicatorLine = new Chart.IndicatorLine();
      indicatorLine.path = pane.group.append('g').attr('class', `line-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('stroke', line.color)
        .attr('stroke-width', line.width)
        .attr('stroke-dasharray', line.dash)
        .attr('stroke-linejoin', 'round')
        .attr('stroke-linecap', 'round')
        .attr('fill', 'none');
      indicatorLine.line = d3.line()
        .curve(OhlcvChartComponent.convertInterpolation(line.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.value); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y(d => { const w: any = d; return pane.yPrice(w.value) as number; });
      indicatorLine.data = line.data;
      pane.indicatorLines.push(indicatorLine);
    }

    pane.groupPrice = pane.group.append('g').attr('class', 'price').attr('clip-path', clipUrl);

    if (isVolume) {
      pane.yVolume = d3.scaleLinear().range([pane.yPrice(0) as number, pane.yPrice(0.3) as number]);
      pane.volume = (primitives.plot.volume() as any).xScale(timeScale).yScale(pane.yVolume);
      pane.groupVolume = pane.group.append('g').attr('class', 'volume').attr('clip-path', clipUrl);
    }

    let gArrows;
    for (const arrow of cf.arrows) {
      if (!gArrows) {
        gArrows = pane.group.append('g').attr('class', `arrows`).attr('clip-path', clipUrl);
      }
      const indicatorArrow = new Chart.IndicatorArrow();
      indicatorArrow.isDown = arrow.down;
      indicatorArrow.path = gArrows.append('path')
        .attr('stroke-width', 0)
        .attr('fill', arrow.color);
      const price = OhlcvChartComponent.getArrowPrice(cfg.ohlcv.data, arrow);
      indicatorArrow.price = price;
      indicatorArrow.arrow = primitives.shapes.arrow()
        .orient(arrow.down ? 'down' : 'up')
        .x(() => timeScale(arrow.time))
        .y(() => {
          const p = pane.yPrice(price) as number;
          return arrow.down ? p - Chart.verticalArrowGap : p + Chart.verticalArrowGap;
        });
      pane.indicatorArrows.push(indicatorArrow);
    }

    if (cfg.axisLeft) {
      pane.yAxisLeft = d3.axisLeft(pane.yPrice).tickSizeOuter(0).tickFormat(d3.format(cf.valueFormat));
      if (cf.valueTicks) {
        pane.yAxisLeft.ticks(cf.valueTicks);
      } else {
        pane.yAxisLeft.ticks(OhlcvChartComponent.estimateNumberOfVerticalTicks(lv.pricePane.height));
      }
      pane.groupAxisLeft = pane.group.append('g').attr('class', 'y axis left');
    }
    if (cfg.axisRight) {
      pane.yAxisRight = d3.axisRight(pane.yPrice).tickSizeOuter(0).tickFormat(d3.format(cf.valueFormat));
      if (cf.valueTicks) {
        pane.yAxisRight.ticks(cf.valueTicks);
      } else {
        pane.yAxisRight.ticks(OhlcvChartComponent.estimateNumberOfVerticalTicks(lv.pricePane.height));
      }
      pane.groupAxisRight = pane.group.append('g').attr('class', 'y axis right')
        .attr('transform', `translate(${lh.content.width}, 0)`);
    }

    if (isCrossHair) {
      let crosshair = (primitives.plot.crosshair() as any).xScale(timeScale).yScale(pane.yPrice).xAnnotation(timeAnnotationBottom)
        .verticalWireRange([0, lv.timeAxis.top]);
      if (cfg.axisLeft && cfg.axisRight) {
        const annotationLeft = (primitives.plot.axisannotation().axis(pane.yAxisLeft) as any).orient('left')
          .format(d3.format(cf.valueFormat));
        const annotationRight = (primitives.plot.axisannotation().axis(pane.yAxisRight) as any).orient('right')
          .format(d3.format(cf.valueFormat)).translate([timeScale(1), 0]);
        crosshair = crosshair.yAnnotation([annotationLeft, annotationRight]);
      } else
        if (cfg.axisLeft) {
          const annotationLeft = (primitives.plot.axisannotation().axis(pane.yAxisLeft) as any).orient('left')
            .format(d3.format(cf.valueFormat));
          crosshair = crosshair.yAnnotation(annotationLeft);
        } else
          if (cfg.axisRight) {
            const annotationRight = (primitives.plot.axisannotation().axis(pane.yAxisRight) as any).orient('right')
              .format(d3.format(cf.valueFormat)).translate([timeScale(1), 0]);
            crosshair = crosshair.yAnnotation(annotationRight);
          }
      pane.group.append('g').attr('class', 'crosshair').call(crosshair);
    }

    return pane;
  }

  private static createIndicatorPane(index: number, cfg: Template.Configuration, lh: Chart.HorizontalLayout,
                                     lv: Chart.VerticalLayout, timeScale: any, timeAxisBottom: any, svg: any,
                                     isCrossHair: boolean, id: string): Chart.IndicatorPane {
    const block = lv.indicatorPanes[index];
    const cf = cfg.indicatorPanes[index];
    const pane = new Chart.IndicatorPane();
    pane.yValue = d3.scaleLinear().range([block.height, 0]);
    const factor = cf.valueMarginPercentageFactor;
    pane.yMarginFactorTop = 1 + factor;
    pane.yMarginFactorBottom = 1 - factor;

    const clip = `indicator-clip-${index}-${id}`;
    const clipUrl = `url(#${clip})`;
    pane.group = svg.append('g').attr('class', 'indicator-pane').attr('transform', `translate(${lh.content.left}, ${block.top})`);
    pane.group.append('clipPath').attr('id', clip).append('rect').attr('x', 0).attr('y', pane.yValue(1))
      .attr('width', lh.content.width).attr('height', pane.yValue(0) as number - pane.yValue(1) as number);

    if (cf.heatmap) {
      const heatmap = cf.heatmap;
      const indicatorHeatmap = new Chart.IndicatorHeatmap();
      indicatorHeatmap.path = pane.group.append('g').attr('class', `heatmap`).attr('clip-path', clipUrl);
      indicatorHeatmap.invertGradient = heatmap.invertGradient;
      indicatorHeatmap.gradient = OhlcvChartComponent.convertGradient(heatmap.gradient);
      indicatorHeatmap.data = heatmap.data;
      indicatorHeatmap.height = block.height;
      pane.indicatorHeatmap = indicatorHeatmap;
    }

    for (let i = 0; i < cf.bands.length; ++i) {
      const band = cf.bands[i];
      const indicatorBand = new Chart.IndicatorBand();
      indicatorBand.path = pane.group.append('g').attr('class', `band-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('fill', band.color);
      indicatorBand.area = d3.area()
        .curve(OhlcvChartComponent.convertInterpolation(band.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.lower) && !isNaN(w.upper); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y0(d => { const w: any = d; return pane.yValue(w.lower) as number; })
        .y1(d => { const w: any = d; return pane.yValue(w.upper) as number; });
      indicatorBand.data = band.data;
      pane.indicatorBands.push(indicatorBand);
    }

    for (let i = 0; i < cf.lineAreas.length; ++i) {
      const lineArea = cf.lineAreas[i];
      const value = lineArea.value;
      const indicatorLineArea = new Chart.IndicatorLineArea();
      indicatorLineArea.path = pane.group.append('g').attr('class', `linearea-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('fill', lineArea.color);
      indicatorLineArea.area = d3.area()
        .curve(OhlcvChartComponent.convertInterpolation(lineArea.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.value); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y0(d => { const w: any = d; return pane.yValue(w.value) as number; })
        .y1(() => pane.yValue(value) as number);
      indicatorLineArea.data = lineArea.data;
      indicatorLineArea.value = lineArea.value;
      pane.indicatorLineAreas.push(indicatorLineArea);
    }

    for (let i = 0; i < cf.horizontals.length; ++i) {
      const horizontal = cf.horizontals[i];
      const indicatorHorizontal = new Chart.IndicatorHorizontal();
      indicatorHorizontal.path = pane.group.append('g').attr('class', `horizontal-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('stroke', horizontal.color)
        .attr('stroke-width', horizontal.width)
        .attr('stroke-dasharray', horizontal.dash)
        .attr('stroke-linejoin', 'round')
        .attr('stroke-linecap', 'round')
        .attr('fill', 'none');
      const val = horizontal.value;
      indicatorHorizontal.value = val;
      indicatorHorizontal.data =
        [new Scalar({ time: minDate, value: val }), new Scalar({ time: maxDate, value: val })];
      indicatorHorizontal.line = d3.line()
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y(() => pane.yValue(val) as number);
      pane.indicatorHorizontals.push(indicatorHorizontal);
    }

    for (let i = 0; i < cf.lines.length; ++i) {
      const line = cf.lines[i];
      const indicatorLine = new Chart.IndicatorLine();
      indicatorLine.path = pane.group.append('g').attr('class', `line-${i}`).attr('clip-path', clipUrl).append('path')
        .attr('stroke', line.color)
        .attr('stroke-width', line.width)
        .attr('stroke-dasharray', line.dash)
        .attr('stroke-linejoin', 'round')
        .attr('stroke-linecap', 'round')
        .attr('fill', 'none');
      indicatorLine.line = d3.line()
        .curve(OhlcvChartComponent.convertInterpolation(line.interpolation))
        .defined(d => { const w: any = d; return !isNaN(w.value); })
        .x(d => { const w: any = d; return timeScale(w.time); })
        .y(d => { const w: any = d; return pane.yValue(w.value) as number; });
      indicatorLine.data = line.data;
      pane.indicatorLines.push(indicatorLine);
    }

    if (cfg.axisLeft) {
      pane.yAxisLeft = d3.axisLeft(pane.yValue).tickSizeOuter(0).tickFormat(d3.format(cf.valueFormat));
      if (cf.valueTicks) {
        pane.yAxisLeft.ticks(cf.valueTicks);
      } else {
        pane.yAxisLeft.ticks(OhlcvChartComponent.estimateNumberOfVerticalTicks(block.height));
      }
      pane.groupAxisLeft = pane.group.append('g').attr('class', 'y axis left');
    }
    if (cfg.axisRight) {
      pane.yAxisRight = d3.axisRight(pane.yValue).tickSizeOuter(0).tickFormat(d3.format(cf.valueFormat));
      if (cf.valueTicks) {
        pane.yAxisRight.ticks(cf.valueTicks);
      } else {
        pane.yAxisRight.ticks(OhlcvChartComponent.estimateNumberOfVerticalTicks(block.height));
      }
      pane.groupAxisRight = pane.group.append('g').attr('class', 'y axis right')
        .attr('transform', `translate(${lh.content.width}, 0)`);
    }

    if (isCrossHair) {
      const delta = lv.timeAxis.top - block.top;
      const timeAnnotationBottom = (primitives.plot.axisannotation().axis(timeAxisBottom) as any).orient('bottom')
        .width(65).translate([0, delta]);
      if (cfg.timeAnnotationFormat) {
        timeAnnotationBottom.format(d3.timeFormat(cfg.timeAnnotationFormat));
      }
      let crosshair = (primitives.plot.crosshair() as any).xScale(timeScale).yScale(pane.yValue).xAnnotation(timeAnnotationBottom)
        .verticalWireRange([lv.pricePane.top - block.top, delta]);
      if (cfg.axisLeft && cfg.axisRight) {
        const annotationLeft = (primitives.plot.axisannotation().axis(pane.yAxisLeft) as any).orient('left')
          .format(d3.format(cf.valueFormat));
        const annotationRight = (primitives.plot.axisannotation().axis(pane.yAxisRight) as any).orient('right')
          .format(d3.format(cf.valueFormat)).translate([timeScale(1), 0]);
        crosshair = crosshair.yAnnotation([annotationLeft, annotationRight]);
      } else
        if (cfg.axisLeft) {
          const annotationLeft = (primitives.plot.axisannotation().axis(pane.yAxisLeft) as any).orient('left')
            .format(d3.format(cf.valueFormat));
          crosshair = crosshair.yAnnotation(annotationLeft);
        } else
          if (cfg.axisRight) {
            const annotationRight = (primitives.plot.axisannotation().axis(pane.yAxisRight) as any).orient('right')
              .format(d3.format(cf.valueFormat)).translate([timeScale(1), 0]);
            crosshair = crosshair.yAnnotation(annotationRight);
          }
      pane.group.append('g').attr('class', 'crosshair').call(crosshair);
    }

    return pane;
  }

  private static createNavPane(cfg: Template.Configuration, lh: Chart.HorizontalLayout,
                               lv: Chart.VerticalLayout, svg: any): Chart.NavigationPane {
    const width = lh.content.width;
    const height = lv.navigationPane.height;
    const pane = new Chart.NavigationPane();
    if (cfg.navigationPane !== undefined) {
      const nav = cfg.navigationPane;
      const heightWithoutTimeAxis = nav.hasTimeAxis ? height - timeAxisHeight : height;
      pane.timeScale = (primitives.scale.financetime() as any).range([0, width]);
      pane.priceScale = d3.scaleLinear().range([heightWithoutTimeAxis, 0]);
      pane.brush = d3.brushX().extent([[0, 0], [width, heightWithoutTimeAxis]]);
      if (nav.hasArea) {
        pane.area = (primitives.plot.ohlcarea() as any).xScale(pane.timeScale).yScale(pane.priceScale);
      }
      if (nav.hasLine) {
        pane.line = (primitives.plot.closeline() as any).xScale(pane.timeScale).yScale(pane.priceScale);
      }
      if (nav.hasTimeAxis) {
        pane.timeAxis = d3.axisBottom(pane.timeScale).tickSizeOuter(0);
        if (nav.timeTicksFormat !== undefined) {
          pane.timeAxis.tickFormat(d3.timeFormat(nav.timeTicksFormat));
        }
        if (nav.timeTicks !== undefined) {
          pane.timeAxis.ticks(nav.timeTicks);
        } else {
          pane.timeAxis.ticks(lh.content.width / defaultWhitespaceBetweenTimeTicks);
        }
      }
      pane.group = svg.append('g').attr('class', 'nav').attr('transform', `translate(${lh.content.left}, ${lv.navigationPane.top})`)
        .attr('height', height);
      if (nav.hasArea) {
        pane.areaSelection = pane.group.append('g').attr('class', 'area').attr('height', heightWithoutTimeAxis);
      }
      if (nav.hasLine) {
        pane.lineSelection = pane.group.append('g').attr('class', 'line').attr('fill', 'none').attr('stroke-width', 0.5)
          .attr('height', heightWithoutTimeAxis);
      }
      pane.paneSelection = pane.group.append('g').attr('class', 'pane').attr('height', heightWithoutTimeAxis);
      if (nav.hasTimeAxis) {
        pane.timeAxisSelection = pane.group.append('g').attr('class', 'x axis').attr('height', timeAxisHeight)
          .attr('transform', `translate(0, ${heightWithoutTimeAxis})`);
      }
    }
    return pane;
  }

  private static createTimePane(cfg: Template.Configuration, lh: Chart.HorizontalLayout,
                                lv: Chart.VerticalLayout, svg: any): Chart.TimePane {
    const pane = new Chart.TimePane();
    pane.timeScale = (primitives.scale.financetime() as any).range([0, lh.content.width]);
    pane.timeAxis = d3.axisBottom(pane.timeScale).tickSizeOuter(0);
    if (cfg.timeTicksFormat !== undefined) {
      pane.timeAxis.tickFormat(d3.timeFormat(cfg.timeTicksFormat));
    }
    if (cfg.timeTicks !== undefined) {
      pane.timeAxis.ticks(cfg.timeTicks);
    } else {
      pane.timeAxis.ticks(lh.content.width / defaultWhitespaceBetweenTimeTicks);
    }
    pane.timeAnnotation = (primitives.plot.axisannotation().axis(pane.timeAxis) as any).orient('bottom')
      .width(65).translate([0, lv.timeAxis.top - lv.pricePane.top]);
    if (cfg.timeAnnotationFormat !== undefined) {
      pane.timeAnnotation.format(d3.timeFormat(cfg.timeAnnotationFormat));
    }

    pane.group = svg.append('g').attr('class', 'x axis').attr('height', lv.timeAxis.height)
      .attr('transform', `translate(${lh.content.left}, ${lv.timeAxis.top})`);

    return pane;
  }

  private static convertInterpolation(interpolation: string): d3.CurveFactory {
    switch (interpolation.toLowerCase()) {
      case 'step': return d3.curveStep;
      case 'stepbefore': return d3.curveStepBefore;
      case 'stepafter': return d3.curveStepAfter;
      case 'natural': return d3.curveNatural;
      case 'basis': return d3.curveBasis;
      case 'catmullrom': return d3.curveCatmullRom;
      case 'cardinal': return d3.curveCardinal;
      default: return d3.curveLinear;
    }
  }

  private static findOhlcv(data: Ohlcv[], time: Date): Ohlcv | null {
    for (const d of data) {
      if (+d.time >= +time) {
        return d;
      }
    }
    return null;
  }

  private static getArrowPrice(data: Ohlcv[], arrow: Template.ArrowData): number {
    if (arrow.value) {
      return +arrow.value;
    }
    const ohlcv = OhlcvChartComponent.findOhlcv(data, arrow.time);
    if (ohlcv != null) {
      return arrow.down ? ohlcv.high : ohlcv.low;
    }
    return 0;
  }

  private static ramp(color: any, invertGradient: boolean, width: number, height: number): HTMLCanvasElement {
    const canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    canvas.style.width = width + 'px';
    canvas.style.height = height + 'px';
    canvas.style.imageRendering = 'pixelated';
    const context = canvas.getContext('2d');
    const k = 1 / (width - 1);
    for (let i = 0; i < width; ++i) {
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      context.fillStyle = color(invertGradient ? (1 - i * k) : (i * k));
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      context.fillRect(i, 0, 1, height);
    }
    return canvas;
  }

  private static convertGradient(gradient: string): any {
    switch (gradient.toLowerCase()) {
      case 'viridis': return d3.interpolateViridis;
      case 'inferno': return d3.interpolateInferno;
      case 'magma': return d3.interpolateMagma;
      case 'plasma': return d3.interpolatePlasma;
      case 'warm': return d3.interpolateWarm;
      case 'cool': return d3.interpolateCool;
      case 'rainbow': return d3.interpolateRainbow;
      case 'cubehelixdefault': return d3.interpolateCubehelixDefault;
      case 'bugn': return d3.interpolateBuGn;
      case 'bupu': return d3.interpolateBuPu;
      case 'gnbu': return d3.interpolateGnBu;
      case 'orrd': return d3.interpolateOrRd;
      case 'pubugn': return d3.interpolatePuBuGn;
      case 'pubu': return d3.interpolatePuBu;
      case 'purd': return d3.interpolatePuRd;
      case 'rdpu': return d3.interpolateRdPu;
      case 'ylgnbu': return d3.interpolateYlGnBu;
      case 'ylgn': return d3.interpolateYlGn;
      case 'ylorbr': return d3.interpolateYlOrBr;
      case 'ylorrd': return d3.interpolateYlOrRd;
      case 'blues': return d3.interpolateBlues;
      case 'greens': return d3.interpolateGreens;
      case 'greys': return d3.interpolateGreys;
      case 'oranges': return d3.interpolateOranges;
      case 'purples': return d3.interpolatePurples;
      case 'reds': return d3.interpolateReds;
      default: return d3.interpolateGreys;
    }
  }

  /** Gets if menu is visible. */
  public get viewMenu(): boolean {
    return this.config ? this.config.menuVisible : false;
  }

  /** Gets if *download SVG* menu setting is visible. */
  public get viewDownloadSvg(): boolean {
    return this.config ? this.config.downloadSvgVisible : false;
  }

  /** Gets or sets the *ohlcv* view type: *candlesticks* or *bars*. */
  public get ohlcvViewType(): number {
    return this.ohlcvView;
  }
  public set ohlcvViewType(value: number) {
    this.ohlcvView = value;
    this.render();
  }

  /** Gets or sets if *crosshair* is visible. */
  public get viewCrosshair(): boolean {
    return this.renderCrosshair;
  }
  public set viewCrosshair(value: boolean) {
    this.renderCrosshair = value;
    this.render();
  }

  /** Gets or sets if *volume* in price pane is visible. */
  public get viewVolume(): boolean {
    return this.renderVolume;
  }
  public set viewVolume(value: boolean) {
    this.renderVolume = value;
    this.render();
  }

  /** Gets a title of the chart. */
  public get chartTitle(): string {
    return this.config ? this.config.ohlcv.name : '---';
  }

  configuration = input.required<Template.Configuration>();

  constructor() {
    effect(() => {
      const cfg = this.configuration();
      if (cfg && cfg != null) {
        this.config = cfg;
        this.ohlcvView = cfg.ohlcv.candlesticks ? ohlcvViewCandlesticks : ohlcvViewBars;
        this.renderCrosshair = cfg.crosshair;
        this.renderVolume = cfg.volumeInPricePane;
        this.currentSelection = null;
      } else {
        this.config = new Template.Configuration();
      }
      this.render();
    });

    const iconRegistry = inject(MatIconRegistry);
    const sanitizer = inject(DomSanitizer);

    iconRegistry.addSvgIcon('mb-candlesticks',
      sanitizer.bypassSecurityTrustResourceUrl('assets/mb/mb-candlesticks.svg'));
    iconRegistry.addSvgIcon('mb-bars',
      sanitizer.bypassSecurityTrustResourceUrl('assets/mb/mb-bars.svg'));

    this.renderVolume = this.config ? this.config.volumeInPricePane : true;
    this.renderCrosshair = this.config ? this.config.crosshair : true;
    this.ohlcvView = (this.config && !this.config.ohlcv.candlesticks) ? ohlcvViewBars : ohlcvViewCandlesticks;

    afterNextRender({
      write: () => {
        this.render();
      }
    });
  }

  public downloadSvg(): void {
    const d = new Date();
    const filename =
      `olcv-chart_${d.getFullYear()}-${d.getMonth()}-${d.getDay()}_${d.getHours()}-${d.getMinutes()}-${d.getSeconds()}.html`;
    const e = d3.select('#' + this.widthContainerId).node() as Element;
    Downloader.download(Downloader.serializeToSvg(Downloader.getChildElementById(e.parentNode, this.svgContainerId),
      textBeforeSvg, textAfterSvg), filename);
  }

  @HostListener('window:resize', [])
  public render(): void {
    if (!isPlatformBrowser(this.platformId) || !this.document || this.document === null) {
      return;
    }

    const cfg = this.config;
    if (!cfg || !cfg.width) {
      return;
    }

    const id = this.random;
    const e = d3.select('#' + this.widthContainerId).node() as any;
    if (!e || e === null) {
      return;
    }

    // console.log('offsetWidth=' + e.offsetWidth);
    // console.log('width=' + e.getBoundingClientRect().width);
    // const w = e.getBoundingClientRect().width;
    const w = e.offsetWidth;
    const lh = OhlcvChartComponent.layoutHorizontal(cfg, w);

    const container = d3.select('#' + this.svgContainerId);
    container.select('svg').remove();
    const svg: any = container.append('svg')
      .attr('preserveAspectRatio', 'xMinYMin meet').attr('width', lh.width);

    const lv = OhlcvChartComponent.layoutVertical(svg, cfg, lh);
    svg.attr('height', lv.height).attr('viewBox', `0 0 ${lh.width} ${lv.height}`);

    const timePane = OhlcvChartComponent.createTimePane(cfg, lh, lv, svg);

    const pricePane = OhlcvChartComponent.createPricePane(cfg, lh, lv, timePane.timeScale, timePane.timeAnnotation,
      svg, this.ohlcvView === ohlcvViewCandlesticks, this.renderVolume, this.renderCrosshair, id);

    const indicatorPanes: Chart.IndicatorPane[] = [];
    for (let i = 0; i < cfg.indicatorPanes.length; ++i) {
      const pane = OhlcvChartComponent.createIndicatorPane(i, cfg, lh, lv, timePane.timeScale, timePane.timeAxis,
        svg, this.renderCrosshair, id);
      indicatorPanes.push(pane);
    }

    const navPane = OhlcvChartComponent.createNavPane(cfg, lh, lv, svg);

    // zeslint-disable-next-line prefer-arrow/prefer-arrow-functions
    function draw(): void {
      timePane.draw();
      pricePane.draw(timePane);
      indicatorPanes.forEach(item => item.draw(timePane, pricePane));
    }

    const setCurrentSelection = (x: any) => { this.currentSelection = x; };

    // zeslint-disable-next-line prefer-arrow/prefer-arrow-functions
    function brushed(event: any): void {
      const zoomable = timePane.timeScale.zoomable();
      const zoomableNav = navPane.timeScale.zoomable();
      zoomable.domain(zoomableNav.domain());
      if (!event.selection) {
        setCurrentSelection(null);
        draw();
      } else {
        setCurrentSelection(event.selection);
        zoomable.domain(event.selection.map(zoomable.invert));
        if (!smoothBrushing) {
          draw();
        }
      }
    }

    // zeslint-disable-next-line prefer-arrow/prefer-arrow-functions
    function brushing(event: any): void {
      if (event.selection) {
        const sel = event.selection;
        if (sel[1] - sel[0] > minSelection) {
          // setCurrentSelection(sel);
          const zoomable = timePane.timeScale.zoomable();
          const zoomableNav = navPane.timeScale.zoomable();
          zoomable.domain(zoomableNav.domain());
          zoomable.domain(event.selection.map(zoomable.invert));
          draw();
        }
      }
    }

    if (smoothBrushing) {
      navPane.brush.on('brush', brushing);
    }
    navPane.brush.on('end', brushed);

    timePane.timeScale.domain(cfg.ohlcv.data.map(pricePane.priceAccessor.time));
    // console.log(OhlcvChartComponent.firstTime(cfg));
    // console.log(OhlcvChartComponent.lastTime(cfg));
    // timePane.timeScale.domain([OhlcvChartComponent.firstTime(cfg), OhlcvChartComponent.lastTime(cfg)]);
    // navPane.timeScale.domain([OhlcvChartComponent.firstTime(cfg), OhlcvChartComponent.lastTime(cfg)]);
    navPane.timeScale.domain(timePane.timeScale.domain());
    pricePane.yPrice.domain(primitives.scale.plot.ohlc(cfg.ohlcv.data, pricePane.priceAccessor).domain());

    navPane.priceScale.domain(pricePane.yPrice.domain());
    if (pricePane.yVolume) {
      pricePane.yVolume.domain(primitives.scale.plot.volume(cfg.ohlcv.data).domain());
    }
    pricePane.groupPrice.datum(cfg.ohlcv.data);
    if (this.renderVolume) {
      pricePane.groupVolume.datum(cfg.ohlcv.data);
    }

    pricePane.setIndicatorDatum();
    indicatorPanes.forEach(item => item.setIndicatorDatum());

    if (navPane.area) {
      navPane.areaSelection.datum(cfg.ohlcv.data).call(navPane.area);
    }
    if (navPane.line) {
      navPane.lineSelection.datum(cfg.ohlcv.data).call(navPane.line);
    }
    if (navPane.timeAxis) {
      navPane.timeAxisSelection.call(navPane.timeAxis);
    }

    // Associate the brush with the scale and render the brush only AFTER a domain has been applied.
    navPane.paneSelection.call(navPane.brush).selectAll('rect').attr('height', lv.navigationPane.height);

    if (this.currentSelection != null && (this.currentSelection[1] - this.currentSelection[0] > minSelection)) {
      navPane.brush.move(navPane.paneSelection, this.currentSelection);
      const zoomable = timePane.timeScale.zoomable();
      const zoomableNav = navPane.timeScale.zoomable();
      zoomable.domain(zoomableNav.domain());
      zoomable.domain(this.currentSelection.map(zoomable.invert));
    }
    draw();
  }
}
