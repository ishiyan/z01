import * as d3 from 'd3';

import { Heatmap } from '../../entities/heatmap';
import { IndicatorHeatmap } from './indicator-heatmap';
import { IndicatorBand } from './indicator-band';
import { IndicatorLineArea } from './indicator-line-area';
import { IndicatorLine } from './indicator-line';
import { IndicatorHorizontal } from './indicator-horizontal';
import { TimePane } from './time-pane';
import { PricePane } from './price-pane';

export class IndicatorPane {
  group: any;
  groupAxisLeft: any;
  groupAxisRight: any;
  yValue!: d3.ScaleLinear<number, number>;
  yMarginFactorTop!: number;
  yMarginFactorBottom!: number;
  yAxisLeft: any;
  yAxisRight: any;
  indicatorHeatmap?: IndicatorHeatmap;
  indicatorBands: IndicatorBand[] = [];
  indicatorLineAreas: IndicatorLineArea[] = [];
  indicatorHorizontals: IndicatorHorizontal[] = [];
  indicatorLines: IndicatorLine[] = [];

  draw(timePane: TimePane, pricePane: PricePane): void {
    const datum = pricePane.groupPrice.datum();
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
    if (datum[min] !== undefined) {
      min = +datum[min].time;
    }
    if (datum[max] !== undefined) {
      max = +datum[max].time;
    }
    let minValue = Number.MAX_VALUE;
    let maxValue = Number.MIN_VALUE;
    let minIntensity = Number.MAX_VALUE;
    let maxIntensity = Number.MIN_VALUE;
    if (this.indicatorHeatmap) {
      const data = this.indicatorHeatmap.data;
      if (data.length > 0) {
        const paramFirst = data[0].parameterFirst;
        const paramLast = data[0].parameterLast;
        minValue = Math.min(paramFirst, paramLast);
        maxValue = Math.max(paramFirst, paramLast);
        for (const d of data) {
          const t = +d.time;
          if (min <= t && t <= max && d.values.length > 0) {
            if (minIntensity > d.valueMin) {
              minIntensity = d.valueMin;
            }
            if (maxIntensity < d.valueMax) {
              maxIntensity = d.valueMax;
            }
          }
        }
        if (maxIntensity === Number.MIN_VALUE || minIntensity === Number.MAX_VALUE) {
          minIntensity = 0;
          maxIntensity = 0;
        }
      }
    }
    for (const item of this.indicatorBands) {
      const data = item.data;
      for (const d of data) {
        const t = +d.time;
        if (min <= t && t <= max) {
          if (minValue > d.lower) {
            minValue = d.lower;
          }
          if (maxValue < d.upper) {
            maxValue = d.upper;
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
          if (minValue > d.value) {
            minValue = d.value;
          }
          if (maxValue < d.value) {
            maxValue = d.value;
          }
          if (minValue > value) {
            minValue = value;
          }
          if (maxValue < value) {
            maxValue = value;
          }
        }
      }
    }
    for (const item of this.indicatorHorizontals) {
      const value = item.value;
      if (minValue > value) {
        minValue = value;
      }
      if (maxValue < value) {
        maxValue = value;
      }
    }
    for (const item of this.indicatorLines) {
      const data = item.data;
      for (const d of data) {
        const t = +d.time;
        if (min <= t && t <= max) {
          const value = d.value;
          if (minValue > value) {
            minValue = value;
          }
          if (maxValue < value) {
            maxValue = value;
          }
        }
      }
    }
    if (this.indicatorHeatmap) {
      this.yValue.domain([minValue, maxValue]).nice();
    } else {
      minValue *= this.yMarginFactorBottom;
      maxValue *= this.yMarginFactorTop;
      this.yValue.domain([minValue, maxValue]).nice();
    }

    if (this.indicatorHeatmap) {
      this.indicatorHeatmap.path.selectAll('image').remove();
      const data = this.indicatorHeatmap.data;
      if (data.length > 0) {
        const slotWidth = 1 + (data.length > 1 ?
          timePane.timeScale(data[1].time) - timePane.timeScale(data[0].time) :
          pricePane.priceShape.width()(timePane.timeScale));
        const h = this.indicatorHeatmap.height;
        const gradient = this.indicatorHeatmap.gradient;
        const invertGradient = this.indicatorHeatmap.invertGradient;
        const periodFirst = data[0].parameterFirst;
        const periodLast = data[0].parameterLast;
        const periodRes = data[0].parameterResolution;
        const periodInverted = periodFirst > periodLast;
        const periodMin = Math.min(periodFirst, periodLast);
        for (const d of data) {
          const t = +d.time;
          if (min <= t && t <= max && d.values.length > 0) {
            const xMid = timePane.timeScale(t);
            const xMin = xMid - slotWidth / 2;
            const img = this.heatColumn(d, periodMin, periodRes, periodInverted, d.valueMin, d.valueMax,
              gradient, invertGradient, slotWidth, h);
            this.indicatorHeatmap.path.append('image').attr('x', xMin).attr('width', slotWidth)
              .attr('y', 0).attr('height', h).attr('preserveAspectRatio', 'none')
              .attr('xlink:href', img.toDataURL());
          }
        }
      }
    }

    // Draw bands and areas below lines.
    this.indicatorBands.forEach(item => item.path.attr('d', item.area));
    this.indicatorLineAreas.forEach(item => item.path.attr('d', item.area));

    // Draw horizontals above bands and areas but below lines.
    this.indicatorHorizontals.forEach(item => item.path.attr('d', item.line));
    this.indicatorLines.forEach(item => item.path.attr('d', item.line));

    if (this.yAxisLeft) {
      this.groupAxisLeft.call(this.yAxisLeft);
    }
    if (this.yAxisRight) {
      this.groupAxisRight.call(this.yAxisRight);
    }
  }

  setIndicatorDatum(): void {
    for (const item of this.indicatorBands) {
      item.path.datum(item.data);
    }
    for (const item of this.indicatorLineAreas) {
      item.path.datum(item.data);
    }
    for (const item of this.indicatorHorizontals) {
      item.path.datum(item.data);
    }
    for (const item of this.indicatorLines) {
      item.path.datum(item.data);
    }
  }

  private heatColumn(heatmap: Heatmap, periodMin: number, periodRes: number, periodInverted: boolean, min: number, max: number,
                     color: any, invertColor: boolean, width: number, height: number): HTMLCanvasElement {
    const canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    canvas.style.width = width + 'px';
    canvas.style.height = height + 'px';
    canvas.style.imageRendering = 'pixelated';
    const context = canvas.getContext('2d');
    const y = this.yValue;
    const heat = heatmap.values;
    if (min !== 0 && max !== 1) {
      const delta = max - min;
      for (let i = 0; i < height; ++i) {
        const index = Math.round((y.invert(i) - periodMin) * periodRes);
        const value = (heat[index] - min) / delta;
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        context.fillStyle = color(invertColor ? 1 - value : value);
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        context.fillRect(0, periodInverted ? height - i : i, width, 1);
      }
    } else {
      for (let i = 0; i < height; ++i) {
        const index = Math.round((y.invert(i) - periodMin) * periodRes);
        const value = heat[index];
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        context.fillStyle = color(invertColor ? 1 - value : value);
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        context.fillRect(0, periodInverted ? height - i : i, width, 1);
      }
    }
    return canvas;
  }
}
