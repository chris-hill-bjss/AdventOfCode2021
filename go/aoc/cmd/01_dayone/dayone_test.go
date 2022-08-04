package dayone_test

import (
	"testing"

	dayone "aoc/cmd/01_dayone"

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

func TestPartOne(t *testing.T) {
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
				dayOne := dayone.NewDayOne(test.readInput(t))
				actual := dayOne.RunPartOne()

				Assert(t).Equals(test.expected, actual)
			}
		})
	}
}

func TestPartTwo(t *testing.T) {
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
				dayOne := dayone.NewDayOne(test.readInput(t))
				actual := dayOne.RunPartTwo()

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

	actual, err := client.GetInput(1)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	return actual
}
