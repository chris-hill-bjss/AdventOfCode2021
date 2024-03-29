package day01_test

import (
	"testing"

	"aoc/cmd/day01"
	"aoc/internal/tests/adventclient"

	. "aoc/internal/tests/assertions"
	. "aoc/internal/tests/config"
)

const sampleInput = `199
200
208
210
200
207
240
269
260
263`

func TestDepthMonitorIncremental(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 7},
		{"actual", readActualInput, 1215},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				monitor := day01.NewDepthMonitor(test.readInput(t))
				actual := monitor.Incremental()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func TestDepthMonitorWindowed(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 5},
		{"actual", readActualInput, 1150},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				monitor := day01.NewDepthMonitor(test.readInput(t))
				actual := monitor.Windowed()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func readSampleInput(t *testing.T) []byte {
	return []byte(sampleInput)
}

func readActualInput(t *testing.T) []byte {
	config := NewTestConfig()
	client := adventclient.NewAdventClient(config.BaseUrl, config.SessionToken)

	actual, err := client.GetInput(1)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	return actual
}
