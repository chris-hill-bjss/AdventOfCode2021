package day02

import (
	"strings"

	. "aoc/internal/inputparser"
)

type instruction struct {
	direction string
	distance  int
}

type routeSimulator struct {
	instructions []instruction
}

func NewRouteSimulator(input []byte) *routeSimulator {
	instructions := MapArray(ToStringArray(input), stringToInstruction)
	return &routeSimulator{instructions}
}

func stringToInstruction(input string) instruction {
	components := strings.Split(input, " ")

	return instruction{components[0], StringToInt(components[1])}
}

func (s *routeSimulator) SimpleSimulation() int {
	var x, y int

	for _, instruction := range s.instructions {
		switch instruction.direction {
		case "forward":
			x += instruction.distance
		case "up":
			y -= instruction.distance
		case "down":
			y += instruction.distance
		}
	}

	return x * y
}

func (s *routeSimulator) AdvancedSimulation() int {
	var x, y, aim int

	for _, instruction := range s.instructions {
		switch instruction.direction {
		case "forward":
			x += instruction.distance
			y += instruction.distance * aim
		case "up":
			aim -= instruction.distance
		case "down":
			aim += instruction.distance
		}
	}

	return x * y
}
