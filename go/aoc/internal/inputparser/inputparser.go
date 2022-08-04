package inputparser

import (
	"log"
	"strconv"
	"strings"
)

func ToStringArray(input []byte) []string {
	return strings.Split(strings.TrimSpace(string(input)), "\n")
}

func MapArray[F comparable, T comparable](from []F, mapper func(F) T) []T {
	to := make([]T, len(from))

	for i, v := range from {
		to[i] = mapper(v)
	}

	return to
}

func StringToInt(input string) int {
	value, err := strconv.Atoi(input)
	if err != nil {
		log.Fatal(err)
	}

	return value
}
