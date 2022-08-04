package dayone

import (
	. "aoc/internal/inputparser"
)

type dayOne struct {
	readings []int
}

func NewDayOne(input []byte) *dayOne {
	readings := MapArray(ToStringArray(input), StringToInt)
	return &dayOne{readings}
}

func (d *dayOne) RunPartOne() int {
	var prev, increments int

	for _, depth := range d.readings {
		if prev > 0 && depth > prev {
			increments++
		}

		prev = depth
	}

	return increments
}

func (d *dayOne) RunPartTwo() int {
	var prev, increments int

	for i := range d.readings {

		rangeEnd := i + 3
		if rangeEnd > len(d.readings) {
			break
		}

		var sumDepth int
		depths := d.readings[i : i+3]
		for _, v := range depths {
			sumDepth += v
		}

		if prev > 0 && sumDepth > prev {
			increments++
		}

		prev = sumDepth
	}

	return increments
}
