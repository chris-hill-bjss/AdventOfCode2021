package dayone_test

import (
	"testing"

	dayone "aoc/cmd/01_dayone"

	. "aoc/internal/tests/assertions"

	"aoc/internal/tests/context"
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

func TestDayOneWithSampleInput(t *testing.T) {
	t.Log("Running tests for DayOne with sample input")
	{
		t.Log("PartOne")
		{
			partOneSample(t)
		}
		t.Log("PartTwo")
		{
			partTwoSample(t)
		}
	}
}

func TestDayOneWithActualInput(t *testing.T) {
	t.Log("Running tests for DayOne with actual input")
	{
		t.Log("PartOne")
		{
			partOneActual(t)
		}
		t.Log("PartTwo")
		{
			partTwoActual(t)
		}
	}
}

func partOneSample(t *testing.T) {
	dayOne := dayone.NewDayOne([]byte(sampleInput))
	actual := dayOne.RunPartOne()

	Assert(t).Equals(7, actual)
}

func partTwoSample(t *testing.T) {
	dayOne := dayone.NewDayOne([]byte(sampleInput))
	actual := dayOne.RunPartTwo()

	Assert(t).Equals(5, actual)
}

func partOneActual(t *testing.T) {
	context := context.NewContext()

	input, err := context.AdventClient.GetInput(1)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	dayOne := dayone.NewDayOne(input)
	actual := dayOne.RunPartOne()

	Assert(t).Equals(1215, actual)
}

func partTwoActual(t *testing.T) {
	context := context.NewContext()

	input, err := context.AdventClient.GetInput(1)
	if err != nil {
		Assert(t).Fail("failed to read input from AoC")
	}

	dayOne := dayone.NewDayOne(input)
	actual := dayOne.RunPartTwo()

	Assert(t).Equals(1150, actual)
}
