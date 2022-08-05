package day03_test

import (
	"testing"

	"aoc/cmd/day03"
	"aoc/internal/tests/adventclient"

	. "aoc/internal/tests/assertions"
	. "aoc/internal/tests/config"
)

const sampleInput = `00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010`

func TestPowerAnalyserCalculateConsumption(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 198},
		{"actual", readActualInput, 845186},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				simulator := day03.NewPowerAnalyser(test.readInput(t))
				actual := simulator.CalculateConsumption()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func TestPowerAnalyserCalculateLifeSupportRating(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 230},
		{"actual", readActualInput, 4636702},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				simulator := day03.NewPowerAnalyser(test.readInput(t))
				actual := simulator.LifeSupportRating()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func readSampleInput(t *testing.T) []byte {
	return []byte(sampleInput)
}

func readActualInput(t *testing.T) []byte {
	client := adventclient.NewAdventClient(Config.BaseUrl, Config.SessionToken)

	actual, err := client.GetInput(3)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	return actual
}
