package chande

//nolint: gofumpt
import (
	"fmt"
	"math"
	"sync"

	"mbg/trading/data"
	"mbg/trading/indicators/indicator"
	"mbg/trading/indicators/indicator/output"
)

const epsilon = 1e-12

// ChandeMomentumOscillator is a momentum indicator based on the average
// of up samples and down samples over a specified length ℓ.
//
// The calculation formula is:
//
// CMOᵢ = 100 (SUᵢ-SDᵢ) / (SUᵢ + SDᵢ),
//
// where SUᵢ (sum up) is the sum of gains and SDᵢ (sum down)
// is the sum of losses over the chosen length [i-ℓ, i].
//
// The indicator is not primed during the first ℓ updates.
type ChandeMomentumOscillator struct {
	mu           sync.RWMutex
	name         string
	description  string
	length     int
	count      int        // number of sampls seen
	ringBuffer []float64  // ring buffer of last length deltas
	ringHead   int        // next position to overwrite
	prevSample float64    // previous sample
	gainSum    float64    // sum of positive deltas in window
	lossSum    float64    // sum of |negative deltas| in window
	lastValue    float64
	primed       bool
	barFunc      data.BarFunc
	quoteFunc    data.QuoteFunc
	tradeFunc    data.TradeFunc
}

// NewChandeMomentumOscillator returns an instnce of the indicator created using supplied parameters.
func NewChandeMomentumOscillator(p *ChandeMomentumOscillatorParams) (*ChandeMomentumOscillator, error) {
	const (
		invalid = "invalid Chande momentum oscillator parameters"
		fmts    = "%s: %s"
		fmtw    = "%s: %w"
		fmtn    = "cmo(%d)"
		minlen  = 1
	)

	length := p.Length
	if length < minlen {
		return nil, fmt.Errorf(fmts, invalid, "length should be positive")
	}

	var (
		err       error
		barFunc   data.BarFunc
		quoteFunc data.QuoteFunc
		tradeFunc data.TradeFunc
	)

	if barFunc, err = data.BarComponentFunc(p.BarComponent); err != nil {
		return nil, fmt.Errorf(fmtw, invalid, err)
	}

	if quoteFunc, err = data.QuoteComponentFunc(p.QuoteComponent); err != nil {
		return nil, fmt.Errorf(fmtw, invalid, err)
	}

	if tradeFunc, err = data.TradeComponentFunc(p.TradeComponent); err != nil {
		return nil, fmt.Errorf(fmtw, invalid, err)
	}

	name := fmt.Sprintf(fmtn, length)
	desc := "Chande Momentum Oscillator " + name

	return &ChandeMomentumOscillator{
		name:        name,
		description: desc,
		length:      length,
		ringBuffer:  make([]float64, length),
		barFunc:     barFunc,
		quoteFunc:   quoteFunc,
		tradeFunc:   tradeFunc,
	}, nil
}

// IsPrimed indicates whether an indicator is primed.
func (s *ChandeMomentumOscillator) IsPrimed() bool {
	s.mu.RLock()
	defer s.mu.RUnlock()

	return s.primed
}

// Metadata describes an output data of the indicator.
// It always has a single scalar output -- the calculated value of the indicator.
func (s *ChandeMomentumOscillator) Metadata() indicator.Metadata {
	return indicator.Metadata{
		Type: indicator.ChandeMomentumOscillator,
		Outputs: []output.Metadata{
			{
				Kind:        int(ChandeMomentumOscillatorValue),
				Type:        output.Scalar,
				Name:        s.name,
				Description: s.description,
			},
		},
	}
}

// Update updates the value of the iChande momentum oscillator given the next sample.
//
// The indicator is not primed during the first ℓ updates.
func (s *ChandeMomentumOscillator) Update(sample float64) float64 {
	if math.IsNaN(sample) {
		return sample
	}

	s.mu.Lock()
	defer s.mu.Unlock()

	s.count++
	if s.count == 1 {
		s.prevSample = sample
		s.lastValue = math.NaN()

		return s.lastValue
	}

	// New delta
	delta := sample - s.prevSample
	s.prevSample = sample

	if !s.primed {
		// Fill until we have s.length deltas (i.e., s.length+1 samples)
		s.ringBuffer[s.ringHead] = delta
		s.ringHead = (s.ringHead + 1) % s.length

		if delta > 0 {
			s.gainSum += delta
		} else if delta < 0 {
			s.lossSum += -delta
		}

		if s.count <= s.length {
			s.lastValue = math.NaN()

			return s.lastValue
		}

		// Now we have exactly s.length deltas in the buffer
		s.primed = true
	} else {
		// Remove oldest delta and add the new one
		old := s.ringBuffer[s.ringHead]
		if old > 0 {
			s.gainSum -= old
		} else if old < 0 {
			s.lossSum -= -old
		}

		s.ringBuffer[s.ringHead] = delta
		s.ringHead = (s.ringHead + 1) % s.length

		if delta > 0 {
			s.gainSum += delta
		} else if delta < 0 {
			s.lossSum += -delta
		}

		// Clamp to avoid tiny negative sums from FP noise
		if s.gainSum < 0 {
			s.gainSum = 0
		}

		if s.lossSum < 0 {
			s.lossSum = 0
		}
	}

	den := s.gainSum + s.lossSum
	if math.Abs(den) < epsilon {
		s.lastValue = 0

		return s.lastValue
	}

	s.lastValue = 100.0 * (s.gainSum - s.lossSum) / den

	return s.lastValue
}

// UpdateScalar updates the indicator given the next scalar sample.
func (s *ChandeMomentumOscillator) UpdateScalar(sample *data.Scalar) indicator.Output {
	output := make([]any, 1)
	output[0] = data.Scalar{Time: sample.Time, Value: s.Update(sample.Value)}

	return output
}

// UpdateBar updates the indicator given the next bar sample.
func (s *ChandeMomentumOscillator) UpdateBar(sample *data.Bar) indicator.Output {
	return s.UpdateScalar(&data.Scalar{Time: sample.Time, Value: s.barFunc(sample)})
}

// UpdateQuote updates the indicator given the next quote sample.
func (s *ChandeMomentumOscillator) UpdateQuote(sample *data.Quote) indicator.Output {
	return s.UpdateScalar(&data.Scalar{Time: sample.Time, Value: s.quoteFunc(sample)})
}

// UpdateTrade updates the indicator given the next trade sample.
func (s *ChandeMomentumOscillator) UpdateTrade(sample *data.Trade) indicator.Output {
	return s.UpdateScalar(&data.Scalar{Time: sample.Time, Value: s.tradeFunc(sample)})
}
