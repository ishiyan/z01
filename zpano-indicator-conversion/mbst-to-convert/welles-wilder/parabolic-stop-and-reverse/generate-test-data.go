//go:build ignore
package main
import (
	"fmt"
	"math"
)
func sarext(highs, lows []float64, startValue, offsetOnReverse, afInitLong, afLong, afMaxLong, afInitShort, afShort, afMaxShort float64) []float64 {
	n := len(highs)
	if n < 2 {
		return nil
	}
	// Clamp acceleration factors
	if afInitLong > afMaxLong {
		afInitLong = afMaxLong
	}
	if afLong > afMaxLong {
		afLong = afMaxLong
	}
	if afInitShort > afMaxShort {
		afInitShort = afMaxShort
	}
	if afShort > afMaxShort {
		afShort = afMaxShort
	}
	// Output: index 0 = NaN (lookback=1), indices 1..n-1 = valid
	out := make([]float64, n)
	out[0] = math.NaN()
	startIdx := 1
	var isLong bool
	var ep, sar float64
	var curAFLong, curAFShort float64
	curAFLong = afInitLong
	curAFShort = afInitShort
	// Determine initial direction
	if startValue == 0 {
		// Use MINUS_DM logic: if prevLow - low > high - prevHigh and prevLow - low > 0 => short
		minusDM := lows[startIdx-1] - lows[startIdx]
		plusDM := highs[startIdx] - highs[startIdx-1]
		if minusDM < 0 {
			minusDM = 0
		}
		if plusDM < 0 {
			plusDM = 0
		}
		if minusDM > plusDM {
			isLong = false
		} else {
			isLong = true
		}
	} else if startValue > 0 {
		isLong = true
	} else {
		isLong = false
	}
	// Initialize SAR and EP
	newHigh := highs[startIdx-1]
	newLow := lows[startIdx-1]
	if startValue == 0 {
		if isLong {
			ep = highs[startIdx]
			sar = newLow
		} else {
			ep = lows[startIdx]
			sar = newHigh
		}
	} else if startValue > 0 {
		ep = highs[startIdx]
		sar = startValue
	} else {
		ep = lows[startIdx]
		sar = math.Abs(startValue)
	}
	newLow = lows[startIdx]
	newHigh = highs[startIdx]
	todayIdx := startIdx
	outIdx := 1 // output index (0 is NaN)
	for todayIdx <= n-1 {
		prevLow := newLow
		prevHigh := newHigh
		newLow = lows[todayIdx]
		newHigh = highs[todayIdx]
		todayIdx++
		if isLong {
			if newLow <= sar {
				// Switch to short
				isLong = false
				sar = ep
				if sar < prevHigh {
					sar = prevHigh
				}
				if sar < newHigh {
					sar = newHigh
				}
				if offsetOnReverse != 0.0 {
					sar += sar * offsetOnReverse
				}
				out[outIdx] = -sar
				outIdx++
				curAFShort = afInitShort
				ep = newLow
				sar = sar + curAFShort*(ep-sar)
				if sar < prevHigh {
					sar = prevHigh
				}
				if sar < newHigh {
					sar = newHigh
				}
			} else {
				// No switch
				out[outIdx] = sar
				outIdx++
				if newHigh > ep {
					ep = newHigh
					curAFLong += afLong
					if curAFLong > afMaxLong {
						curAFLong = afMaxLong
					}
				}
				sar = sar + curAFLong*(ep-sar)
				if sar > prevLow {
					sar = prevLow
				}
				if sar > newLow {
					sar = newLow
				}
			}
		} else {
			if newHigh >= sar {
				// Switch to long
				isLong = true
				sar = ep
				if sar > prevLow {
					sar = prevLow
				}
				if sar > newLow {
					sar = newLow
				}
				if offsetOnReverse != 0.0 {
					sar -= sar * offsetOnReverse
				}
				out[outIdx] = sar
				outIdx++
				curAFLong = afInitLong
				ep = newHigh
				sar = sar + curAFLong*(ep-sar)
				if sar > prevLow {
					sar = prevLow
				}
				if sar > newLow {
					sar = newLow
				}
			} else {
				// No switch
				out[outIdx] = -sar
				outIdx++
				if newLow < ep {
					ep = newLow
					curAFShort += afShort
					if curAFShort > afMaxShort {
						curAFShort = afMaxShort
					}
				}
				sar = sar + curAFShort*(ep-sar)
				if sar < prevHigh {
					sar = prevHigh
				}
				if sar < newHigh {
					sar = newHigh
				}
			}
		}
	}
	return out
}
func main() {
	// Wilder data
	wilderHigh := []float64{
		51.12,
		52.35, 52.1, 51.8, 52.1, 52.5, 52.8, 52.5, 53.5, 53.5, 53.8, 54.2, 53.4, 53.5,
		54.4, 55.2, 55.7, 57, 57.5, 58, 57.7, 58, 57.5, 57, 56.7, 57.5,
		56.70, 56.00, 56.20, 54.80, 55.50, 54.70, 54.00, 52.50, 51.00, 51.50, 51.70, 53.00,
	}
	wilderLow := []float64{
		50.0,
		51.5, 51, 50.5, 51.25, 51.7, 51.85, 51.5, 52.3, 52.5, 53, 53.5, 52.5, 52.1, 53,
		54, 55, 56, 56.5, 57, 56.5, 57.3, 56.7, 56.3, 56.2, 56,
		55.50, 55.00, 54.90, 54.00, 54.50, 53.80, 53.00, 51.50, 50.00, 50.50, 50.20, 51.50,
	}
	// Standard SAR = SAREXT with all defaults (same accel for long/short)
	// The test uses TA_SAR with acceleration=0.02, maximum=0.20
	// TA_SAR returns positive values always; SAREXT returns negative when short.
	// But the Wilder test spot checks are all from TA_SAR which doesn't negate.
	// Let's also compute TA_SAR style (absolute values) for verification.
	wilderOut := sarext(wilderHigh, wilderLow, 0, 0, 0.02, 0.02, 0.20, 0.02, 0.02, 0.20)
	fmt.Println("// Wilder SAREXT output (negative=short):")
	for i, v := range wilderOut {
		if math.IsNaN(v) {
			fmt.Printf("// [%d] NaN\n", i)
		} else {
			fmt.Printf("// [%d] %.6f (abs=%.6f)\n", i, v, math.Abs(v))
		}
	}
	// Verify Wilder spot checks (TA_SAR, output indices from begIdx=1):
	// output[0] = 50.00, output[1] = 50.047, output[4] = 50.182, output[35] = 52.93, output[36] = 50.00
	fmt.Println("\n// Wilder spot checks (TA_SAR absolute values):")
	indices := []int{0, 1, 4, 35, 36}
	expected := []float64{50.00, 50.047, 50.182, 52.93, 50.00}
	for i, idx := range indices {
		outVal := math.Abs(wilderOut[idx+1]) // +1 because wilderOut[0]=NaN, output[0]=wilderOut[1]
		fmt.Printf("// output[%d] = %.6f, expected = %.3f, diff = %.6f\n", idx, outVal, expected[i], outVal-expected[i])
	}
	// 252-bar dataset
	highs := []float64{
		93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,
		96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,
		90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,
		87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,
		84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,
		90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,
		90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,
		89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,
		104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,
		123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,
		116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,
		116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,
		125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,
		138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,
		125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,
		124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,
		124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,
		132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,
		129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,
		123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,
		108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,
		96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,
		95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,
		104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,
		107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,
		110.5, 109.5,
	}
	lows := []float64{
		90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,
		92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,
		87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,
		84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,
		82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,
		89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,
		88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,
		86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,
		101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,
		117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,
		112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,
		112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,
		123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,
		135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,
		122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,
		121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,
		122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,
		128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,
		126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,
		119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,
		105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,
		93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,
		93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,
		102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,
		104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,
		108.56, 106.62,
	}
	result := sarext(highs, lows, 0, 0, 0.02, 0.02, 0.20, 0.02, 0.02, 0.20)
	fmt.Println("\n// 252-bar SAREXT expected values:")
	fmt.Println("func testExpected() []float64 {")
	fmt.Println("\treturn []float64{")
	for i := 0; i < len(result); i += 5 {
		fmt.Print("\t\t")
		end := i + 5
		if end > len(result) {
			end = len(result)
		}
		for j := i; j < end; j++ {
			if math.IsNaN(result[j]) {
				fmt.Print("math.NaN()")
			} else {
				fmt.Printf("%.10f", result[j])
			}
			if j < len(result)-1 {
				fmt.Print(", ")
			}
		}
		fmt.Println()
	}
	fmt.Println("\t}")
	fmt.Println("}")
}
