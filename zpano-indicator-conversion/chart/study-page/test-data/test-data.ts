import { Configuration } from 'projects/mb/src/lib/charts/ohlcv-chart/template/configuration';

import { testDataOhlcv } from '../../../test-data/indicators/test-data-ohlcv';
import { testDataBb } from '../../../test-data/indicators/test-data-bb';
import { testDataBbMa } from '../../../test-data/indicators/test-data-bb-ma';
import { testDataBbLo } from '../../../test-data/indicators/test-data-bb-lo';
import { testDataBbUp } from '../../../test-data/indicators/test-data-bb-up';
import { testDataBbPercentB } from '../../../test-data/indicators/test-data-bb-percentb';
import { testDataBbBw } from '../../../test-data/indicators/test-data-bb-bw';
import { testDataGoertzel1 } from '../../../test-data/indicators/test-data-goertzel_1';
import { testDataOutputs } from './test-data-bb-goertzel-combined';

const outputsCount = testDataOutputs.length;
const ohlcvCount = testDataOhlcv.length;

export class TestData {
  private static configTemplate: Configuration = {
    width: '100%', // widthMin: 500, widthMax: 700,
    navigationPane: {
      height: 30, // heightMin: 30, heightMax: 30, timeTicksFormat: '%Y-%m-%d',
      hasLine: true, hasArea: false, hasTimeAxis: true, timeTicks: 0,
      // hasLine: true, hasArea: false, hasTimeAxis: true,
      // hasLine: false, hasArea: true, hasTimeAxis: false
    },
    heightNavigationPane: 30, // heightNavigationPaneMin: 20, heightNavigationPaneMax: 60,
    // navigationLine: true, navigationTimeAxis: true,
    timeAnnotationFormat: '%Y-%m-%d', // timeTicks: 5, timeTicksFormat: '%Y-%m-%d',
    axisLeft: true,
    axisRight: false,
    margin: { left: 0, top: 10, right: 20, bottom: 0 },
    ohlcv: { name: 'BRILL@XAMS', data: [], candlesticks: true },
    pricePane: {
      height: '30%', valueFormat: ',.2f', /*valueTicks: 10,*/ valueMarginPercentageFactor: 0.01, // heightMin: 300, heightMax: 300,
      bands: [
        {
          name: 'bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 5,
          color: 'rgba(0,255,0,0.3)', legendColor: 'rgba(0,200,0,1)', interpolation: 'natural'
        },
      ], lineAreas: [], horizontals: [], lines: [
        {
          name: 'ma-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 0,
          color: 'red', width: 1, dash: '', interpolation: 'natural'
        },
        {
          name: 'lo-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 1,
          color: 'blue', width: 0.5, dash: '5,5', interpolation: 'cardinal'
        },
        {
          name: 'up-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 2,
          color: 'blue', width: 0.5, dash: '2,2', interpolation: 'linear'
        }
      ], arrows: []
    },
    indicatorPanes: [
      {
        height: '60', valueFormat: ',.2f', /*valueTicks: 5,*/ valueMarginPercentageFactor: 0.01,
        bands: [], lineAreas: [], horizontals: [
          { value: 0, color: 'red', width: 0.5, dash: '' },
          { value: 1, color: 'red', width: 0.5, dash: '' }
        ], lines: [
          {
            name: '%b(c)-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 3,
            color: 'green', width: 1, dash: '', interpolation: 'natural'
          }
        ], arrows: []
      },
      {
        height: '60', valueFormat: ',.2f', /*valueTicks: 3,*/ valueMarginPercentageFactor: 0.01,
        bands: [], lineAreas: [
          {
            name: 'bw-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 4,
            color: 'rgba(0,0,255,0.3)', legendColor: '#0000ff', value: 0.3, interpolation: 'natural'
          }
        ], horizontals: [
          { value: 0.3, color: 'blue', width: 0.5, dash: '' }
        ], lines: [], arrows: []
      },
      {
        height: '120', valueFormat: ',.2f', /*valueTicks: 5,*/ valueMarginPercentageFactor: 0, // heightMin: 50, heightMax: 100,
        heatmap: {
          name: 'goertzel(64, [2,28,1/10])', gradient: 'Viridis', invertGradient: false,
          data: [], indicator: 1, output: 0
        }, bands: [], lineAreas: [], horizontals: [], lines: [], arrows: []
      }
    ],
    crosshair: false,
    volumeInPricePane: true,
    menuVisible: true, downloadSvgVisible: true
  };

  private dataLength = 0;
  public config: Configuration;

  public static get configDataPrefilled(): Configuration {
    const cloned = TestData.deepCopy(TestData.configTemplate) as Configuration;
    cloned.ohlcv.data = testDataOhlcv;
    cloned.pricePane.bands[0].data = testDataBb;
    cloned.pricePane.lines[0].data = testDataBbMa;
    cloned.pricePane.lines[1].data = testDataBbLo;
    cloned.pricePane.lines[2].data = testDataBbUp;
    cloned.indicatorPanes[0].lines[0].data = testDataBbPercentB;
    cloned.indicatorPanes[1].lineAreas[0].data = testDataBbBw;
    // @ts-ignore
    cloned.indicatorPanes[2].heatmap.data = testDataGoertzel1;
    TestData.addArrows(cloned);
    return cloned;
  }

  private static add(count: number, currentCount: number, cfg: Configuration): number {
    const sum = currentCount + count;
    const newCount = sum > outputsCount ? outputsCount : sum;
    for (let i = currentCount; i < newCount; ++i) {
      cfg.ohlcv.data.push(testDataOhlcv[i]);
      const indicators = (testDataOutputs[i] as any).indicators;
      for (const band of cfg.pricePane.bands) {
        const outputs = (indicators[band.indicator] as any).outputs;
        band.data.push(outputs[band.output]);
      }
      for (const lineArea of cfg.pricePane.lineAreas) {
        const outputs = (indicators[lineArea.indicator] as any).outputs;
        lineArea.data.push(outputs[lineArea.output]);
      }
      for (const line of cfg.pricePane.lines) {
        const outputs = (indicators[line.indicator] as any).outputs;
        line.data.push(outputs[line.output]);
      }
      for (const pane of cfg.indicatorPanes) {
        if (pane.heatmap) {
          const heatmap = pane.heatmap;
          const outputs = (indicators[heatmap.indicator] as any).outputs;
          heatmap.data.push(outputs[heatmap.output]);
        }
        for (const band of pane.bands) {
          const outputs = (indicators[band.indicator] as any).outputs;
          band.data.push(outputs[band.output]);
        }
        for (const lineArea of pane.lineAreas) {
          const outputs = (indicators[lineArea.indicator] as any).outputs;
          lineArea.data.push(outputs[lineArea.output]);
        }
        for (const line of pane.lines) {
          const outputs = (indicators[line.indicator] as any).outputs;
          line.data.push(outputs[line.output]);
        }
      }
    }
    TestData.addArrows(cfg);
    return newCount;
  }

  private static addArrows(cfg: Configuration): void {
    const count = cfg.ohlcv.data.length;
    if (count > 25 && cfg.pricePane.arrows.length < 1) {
      const arrow = {
        name: 'sell', down: true, time: testDataOhlcv[25].time, /*value: testDataOhlcv[25].high,*/
        indicator: 0, output: 0, color: 'rgb(255,0,0)'
      };
      cfg.pricePane.arrows.push(arrow);
    }
    if (count > 26 && cfg.pricePane.arrows.length < 2) {
      const arrow = {
        name: 'buy', down: false, time: testDataOhlcv[26].time, /*value: testDataOhlcv[26].low,*/
        indicator: 0, output: 0, color: 'rgb(0,255,0)'
      };
      cfg.pricePane.arrows.push(arrow);
    }
  }

  private static deepCopy(obj: any): any {
    // Handle the 3 simple types, and null or undefined.
    if (null == obj || 'object' !== typeof obj) {
      return obj;
    }
    // Handle Date.
    if (obj instanceof Date) {
      const copy = new Date();
      copy.setTime(obj.getTime());
      return copy;
    }
    // Handle Array.
    if (obj instanceof Array) {
      const copy = [];
      for (let i = 0, len = obj.length; i < len; i++) {
        copy[i] = TestData.deepCopy(obj[i]);
      }
      return copy;
    }
    // Handle Object.
    if (obj instanceof Object) {
      const copy: any = {};
      for (const attr of Object.keys(obj)) {
        copy[attr] = TestData.deepCopy(obj[attr]);
      }
      return copy;
    }
    throw new Error('Unable to copy obj! Its type isn\'t supported.');
  }

  public constructor() {
    this.config = TestData.configDataPrefilled; // <OhlcvChartConfig>TestData.deepCopy(TestData.configTemplate);
    if (outputsCount !== ohlcvCount) {
      throw new Error('Lengths does not match: ohlcv=' + ohlcvCount + ', outputs=' + outputsCount);
    }
  }

  public clear(): void {
    this.config.ohlcv.data = [];
    this.config.pricePane.bands[0].data = [];
    this.config.pricePane.lines[0].data = [];
    this.config.pricePane.lines[1].data = [];
    this.config.pricePane.lines[2].data = [];
    this.config.pricePane.arrows = [];
    this.config.indicatorPanes[0].lines[0].data = [];
    this.config.indicatorPanes[1].lineAreas[0].data = [];
    // @ts-ignore
    this.config.indicatorPanes[2].heatmap.data = [];
    this.dataLength = 0;
  }

  public addSingle(): void {
    this.dataLength = TestData.add(1, this.dataLength, this.config);
  }

  public addTen(): void {
    this.dataLength = TestData.add(10, this.dataLength, this.config);
  }

  public addAll(): void {
    this.dataLength = TestData.add(ohlcvCount, this.dataLength, this.config);
  }
}
