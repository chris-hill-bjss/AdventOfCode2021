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
