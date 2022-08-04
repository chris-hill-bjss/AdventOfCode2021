package dayone

import (
	"log"
	"strconv"
	"strings"
)

type dayOne struct {
	input []byte
}

func NewDayOne(content []byte) *dayOne {
	return &dayOne{content}
}

func (d *dayOne) RunPartOne() int {
	var prev, increments int

	for _, line := range strings.Split(strings.TrimSpace(string(d.input)), "\n") {
		depth, err := strconv.Atoi(line)
		if err != nil {
			log.Fatal(err)
		}

		if prev > 0 && depth > prev {
			increments++
		}

		prev = depth
	}

	return increments
}

func (d *dayOne) RunPartTwo() int {
	var prev, increments int

	readings := stringArrayToIntArray(strings.Split(strings.TrimSpace(string(d.input)), "\n"))
	for i, _ := range readings {

		rangeEnd := i + 3
		if rangeEnd > len(readings) {
			break
		}

		var sumDepth int
		depths := readings[i : i+3]
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

func stringArrayToIntArray(slice []string) []int {
	out := make([]int, len(slice))
	for i, v := range slice {
		out[i] = stringToInt(v)
	}
	return out
}
func stringToInt(input string) int {
	value, err := strconv.Atoi(input)
	if err != nil {
		log.Fatal(err)
	}

	return value
}
