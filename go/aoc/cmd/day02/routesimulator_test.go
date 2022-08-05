package day02_test

import (
	"testing"

	"aoc/cmd/day02"
	"aoc/internal/tests/adventclient"

	. "aoc/internal/tests/assertions"
	. "aoc/internal/tests/config"
)

const sampleInput = `forward 5
down 5
forward 8
up 3
down 8
forward 2`

func TestRouteSimulatorSimpleSimulation(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 150},
		{"actual", readActualInput, 1635930},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				simulator := day02.NewRouteSimulator(test.readInput(t))
				actual := simulator.SimpleSimulation()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func TestRouteSimulatorAdvancedSimulation(t *testing.T) {
	tests := []struct {
		name      string
		readInput func(t *testing.T) []byte
		expected  int
	}{
		{"sample", readSampleInput, 900},
		{"actual", readActualInput, 1781819478},
	}

	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			t.Log("Running tests with " + test.name + " input")
			{
				simulator := day02.NewRouteSimulator(test.readInput(t))
				actual := simulator.AdvancedSimulation()

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

	actual, err := client.GetInput(2)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	return actual
}
