package day03

import (
	"log"
	"math"
	"strconv"
	"strings"

	. "aoc/internal/inputparser"
	. "aoc/internal/utility"
)

type powerAnalyser struct {
	readings []string
}

func NewPowerAnalyser(input []byte) *powerAnalyser {
	readings := ToStringArray(input)
	return &powerAnalyser{readings}
}

func (a *powerAnalyser) CalculateConsumption() int {
	gamma, epsilon := calculatePowerRates(a.readings)

	g := ratingToInt(gamma)
	e := ratingToInt(epsilon)

	return int(g * e)
}

func calculatePowerRates(readings []string) ([]int, []int) {
	readingLength := len(readings[0])

	gamma := make([]int, len(readings[0]))
	for x := 0; x < readingLength; x++ {
		var sumCol float64
		for _, v := range readings {
			bitVal := v[x]

			if bitVal == '1' {
				sumCol++
			}
		}
		avg := sumCol / float64(len(readings))
		gamma[x] = int(math.Round(avg))
	}

	epsilon := gammaToEpsilon(gamma)

	return gamma, epsilon
}

func (a *powerAnalyser) LifeSupportRating() int {
	o2Rating := filterReadings(a.readings, roundUp, 0)
	co2Rating := filterReadings(a.readings, roundDown, 0)

	o := binaryStringToInt(o2Rating)
	c := binaryStringToInt(co2Rating)

	return int(o * c)
}

func roundUp(b float64) uint8 {
	switch {
	case b >= 0.5:
		return '1'
	default:
		return '0'
	}
}

func roundDown(b float64) uint8 {
	switch {
	case b >= 0.5:
		return '0'
	default:
		return '1'
	}
}

func filterReadings(readings []string, filter func(c float64) uint8, pos int) string {
	var sumCol int

	for _, reading := range readings {
		bit := reading[pos]
		if bit == '1' {
			sumCol++
		}
	}

	avg := float64(sumCol) / float64(len(readings))
	significantBit := filter(avg)

	var validReadings []string
	for _, reading := range readings {
		bit := reading[pos]
		if bit == significantBit {
			validReadings = append(validReadings, reading)
		}
	}

	if len(validReadings) > 1 {
		return filterReadings(validReadings, filter, pos+1)
	}

	return validReadings[0]
}

func gammaToEpsilon(gamma []int) []int {
	epsilon := make([]int, len(gamma))

	for i, v := range gamma {
		switch v {
		case 1:
			epsilon[i] = 0
		default:
			epsilon[i] = 1
		}
	}

	return epsilon
}

func ratingToInt(rating []int) int64 {
	return binaryStringToInt(strings.Join(Map(rating, strconv.Itoa), ""))
}

func binaryStringToInt(rating string) int64 {
	val, err := strconv.ParseInt(rating, 2, 64)
	if err != nil {
		log.Fatal(err)
	}

	return val
}
