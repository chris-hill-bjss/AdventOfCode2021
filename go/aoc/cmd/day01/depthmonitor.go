package day01

import (
	. "aoc/internal/tests/inputparser"
)

type depthMonitor struct {
	readings []int
}

func NewDepthMonitor(input []byte) *depthMonitor {
	readings := MapArray(ToStringArray(input), StringToInt)
	return &depthMonitor{readings}
}

func (d *depthMonitor) Incremental() int {
	var prev, increments int

	for _, depth := range d.readings {
		if prev > 0 && depth > prev {
			increments++
		}

		prev = depth
	}

	return increments
}

func (d *depthMonitor) Windowed() int {
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
