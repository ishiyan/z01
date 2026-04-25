import * as d3 from 'd3';

import { primitives } from '../../d3-primitives';
import { IndicatorArrow, verticalArrowGap, minArrowWidth, arowHeightToWidthRatio } from './indicator-arrow';
import { IndicatorBand } from './indicator-band';
import { IndicatorLineArea } from './indicator-line-area';
import { IndicatorLine } from './indicator-line';
import { IndicatorHorizontal } from './indicator-horizontal';
import { TimePane } from './time-pane';

export class PricePane {
  group: any;
  groupPrice: any;
  groupVolume: any;
  groupAxisLeft: any;
  groupAxisRight: any;
  yPrice!: d3.ScaleLinear<number, number>;
  yVolume!: d3.ScaleLinear<number, number>;
  yMarginFactorTop!: number;
  yMarginFactorBottom!: number;
  yAxisLeft: any;
  yAxisRight: any;
  priceShape: any;
  priceAccessor: any;
  volume: any;
  indicatorBands: IndicatorBand[] = [];
  indicatorLineAreas: IndicatorLineArea[] = [];
  indicatorHorizontals: IndicatorHorizontal[] = [];
  indicatorLines: IndicatorLine[] = [];
  indicatorArrows: IndicatorArrow[] = [];

  draw(timePane: TimePane): void {
    const datum = this.groupPrice.datum();
    const datumLastIndex = datum.length - 1;
    const timeDomain: [number, number] = timePane.timeScale.zoomable().domain();
    let min = Math.round(timeDomain[0]);
    let max = Math.round(timeDomain[1]);
    if (min < 0) {
      min = 0;
    }
    if (max > datumLastIndex) {
      max = datumLastIndex;
    }
    const priceDomain: [number, number] =
      primitives.scale.plot.ohlc(datum.slice.apply(datum, [min, max]), this.priceAccessor).domain() as  [number, number];
    if (datum[min] !== undefined) {
      min = +datum[min].time;
    }
    if (datum[max] !== undefined) {
      max = +datum[max].time;
    }
    let minPrice = priceDomain[0];
    let maxPrice = priceDomain[1];
    for (const item of this.indicatorBands) {
      const data = item.data;
      for (const d of data) {
        const t = +d.time;
        if (min <= t && t <= max) {
          if (minPrice > d.lower) {
            minPrice = d.lower;
          }
          if (maxPrice < d.upper) {
            maxPrice = d.upper;
          }
        }
      }
    }
    for (const item of this.indicatorLineAreas) {
      const data = item.data;
      const value = item.value;
      for (const d of data) {
        const t = +d.time;
        if (min <= t && t <= max) {
          if (minPrice > d.value) {
            minPrice = d.value;
          }
          if (maxPrice < d.value) {
            maxPrice = d.value;
          }
          if (minPrice > value) {
            minPrice = value;
          }
          if (maxPrice < value) {
            maxPrice = value;
          }
        }
      }
    }
    for (const item of this.indicatorHorizontals) {
      const value = item.value;
      if (minPrice > value) {
        minPrice = value;
      }
      if (maxPrice < value) {
        maxPrice = value;
      }
    }
    for (const item of this.indicatorLines) {
      const data = item.data;
      for (const d of data) {
        const t = +d.time;
        if (min <= t && t <= max) {
          const value = d.value;
          if (minPrice > value) {
            minPrice = value;
          }
          if (maxPrice < value) {
            maxPrice = value;
          }
        }
      }
    }
    let arrowWidth;
    let arrowHeight;
    if (this.indicatorArrows.length > 0) {
      const slotWidth = this.priceShape.width()(timePane.timeScale);
      arrowWidth = slotWidth < minArrowWidth ? minArrowWidth : slotWidth;
      arrowHeight = arrowWidth * arowHeightToWidthRatio;
      const arrowDelta = arrowHeight + verticalArrowGap;
      const h = this.yPrice.range()[0];
      if (arrowDelta < h) {
        const delta = h - arrowDelta;
        for (const item of this.indicatorArrows) {
          const p = item.price;
          const ph = p * h;
          if (maxPrice < p) {
            maxPrice = p;
          }
          if (minPrice > p) {
            minPrice = p;
          }
          if (item.isDown) {
            const maxNew = (ph - minPrice * arrowDelta) / delta;
            if (maxPrice < maxNew) {
              maxPrice = maxNew;
            }
          } else {
            const minNew = (ph - maxPrice * arrowDelta) / delta;
            if (minPrice > minNew) {
              minPrice = minNew;
            }
            if (minPrice < 0) {
              minPrice = 0;
            }
          }
        }
      }
    }
    minPrice *= this.yMarginFactorBottom;
    maxPrice *= this.yMarginFactorTop;
    this.yPrice.domain([minPrice, maxPrice]).nice();

    // Draw bands and areas below price and lines.
    this.indicatorBands.forEach(item => item.path.attr('d', item.area));
    this.indicatorLineAreas.forEach(item => item.path.attr('d', item.area));

    // Draw horizontals above bands and areas but below price and lines.
    this.groupPrice.call(this.priceShape);
    if (this.volume) {
      this.groupVolume.call(this.volume);
    }

    this.indicatorHorizontals.forEach(item => item.path.attr('d', item.line));
    this.indicatorLines.forEach(item => item.path.attr('d', item.line));
    for (const item of this.indicatorArrows) {
      item.arrow.width(arrowWidth).height(arrowHeight);
      item.path.attr('d', item.arrow);
    }

    if (this.yAxisLeft) {
      this.groupAxisLeft.call(this.yAxisLeft);
    }
    if (this.yAxisRight) {
      this.groupAxisRight.call(this.yAxisRight);
    }
  }

  setIndicatorDatum(): void {
    this.indicatorBands.forEach(item => item.path.datum(item.data));
    this.indicatorLineAreas.forEach(item => item.path.datum(item.data));
    this.indicatorHorizontals.forEach(item => item.path.datum(item.data));
    this.indicatorLines.forEach(item => item.path.datum(item.data));
  }
}
